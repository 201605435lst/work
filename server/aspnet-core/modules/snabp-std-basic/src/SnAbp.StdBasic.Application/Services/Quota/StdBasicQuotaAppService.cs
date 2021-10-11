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

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicQuotaAppService : StdBasicAppService, IStdBasicQuotaAppService
    {
        private readonly IRepository<QuotaCategory, Guid> _repositoryQuotaCategory;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<Quota, Guid> _repositoryQuota;
        private readonly IRepository<QuotaItem, Guid> _repositoryQuotaItem;
        private readonly IRepository<BasePrice, Guid> _repositoryBasePrice;
        private readonly IRepository<ComputerCode, Guid> _repositoryComputerCode;
        private readonly IRepository<DataDictionary, Guid> _repositoryDataDictionary;
        private readonly IRepository<Area> _repositoryArea;//区域
        /// <summary>
        /// 构造函数
        /// </summary>
        public StdBasicQuotaAppService(IRepository<QuotaCategory, Guid> repositoryQuotaCategory,
            IRepository<Quota, Guid> repositoryQuota,
            IRepository<QuotaItem, Guid> repositoryQuotaItem,
            IRepository<BasePrice, Guid> repositoryBasePrice,
            IRepository<ComputerCode, Guid> repositoryComputerCode,
            IRepository<DataDictionary, Guid> repositoryDataDictionary,
            IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
            IDataFilter dataFilter,
            IRepository<Area> repositoryArea
        )
        {
            _repositoryQuotaCategory = repositoryQuotaCategory;
            _repositoryQuota = repositoryQuota;
            _repositoryQuotaItem = repositoryQuotaItem;
            _repositoryBasePrice = repositoryBasePrice;
            _repositoryComputerCode = repositoryComputerCode;
            _repositoryDataDictionary = repositoryDataDictionary;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _repositoryArea = repositoryArea;
        }

        [Authorize(StdBasicPermissions.Quota.Create)]
        public async Task<QuotaDto> Create(QuotaCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("定额名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("单位不能为纯数字");
            }
            var quota = new Quota(_guidGenerator.Create());
            quota.Name = input.Name;

            quota.Code = input.Code;
            quota.LaborCost = Convert.ToDecimal(input.LaborCost >= 0 ? input.LaborCost : 0);
            quota.MachineCost = Convert.ToDecimal(input.MachineCost >= 0 ? input.MachineCost : 0);
            quota.MaterialCost = Convert.ToDecimal(input.MaterialCost >= 0 ? input.MaterialCost : 0);
            quota.QuotaCategoryId = input.QuotaCategoryId;
            quota.Remark = input.Remark;
            quota.Unit = input.Unit;
            quota.Weight = Convert.ToDecimal(input.Weight >= 0 ? input.Weight : 0);
            await _repositoryQuota.InsertAsync(quota);

            QuotaDto res = ObjectMapper.Map<Quota, QuotaDto>(quota);
            if (input.QuotaCategoryId != null && input.QuotaCategoryId != Guid.Empty)
            {
                var category = _repositoryQuotaCategory.WithDetails().FirstOrDefault(s => input.QuotaCategoryId == s.Id);
                if (category != null)
                {
                    res.QuotaCategoryName = category.Name;
                    res.AreaId = category.AreaId;
                    res.SpecialtyId = category.SpecialtyId;
                    res.StandardCodeId = category.StandardCodeId;
                    if (category.Area != null)
                    {

                        List<string> nameList = new List<string>();
                        nameList.Add(category.Area.FullName);
                        if (category.Area.ParentId.HasValue && category.Area.ParentId.Value >= 0)
                        {
                            FindAllAreaParents(category.Area.ParentId.Value, ref nameList);

                        }
                        if (nameList?.Count > 0)
                        {
                            for (int i = nameList.Count - 1; i >= 0; i--)
                            {
                                if (i == nameList.Count - 1)
                                {
                                    res.AreaName = nameList[i];
                                }
                                else
                                {
                                    res.AreaName += "_" + nameList[i];
                                }
                            }


                        }

                    }
                    if (category.Specialty != null)
                    {
                        res.SpecialtyName = category.Specialty.Name;
                    }
                    if (category.StandardCode != null)
                    {
                        res.StandardCodeName = category.StandardCode.Name;
                    }
                }
            }
            return res;
        }
        [Authorize(StdBasicPermissions.Quota.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryQuota.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此定额不存在");

            await _repositoryQuota.DeleteAsync(id);
            return true;
        }
        [Authorize(StdBasicPermissions.Quota.Export)]
        public async Task<Stream> Export(QuotaData input)
        {
            var list = GetQuotaData(input.Paramter);
            List<QuotaTemplate> dtoList = new List<QuotaTemplate>();
            if (list?.Count > 0)
            {
                var categoryIds = list.ConvertAll(x => x.QuotaCategoryId);
                var categoryList = _repositoryQuotaCategory.WithDetails().Where(x => categoryIds.Contains(x.Id)).ToList();

                foreach (var item in list)
                {
                    var category = categoryList.Find(x => x.Id == item.QuotaCategoryId);
                    QuotaTemplate model = new QuotaTemplate();
                    if (category.Area != null)
                    {

                        List<string> nameList = new List<string>();
                        nameList.Add(category.Area.FullName);
                        if (category.Area.ParentId.HasValue && category.Area.ParentId.Value >= 0)
                        {
                            FindAllAreaParents(category.Area.ParentId.Value, ref nameList);

                        }
                        if (nameList?.Count > 0)
                        {
                            for (int i = nameList.Count - 1; i >= 0; i--)
                            {
                                if (i == nameList.Count - 1)
                                {
                                    model.AreaName = nameList[i];
                                }
                                else
                                {
                                    model.AreaName += "_" + nameList[i];
                                }
                            }


                        }

                    }
                    model.Code = item.Code;
                    model.LaborCost = (float)item.LaborCost;
                    model.MachineCost = (float)item.MachineCost;
                    model.MaterialCost = (float)item.MaterialCost;
                    model.Name = item.Name;
                    model.QuotaCategoryName = category.Name;
                    model.Remark = item.Remark;
                    model.SpecialtyName = category.Specialty?.Name;
                    model.StandardCode = category.StandardCode?.Name;
                    model.Unit = item.Unit;
                    model.Weight = (float)item.Weight;
                    dtoList.Add(model);
                }

            }
            //var dtoList = ObjectMapper.Map<List<Quota>, List<QuotaTemplate>>(list);
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }
        [Authorize(StdBasicPermissions.Quota.Detail)]
        public async Task<QuotaDto> Get(Guid id)
        {
            QuotaDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryQuota.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此定额不存在");
                result = ObjectMapper.Map<Quota, QuotaDto>(ent);
                if (ent.QuotaCategoryId != null && ent.QuotaCategoryId != Guid.Empty)
                {
                    var category = _repositoryQuotaCategory.WithDetails().FirstOrDefault(x => x.Id == ent.QuotaCategoryId);
                    if (category != null)
                    {
                        result.QuotaCategoryName = category.Name;
                        result.AreaId = category.AreaId;
                        result.SpecialtyId = category.SpecialtyId;
                        result.StandardCodeId = category.StandardCodeId;
                        if (category.Area != null)
                        {

                            List<string> nameList = new List<string>();
                            nameList.Add(category.Area.FullName);
                            if (category.Area.ParentId.HasValue && category.Area.ParentId.Value >= 0)
                            {
                                FindAllAreaParents(category.Area.ParentId.Value, ref nameList);

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
                        if (category.Specialty != null)
                        {
                            result.SpecialtyName = category.Specialty.Name;
                        }
                        if (category.StandardCode != null)
                        {
                            result.StandardCodeName = category.StandardCode.Name;
                        }
                    }
                    //string categoryName = string.Empty;
                    //List<QuotaCategory> list = new List<QuotaCategory>();
                    //FindAllParents(ent.QuotaCategoryId, ref list);
                    //if (list?.Count > 0)
                    //{

                    //    int i = 0;
                    //    list.ForEach(x =>
                    //    {
                    //        if (i == 0)
                    //        {
                    //            categoryName = x.Name;
                    //        }
                    //        else
                    //        {
                    //            categoryName += "/" + x.Name;
                    //        }

                    //    });
                    //}


                }
            });
            return result;
        }

        public async Task<PagedResultDto<QuotaDto>> GetList(QuotaGetListDto input)
        {
            PagedResultDto<QuotaDto> res = new PagedResultDto<QuotaDto>();
            var allEnt = _repositoryQuota.WithDetails()
                        .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Code.Contains(input.Keyword) || x.Name.Contains(input.Keyword)).ToList();
            if (allEnt.Count() > 0)
            {
                res.TotalCount = allEnt.Count();
                List<QuotaDto> quotaList = new List<QuotaDto>();
                if (input.IsAll == false)
                {

                    quotaList = ObjectMapper.Map<List<Quota>, List<QuotaDto>>(allEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                }
                else
                {
                    quotaList = ObjectMapper.Map<List<Quota>, List<QuotaDto>>(allEnt.ToList());
                    //res.TotalCount = res.Items.Count;
                }
                if (quotaList?.Count > 0)
                {
                    //获取清单

                    var ids = quotaList.ConvertAll(x => x.Id);
                    var quotaItemList = _repositoryQuotaItem.WithDetails().Where(x => ids.Contains(x.QuotaId)).ToList();
                    var basePriceList = new List<BasePrice>();
                    if (quotaItemList?.Count > 0)
                    {

                        var basePriceIds = quotaItemList.ConvertAll(x => x.BasePriceId);
                        basePriceList = _repositoryBasePrice.WithDetails().Where(x => basePriceIds.Contains(x.Id)).ToList();
                    }
                    var categoryIds = quotaList.ConvertAll(x => x.QuotaCategoryId);
                    var categoryList = _repositoryQuotaCategory.WithDetails().Where(x => categoryIds.Contains(x.Id)).ToList();
                    //获取定额分类
                    quotaList.ForEach(m =>
                {
                    if (m.QuotaCategoryId != null)
                    {
                        var category = categoryList.Find(x => x.Id == m.QuotaCategoryId);
                        if (category != null)
                        {
                            m.QuotaCategoryName = category.Name;
                            m.AreaId = category.AreaId;
                            m.SpecialtyId = category.SpecialtyId;
                            m.StandardCodeId = category.StandardCodeId;
                            if (category.Area != null)
                            {

                                List<string> nameList = new List<string>();
                                nameList.Add(category.Area.FullName);
                                if (category.Area.ParentId.HasValue && category.Area.ParentId.Value >= 0)
                                {
                                    FindAllAreaParents(category.Area.ParentId.Value, ref nameList);

                                }
                                if (nameList?.Count > 0)
                                {
                                    for (int i = nameList.Count - 1; i >= 0; i--)
                                    {
                                        if (i == nameList.Count - 1)
                                        {
                                            m.AreaName = nameList[i];
                                        }
                                        else
                                        {
                                            m.AreaName += "_" + nameList[i];
                                        }
                                    }


                                }

                            }
                            if (category.Specialty != null)
                            {
                                m.SpecialtyName = category.Specialty.Name;
                            }
                            if (category.StandardCode != null)
                            {
                                m.StandardCodeName = category.StandardCode.Name;
                            }
                        }
                        //string categoryName = string.Empty;
                        //List<QuotaCategory> list = new List<QuotaCategory>();
                        //FindAllParents(m.QuotaCategoryId, ref list);
                        //if (list?.Count > 0)
                        //{

                        //    int i = 0;
                        //    list.ForEach(x =>
                        //    {
                        //        if (i == 0)
                        //        {
                        //            categoryName = x.Name;
                        //        }
                        //        else
                        //        {
                        //            categoryName += "/" + x.Name;
                        //        }

                        //    });
                        //}
                        //m.QuotaCategoryName = categoryName;
                    }

                    m.QuotaItems = new List<QuotaItemDto>();
                    var itemList = quotaItemList.FindAll(x => x.QuotaId == m.Id);
                    if (itemList?.Count > 0)
                    {
                        itemList.ForEach(q =>
                        {
                            var bfmodel = m.QuotaItems.Find(x => x.ComputerCodeId == q.BasePrice.ComputerCodeId && x.QuotaId == q.QuotaId);
                            if (bfmodel == null)
                            {
                                var bfItemList = itemList.FindAll(x => x.BasePrice.ComputerCodeId == q.BasePrice.ComputerCodeId);
                                var ids = bfItemList.ConvertAll(x => x.BasePriceId);
                                bfmodel = new QuotaItemDto();
                                var basePrices = basePriceList.FindAll(x => ids.Contains(x.Id) && x.ComputerCodeId == q.BasePrice.ComputerCodeId).ToList();
                                bfmodel.ComputerCodeId = q.BasePrice.ComputerCodeId;
                                bfmodel.QuotaId = q.QuotaId;
                                bfmodel.Number = (float)bfItemList[0].Number;
                                bfmodel.Remark = bfItemList[0].Remark;
                                bfmodel.ComputerCodeName = basePrices[0].ComputerCode?.Name;
                                bfmodel.ComputerCode = basePrices[0].ComputerCode?.Code;
                                bfmodel.QuotaItemEditList = new List<QuotaItemEditDto>();
                                bfItemList.ForEach(p =>
                                {
                                    var editDto = ObjectMapper.Map<QuotaItem, QuotaItemEditDto>(p);
                                    var basePrice = basePriceList.Find(x => x.Id == p.BasePriceId);
                                    if (basePrice != null)
                                    {
                                        editDto.AreaId = basePrice.AreaId;
                                        if (basePrice.Area != null)
                                        {

                                            List<string> nameList = new List<string>();
                                            nameList.Add(basePrice.Area.FullName);
                                            if (basePrice.Area.ParentId.HasValue && basePrice.Area.ParentId.Value >= 0)
                                            {
                                                FindAllAreaParents(basePrice.Area.ParentId.Value, ref nameList);

                                            }
                                            if (nameList?.Count > 0)
                                            {
                                                for (int i = nameList.Count - 1; i >= 0; i--)
                                                {
                                                    if (i == nameList.Count - 1)
                                                    {
                                                        editDto.AreaName = nameList[i];
                                                    }
                                                    else
                                                    {
                                                        editDto.AreaName += "_" + nameList[i];
                                                    }
                                                }


                                            }

                                        }
                                        editDto.StandardCodeId = basePrice.StandardCodeId;
                                        editDto.StandardCodeName = basePrice.StandardCode?.Name;
                                        editDto.Price = (float)basePrice.Price;
                                    }
                                    bfmodel.QuotaItemEditList.Add(editDto);
                                });
                                m.QuotaItems.Add(bfmodel);
                            }
                            else
                            {
                                var before = bfmodel.QuotaItemEditList.Find(x => x.BasePriceId == q.BasePriceId);
                                if (before == null)
                                {
                                    var editDto = ObjectMapper.Map<QuotaItem, QuotaItemEditDto>(q);
                                    var basePrice = basePriceList.Find(x => x.Id == q.BasePriceId);
                                    if (basePrice != null)
                                    {
                                        editDto.AreaId = basePrice.AreaId;
                                        if (basePrice.Area != null)
                                        {

                                            List<string> nameList = new List<string>();
                                            nameList.Add(basePrice.Area.FullName);
                                            if (basePrice.Area.ParentId.HasValue && basePrice.Area.ParentId.Value >= 0)
                                            {
                                                FindAllAreaParents(basePrice.Area.ParentId.Value, ref nameList);

                                            }
                                            if (nameList?.Count > 0)
                                            {
                                                for (int i = nameList.Count - 1; i >= 0; i--)
                                                {
                                                    if (i == nameList.Count - 1)
                                                    {
                                                        editDto.AreaName = nameList[i];
                                                    }
                                                    else
                                                    {
                                                        editDto.AreaName += "_" + nameList[i];
                                                    }
                                                }


                                            }

                                        }
                                        editDto.StandardCodeId = basePrice.StandardCodeId;
                                        editDto.StandardCodeName = basePrice.StandardCode?.Name;
                                        editDto.Price = (float)basePrice.Price;
                                    }
                                    bfmodel.QuotaItemEditList.Add(editDto);
                                }
                            }

                        });

                    }

                });
                }
                res.Items = quotaList;
            }
            return res;
        }
        [Authorize(StdBasicPermissions.Quota.Update)]
        public async Task<QuotaDto> Update(QuotaUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("定额名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("单位不能为纯数字");
            }
            var ent = _repositoryQuota.FirstOrDefault(s => input.Id == s.Id);
            if (ent == null) throw new UserFriendlyException("此定额不存在");
            ent.Name = input.Name;
            ent.Code = input.Code;
            ent.LaborCost = Convert.ToDecimal(input.LaborCost >= 0 ? input.LaborCost : 0);
            ent.MachineCost = Convert.ToDecimal(input.MachineCost >= 0 ? input.MachineCost : 0);
            ent.MaterialCost = Convert.ToDecimal(input.MaterialCost >= 0 ? input.MaterialCost : 0);
            ent.QuotaCategoryId = input.QuotaCategoryId;
            ent.Remark = input.Remark;
            ent.Unit = input.Unit;
            ent.Weight = Convert.ToDecimal(input.Weight >= 0 ? input.Weight : 0);
            ent.Remark = input.Remark;

            var resEnt = await _repositoryQuota.UpdateAsync(ent);
            return ObjectMapper.Map<Quota, QuotaDto>(resEnt);
        }

        /// <summary>
        /// 导入未实现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.Quota.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            // 虚拟进度0 %
            await _fileImport.Start(input.ImportKey, 100);
            //QuotaTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<QuotaTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<QuotaTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<QuotaTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            Quota quotaModel = null;
            QuotaCategory quotaCategoryModel = null;
            DataDictionary dataDictionaryModel = null;
            DataDictionary stdCodeModel = null;
            Area areaModel = null;
            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());
                var updateIndex = 1;
                foreach (var item in datalist)
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;
                    var canInsert = true;
                    var newInfo = new WrongInfo(item.Index);
                    if (item.Name.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"定额名称为空");
                    }
                    if (item.Code.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"定额编号为空");
                    }
                    if (item.SpecialtyName.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"专业为空");
                    }
                    if (item.QuotaCategoryName.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"定额分类编码为空");
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
                    if (item.Unit.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"单位为空");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
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
                            quotaCategoryModel = _repositoryQuotaCategory.FirstOrDefault(a => a.Name == item.QuotaCategoryName && a.StandardCodeId == stdCodeModel.Id && a.SpecialtyId == dataDictionaryModel.Id && a.AreaId == areaModel.Id);
                        }
                        if (quotaCategoryModel != null)
                        {
                            quotaModel = _repositoryQuota.FirstOrDefault(a => a.Code == item.Code && a.Name == item.Name && a.QuotaCategoryId == quotaCategoryModel.Id);
                        }
                    }
                    if (quotaModel != null)
                    {
                        newInfo.AppendInfo($"{item.Code}已存在，且已更新");
                        //修改自己本身数据
                        quotaModel.Name = item.Name;
                        quotaModel.Remark = item.Remark;
                        quotaModel.Code = item.Code;
                        quotaModel.LaborCost = Convert.ToDecimal(item.LaborCost >= 0 ? item.LaborCost : 0);
                        quotaModel.MachineCost = Convert.ToDecimal(item.MachineCost >= 0 ? item.MachineCost : 0);
                        quotaModel.MaterialCost = Convert.ToDecimal(item.MaterialCost >= 0 ? item.MaterialCost : 0);
                        quotaModel.QuotaCategoryId = quotaCategoryModel.Id;
                        quotaModel.Unit = item.Unit;
                        quotaModel.Weight = Convert.ToDecimal(item.Weight >= 0 ? item.Weight : 0);
                        await _repositoryQuota.UpdateAsync(quotaModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        if (quotaCategoryModel != null)
                        {
                            quotaModel = new Quota(_guidGenerator.Create());
                            quotaModel.Name = item.Name;
                            quotaModel.Remark = item.Remark;
                            quotaModel.Code = item.Code;
                            quotaModel.LaborCost = Convert.ToDecimal(item.LaborCost >= 0 ? item.LaborCost : 0);
                            quotaModel.MachineCost = Convert.ToDecimal(item.MachineCost >= 0 ? item.MachineCost : 0);
                            quotaModel.MaterialCost = Convert.ToDecimal(item.MaterialCost >= 0 ? item.MaterialCost : 0);
                            quotaModel.QuotaCategoryId = quotaCategoryModel.Id;
                            quotaModel.Unit = item.Unit;
                            quotaModel.Weight = Convert.ToDecimal(item.Weight >= 0 ? item.Weight : 0);

                            await _repositoryQuota.InsertAsync(quotaModel);
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

        private List<Quota> GetQuotaData(QuotaGetListDto input)
        {
            var query = _repositoryQuota
                .WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Keyword),
                    s => s.Name.Contains(input.Keyword) ||

                        s.Code.Contains(input.Keyword)

                 );

            return query.ToList();
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
    }
}
