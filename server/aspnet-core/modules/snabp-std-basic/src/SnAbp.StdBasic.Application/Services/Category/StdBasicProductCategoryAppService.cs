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
using SnAbp.StdBasic.IServices.Category;
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
    public class StdBasicProductCategoryAppService : StdBasicAppService, IStdBasicProductCategoryAppService
    {
        private readonly IRepository<ProductCategory, Guid> _repositoryProductCategory;
        private readonly IRepository<ProductCategoryRltMVDProperty, Guid> _repositoryProductCategoryRltMVDProperty;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IDataFilter _dataFilter;

        public StdBasicProductCategoryAppService(
            IRepository<ProductCategory, Guid> repositoryProductCategory,
            IRepository<ProductCategoryRltMVDProperty, Guid> repositoryProductCategoryRltMVDProperty,
            IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
            IUnitOfWorkManager unitOfWork,
            IDataFilter dataFilter
        )
        {
            _repositoryProductCategory = repositoryProductCategory;
            _repositoryProductCategoryRltMVDProperty = repositoryProductCategoryRltMVDProperty;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _unitOfWork = unitOfWork;
            _dataFilter = dataFilter;
        }

        /// <summary>
        /// 获取产品分类实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProductCategoryDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var productCategory = _repositoryProductCategory.WithDetails().FirstOrDefault(x => x.Id == id);
            if (productCategory == null) throw new UserFriendlyException("此构件不存在");
            return Task.FromResult(ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory));
        }

        /// <summary>
        /// 根据输入条件按需加载数据接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ProductCategoryDto>> GetList(ProductCategoryGetListByIdsDto input)
        {
            var result = new PagedResultDto<ProductCategoryDto>();

            var productCategories = new List<ProductCategory>();

            var dtos = new List<ProductCategoryDto>();

            if (input.Ids != null && input.Ids.Count > 0)
            {
                foreach (var id in input.Ids)
                {
                    var componentCategory = _repositoryProductCategory.FirstOrDefault(x => x.Id == id);
                    if (componentCategory == null) continue;
                    string code = "";
                    if (componentCategory != null)
                    {
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

                    var parents = _repositoryProductCategory.Where(x => array.Contains(x.Code)).ToList();

                    var parentIds = parents.Select(s => s.Id).ToList();
                    var filterList = _repositoryProductCategory
                        .WithDetails(x => x.Children)
                        .Where(x => parentIds.Contains(x.ParentId.Value) || x.ParentId == null)
                        .ToList();
                    productCategories.AddRange(filterList);
                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategories.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<ProductCategoryDto>();
                    }
                }

                dtos = GuidKeyTreeHelper<ProductCategoryDto>.GetTree(listDtos);
            }
            else //按需加载
            {
                //根据关键字查询
                if (!string.IsNullOrEmpty(input.KeyWords))
                {
                    productCategories = _repositoryProductCategory.WithDetails()
                        .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                    dtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategories);
                    foreach (var item in dtos)
                    {
                        item.Children = null;
                    }
                }

                else
                {
                    productCategories = _repositoryProductCategory.WithDetails()
                        .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                        .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                        .ToList();
                    dtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategories);
                    foreach (var item in dtos)
                    {
                        if (item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            item.Children = new List<ProductCategoryDto>();
                        }
                    }
                }


            }
            result.TotalCount = dtos.Count();
            result.Items = input.IsAll ? dtos.OrderBy(x => x.Code).ToList()
                : dtos.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }
        /// <summary>
        /// 根据id获得当前分类中的最大编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProductCategoryDto> GetListCode(Guid? id)
        {
            var list = new List<ProductCategory>();
            if (id != null && id != Guid.Empty)
            {
                list = _repositoryProductCategory.Where(x => x.ParentId == id).OrderBy(x => x.Code).ToList();
            }
            else
            {
                list = _repositoryProductCategory.Where(x => x.ParentId == null).OrderBy(x => x.Code).ToList();
            }
            var dtos = Task.FromResult(ObjectMapper.Map<ProductCategory, ProductCategoryDto>(list.Count > 0 ? list.Last() : null));
            return dtos;
        }
        /// <summary>
        /// 根据ids获得当前对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ProductCategoryDto>> GetListProduct(ProductCategoryGetListByIdsDto input)
        {
            var productCategories = new List<ProductCategory>();
            var result = new PagedResultDto<ProductCategoryDto>();
            if (input.Ids == null)
                input.Ids = new List<Guid>();
            //获取当前对象实体
            if (input.Ids.Count > 0)
            {
                productCategories = _repositoryProductCategory.Where(x => input.Ids.Contains(x.Id)).ToList();
                var dtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategories);
                result.TotalCount = dtos.Count;
                result.Items = input.IsAll
                ? dtos.OrderBy(x => x.Code).ToList()
                : dtos.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            }

            return Task.FromResult(result);
        }
        /// <summary>
        /// 根据产品分类 id 获取端子图符
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetTerminalSymbolById(Guid id)
        {
            var category = await _repositoryProductCategory.GetAsync(id);

            if (category != null)
            {
                return category.TerminalSymbol;
            }

            throw new UserFriendlyException("产品分类不存在");
        }

        /// <summary>
        /// 根据产品分类 code 获取端子图符
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<string> GetTerminalSymbolByCode(string code)
        {
            var category = _repositoryProductCategory.Where(x => x.Code == code).FirstOrDefault();

            if (category != null)
            {
                return Task.FromResult(category.TerminalSymbol);
            }

            throw new UserFriendlyException("产品分类不存在");
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.ProductCategory.Create)]
        public async Task<ProductCategoryDto> Create(ProductCategoryCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("产品名不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("产品编码不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("产品名称不能包含空格");
            }
            if (!StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("产品编码不合法");

            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("产品单位不能为纯数字");
            }

            if (!string.IsNullOrEmpty(input.LevelName) && !StringUtil.CheckSpaceValidity(input.LevelName))
            {
                throw new UserFriendlyException("产品层级名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendName) && !StringUtil.CheckSpaceValidity(input.ExtendName))
            {
                throw new UserFriendlyException("产品扩展名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendCode) && !StringUtil.CheckCodeValidity(input.ExtendCode))
            {
                throw new UserFriendlyException("产品扩展编码不合法");
            }
            CheckSameName(input.Name, null, input.ParentId);
            CheckSameCode(input.Code, null, input.ParentId);
            var productCategory = new ProductCategory(_guidGenerator.Create());
            productCategory.Name = input.Name;
            productCategory.LevelName = input.LevelName;
            productCategory.ParentId = input.ParentId;
            productCategory.Code = input.Code;
            productCategory.FullName = input.FullName;
            productCategory.Remark = input.Remark;
            productCategory.ExtendName = input.ExtendName;
            productCategory.ExtendCode = input.ExtendCode;
            productCategory.Unit = input.Unit;
            await _repositoryProductCategory.InsertAsync(productCategory);
            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
        }

        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.ProductCategory.Update)]
        public async Task<ProductCategoryDto> Update(ProductCategoryUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入产品的id");
            var productCategory = await _repositoryProductCategory.GetAsync(input.Id);
            if (productCategory == null) throw new UserFriendlyException("该产品不存在");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("产品名不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("产品编码不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("产品名称不能包含空格");
            }
            if (!StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("产品编码不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("产品单位不能为纯数字");
            }

            if (!string.IsNullOrEmpty(input.LevelName) && !StringUtil.CheckSpaceValidity(input.LevelName))
            {
                throw new UserFriendlyException("产品层级名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendName) && !StringUtil.CheckSpaceValidity(input.ExtendName))
            {
                throw new UserFriendlyException("产品扩展名称不能有空格");
            }
            if (!string.IsNullOrEmpty(input.ExtendCode) && !StringUtil.CheckCodeValidity(input.ExtendCode))
            {
                throw new UserFriendlyException("产品扩展编码不合法");
            }
            CheckSameName(input.Name, input.Id, input.ParentId);
            CheckSameCode(input.Code, input.Id, input.ParentId);
            productCategory.Name = input.Name;
            productCategory.LevelName = input.LevelName;
            productCategory.ParentId = input.ParentId;
            productCategory.Code = input.Code;
            productCategory.FullName = input.FullName;
            productCategory.Remark = input.Remark;
            productCategory.ExtendName = input.ExtendName;
            productCategory.ExtendCode = input.ExtendCode;
            productCategory.Unit = input.Unit;
            await _repositoryProductCategory.UpdateAsync(productCategory);
            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.ProductCategory.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var productCategory = _repositoryProductCategory.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (string.IsNullOrEmpty(productCategory.Name)) throw new UserFriendlyException("该产品不存在");
            if (productCategory.Children != null && productCategory.Children.Count > 0) throw new UserFriendlyException("请先删除该产品的下级分类");
            await _repositoryProductCategory.DeleteAsync(id);

            return true;
        }

        private bool CheckSameName(string Name, Guid? id, Guid? parentId)
        {
            var productCategory = _repositoryProductCategory.WithDetails().Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                productCategory = productCategory.Where(x => x.ParentId == parentId);
            }
            else
            {
                productCategory = productCategory.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                productCategory = productCategory.Where(x => x.Id != id.Value);
            }
            if (productCategory.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前分类中已存在相同名字的产品!!!");
            }
            return true;
        }
        private bool CheckSameCode(string Code, Guid? id, Guid? parentId)
        {
            var productCategory = _repositoryProductCategory.WithDetails().Where(x => x.Code.ToUpper() == Code.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                productCategory = productCategory.Where(x => x.ParentId == parentId);
            }
            else
            {
                productCategory = productCategory.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                productCategory = productCategory.Where(x => x.Id != id.Value);
            }
            if (productCategory.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前分类中已存在相同编号的构件!!!");
            }
            return true;
        }

        private List<ProductCategory> GetChildren(IEnumerable<ProductCategory> data, Guid? id)
        {
            List<ProductCategory> productCategory = new List<ProductCategory>();
            var children = data.Where(x => x.ParentId == id);
            foreach (var item in children)
            {
                var node = new ProductCategory(item.Id);
                node.Name = item.Name;
                node.LevelName = item.LevelName;
                node.ParentId = item.ParentId;
                node.Code = item.Code;
                node.FullName = item.FullName;
                node.Remark = item.Remark;
                node.ExtendName = item.ExtendName;
                node.ExtendCode = item.ExtendCode;
                node.Unit = item.Unit;
                node.Children = GetChildren(data, item.Id);
                productCategory.Add(node);
            }
            return productCategory;
        }

        /*private void ProductCategoryImport(FileUploadDto fileData, string key)
        {
            //获取excel表格，判断是否满足模板
            if (fileData == null)
            {
                throw new UserFriendlyException("导入文件为空");
            }

            if (fileData.File == null)
            {
                throw new UserFriendlyException("导入文件不能为空");
            }

            using (var fileStream = fileData.File.OpenReadStream())
            {
                //创建工作表
                IWorkbook workbook = null;
                // 2007版本
               if (fileData.File.FileName.IndexOf(".xlsx") > 0)
                {
                    workbook = new XSSFWorkbook(fileStream);
               }
                else if (fileData.File.FileName.IndexOf(".xls") > 0)
                {
                    // 2003版本
                    workbook = new HSSFWorkbook(fileStream);
                }

                var sheet = workbook.GetSheetAt(0);
                var firstRow = sheet.GetRow(0);
                if (firstRow == null || firstRow.Cells.Count != 8)
                {
                    throw new UserFriendlyException("该文件不符合设备导入模板的要求");
                }

                var dataList = ExcelHelper.SheetToList<ProductCategoryModel>(sheet,2);
                if (dataList == null)
                {
                    throw new UserFriendlyException("导入文件缺失数据");
                }

                _fileImport.Start(key, dataList.Count);

                //定义错误信息列
                var wrongInfos = new List<WrongInfo>();
                dataList?.ForEach(pro =>
                {
                    var productCategory = new ProductCategory(_guidGenerator.Create())
                    {
                        Code = pro.Code,
                        Name = pro.Name,
                        ExtendCode = pro.ExtendCode,
                        ExtendName = pro.ExtendName,
                        LevelName = pro.LevelName,
                        Unit = pro.Unit,
                        Remark = pro.Remark
                    };
                });
                _fileImport.Complete(key);
            }

            for (var i = 0; i < 30; i++)
            {
                Thread.Sleep(200);
                _fileImport.UpdateState(key, i);
            }

            var file = fileData.File;
            var read = file.OpenReadStream();
            var fileByte = read.GetAllBytes();
            _fileImport.Complete(key);
        }*/


        [UnitOfWork]
        public async Task Upload([FromForm] ImportData input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            //ProductCategoryTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 2;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ProductCategoryTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ProductCategoryTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<ProductCategoryTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            ProductCategory hasProductModel;
            if (datalist.Any())
            {
                // 校验正则
                Regex checkReg = new Regex(@"^[A-Z]{2}(-[0-9]{2}){0,1}( [0-9]{2})*$");
                // 分割正则
                Regex spliteReg = new Regex("[-_. ]&");

                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());
                var groupList = datalist.GroupBy(a => spliteReg.Split(a.Code).Length).ToList();
                var updateIndex = 1;
                foreach (var ite in groupList)
                {
                    foreach (var item in ite.ToList())
                    {
                        await _fileImport.UpdateState(input.ImportKey, updateIndex);
                        updateIndex++;
                        var canInsert = true;
                        var newInfo = new WrongInfo(item.Index);
                        if (item.Code.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"编码为空");
                        }
                        if (item.Name.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"名称为空");
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
                        using var uow = _unitOfWork.Begin(true, false);
                        using (_dataFilter.Disable<ISoftDelete>())
                        {
                            hasProductModel = _repositoryProductCategory.FirstOrDefault(a => a.Code == item.Code);
                        }
                        if (hasProductModel != null)
                        {
                            newInfo.AppendInfo($"{item.Code}已存在，且已更新");
                            hasProductModel.Name = item.Name;
                            hasProductModel.Remark = item.Remark;
                            hasProductModel.ExtendCode = item.ExtendCode;
                            hasProductModel.ExtendName = item.ExtendName;
                            hasProductModel.LevelName = item.LevelName;
                            hasProductModel.Unit = item.Unit;
                            hasProductModel.IsDeleted = false;
                            await _repositoryProductCategory.UpdateAsync(hasProductModel);
                            await uow.SaveChangesAsync();
                            wrongInfos.Add(newInfo);
                        }
                        else
                        {
                            var parentCode = GetParentProductCategoryCode(item.Code);
                            var parentModel = _repositoryProductCategory.FirstOrDefault(a => a.Code == parentCode);
                            if (spliteReg.Split(parentCode).Length > 1 && parentModel == null)
                            {
                                newInfo.AppendInfo($"{parentCode}出现断层，不存在父级");
                                wrongInfos.Add(newInfo);
                                continue;
                            }
                            hasProductModel = new ProductCategory(_guidGenerator.Create());
                            hasProductModel.Name = item.Name;
                            hasProductModel.Code = item.Code;
                            hasProductModel.Remark = item.Remark;
                            hasProductModel.ExtendCode = item.ExtendCode;
                            hasProductModel.ExtendName = item.ExtendName;
                            hasProductModel.LevelName = item.LevelName;
                            hasProductModel.Unit = item.Unit;
                            if (parentModel != null)
                            {
                                hasProductModel.ParentId = parentModel.Id;
                            }
                            await _repositoryProductCategory.InsertAsync(hasProductModel);
                            await uow.SaveChangesAsync();
                        }
                    }
                }

                await _fileImport.Complete(input.ImportKey);
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
                }
            }
        }
        public async Task<Stream> Export(ProductCategoryData input)
        {
            // 数据转换 start
            var _list = _repositoryProductCategory.ToList();

            foreach (var item in _list)
            {
                if (!item.Code.Contains('.'))
                {
                    continue;
                }
                //using var uow = _unitOfWork.Begin(true,false);
                var codes = item.Code.Replace("SPC.DT.", "CP.").Trim().Split(".");
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

                item.Code = item.Code.Trim();

                await _repositoryProductCategory.UpdateAsync(item);
                //await uow.SaveChangesAsync();
            }

            var dt = _repositoryProductCategory.Where(x => x.Code == "SPC-DT").FirstOrDefault();
            if (dt != null)
            {
                await _repositoryProductCategory.DeleteAsync(x => x.ParentId == dt.Id);
                await _repositoryProductCategory.DeleteAsync(dt);
            }
            //数据转换 end



            List<ProductCategory> list;
            if (input.Paramter.KeyWords != "" || input.Paramter.ParentId != null)
            {
                list = GetProductCategoriesData(input.Paramter).AsEnumerable().OrderBy(y => y.Code).ToList();
            }
            else
            {
                list = _repositoryProductCategory.Where(x => x.Id != Guid.Empty).OrderBy(y => y.Code).ToList();
            }
            var dtoList = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryTemplate>>(list);
            var rltMVDList = _repositoryProductCategoryRltMVDProperty.WithDetails(x => x.MVDProperty.MVDCategory, x => x.ProductCategory).ToList();

            dtoList.ForEach(temp =>
            {
                var mvdListDtos = ObjectMapper.Map<List<ProductCategoryRltMVDProperty>, List<ProductCategoryRltMVDPropertyDto>>(rltMVDList.Where(x => x.ProductCategory.Code == temp.Code).ToList());

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

                temp.MVD = JsonConvert.SerializeObject(ObjectMapper.Map<List<MVDCategoryDto>, List<MVDCategoryExportDto>>(mvdCategoriesDtos)).ToString();
            });

            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        private string GetParentProductCategoryCode(string code)
        {
            var reg = new Regex("[-_. ][0-9]{2}$");
            return reg.Replace(code, "");
        }

        private List<ProductCategory> GetProductCategoriesData(ProductCategoryGetListByIdsDto input)
        {
            var productCategories = new List<ProductCategory>();
            var dtos = new List<ProductCategoryDto>();

            //根据关键字查询
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                productCategories = _repositoryProductCategory.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                dtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategories);
                foreach (var item in dtos)
                {
                    item.Children = null;
                }
            }
            else
            {
                productCategories = _repositoryProductCategory.WithDetails()
                               .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                               .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                               .ToList();
                dtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategories);
                foreach (var item in dtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<ProductCategoryDto>();
                    }
                }
            }

            var resource = ObjectMapper.Map<List<ProductCategoryDto>, List<ProductCategory>>(dtos);
            return resource;
        }

    }
}