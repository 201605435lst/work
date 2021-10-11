using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.StdBasic.Services.Category
{
    [Authorize]
    public class StdBasicComponentCategoryAppService : StdBasicAppService, IStdBasicComponentCategoryAppService
    {
        readonly IRepository<ComponentCategory, Guid> _repositoryComponentCategory;
        readonly IRepository<ComponentCategoryRltProductCategory, Guid> _repositoryComponentCategoryRltProductCategory;
        readonly IRepository<ComponentCategoryRltMVDProperty, Guid> _repositoryComponentCategoryRltMVDProperty;
        readonly IRepository<ProductCategory, Guid> _repositoryProductCategory;
        readonly IGuidGenerator _guidGenerator;
        readonly IUnitOfWorkManager _unitOfWork;
        readonly IFileImportHandler _fileImport;
        readonly IDataFilter _dataFilter;

        public StdBasicComponentCategoryAppService(
            IRepository<ComponentCategory, Guid> repositoryComponentCategory,
            IRepository<ComponentCategoryRltProductCategory, Guid> repositoryComponentCategoryRltProductCategory,
            IRepository<ComponentCategoryRltMVDProperty, Guid> repositoryComponentCategoryRltMVDProperty,
            IRepository<ProductCategory, Guid> repositoryProductCategory,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWork,
            IFileImportHandler fileImport,
            IDataFilter dataFilter
        )
        {
            _repositoryComponentCategory = repositoryComponentCategory;
            _repositoryComponentCategoryRltProductCategory = repositoryComponentCategoryRltProductCategory;
            _repositoryComponentCategoryRltMVDProperty = repositoryComponentCategoryRltMVDProperty;
            _repositoryProductCategory = repositoryProductCategory;
            _guidGenerator = guidGenerator;
            _unitOfWork = unitOfWork;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
        }

        /// <summary>
        ///     获取构件分类实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.ComponentCategory.Detail)]
        public Task<ComponentCategoryDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new UserFriendlyException("请输入正确的id");
            }

            var componentCategory = _repositoryComponentCategory.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (componentCategory == null)
            {
                throw new UserFriendlyException("此构件不存在");
            }

            return Task.FromResult(ObjectMapper.Map<ComponentCategory, ComponentCategoryDto>(componentCategory));
        }

        /// <summary>
        /// 根据code获取构件分类信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<ComponentCategoryDto> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new UserFriendlyException("构件分类编码不能为空");
            }

            var componentCategory = _repositoryComponentCategory.WithDetails().Where(x => x.Code == code).FirstOrDefault();
            if (componentCategory == null)
            {
                throw new UserFriendlyException("此构件不存在");
            }

            return Task.FromResult(ObjectMapper.Map<ComponentCategory, ComponentCategoryDto>(componentCategory));
        }

        /// <summary>
        ///     根据输入条件按需加载数据接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ComponentCategoryDto>> GetList(ComponentCategoryGetListByIdsDto input)
        {
            var result = new PagedResultDto<ComponentCategoryDto>();

            var componentCategories = new List<ComponentCategory>();

            var dtos = new List<ComponentCategoryDto>();

            //根据传入的ids集合获取兄弟及父级元素
            if (input.Ids != null && input.Ids.Count > 0)
            {
                foreach (var id in input.Ids)
                {
                    //获取当前对象实体
                    var componentCategory = _repositoryComponentCategory.FirstOrDefault(x => x.Id == id);
                    if (componentCategory == null)
                    {
                        continue;
                    }

                    var code = "";
                    if (componentCategory != null)
                    {
                        //当前实体code
                        code = componentCategory.Code;
                    }

                    var regSplit = new Regex("[-_. ]");
                    var regLast = new Regex(regSplit + "&");
                    var simpleCode = regSplit.Split(code);
                    var array = new List<string>();
                    for (var i = 1; i <= simpleCode.Length - 1; i++)
                    {
                        var last = array.LastOrDefault();
                        var _code = last != null ? last : code;
                        var str = regLast.Replace(_code, "");
                        array.Add(str);
                    }

                    //根据array获取所有parent实体集合
                    var parents = _repositoryComponentCategory.Where(x => array.Contains(x.Code)).ToList();

                    var parentIds = parents.Select(s => s.Id).ToList();

                    //根据parentIds查询符合条件的数据
                    var filterList = _repositoryComponentCategory
                        .WithDetails(x => x.Children)
                        .Where(x => parentIds.Contains(x.ParentId.Value) || x.ParentId == null)
                        .ToList();
                    componentCategories.AddRange(filterList);
                }

                // 数据去重并转成dto
                var listDtos =
                    ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(componentCategories.Distinct()
                        .ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<ComponentCategoryDto>();
                    }
                }

                dtos = GuidKeyTreeHelper<ComponentCategoryDto>.GetTree(listDtos);
            }
            else //如果ids为空，根据parentId有无去按需加载数据
            {
                //根据关键字查询
                if (!string.IsNullOrEmpty(input.KeyWords))
                {
                    componentCategories = _repositoryComponentCategory.WithDetails()
                        .WhereIf(condition: !string.IsNullOrEmpty(input.KeyWords),
                            x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                    dtos = ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(componentCategories);
                    foreach (var item in dtos)
                    {
                        item.Children = null;
                    }
                }

                else
                {
                    componentCategories = _repositoryComponentCategory.WithDetails()
                        .WhereIf(condition: input.ParentId != null && input.ParentId != Guid.Empty,
                            x => x.ParentId == input.ParentId)
                        .WhereIf(condition: input.ParentId == null || input.ParentId == Guid.Empty,
                            x => x.Parent == null) // 顶级
                        .ToList();
                    dtos = ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(componentCategories);
                    foreach (var item in dtos)
                    {
                        if (item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            item.Children = new List<ComponentCategoryDto>();
                        }
                    }
                }
            }

            result.TotalCount = dtos.Count();
            result.Items = input.IsAll
                ? dtos.OrderBy(x => x.Code).ToList()
                : dtos.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }

        /// <summary>
        ///     根据id获得当前分类中的最大编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ComponentCategoryDto> GetListCode(Guid? id)
        {
            var list = new List<ComponentCategory>();
            if (id != null && id != Guid.Empty)
            {
                list = _repositoryComponentCategory.Where(x => x.ParentId == id).OrderBy(x => x.Code).ToList();
            }
            else
            {
                list = _repositoryComponentCategory.Where(x => x.ParentId == null).OrderBy(x => x.Code).ToList();
            }

            var dtos = Task.FromResult(
                ObjectMapper.Map<ComponentCategory, ComponentCategoryDto>(list.Count > 0 ? list.Last() : null));
            return dtos;
        }

        /// <summary>
        /// 根据ids获得当前对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ComponentCategoryDto>> GetListComponent(ComponentCategoryGetListByIdsDto input)
        {
            var componentCategories = new List<ComponentCategory>();
            var result = new PagedResultDto<ComponentCategoryDto>();
            if (input.Ids == null)
                input.Ids = new List<Guid>();
            //获取当前对象实体
            if (input.Ids.Count > 0)
            {
                componentCategories = _repositoryComponentCategory.Where(x => input.Ids.Contains(x.Id)).ToList();
                var dtos = ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(componentCategories);
                result.TotalCount = dtos.Count;
                result.Items = input.IsAll
                ? dtos.OrderBy(x => x.Code).ToList()
                : dtos.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            }

            return Task.FromResult(result);
        }
        [UnitOfWork, Authorize(StdBasicPermissions.ComponentCategory.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            var rowIndex = 2; //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ComponentCategoryTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ComponentCategoryTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<ComponentCategoryTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();

            var allProductCategory = _repositoryProductCategory.Where(s => s.IsDeleted == false);
            ComponentCategory hasComponentCategoryModel;
            var addedComponentCategories = new List<ComponentCategory>();

            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, count: datalist.Count());

                // 校验正则
                Regex checkReg = new Regex(@"^[A-Z]{2}(-[0-9]{2}){0,1}( [0-9]{2})*$");
                // 分割正则
                Regex spliteReg = new Regex("[-_. ]");


                // 按照层级分组
                var groupList = datalist.GroupBy(a => spliteReg.Split(a.Code).Length).ToList();
                var updateIndex = 1;
                foreach (var ite in groupList)
                {
                    // 相同层级的数据
                    foreach (var item in ite.ToList())
                    {
                        await _fileImport.UpdateState(input.ImportKey, updateIndex);
                        updateIndex++;
                        var canInsert = true;
                        var newInfo = new WrongInfo(item.Index);

                        if (item.Code.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo("编码为空");
                        }

                        if (item.Name.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo("名称为空");
                        }

                        if (!checkReg.IsMatch(item.Code))
                        {
                            canInsert = false;
                            newInfo.AppendInfo("编码不合法");
                        }

                        if (!canInsert)
                        {
                            wrongInfos.Add(newInfo);
                            continue;
                        }

                        using var uow = _unitOfWork.Begin(true);

                        using (_dataFilter.Disable<ISoftDelete>())
                        {
                            hasComponentCategoryModel =
                                _repositoryComponentCategory.FirstOrDefault(a => a.Code == item.Code);
                        }

                        if (hasComponentCategoryModel != null)
                        {
                            newInfo.AppendInfo($"{item.Code}已存在，且已更新");
                            hasComponentCategoryModel.Name = item.Name;
                            hasComponentCategoryModel.Remark = item.Remark;
                            hasComponentCategoryModel.ExtendCode = item.ExtendCode;
                            hasComponentCategoryModel.ExtendName = item.ExtendName;
                            hasComponentCategoryModel.LevelName = item.LevelName;
                            hasComponentCategoryModel.Unit = item.Unit;
                            hasComponentCategoryModel.IsDeleted = false;
                            await _repositoryComponentCategory.UpdateAsync(hasComponentCategoryModel);
                            await uow.SaveChangesAsync();
                            addedComponentCategories.Add(hasComponentCategoryModel);
                            wrongInfos.Add(newInfo);
                        }
                        else
                        {
                            var parentCode = GetParentComponentCategoryCode(item.Code);
                            var parentModel = _repositoryComponentCategory.FirstOrDefault(a => a.Code == parentCode);
                            if (spliteReg.Split(parentCode).Length > 1 && parentModel == null)
                            {
                                newInfo.AppendInfo($"{item.Code}不存在父级");
                                wrongInfos.Add(newInfo);
                                continue;
                            }


                            hasComponentCategoryModel = new ComponentCategory(_guidGenerator.Create());
                            hasComponentCategoryModel.Name = item.Name;
                            hasComponentCategoryModel.Code = item.Code;
                            hasComponentCategoryModel.Remark = item.Remark;
                            hasComponentCategoryModel.ExtendCode = item.ExtendCode;
                            hasComponentCategoryModel.ExtendName = item.ExtendName;
                            hasComponentCategoryModel.LevelName = item.LevelName;
                            hasComponentCategoryModel.Unit = item.Unit;
                            if (parentModel != null)
                            {
                                hasComponentCategoryModel.ParentId = parentModel.Id;
                            }

                            await _repositoryComponentCategory.InsertAsync(hasComponentCategoryModel);
                            addedComponentCategories.Add(hasComponentCategoryModel);
                            await uow.SaveChangesAsync();
                        }

                        //添加产品分类信息
                        if (addedComponentCategories.Count > 0)
                        {
                            await _repositoryComponentCategoryRltProductCategory.DeleteAsync(s =>
                                s.ComponentCategoryId == addedComponentCategories.Last().Id);
                        }

                        await uow.SaveChangesAsync();
                        if (!string.IsNullOrEmpty(item.ProductCategories))
                        {
                            //if (!productCategories.Substring(0, 1).Equals("[")) throw new UserFriendlyException("关联产品格式错误");
                            // TODO 需要适配新的split
                            //var matchCodeResult = Regex.Matches(item.ProductCategories, @"\[(.+?)\]"); //解析关联产品的编码
                            //for (var j = 0; j < matchCodeResult.Count; j++)
                            //{
                            //    var componentCategory_Pro =
                            //        new ComponentCategoryRltProductCategory(_guidGenerator.Create());
                            //    var samePro = allProductCategory.FirstOrDefault(s =>
                            //        '[' + s.Code + ']' == matchCodeResult[j].Value);
                            //    if (samePro != null)
                            //    {
                            //        foreach (var com in addedComponentCategories)
                            //        {
                            //            if (com.Code == item.Code)
                            //            {
                            //                componentCategory_Pro.ComponentCategoryId = com.Id;
                            //                break;
                            //            }
                            //        }

                            //        componentCategory_Pro.ProductionCategoryId = samePro.Id;
                            //        await _repositoryComponentCategoryRltProductCategory.InsertAsync(
                            //            componentCategory_Pro);
                            //    }
                            //}
                        }
                    }
                }

                await _fileImport.Complete(input.ImportKey);
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(useId: CurrentUser.Id.GetValueOrDefault(), input.ImportKey,
                        fileBytes: workbook.ConvertToBytes());
                }
            }

            /* //软删除excel中不存在的构件数据
             var softDeleteManu = allComponentCategory.Where(s => !addedComponentCategories.Select(s => s.Code).Contains(s.Code));
             foreach (var item in softDeleteManu)
             {
                 item.IsDeleted = true;
                 await _repositoryComponentCategory.UpdateAsync(item);
                 deleteCount++;
             }
             failMsg += string.Format($"成功导入{addComCount}条构件数据，更新{updateCount}条构件数据，删除{deleteCount}条构件数据，成功导入{addComRltProCount}条构件产品数据");

             return failMsg;*/
        }

        public async Task<Stream> Export(ComponentCategoryData input)
        {
            // 数据转换 start
            var list1 = _repositoryComponentCategory.ToList();

            foreach (var item in list1)
            {
                if (!item.Code.Contains('.'))
                {
                    continue;
                }
                //using var uow = _unitOfWork.Begin(true,false);
                var codes = item.Code.Replace("SCC.DT.", "").Trim().Split(".");
                item.Code = "";

                for (var index = 0; index < codes.Length; index++)
                {
                    var c = codes[index];
                    if (index == 0)
                    {
                        item.Code += c + "-";
                    }
                    else
                    {
                        item.Code += c + " ";
                    }

                }

                var reg = new Regex("[-]{1}$");
                item.Code = reg.Replace(item.Code, "");
                item.Code = item.Code.Trim();

                await _repositoryComponentCategory.UpdateAsync(item);
                //await uow.SaveChangesAsync();
            }
            //数据转换 end


            List<ComponentCategory> list;
            if (input.Paramter.KeyWords != "" || input.Paramter.ParentId != null)
            {
                list = GetComponentCategoriesData(input.Paramter).AsEnumerable().OrderBy(y => y.Code).ToList();
            }
            else
            {
                list = _repositoryComponentCategory.Where(x => x.Id != Guid.Empty).OrderBy(y => y.Code).ToList();
            }

            var CRPList = await _repositoryComponentCategoryRltProductCategory.GetListAsync();
            var proList = await _repositoryProductCategory.GetListAsync();
            var rltMVDList = _repositoryComponentCategoryRltMVDProperty.WithDetails(x => x.MVDProperty.MVDCategory).ToList();

            var componentCategories = new List<ComponentCategoryTemplate>();
            var componentCategory = new ComponentCategoryTemplate();
            foreach (var item in list)
            {
                componentCategory = new ComponentCategoryTemplate();
                var crp = CRPList.Where(x => x.ComponentCategoryId == item.Id);
                if (crp != null)
                {
                    foreach (var cr in crp)
                    {
                        var pros = proList.FirstOrDefault(x => x.Id == cr.ProductionCategoryId);
                        if (pros != null)
                        {
                            componentCategory.ProductCategories =
                               componentCategory.ProductCategories + pros.Name + $"{"[" + pros.Code + "]"} " + Environment.NewLine;
                        }

                    }
                }

                componentCategory.Code = item.Code;
                componentCategory.Name = item.Name;
                componentCategory.ExtendCode = item.ExtendCode;
                componentCategory.ExtendName = item.ExtendName;
                componentCategory.LevelName = item.LevelName;
                componentCategory.Unit = item.Unit;
                componentCategory.MVD = "mvd";
                componentCategory.Remark = item.Remark;

                var mvdListDtos = ObjectMapper.Map<List<ComponentCategoryRltMVDProperty>, List<ComponentCategoryRltMVDPropertyDto>>(rltMVDList.Where(x => x.ComponentCategoryId == item.Id).ToList());

                var mvdCategoriesDtos = new List<MVDCategoryDto>();
                // 构建顶级
                mvdListDtos.ForEach(x =>
                {
                    var categoryDto = x.MVDProperty.MVDCategory;
                    if (mvdCategoriesDtos.Find(y => y.Id == categoryDto.Id) == null)
                    {
                        mvdCategoriesDtos.Add(categoryDto);
                    }
                });
                mvdCategoriesDtos.ForEach(x =>
                {
                    x.MVDProperties = new List<MVDPropertyDto>();
                    var properties = mvdListDtos.Where(p => p.MVDProperty.MVDCategoryId == x.Id).Select(r => r.MVDProperty).ToList();
                    properties.ForEach(x => x.MVDCategory = null);
                    x.MVDProperties.AddRange(properties);
                });

                componentCategory.MVD = JsonConvert.SerializeObject(ObjectMapper.Map<List<MVDCategoryDto>, List<MVDCategoryExportDto>>(mvdCategoriesDtos)).ToString();

                componentCategories.Add(componentCategory);
            }

            var dtoList =
                ObjectMapper.Map<List<ComponentCategoryTemplate>, List<ComponentCategoryTemplate>>(componentCategories);
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);

            return stream;
        }


        [Authorize(StdBasicPermissions.ComponentCategory.Create)]
        public async Task<ComponentCategoryDto> Create(ComponentCategoryCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                throw new UserFriendlyException("构件名称不能为空");
            }
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("构件名称不能包含空格");
            }

            if (string.IsNullOrEmpty(input.Code))
            {
                throw new UserFriendlyException("构件编码不能为空");
            }
            if (!StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("构件编码不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("单位不能为纯数字");
            }

            if (!string.IsNullOrEmpty(input.LevelName) && !StringUtil.CheckSpaceValidity(input.LevelName))
            {
                throw new UserFriendlyException("层级名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendName) && !StringUtil.CheckSpaceValidity(input.ExtendName))
            {
                throw new UserFriendlyException("扩展名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendCode) && !StringUtil.CheckCodeValidity(input.ExtendCode))
            {
                throw new UserFriendlyException("扩展编码不合法");
            }
            CheckSameName(input.Name, null, input.ParentId);
            CheckSameCode(input.Code, null, input.ParentId);
            var componentCategory = new ComponentCategory(_guidGenerator.Create());
            componentCategory.Name = input.Name;
            componentCategory.Remark = input.Remark;
            componentCategory.LevelName = input.LevelName;
            componentCategory.ExtendName = input.ExtendName;
            componentCategory.ExtendCode = input.ExtendCode;
            componentCategory.ParentId = input.ParentId;
            componentCategory.Code = input.Code;
            componentCategory.Unit = input.Unit;
            componentCategory.ComponentCategoryRltProductCategories = new List<ComponentCategoryRltProductCategory>();
            if (input.ProductCategoryId != null && input.ProductCategoryId != Guid.Empty)
            {
                componentCategory.ComponentCategoryRltProductCategories.Add(
                    new ComponentCategoryRltProductCategory(_guidGenerator.Create())
                    {
                        ProductionCategoryId = input.ProductCategoryId
                    }
                );
            }

            await _repositoryComponentCategory.InsertAsync(componentCategory);
            return ObjectMapper.Map<ComponentCategory, ComponentCategoryDto>(componentCategory);
        }

        [Authorize(StdBasicPermissions.ComponentCategory.Update)]
        public async Task<ComponentCategoryDto> Update(ComponentCategoryUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty)
            {
                throw new UserFriendlyException("请输入构件的id");
            }

            var componentCategory = await _repositoryComponentCategory.GetAsync(input.Id);
            if (componentCategory == null)
            {
                throw new UserFriendlyException("该构件不存在");
            }

            if (string.IsNullOrEmpty(input.Name))
            {
                throw new UserFriendlyException("构件名称不能为空");
            }

            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("构件名称不能包含空格");
            }

            if (string.IsNullOrEmpty(input.Code))
            {
                throw new UserFriendlyException("构件编码不能为空");
            }
            if (!StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("构件编码不合法");
            }

            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("构件单位不能为纯数字");
            }

            if (!string.IsNullOrEmpty(input.LevelName) && !StringUtil.CheckSpaceValidity(input.LevelName))
            {
                throw new UserFriendlyException("构件层级名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendName) && !StringUtil.CheckSpaceValidity(input.ExtendName))
            {
                throw new UserFriendlyException("构件扩展名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendCode) && !StringUtil.CheckCodeValidity(input.ExtendCode))
            {
                throw new UserFriendlyException("构件扩展编码不合法");
            }

            CheckSameName(input.Name, input.Id, input.ParentId);
            CheckSameCode(input.Code, input.Id, input.ParentId);
            componentCategory.Name = input.Name;
            componentCategory.Remark = input.Remark;
            componentCategory.LevelName = input.LevelName;
            componentCategory.ExtendName = input.ExtendName;
            componentCategory.ExtendCode = input.ExtendCode;
            componentCategory.ParentId = input.ParentId;
            componentCategory.Code = input.Code;
            componentCategory.Unit = input.Unit;

            //清除之前的关联表信息
            if (componentCategory.ComponentCategoryRltProductCategories.Count > 0)
            {
                await _repositoryComponentCategoryRltProductCategory.DeleteAsync(x =>
                    x.ComponentCategoryId == componentCategory.Id);
            }

            //重新保存
            componentCategory.ComponentCategoryRltProductCategories = new List<ComponentCategoryRltProductCategory>();
            if (input.ProductCategoryId != null && input.ProductCategoryId != Guid.Empty)
            {
                componentCategory.ComponentCategoryRltProductCategories.Add(
                    new ComponentCategoryRltProductCategory(_guidGenerator.Create())
                    {
                        ProductionCategoryId = input.ProductCategoryId
                    }
                );
            }

            await _repositoryComponentCategory.UpdateAsync(componentCategory);
            return ObjectMapper.Map<ComponentCategory, ComponentCategoryDto>(componentCategory);
        }

        [Authorize(StdBasicPermissions.ComponentCategory.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new UserFriendlyException("请输入正确的id");
            }

            var componentCategory = _repositoryComponentCategory.WithDetails(x => x.Children).Where(x => x.Id == id)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(componentCategory.Name))
            {
                throw new UserFriendlyException("该构件不存在");
            }

            if (componentCategory.Children != null && componentCategory.Children.Count > 0)
            {
                throw new UserFriendlyException("请先删除该构件的下级分类");
            }

            await _repositoryComponentCategory.DeleteAsync(id);

            return true;
        }


        #region 私有方法

        bool CheckSameName(string Name, Guid? id, Guid? parentId)
        {
            var componentCategory = _repositoryComponentCategory.WithDetails()
                .Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                componentCategory = componentCategory.Where(x => x.ParentId == parentId);
            }
            else
            {
                componentCategory = componentCategory.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }

            if (id.HasValue)
            {
                componentCategory = componentCategory.Where(x => x.Id != id.Value);
            }

            if (componentCategory.Count() > 0)
            {
                throw new UserFriendlyException("当前分类中已存在相同名字的构件!!!");
            }

            return true;
        }

        bool CheckSameCode(string Code, Guid? id, Guid? parentId)
        {
            var componentCategory = _repositoryComponentCategory.WithDetails()
                .Where(x => x.Code.ToUpper() == Code.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                componentCategory = componentCategory.Where(x => x.ParentId == parentId);
            }
            else
            {
                componentCategory = componentCategory.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }

            if (id.HasValue)
            {
                componentCategory = componentCategory.Where(x => x.Id != id.Value);
            }

            if (componentCategory.Count() > 0)
            {
                throw new UserFriendlyException("当前分类中已存在相同编号的构件!!!");
            }

            return true;
        }

        List<ComponentCategory> GetChildren(IEnumerable<ComponentCategory> data, Guid? id)
        {
            var componentCategory = new List<ComponentCategory>();
            var children = data.Where(x => x.ParentId == id);
            foreach (var item in children)
            {
                var node = new ComponentCategory(item.Id);
                node.Name = item.Name;
                node.LevelName = item.LevelName;
                node.ParentId = item.ParentId;
                //node.Parent = item.Parent;
                node.ComponentCategoryRltProductCategories = item.ComponentCategoryRltProductCategories;
                node.Code = item.Code;
                node.Remark = item.Remark;
                node.ExtendName = item.ExtendName;
                node.ExtendCode = item.ExtendCode;
                node.Unit = item.Unit;
                node.Children = GetChildren(data, item.Id);
                componentCategory.Add(node);
            }

            return componentCategory;
        }

        string GetParentComponentCategoryCode(string code)
        {
            // DT-01 22 55
            // DT-01 22
            var reg = new Regex("[-_. ][0-9]{2}$");
            return reg.Replace(code, "");
        }

        List<ComponentCategory> GetComponentCategoriesData(ComponentCategoryGetListByIdsDto input)
        {
            var componentCategories = new List<ComponentCategory>();
            var dtos = new List<ComponentCategoryDto>();

            //根据关键字查询
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                componentCategories = _repositoryComponentCategory.WithDetails()
                    .WhereIf(condition: !string.IsNullOrEmpty(input.KeyWords),
                        x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                dtos = ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(componentCategories);
                foreach (var item in dtos)
                {
                    item.Children = null;
                }
            }
            else
            {
                componentCategories = _repositoryComponentCategory.WithDetails()
                    .WhereIf(condition: input.ParentId != null && input.ParentId != Guid.Empty,
                        x => x.ParentId == input.ParentId)
                    .WhereIf(condition: input.ParentId == null || input.ParentId == Guid.Empty,
                        x => x.Parent == null) // 顶级
                    .ToList();
                dtos = ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(componentCategories);
                foreach (var item in dtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<ComponentCategoryDto>();
                    }
                }
            }

            var resource = ObjectMapper.Map<List<ComponentCategoryDto>, List<ComponentCategory>>(dtos);
            return resource;
        }

        #endregion
    }
}