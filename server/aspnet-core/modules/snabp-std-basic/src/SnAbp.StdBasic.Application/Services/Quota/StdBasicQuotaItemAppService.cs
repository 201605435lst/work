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
    /// <summary>
    /// 定额清单
    /// </summary>
    [Authorize]
    public class StdBasicQuotaItemAppService : StdBasicAppService, IStdBasicQuotaItemAppService
    {
        private readonly IRepository<QuotaItem, Guid> _repositoryQuotaItem;
        private readonly IRepository<BasePrice, Guid> _repositoryBasePrice;
        private readonly IRepository<ComputerCode, Guid> _repositoryComputerCode;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<Area> _repositoryArea;//区域
        private readonly IRepository<DataDictionary, Guid> _repositoryDataDictionary;

        public StdBasicQuotaItemAppService(IRepository<QuotaItem, Guid> repositoryQuotaItem,
        IRepository<BasePrice, Guid> repositoryBasePrice,
        IRepository<ComputerCode, Guid> repositoryComputerCode,
        IGuidGenerator guidGenerator,
        IFileImportHandler fileImport,
        IDataFilter dataFilter,
        IRepository<DataDictionary, Guid> repositoryDataDictionary,
        IRepository<Area> repositoryArea//区域
            )
        {
            _repositoryQuotaItem = repositoryQuotaItem;
            _repositoryBasePrice = repositoryBasePrice;
            _repositoryComputerCode = repositoryComputerCode;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _repositoryArea = repositoryArea;//区域
            _repositoryDataDictionary = repositoryDataDictionary;
        }

        [Authorize(StdBasicPermissions.QuotaItem.Create)]
        public async Task<QuotaItemDto> Create(QuotaItemCreateDto input)
        {
            QuotaItemDto res = null;
            if (input.ComputerCodeId == null && input.ComputerCodeId == Guid.Empty) throw new UserFriendlyException("未选择电算代号");
            if (input.BasePriceIdList == null || input.BasePriceIdList.Count < 1) throw new UserFriendlyException("未选择基价");
            if (input.Number <= 0) throw new UserFriendlyException("数量不能小于等于0");
            await Task.Run(() =>
            {
                var bfItemList = _repositoryQuotaItem.WithDetails().Where(x => x.QuotaId == input.QuotaId && x.BasePrice != null && x.BasePrice.ComputerCodeId == input.ComputerCodeId).ToList();
                if (bfItemList?.Count > 0)
                {
                    bfItemList.ForEach(m =>
                    {
                        _repositoryQuotaItem.DeleteAsync(m.Id);
                    });
                }
                if (input.BasePriceIdList?.Count > 0)
                {
                    res = new QuotaItemDto();
                    res.ComputerCodeId = input.ComputerCodeId;
                    res.QuotaId = input.QuotaId;
                    res.Number = input.Number;
                    res.Remark = input.Remark;
                    res.QuotaItemEditList = new List<QuotaItemEditDto>();
                    var basePriceList = _repositoryBasePrice.WithDetails().Where(x => input.BasePriceIdList.Contains(x.Id)).ToList();
                    input.BasePriceIdList.ForEach(m =>
                    {
                        var before = res.QuotaItemEditList.Find(x => x.BasePriceId == m);
                        if (before == null)
                        {
                            var quotaItem = new QuotaItem(_guidGenerator.Create());
                            quotaItem.BasePriceId = m;

                            quotaItem.QuotaId = input.QuotaId;
                            quotaItem.Number = Convert.ToDecimal(input.Number);
                            quotaItem.Remark = input.Remark;
                            _repositoryQuotaItem.InsertAsync(quotaItem);

                            var editDto = ObjectMapper.Map<QuotaItem, QuotaItemEditDto>(quotaItem);
                            var basePrice = basePriceList.Find(x => x.Id == m);
                            if (basePrice != null)
                            {
                                editDto.AreaId = basePrice.AreaId;
                                editDto.AreaName = basePrice.Area?.Name;
                                editDto.StandardCodeId = basePrice.StandardCodeId;
                                editDto.StandardCodeName = basePrice.StandardCode?.Name;
                                editDto.Price = (float)basePrice.Price;
                                res.ComputerCodeName = basePrice.ComputerCode?.Name;
                                res.ComputerCode = basePrice.ComputerCode?.Code;
                            }
                            res.QuotaItemEditList.Add(editDto);
                        }
                    });

                }
            });
            return res;
        }

        [Authorize(StdBasicPermissions.QuotaItem.Delete)]
        public async Task<bool> Delete(Guid id, Guid computerCodeId)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("定额Id不准确");
            if (computerCodeId == null || computerCodeId == Guid.Empty) throw new UserFriendlyException("电算代号Id不准确");
            await Task.Run(() =>
            {
                var bfItemList = _repositoryQuotaItem.WithDetails().Where(x => x.QuotaId == id&&x.BasePrice!=null&&x.BasePrice.ComputerCodeId== computerCodeId).ToList();
                if(bfItemList?.Count>0)
                {
                    
                    bfItemList.ForEach(m =>
                        {
                            _repositoryQuotaItem.DeleteAsync(m);

                        });
                }
                else
                {
                    throw new UserFriendlyException("此清单不存在");
                }
                //var bfItemList = _repositoryQuotaItem.Where(x => x.QuotaId == id).ToList();
                //var ids = bfItemList.ConvertAll(x => x.BasePriceId);
                //var basePriceList = _repositoryBasePrice.Where(x => ids.Contains(x.Id) && x.ComputerCodeId == computerCodeId).ToList();
                //if (basePriceList?.Count > 0)
                //{
                //    var baseIds = basePriceList.ConvertAll(x => x.Id);
                //    var items = _repositoryQuotaItem.Where(x => x.QuotaId == id && baseIds.Contains(x.BasePriceId)).ToList();
                //    if (items?.Count > 0)
                //    {
                //        items.ForEach(m =>
                //        {
                //           _repositoryQuotaItem.DeleteAsync(m.Id);

                //        });
                //    }
                //    else
                //    {
                //        throw new UserFriendlyException("此清单不存在");
                //    }
                //}
            });
            return true;

        }

        [Authorize(StdBasicPermissions.QuotaItem.Export)]
        public async Task<Stream> Export(QuotaItemData input)
        {
            var list = GetQuotaItemData(input.Paramter);

            var dtoList = new List<QuotaItemTemplate>();// ObjectMapper.Map<List<QuotaItem>, List<QuotaItemTemplate>>(list);
            if (list?.Count > 0)
            {
                var ids = list.ConvertAll(x => x.BasePriceId);
                var basePriceList = _repositoryBasePrice.WithDetails().Where(x => ids.Contains(x.Id)).ToList();

                foreach (var item in list)
                {
                    var dto = ObjectMapper.Map<QuotaItem, QuotaItemTemplate>(item);
                    var basePrice = basePriceList.Find(m => m.Id == item.BasePriceId);
                    if (basePrice != null)
                    {
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
                                        dto.AreaName = nameList[i];
                                    }
                                    else
                                    {
                                        dto.AreaName += "_" + nameList[i];
                                    }
                                }


                            }

                        }
                        dto.ComputerCodeCode = basePrice.ComputerCode?.Code;
                        dto.ComputerCodeName = basePrice.ComputerCode?.Name;
                        dto.StandardCode = basePrice.StandardCode?.Name;
                        dto.Price = (float)basePrice.Price;
                    }
                    dtoList.Add(dto);
                }
                //var computerCodes = _repositoryComputerCode.Where();
            }
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        [Authorize(StdBasicPermissions.QuotaItem.Detail)]
        public async Task<QuotaItemDto> Get(Guid id, Guid computerCodeId)
        {
            QuotaItemDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的定额id");
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的电算代号id");
            await Task.Run(() =>
            {
                var bfItemList = _repositoryQuotaItem.WithDetails().Where(x => x.QuotaId == id && x.BasePrice != null && x.BasePrice.ComputerCodeId == computerCodeId).ToList();
                if (bfItemList?.Count > 0)
                {
                    var basePriceIds = bfItemList.ConvertAll(x => x.BasePriceId);
                    var basePriceList = _repositoryBasePrice.Where(x => basePriceIds.Contains(x.Id)).ToList();
                    result = new QuotaItemDto();
                    result.ComputerCodeId = computerCodeId;
                    result.QuotaId = id;
                    result.Number = (float)bfItemList[0].Number;
                    result.Remark = bfItemList[0].Remark;
                    result.ComputerCodeName = basePriceList[0].ComputerCode.Name;
                    result.QuotaItemEditList = new List<QuotaItemEditDto>();
                    bfItemList.ForEach(m =>
                    {
                        var editDto = ObjectMapper.Map<QuotaItem, QuotaItemEditDto>(m);
                        var basePrice = basePriceList.Find(x => x.Id == m.BasePriceId);
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
                        result.QuotaItemEditList.Add(editDto);
                    });
                }

            });
            return result;
        }

        public async Task<PagedResultDto<QuotaItemDto>> GetList(QuotaItemGetListDto input)
        {
            PagedResultDto<QuotaItemDto> res = new PagedResultDto<QuotaItemDto>();
            await Task.Run(() =>
            {
                var allEnt = _repositoryQuotaItem.WithDetails()
                        .WhereIf(input.QuotaId != null, x => x.QuotaId == input.QuotaId).ToList();
                var computerCodeIds = new List<Guid>();
                List<QuotaItem> entList = new List<QuotaItem>();
                var basePriceList = new List<BasePrice>();
                if (allEnt?.Count > 0)
                {
                    if (input.ComputerCodeId != null && input.ComputerCodeId != Guid.Empty)
                    {
                        basePriceList = _repositoryBasePrice.Where(a => a.ComputerCodeId == input.ComputerCodeId).ToList();
                        if (basePriceList?.Count > 0)
                        {
                            var ids = basePriceList.ConvertAll(x => x.Id);
                            entList = allEnt.FindAll(x => ids.Contains(x.BasePriceId));
                        }

                        computerCodeIds.Add(input.ComputerCodeId);
                    }
                    else
                    {
                        var ids = allEnt.ConvertAll(x => x.BasePriceId);
                        basePriceList = _repositoryBasePrice.Where(x => ids.Contains(x.Id)).ToList();
                        computerCodeIds = basePriceList?.ConvertAll(x => x.ComputerCodeId).Distinct().ToList();
                        entList = allEnt;
                    }
                }

                res.TotalCount = computerCodeIds.Count();
                List<QuotaItemDto> dtoList = new List<QuotaItemDto>();

                if (entList?.Count > 0)
                {
                    entList.ForEach(m =>
                    {
                        var bfmodel = dtoList.Find(x => x.ComputerCodeId == m.BasePrice.ComputerCodeId);
                        if (bfmodel == null)
                        {
                            var bfItemList = entList.FindAll(x => x.BasePrice.ComputerCodeId == m.BasePrice.ComputerCodeId);
                            bfmodel = new QuotaItemDto();
                            var basePrices = basePriceList.FindAll(x => x.ComputerCodeId == m.BasePrice.ComputerCodeId).ToList();
                            bfmodel = new QuotaItemDto();
                            bfmodel.ComputerCodeId = m.BasePrice.ComputerCodeId;
                            bfmodel.QuotaId = input.QuotaId;
                            bfmodel.Number = (float)bfItemList[0].Number;
                            bfmodel.Remark = bfItemList[0].Remark;
                            bfmodel.ComputerCodeName = basePrices[0].ComputerCode?.Name;
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
                            dtoList.Add(bfmodel);
                        }
                        else
                        {
                            var editDto = ObjectMapper.Map<QuotaItem, QuotaItemEditDto>(m);
                            var basePrice = basePriceList.Find(x => x.Id == m.BasePriceId);
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


                    });
                }
                if (input.IsAll == false)
                {


                    dtoList = dtoList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                }
                res.Items = dtoList;
            });
            return res;
        }

        [Authorize(StdBasicPermissions.QuotaItem.Update)]
        public async Task<QuotaItemDto> Update(QuotaItemUpdateDto input)
        {
            if (input.ComputerCodeId == null || input.ComputerCodeId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            QuotaItemDto res = null;
            if (input.BasePriceIdList == null || input.BasePriceIdList.Count < 1) throw new UserFriendlyException("未选择基价");
            if (input.Number <= 0) throw new UserFriendlyException("数量不能小于等于0");
            await Task.Run(() =>
            {
                var bfItemList = _repositoryQuotaItem.WithDetails().Where(x => x.QuotaId == input.QuotaId && x.BasePrice != null && x.BasePrice.ComputerCodeId == input.ComputerCodeId).ToList();
                if (bfItemList?.Count > 0)
                {
                    bfItemList.ForEach(m =>
                    {
                        _repositoryQuotaItem.DeleteAsync(m.Id);
                    });
                }
                if (input.BasePriceIdList?.Count > 0)
                {
                    res = new QuotaItemDto();
                    res.ComputerCodeId = input.ComputerCodeId;
                    res.QuotaId = input.QuotaId;
                    res.Number = input.Number;
                    res.Remark = input.Remark;
                    res.QuotaItemEditList = new List<QuotaItemEditDto>();
                    var basePriceList = _repositoryBasePrice.WithDetails().Where(x => input.BasePriceIdList.Contains(x.Id)).ToList();
                    input.BasePriceIdList.ForEach(m =>
                    {
                        var before = res.QuotaItemEditList.Find(x => x.BasePriceId == m);
                        if (before == null)
                        {
                            var quotaItem = new QuotaItem(_guidGenerator.Create());
                            quotaItem.BasePriceId = m;

                            quotaItem.QuotaId = input.QuotaId;
                            quotaItem.Number = Convert.ToDecimal(input.Number);
                            quotaItem.Remark = input.Remark;
                            _repositoryQuotaItem.InsertAsync(quotaItem);

                            var editDto = ObjectMapper.Map<QuotaItem, QuotaItemEditDto>(quotaItem);
                            var basePrice = basePriceList.Find(x => x.Id == m);
                            if (basePrice != null)
                            {
                                editDto.AreaId = basePrice.AreaId;
                                editDto.AreaName = basePrice.Area?.Name;
                                editDto.StandardCodeId = basePrice.StandardCodeId;
                                editDto.StandardCodeName = basePrice.StandardCode?.Name;
                                editDto.Price = (float)basePrice.Price;
                                res.ComputerCodeName = basePrice.ComputerCode?.Name;
                                res.ComputerCode = basePrice.ComputerCode?.Code;
                            }
                            res.QuotaItemEditList.Add(editDto);
                        }
                    });

                }
            });
            return res;
        }

        [Authorize(StdBasicPermissions.QuotaItem.Import)]
        public async Task Upload([FromForm] ImportData input, Guid id)
        {
            // 虚拟进度0 %
            await _fileImport.Start(input.ImportKey, 100);
            //QuotaItemTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<QuotaItemTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<QuotaItemTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<QuotaItemTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            QuotaItem quotaItemModel = null;
            ComputerCode computerCodeModel = null;
            BasePrice basePriceModel = null;
            Area areaModel = null;
            DataDictionary stdCodeModel = null;
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
                    if (item.StandardCode.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"标准编号为空");
                    }
                    if (item.ComputerCodeCode.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"电算代号编号为空");
                    }
                    if (item.ComputerCodeName.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"电算代号名称为空");
                    }
                    if (item.AreaName.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"区域名称为空");
                    }
                    if (item.Number <= 0)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"数量小于等于0");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        computerCodeModel = _repositoryComputerCode.FirstOrDefault(a => a.Code == item.ComputerCodeCode && a.Name == item.ComputerCodeName);
                        if (computerCodeModel != null)
                        {
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
                            if (areaModel != null && stdCodeModel != null)
                            {
                                basePriceModel = _repositoryBasePrice.FirstOrDefault(a => a.ComputerCodeId == computerCodeModel.Id && a.StandardCodeId == stdCodeModel.Id && a.Price == Convert.ToDecimal(item.Price) && a.AreaId == areaModel.Id);
                                if (basePriceModel != null)
                                {
                                    quotaItemModel = _repositoryQuotaItem.FirstOrDefault(a => a.BasePriceId == basePriceModel.Id && a.QuotaId == id);
                                }
                            }
                        }


                    }
                    if (basePriceModel != null && quotaItemModel != null)
                    {
                        newInfo.AppendInfo($"{item.StandardCode}已存在，且已更新");
                        //修改自己本身数据
                        quotaItemModel.QuotaId = id;
                        quotaItemModel.Number = Convert.ToDecimal(item.Number);
                        quotaItemModel.BasePriceId = basePriceModel.Id;
                        await _repositoryQuotaItem.UpdateAsync(quotaItemModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        if (basePriceModel != null)
                        {
                            quotaItemModel = new QuotaItem(_guidGenerator.Create());
                            quotaItemModel.QuotaId = id;
                            quotaItemModel.Number = Convert.ToDecimal(item.Number);
                            quotaItemModel.BasePriceId = basePriceModel.Id;
                            await _repositoryQuotaItem.InsertAsync(quotaItemModel);
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

        private List<QuotaItem> GetQuotaItemData(QuotaItemGetListDto input)
        {
            var query = _repositoryQuotaItem
                .WithDetails()
                .WhereIf(input.QuotaId != null && input.QuotaId != Guid.Empty,
                    s => s.QuotaId == input.QuotaId);

            return query.ToList();
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
