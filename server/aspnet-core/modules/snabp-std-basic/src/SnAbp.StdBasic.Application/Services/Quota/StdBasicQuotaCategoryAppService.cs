using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.Common.Entities;
using SnAbp.Identity;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicQuotaCategoryAppService : StdBasicAppService, IStdBasicQuotaCategoryAppService
    {
        private readonly IRepository<QuotaCategory, Guid> _repositoryQuotaCategory;
        private readonly IRepository<Quota, Guid> _repositoryQuota;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IDataFilter _dataFilter;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IRepository<Area> _repositoryArea;//区域
        private readonly IRepository<DataDictionary, Guid> _repositoryDataDictionary;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repositoryQuotaCategory"></param>
        /// <param name="guidGenerator"></param>
        /// <param name="fileImport"></param>
        /// <param name="dataFilter"></param>
        public StdBasicQuotaCategoryAppService(IRepository<QuotaCategory, Guid> repositoryQuotaCategory,
            IRepository<Quota, Guid> repositoryQuota,
            IRepository<Area> repositoryArea,
            IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
            IUnitOfWorkManager unitOfWork,
            IRepository<DataDictionary, Guid> repositoryDataDictionary,
        IDataFilter dataFilter
        )
        {
            _repositoryQuotaCategory = repositoryQuotaCategory;
            _repositoryQuota = repositoryQuota;
            _repositoryArea = repositoryArea;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWork;
            _repositoryDataDictionary = repositoryDataDictionary;
        }

        [Authorize(StdBasicPermissions.QuotaCategory.Create)]
        public async Task<QuotaCategoryDto> Create(QuotaCategoryCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            if (input.StandardCodeId == null) throw new UserFriendlyException("定额分类标准编号不能为空");
            bool isSame = CheckSameCode(input.Code, null, input.AreaId, input.SpecialtyId, input.StandardCodeId);
            if (isSame)
            {
                throw new UserFriendlyException("定额分类编码重复");
            }
            var quotaCategory = new QuotaCategory(_guidGenerator.Create());
            quotaCategory.Name = input.Name;
            quotaCategory.ParentId = input.ParentId;
            quotaCategory.Code = input.Code;
            quotaCategory.SpecialtyId = input.SpecialtyId;
            quotaCategory.StandardCodeId = input.StandardCodeId;
            quotaCategory.Content = input.Content;
            quotaCategory.AreaId = input.AreaId;
            await _repositoryQuotaCategory.InsertAsync(quotaCategory);

            return ObjectMapper.Map<QuotaCategory, QuotaCategoryDto>(quotaCategory);
        }

        [Authorize(StdBasicPermissions.QuotaCategory.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var quotaCategory = _repositoryQuotaCategory.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (string.IsNullOrEmpty(quotaCategory.Name)) throw new UserFriendlyException("该定额分类不存在");
            //var children = _repositoryQuotaCategory.WithDetails().Where(x => x.ParentId != null && x.ParentId == id).ToList();
            if (quotaCategory.Children.Count() > 0) throw new UserFriendlyException("请先删除该定额分类的下级分类");
            var list = _repositoryQuota.Where(x => x.QuotaCategoryId == id).ToList();
            if (list?.Count > 0) throw new UserFriendlyException("此定额分类已关联定额，无法删除");
            await _repositoryQuotaCategory.DeleteAsync(id);

            return true;
        }

        [Authorize(StdBasicPermissions.QuotaCategory.Export)]
        public async Task<Stream> Export(QuotaCategoryData input)
        {
            List<QuotaCategory> list;
            if (input.Paramter.KeyWords != "" || input.Paramter.ParentId != null)
            {
                list = GetQuotaCategoryData(input.Paramter).AsEnumerable().OrderBy(y => y.Code).ToList();
            }
            else
            {
                list = _repositoryQuotaCategory.WithDetails().Where(x => x.Id != Guid.Empty).OrderBy(y => y.Code).ToList();
            }
            List<QuotaCategoryTemplate> resList = new List<QuotaCategoryTemplate>();
            if (list?.Count > 0)
            {
                list.ForEach(m =>
                {
                    var template = ObjectMapper.Map<QuotaCategory, QuotaCategoryTemplate>(m);
                    if (m.Area != null)
                    {

                        List<string> nameList = new List<string>();
                        nameList.Add(m.Area.FullName);
                        if (m.Area.ParentId.HasValue && m.Area.ParentId.Value >= 0)
                        {
                            FindAllAreaParents(m.Area.ParentId.Value, ref nameList);

                        }
                        if (nameList?.Count > 0)
                        {
                            for (int i = nameList.Count-1; i >=0; i--)
                            {
                                if (i == nameList.Count - 1)
                                {
                                    template.AreaName = nameList[i];
                                }
                                else
                                {
                                    template.AreaName += "_" + nameList[i];
                                }
                            }


                        }

                    }
                    if (m.StandardCode != null)
                    {
                        template.StandardCode = m.StandardCode.Name;
                    }
                    if (m.Specialty != null)
                    {
                        template.SpecialtyName = m.Specialty.Name;
                    }
                    resList.Add(template);
                });
            }
            var dtoList = resList;
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        [Authorize(StdBasicPermissions.QuotaCategory.Detail)]
        public async Task<QuotaCategoryDto> Get(Guid id)
        {
            QuotaCategoryDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryQuotaCategory.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此定额分类不存在");
                result = ObjectMapper.Map<QuotaCategory, QuotaCategoryDto>(ent);
                if (ent.Area != null)
                {

                    List<string> nameList = new List<string>();
                    nameList.Add(ent.Area.FullName);
                    if (ent.Area.ParentId.HasValue && ent.Area.ParentId.Value >= 0)
                    {
                        FindAllAreaParents(ent.Area.ParentId.Value, ref nameList);

                    }
                    if (nameList?.Count > 0)
                    {
                        for (int i = nameList.Count - 1; i >= 0; i--)
                        {
                            if (i == nameList.Count - 1)
                            {
                                result.AreaName = nameList[i];
                            }
                            else
                            {
                                result.AreaName += "_" + nameList[i];
                            }
                        }


                    }

                }
                if (ent.Specialty != null)
                {

                    result.SpecialtyName = ent.Specialty.Name;

                }
                if (ent.StandardCode != null)
                {
                    result.StandardCodeName = ent.StandardCode.Name;
                }

            });
            return result;
        }

        public Task<PagedResultDto<QuotaCategoryDto>> GetList(QuotaCategoryGetListByIdsDto input)
        {
            var result = new PagedResultDto<QuotaCategoryDto>();

            var productCategories = new List<QuotaCategory>();

            var dtos = new List<QuotaCategoryDto>();

            if (input.Ids != null && input.Ids.Count > 0)
            {

                foreach (var id in input.Ids)
                {
                    var componentCategory = _repositoryQuotaCategory.WithDetails().FirstOrDefault(x => x.Id == id);
                    if (componentCategory == null) continue;
                    productCategories.Add(componentCategory);
                    if (componentCategory.ParentId.HasValue && componentCategory.ParentId != Guid.Empty)
                    {
                        FindAllParents(componentCategory.ParentId.Value, ref productCategories);

                    }

                    var parentIds = productCategories.Select(x => x.Id).ToList();
                    var filterList = _repositoryQuotaCategory.WithDetails()
                        .Where(x => parentIds.Contains(x.ParentId.Value) || x.ParentId == null)
                        .ToList();
                    productCategories.AddRange(filterList);

                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<QuotaCategory>, List<QuotaCategoryDto>>(productCategories.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    //if (item.ParentId == null || item.ParentId == Guid.Empty)
                    //{
                    //    item.Name = item.Name + "[" + item.StandardCodeName + item.AreaName + item.SpecialtyName + "]";
                    //}
                    if (item.Children == null || item.Children.Count < 1)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<QuotaCategoryDto>();
                    }
                }

                dtos = GuidKeyTreeHelper<QuotaCategoryDto>.GetTree(listDtos);
            }
            else //按需加载
            {
                //根据关键字查询
                if (!string.IsNullOrEmpty(input.KeyWords))
                {
                    productCategories = _repositoryQuotaCategory.WithDetails()
                        .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                    dtos = ObjectMapper.Map<List<QuotaCategory>, List<QuotaCategoryDto>>(productCategories);
                    foreach (var item in dtos)
                    {
                        item.Children = null;
                    }
                }

                else
                {
                    productCategories = _repositoryQuotaCategory.WithDetails()
                        .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                        .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                        .ToList();
                    dtos = ObjectMapper.Map<List<QuotaCategory>, List<QuotaCategoryDto>>(productCategories);
                    foreach (var item in dtos)
                    {
                        //if(item.ParentId==null||item.ParentId==Guid.Empty)
                        //{
                        //    item.Name = item.Name + "[" + item.StandardCodeName+item.AreaName + item.SpecialtyName +"]" ;
                        //}
                        if (item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            item.Children = new List<QuotaCategoryDto>();
                        }
                    }
                }


            }

            result.TotalCount = dtos.Count();
            result.Items = input.IsAll ? dtos.ToList()
                : dtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }

        public Task<PagedResultDto<QuotaCategoryTreeDto>> GetListTree(QuotaCategoryGetListByIdsDto input)
        {

            var result = new PagedResultDto<QuotaCategoryTreeDto>();
            if (input == null) return null;

            var quotaes = new List<Quota>();

            var dtos = new List<QuotaCategoryTreeDto>();
            var listDtos = new List<QuotaCategoryTreeDto>();
            var productCategories = new List<QuotaCategory>();

            //根据关键字查询
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                quotaes = _repositoryQuota.WithDetails()
                    .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).ToList();

            }

            else
            {
                quotaes = _repositoryQuota.WithDetails().Where(x => x.Id != null && x.Id != Guid.Empty).ToList();
            }
            if (quotaes != null && quotaes.Count > 0)
            {
                foreach (var item in quotaes)
                {
                    //定额本身数据
                    QuotaCategoryTreeDto quotaDto = new QuotaCategoryTreeDto();
                    quotaDto.Id = item.Id;
                    quotaDto.Code = item.Code;
                    quotaDto.Name = item.Name;
                    quotaDto.ParentId = item.QuotaCategoryId;
                    quotaDto.Type = 2;
                    quotaDto.Unit = item.Unit;
                    quotaDto.Weight = (float)item.Weight;
                    quotaDto.Children = null;
                    //定额分类数据
                    var componentCategory = _repositoryQuotaCategory.FirstOrDefault(x => x.Id == item.QuotaCategoryId);
                    if (componentCategory == null) continue;
                    quotaDto.QuotaCategoryName = componentCategory.Name;
                    quotaDto.Content = componentCategory.Content;
                    listDtos.Add(quotaDto);
                    productCategories.Add(componentCategory);
                    if (componentCategory.ParentId.HasValue && componentCategory.ParentId != Guid.Empty)
                    {
                        FindAllParents(componentCategory.ParentId.Value, ref productCategories);
                    }
                    //var parentIds = productCategories.Select(x => x.Id).ToList();
                    //var filterList = _repositoryQuotaCategory.WithDetails()
                    //    .Where(x => parentIds.Contains(x.ParentId.Value))
                    //    .ToList();
                    //productCategories.AddRange(filterList);
                }
                productCategories = productCategories?.Distinct().ToList();
                foreach (var category in productCategories)
                {
                    QuotaCategoryTreeDto dto = new QuotaCategoryTreeDto();
                    dto.Id = category.Id;
                    dto.Code = category.Code;
                    dto.Name = category.Name;
                    dto.ParentId = category.ParentId;
                    dto.Type = 1;
                    dto.Children = new List<QuotaCategoryTreeDto>();

                    listDtos.Add(dto);
                }
            }
            if (input.QuotaIds?.Count > 0)
            {
                var quotaList = _repositoryQuota.WithDetails().Where(x => input.QuotaIds.Contains(x.Id)).ToList();
                if (quotaList?.Count > 0)
                {
                    foreach (var item in quotaList)
                    {
                        //定额本身数据
                        QuotaCategoryTreeDto quotaDto = new QuotaCategoryTreeDto();
                        quotaDto.Id = item.Id;
                        quotaDto.Code = item.Code;
                        quotaDto.Name = item.Name;
                        //quotaDto.ParentId = item.QuotaCategoryId;
                        quotaDto.Type = 2;
                        quotaDto.Unit = item.Unit;
                        quotaDto.Weight = (float)item.Weight;
                        quotaDto.Children = null;
                        //定额分类数据
                        var componentCategory = productCategories.Find(x => x.Id == item.QuotaCategoryId);
                        if (componentCategory == null) continue;
                        quotaDto.QuotaCategoryName = componentCategory.Name;
                        quotaDto.Content = componentCategory.Content;
                        listDtos.Add(quotaDto);
                    }
                }
            }
            dtos = GuidKeyTreeHelper<QuotaCategoryTreeDto>.GetTree(listDtos);
            //}
            result.TotalCount = dtos.Count();
            result.Items = input.IsAll ? dtos.ToList()
                : dtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }

        public Task<QuotaCategoryDto> GetListCode(Guid? id)
        {
            var list = new List<QuotaCategory>();
            if (id != null && id != Guid.Empty)
            {
                list = _repositoryQuotaCategory.Where(x => x.ParentId == id).OrderBy(x => x.Code).ToList();
            }
            else
            {
                list = _repositoryQuotaCategory.Where(x => x.ParentId == null).OrderBy(x => x.Code).ToList();
            }
            var dtos = Task.FromResult(ObjectMapper.Map<QuotaCategory, QuotaCategoryDto>(list.Count > 0 ? list.Last() : null));
            return dtos;
        }

        [Authorize(StdBasicPermissions.QuotaCategory.Update)]
        public async Task<QuotaCategoryDto> Update(QuotaCategoryUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入定额分类的id");
            var quotaCategory = await _repositoryQuotaCategory.GetAsync(input.Id);
            if (quotaCategory == null) throw new UserFriendlyException("该定额分类不存在");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }

            if (input.StandardCodeId == null) throw new UserFriendlyException("定额分类标准编号不能为空");
            bool isSame = CheckSameCode(input.Code, input.Id, input.AreaId, input.SpecialtyId, input.StandardCodeId);
            if (isSame)
            {
                throw new UserFriendlyException("定额分类编码重复");
            }

            quotaCategory.Name = input.Name;
            quotaCategory.ParentId = input.ParentId;
            quotaCategory.Code = input.Code;
            quotaCategory.Content = input.Content;
            if (input.ParentId == null || input.ParentId == Guid.Empty)
            {
                List<QuotaCategory> catelist = new List<QuotaCategory>();
                if (quotaCategory.SpecialtyId != input.SpecialtyId || quotaCategory.StandardCodeId != input.StandardCodeId || quotaCategory.AreaId != input.AreaId)
                {
                    quotaCategory.SpecialtyId = input.SpecialtyId;
                    quotaCategory.StandardCodeId = input.StandardCodeId;
                    quotaCategory.AreaId = input.AreaId;
                    ChangeChildrens(quotaCategory, ref catelist);
                    if (catelist?.Count > 0)
                    {
                        foreach (var item in catelist)
                        {
                            await _repositoryQuotaCategory.UpdateAsync(item);
                        }

                    }

                }
            }

            quotaCategory.SpecialtyId = input.SpecialtyId;
            quotaCategory.StandardCodeId = input.StandardCodeId;
            quotaCategory.AreaId = input.AreaId;
            await _repositoryQuotaCategory.UpdateAsync(quotaCategory);

            return ObjectMapper.Map<QuotaCategory, QuotaCategoryDto>(quotaCategory);
        }

        [Authorize(StdBasicPermissions.QuotaCategory.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            //QuotaCategoryTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<QuotaCategoryTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<QuotaCategoryTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<QuotaCategoryTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            QuotaCategory quotaCategoryModel = null;
            DataDictionary dataDictionaryModel = null;
            DataDictionary stdCodeModel = null;
            Area areaModel = null;
            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());
                var groupList = datalist.GroupBy(a => a.Code.Split(".").Length).ToList();
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
                        if (item.SpecialtyName.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"专业为空");
                        }
                        if (item.Name.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"名称为空");
                        }
                        if (item.StandardCode.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"标准标号为空");
                        }
                        if (item.AreaName.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"行政区域为空");
                        }
                        if (!canInsert)
                        {
                            wrongInfos.Add(newInfo);
                            continue;
                        }
                        if (item.Code.Split(".")[0] != "DE")
                        {
                            newInfo.AppendInfo("编码格式有误，应以DE开头");
                            wrongInfos.Add(newInfo);
                        }
                        using var uow = _unitOfWork.Begin(true, false);
                        using (_dataFilter.Disable<ISoftDelete>())
                        {

                            //quotaCategoryModel = _repositoryQuotaCategory.FirstOrDefault(a => a.Name == item.Name && a.Code == item.Code);
                            dataDictionaryModel = _repositoryDataDictionary.FirstOrDefault(a => a.Name == item.SpecialtyName);
                            stdCodeModel = _repositoryDataDictionary.FirstOrDefault(a => a.Name == item.StandardCode);
                            var areas = item.AreaName.Split('_').ToList();
                            if (areas.Count > 0)
                            {
                                if (areas.Count == 1)
                                {
                                    areaModel = _repositoryArea.FirstOrDefault(a => a.FullName == item.AreaName);
                                }
                                else
                                {
                                    var areaParentModel = _repositoryArea.FirstOrDefault(a => a.FullName == areas[0]);
                                    areaModel = _repositoryArea.FirstOrDefault(a => a.FullName == areas[1] && a.ParentId == areaParentModel.Id);
                                }
                            }
                            if (dataDictionaryModel != null && areaModel != null && stdCodeModel != null)
                            {
                                quotaCategoryModel = _repositoryQuotaCategory.WithDetails().FirstOrDefault(a => a.Name == item.Name && a.Code == item.Code && a.StandardCodeId == stdCodeModel.Id && a.SpecialtyId == dataDictionaryModel.Id && a.AreaId == areaModel.Id);
                            }

                        }
                        if (quotaCategoryModel != null)
                        {
                            newInfo.AppendInfo($"{item.Name}已存在，且已更新");
                            //修改自己本身数据
                            quotaCategoryModel.Name = item.Name;
                            quotaCategoryModel.Content = item.Content;

                            if (item.ParentCode != null && !string.IsNullOrEmpty(item.ParentCode))
                            {
                                if (quotaCategoryModel.Parent != null)
                                {
                                    if (quotaCategoryModel.Parent.Code != item.ParentCode)//修改父级
                                    {
                                        var parent = _repositoryQuotaCategory.FirstOrDefault(a => a.Code == item.Code && a.StandardCodeId == quotaCategoryModel.SpecialtyId && a.AreaId == quotaCategoryModel.AreaId && a.SpecialtyId == quotaCategoryModel.SpecialtyId);
                                        if (parent != null)
                                        {
                                            quotaCategoryModel.ParentId = parent.Id;
                                        }
                                        else
                                        {
                                            var parentModel = ite.ToList().Find(x => x.Code == item.ParentCode && x.StandardCode == item.StandardCode && x.SpecialtyName == item.SpecialtyName && x.AreaName == item.AreaName);
                                            if (parentModel != null)
                                            {
                                                var parentEnt = new QuotaCategory(_guidGenerator.Create());
                                                parentEnt.Name = parentModel.Name;
                                                parentEnt.Code = parentModel.Code;
                                                parentEnt.Content = parentModel.Content;
                                                parentEnt.StandardCodeId = quotaCategoryModel.StandardCodeId;
                                                parentEnt.SpecialtyId = quotaCategoryModel.SpecialtyId;
                                                parentEnt.AreaId = quotaCategoryModel.AreaId;
                                                await _repositoryQuotaCategory.InsertAsync(parentEnt);
                                                quotaCategoryModel.ParentId = parentEnt.Id;
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    var parent = _repositoryQuotaCategory.FirstOrDefault(a => a.Code == item.Code && a.StandardCodeId == quotaCategoryModel.SpecialtyId && a.AreaId == quotaCategoryModel.AreaId && a.SpecialtyId == quotaCategoryModel.SpecialtyId);
                                    if (parent != null)
                                    {
                                        quotaCategoryModel.ParentId = parent.Id;
                                    }
                                    else
                                    {
                                        var parentModel = ite.ToList().Find(x => x.Code == item.ParentCode && x.StandardCode == item.StandardCode && x.SpecialtyName == item.SpecialtyName && x.AreaName == item.AreaName);
                                        if (parentModel != null)
                                        {
                                            var parentEnt = new QuotaCategory(_guidGenerator.Create());
                                            parentEnt.Name = parentModel.Name;
                                            parentEnt.Code = parentModel.Code;
                                            parentEnt.Content = parentModel.Content;
                                            parentEnt.StandardCodeId = quotaCategoryModel.StandardCodeId;
                                            parentEnt.SpecialtyId = quotaCategoryModel.SpecialtyId;
                                            parentEnt.AreaId = quotaCategoryModel.AreaId;
                                            await _repositoryQuotaCategory.InsertAsync(parentEnt);
                                            quotaCategoryModel.ParentId = parentEnt.Id;
                                        }
                                    }
                                }



                            }


                            await _repositoryQuotaCategory.UpdateAsync(quotaCategoryModel);
                            await uow.SaveChangesAsync();
                            wrongInfos.Add(newInfo);
                        }
                        else
                        {
                            if (areaModel != null && dataDictionaryModel != null && stdCodeModel != null)
                            {
                                quotaCategoryModel = new QuotaCategory(_guidGenerator.Create());
                                quotaCategoryModel.Name = item.Name;
                                quotaCategoryModel.Code = item.Code;
                                quotaCategoryModel.SpecialtyId = dataDictionaryModel.Id;
                                quotaCategoryModel.Content = item.Content;
                                quotaCategoryModel.StandardCodeId = stdCodeModel.Id;
                                quotaCategoryModel.AreaId = areaModel.Id;
                                if (item.ParentCode != null && !string.IsNullOrEmpty(item.ParentCode))
                                {
                                    var parent = _repositoryQuotaCategory.FirstOrDefault(a => a.Code == item.ParentCode && a.StandardCodeId == stdCodeModel.Id && a.AreaId == areaModel.Id && a.SpecialtyId == dataDictionaryModel.Id);
                                    if (parent == null)
                                    {
                                        var parentModel = ite.ToList().Find(x => x.Code == item.ParentCode && x.StandardCode == item.StandardCode && x.SpecialtyName == item.SpecialtyName && x.AreaName == item.AreaName);
                                        if (parentModel != null)
                                        {
                                            var parentEnt = new QuotaCategory(_guidGenerator.Create());
                                            parentEnt.Name = parentModel.Name;
                                            parentEnt.Code = parentModel.Code;
                                            parentEnt.Content = parentModel.Content;
                                            parentEnt.StandardCode = quotaCategoryModel.StandardCode;
                                            parentEnt.SpecialtyId = quotaCategoryModel.SpecialtyId;
                                            parentEnt.AreaId = quotaCategoryModel.AreaId;
                                            await _repositoryQuotaCategory.InsertAsync(parentEnt);
                                            quotaCategoryModel.ParentId = parentEnt.Id;
                                        }
                                    }
                                    else
                                    {
                                        quotaCategoryModel.ParentId = parent.Id;
                                    }
                                }
                            }
                            await _repositoryQuotaCategory.InsertAsync(quotaCategoryModel);
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

        private List<QuotaCategory> GetQuotaCategoryData(QuotaCategoryGetListByIdsDto input)
        {
            var quotaCategorys = new List<QuotaCategory>();
            var dtos = new List<QuotaCategoryDto>();

            //根据关键字查询
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                quotaCategorys = _repositoryQuotaCategory.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                dtos = ObjectMapper.Map<List<QuotaCategory>, List<QuotaCategoryDto>>(quotaCategorys);
                foreach (var item in dtos)
                {
                    item.Children = null;
                }
            }
            else
            {
                quotaCategorys = _repositoryQuotaCategory.WithDetails()
                               .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                               .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                               .ToList();
                dtos = ObjectMapper.Map<List<QuotaCategory>, List<QuotaCategoryDto>>(quotaCategorys);
                foreach (var item in dtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<QuotaCategoryDto>();
                    }
                }
            }

            var resource = ObjectMapper.Map<List<QuotaCategoryDto>, List<QuotaCategory>>(dtos);
            return resource;
        }

        private void FindAllParents(Guid parentid, ref List<QuotaCategory> list)
        {
            var parentModel = _repositoryQuotaCategory.WithDetails().FirstOrDefault(a => a.Id == parentid);
            if (parentModel != null)
            {
                list.Add(parentModel);
                if (parentModel.ParentId != null && parentModel.ParentId != Guid.Empty)
                {
                    FindAllParents(parentModel.ParentId.Value, ref list);
                }
                //else
                //{
                //    parentModel.Name = parentModel.Name + "[" + parentModel.StandardCode?.Name + parentModel.Area?.Name + parentModel.Specialty?.Name + "]";
                //}

            }

        }

        private void FindAllAreaParents(int parentid, ref List<string> list)
        {
            var parentModel = _repositoryArea.FirstOrDefault(a => a.Id == parentid);
            if (parentModel != null)
            {
                list.Add(parentModel.FullName);
                if (parentModel.ParentId != null && parentModel.ParentId >= 0)
                {
                    FindAllAreaParents(parentModel.ParentId.Value, ref list);
                }
            }

        }

        /// <summary>
        /// 查看是否有重复编码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="areaId"></param>
        /// <param name="specialtyId"></param>
        /// <param name="standardCodeId"></param>
        /// <returns></returns>
        private bool CheckSameCode(string code, Guid? id, int areaId, Guid specialtyId, Guid standardCodeId)
        {
            bool isSame = true;
            var productCategory = _repositoryQuotaCategory.WithDetails().Where(x => x.Code == code &&
            x.SpecialtyId == specialtyId && x.StandardCodeId == standardCodeId && x.AreaId == areaId).ToList();
            if (id.HasValue)
            {
                productCategory = productCategory.FindAll(x => x.Id != id.Value);
            }
            if (productCategory == null || productCategory.Count == 0)
            {
                isSame = false;
            }
            return isSame;
        }

        private void ChangeChildrens(QuotaCategory input, ref List<QuotaCategory> catelist)
        {
            if (catelist == null) catelist = new List<QuotaCategory>();
            var childrens = _repositoryQuotaCategory.WithDetails().Where(x => x.ParentId == input.Id).ToList();
            if (childrens?.Count > 0)
            {

                foreach (var item in childrens)
                {
                    item.SpecialtyId = input.SpecialtyId;
                    item.StandardCodeId = input.StandardCodeId;
                    item.AreaId = input.AreaId;
                    catelist.Add(item);

                    if (item.Children.Count > 0)
                    {
                        ChangeChildrens(item, ref catelist);
                    }
                }

            }

        }
    }
}
