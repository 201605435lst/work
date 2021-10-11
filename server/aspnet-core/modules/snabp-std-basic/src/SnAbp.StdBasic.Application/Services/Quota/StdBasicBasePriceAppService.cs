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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.StdBasic.Services
{
    /// <summary>
    /// 基价管理
    /// </summary>
    [Authorize]
    public class StdBasicBasePriceAppService : StdBasicAppService, IStdBasicBasePriceAppService
    {
        private readonly IRepository<BasePrice, Guid> _repositoryBasePrice;
        private readonly IRepository<ComputerCode, Guid> _repositoryComputerCode;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<Area> _repositoryArea;//区域
        private readonly IRepository<DataDictionary, Guid> _repositoryDataDictionary;

        public StdBasicBasePriceAppService(IRepository<BasePrice, Guid> repositoryBasePrice,
       IRepository<ComputerCode, Guid> repositoryComputerCode,
       IGuidGenerator guidGenerator,
       IFileImportHandler fileImport,
       IDataFilter dataFilter,
       IRepository<DataDictionary, Guid> repositoryDataDictionary,
        IRepository<Area> repositoryArea//区域
           )
        {
            _repositoryBasePrice = repositoryBasePrice;
            _repositoryComputerCode = repositoryComputerCode;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _repositoryArea = repositoryArea;//区域
            _repositoryDataDictionary = repositoryDataDictionary;
        }

        [Authorize(StdBasicPermissions.BasePrice.Create)]
        public async Task<BasePriceDto> Create(BasePriceCreateDto input)
        {
            BasePriceDto res = null;
            if (input.StandardCodeId == null) throw new UserFriendlyException("基价编号不能为空");
            if (input.AreaId<=0) throw new UserFriendlyException("行政区域不能为空");
            if (input.Price<=0) throw new UserFriendlyException("基价不能为空");
            var befor = _repositoryBasePrice.FirstOrDefault(x=>x.ComputerCodeId==input.ComputerCodeId&&x.StandardCodeId==input.StandardCodeId&&x.AreaId==input.AreaId);
            if(befor!=null) throw new UserFriendlyException("该标准编号、行政区域已有基价数据，不能重复添加");
            var basePrice = new BasePrice(_guidGenerator.Create());
            basePrice.AreaId = input.AreaId;

            basePrice.ComputerCodeId = input.ComputerCodeId;
            basePrice.Price = Convert.ToDecimal(input.Price>=0?input.Price:0);
            basePrice.StandardCodeId = input.StandardCodeId;
            await _repositoryBasePrice.InsertAsync(basePrice);
            res = ObjectMapper.Map<BasePrice, BasePriceDto>(basePrice);
            if (input.AreaId > -1)
            {
                var area = _repositoryArea.FirstOrDefault(s => s.Id == input.AreaId);
                if (area != null)
                {
                    res.AreaName = area.Name;
                }
            }
            if (input.StandardCodeId != null)
            {
                var stdCode = _repositoryDataDictionary.FirstOrDefault(s => s.Id == input.StandardCodeId);
                if (stdCode != null)
                {
                    res.StandardCodeName = stdCode.Name;
                }
            }
            return res;
        }

        [Authorize(StdBasicPermissions.BasePrice.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryBasePrice.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此基价不存在");
            await _repositoryBasePrice.DeleteAsync(id);
            return true;
        }

        [Authorize(StdBasicPermissions.BasePrice.Export)]
        public async Task<Stream> Export(BasePriceData input)
        {
            var list = GetBasePriceData(input.Paramter);
            var dtoList = ObjectMapper.Map<List<BasePrice>, List<BasePriceTemplate>>(list);
            if(dtoList?.Count>0)
            {
                dtoList.ForEach(x=>
                {
                    var m = list.Find(y=>y.ComputerCode.Name==x.ComputerCodeName&&y.Area.Name==x.AreaName&&y.StandardCode.Name==x.StandardCodeName&&y.Price== Convert.ToDecimal(x.Price));
                    if(m!=null)
                    {
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
                                for (int i = nameList.Count - 1; i >= 0; i--)
                                {
                                    if (i == nameList.Count - 1)
                                    {
                                        x.AreaName = nameList[i];
                                    }
                                    else
                                    {
                                        x.AreaName += "_" + nameList[i];
                                    }
                                }


                            }

                        }
                    }
                });
            }
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        [Authorize(StdBasicPermissions.BasePrice.Detail)]
        public async Task<BasePriceDto> Get(Guid id)
        {
            BasePriceDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryBasePrice.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此基价不存在");
                result = ObjectMapper.Map<BasePrice, BasePriceDto>(ent);
                if (ent.Area != null)
                {

                    result.AreaName = ent.Area.FullName;

                }
                if (ent.StandardCode != null)
                {
                    result.StandardCodeName = ent.StandardCode.Name;
                }
            });
            return result;
        }

        public async Task<PagedResultDto<BasePriceDto>> GetList(BasePriceGetListDto input)
        {
            PagedResultDto<BasePriceDto> res = new PagedResultDto<BasePriceDto>();
            var allEnt = _repositoryBasePrice.WithDetails()
                        .WhereIf(input.ComputerCodeId != null, x => x.ComputerCodeId == input.ComputerCodeId).ToList();
            res.TotalCount = allEnt.Count();
            List<BasePriceDto> dtoList = new List<BasePriceDto>();
            if (input.IsAll == false)
            {

                dtoList = ObjectMapper.Map<List<BasePrice>, List<BasePriceDto>>(allEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            else
            {
                dtoList = ObjectMapper.Map<List<BasePrice>, List<BasePriceDto>>(allEnt.ToList());

            }
            if (dtoList?.Count > 0)
            {
                dtoList.ForEach(m =>
                {
                    var dto = allEnt.Find(x => x.Id == m.Id);
                    if (dto.Area != null)
                    {

                        List<string> nameList = new List<string>();
                        nameList.Add(dto.Area.FullName);
                        if (dto.Area.ParentId.HasValue && dto.Area.ParentId.Value >= 0)
                        {
                            FindAllAreaParents(dto.Area.ParentId.Value, ref nameList);

                        }
                        if (nameList?.Count > 0)
                        {
                            for (int i = nameList.Count-1; i >=0; i--)
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
                    if (dto.StandardCode != null)
                    {
                        m.StandardCodeName = dto.StandardCode.Name;
                    }
                });
            }
            res.Items = dtoList;
            return res;
        }

        [Authorize(StdBasicPermissions.BasePrice.Update)]
        public async Task<BasePriceDto> Update(BasePriceUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            if (input.StandardCodeId == null) throw new UserFriendlyException("基价编号不能为空");
            if (input.AreaId <= 0) throw new UserFriendlyException("行政区域不能为空");
            if (input.Price <= 0) throw new UserFriendlyException("基价不能为空");
            var befor = _repositoryBasePrice.FirstOrDefault(x => x.ComputerCodeId == input.ComputerCodeId && x.StandardCodeId == input.StandardCodeId && x.AreaId == input.AreaId&&x.Id!=input.Id);
            if (befor != null) throw new UserFriendlyException("该标准编号、行政区域已有基价数据，不能重复添加");
            BasePriceDto result = null;
            var ent = _repositoryBasePrice.FirstOrDefault(s => input.Id == s.Id);
            if (ent == null) throw new UserFriendlyException("此基价不存在");
            ent.AreaId = input.AreaId;

            ent.ComputerCodeId = input.ComputerCodeId;
            ent.Price = Convert.ToDecimal(input.Price>=0?input.Price:0);
            ent.StandardCodeId = input.StandardCodeId;

            var resEnt = await _repositoryBasePrice.UpdateAsync(ent);
            result = ObjectMapper.Map<BasePrice, BasePriceDto>(resEnt);
            if (ent.AreaId > -1)
            {
                var area = _repositoryArea.WithDetails().FirstOrDefault(s => s.Id == ent.AreaId);
                if (area != null)
                {

                    List<string> nameList = new List<string>();
                    nameList.Add(area.FullName);
                    if (area.ParentId.HasValue && area.ParentId.Value >= 0)
                    {
                        FindAllAreaParents(area.ParentId.Value, ref nameList);

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
            }
            if (input.StandardCodeId != null)
            {
                var stdCode = _repositoryDataDictionary.FirstOrDefault(s => s.Id == input.StandardCodeId);
                if (stdCode != null)
                {
                    result.StandardCodeName = stdCode.Name;
                }
            }
            return result;
        }

        [Authorize(StdBasicPermissions.BasePrice.Import)]
        public async Task Upload([FromForm] ImportData input, Guid id)
        {
            // 虚拟进度0 %
            await _fileImport.Start(input.ImportKey, 100);
            //BasePriceTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<BasePriceTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<BasePriceTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<BasePriceTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
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
                    if (item.StandardCodeName.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"标准编号为空");
                    }
                    if (item.ComputerCodeName.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"电算代号为空");
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
                    using (_dataFilter.Disable<ISoftDelete>())
                    {

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
                        stdCodeModel = _repositoryDataDictionary.FirstOrDefault(a => a.Name == item.StandardCodeName);
                    }
                    if (areaModel != null && stdCodeModel != null)
                    {
                        basePriceModel = _repositoryBasePrice.FirstOrDefault(a => a.StandardCodeId == stdCodeModel.Id && a.ComputerCodeId == id && a.AreaId == areaModel.Id);

                        if (basePriceModel != null)
                        {
                            newInfo.AppendInfo($"{item.StandardCodeName}已存在，且已更新");
                            //修改自己本身数据
                            basePriceModel.ComputerCodeId = id;
                            basePriceModel.Price = Convert.ToDecimal(item.Price>=0?item.Price:0);
                            basePriceModel.StandardCodeId = stdCodeModel.Id;

                            basePriceModel.AreaId = areaModel.Id;

                            await _repositoryBasePrice.UpdateAsync(basePriceModel);
                            wrongInfos.Add(newInfo);
                        }
                        else
                        {

                            basePriceModel = new BasePrice(_guidGenerator.Create());
                            basePriceModel.Price = Convert.ToDecimal(item.Price>=0?item.Price:0);
                            basePriceModel.StandardCodeId = stdCodeModel.Id;

                            basePriceModel.ComputerCodeId = id;


                            basePriceModel.AreaId = areaModel.Id;
                            await _repositoryBasePrice.InsertAsync(basePriceModel);
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

        private List<BasePrice> GetBasePriceData(BasePriceGetListDto input)
        {
            var query = _repositoryBasePrice
                .WithDetails()
                .WhereIf(input.ComputerCodeId != null && input.ComputerCodeId != Guid.Empty,
                    s => s.ComputerCodeId == input.ComputerCodeId);

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
