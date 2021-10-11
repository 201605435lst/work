using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Alarm.Dtos;
using SnAbp.Alarm.Entities;
using SnAbp.Alarm.IServices;
using SnAbp.Alarm.TemplateModel;
using SnAbp.Common;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.StdBasic.Entities;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Alarm.Services
{
    public class AlarmAlarmAppService : AlarmAppService, IAlarmAlarmAppService
    {
        private IRepository<Entities.Alarm, Guid> _alarmRepository;
        private IRepository<AlarmEquipmentIdBind, Guid> _alarmEquipmentIdBindRepository;
        private IRepository<ComponentCategory, Guid> _componentCategoryRepository;
        private IRepository<Equipment, Guid> _equipmentRepository;
        private IRepository<EquipmentGroup, Guid> _equipmentGroupRepository;
        private IRepository<AlarmEquipmentIdBind> _alarmEquipmentBindIdsRepository;
        private IGuidGenerator _guidGenerator;
        private IFileImportHandler _fileImport;
        private IUnitOfWorkManager _unitOfWork;

        public AlarmAlarmAppService(
            IRepository<Entities.Alarm, Guid> alarmRepository,
            IRepository<Entities.AlarmEquipmentIdBind, Guid> alarmEquipmentIdBindRepository,
            IRepository<ComponentCategory, Guid> componentCategoryRepository,
            IRepository<Equipment, Guid> equipmentRepository,
            IRepository<EquipmentGroup, Guid> equipmentGroupRepository,
            IRepository<AlarmEquipmentIdBind> alarmEquipmentBindIdsRepository,
            IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
            IUnitOfWorkManager unitOfWork
        )
        {
            _alarmRepository = alarmRepository;
            _alarmEquipmentIdBindRepository = alarmEquipmentIdBindRepository;
            _componentCategoryRepository = componentCategoryRepository;
            _equipmentRepository = equipmentRepository;
            _equipmentGroupRepository = equipmentGroupRepository;
            _alarmEquipmentBindIdsRepository = alarmEquipmentBindIdsRepository;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _unitOfWork = unitOfWork;
        }


        public async Task<PagedResultDto<AlarmDto>> GetList(AlarmGetListDto input)
        {
            var query = _alarmRepository
                .WithDetails(
                    x => x.Equipment.ComponentCategory.Parent.Parent,
                    x => x.Equipment.InstallationSite.Station,
                    x => x.Equipment.InstallationSite.Railway,
                    x => x.Equipment.Group
                )
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x =>
                    x.Name.Contains(input.Keywords) ||
                    x.Content.Contains(input.Keywords) ||
                    x.Equipment.Name.Contains(input.Keywords)
                )
                .WhereIf(input.Level != null, x => x.Level == input.Level)
                .WhereIf(input.RailwayId != null, x => x.Equipment.InstallationSite.RailwayId == input.RailwayId)
                .WhereIf(input.StationId != null, x => x.Equipment.InstallationSite.StationId == input.StationId)
                .WhereIf(input.SystemCode != null, x => x.Equipment.ComponentCategory.Code.StartsWith(input.SystemCode))
                .Where(x => x.State == Enums.AlarmState.Actived);

            var rst = new PagedResultDto<AlarmDto>();
            rst.TotalCount = query.Count();

            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var dtos = ObjectMapper.Map<List<Entities.Alarm>, List<AlarmDto>>(list);
            rst.Items = dtos;
            return rst;
        }


        public async Task<AlarmDto> Create(AlarmCreateDto input)
        {
            var alarm = new Entities.Alarm(_guidGenerator.Create())
            {
                EquipmentId = input.EquipmentId,
                Name = input.Name,
                Code = input.Code,
                Content = input.Content,
                Level = input.Level
            };

            await _alarmRepository.InsertAsync(alarm);
            return ObjectMapper.Map<Entities.Alarm, AlarmDto>(alarm);
        }


        public async Task ImportEquipmentId([FromForm] ImportData input)
        {
            var key = input.ImportKey;
            var file = input.File;
            ISheet sheet = null;
            IWorkbook workbook = null;
            DataTable dt = null;
            var rowIndex = 1;  //有效数据得起始索引
            try
            {
                // 获取excel表格，判断报个是否满足模板
                workbook = file.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0)
                .CheckColumnAccordTempleModel<ImportEquipmentIdModel>(rowIndex);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(key);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            var datalist = sheet
                .TryTransToList<ImportEquipmentIdModel>(rowIndex)
                .CheckNull();


            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();

            if (datalist.Any())
            {
                await _fileImport.Start(key, datalist.Count);

                foreach (var a in datalist)
                {
                    using var uow = _unitOfWork.Begin(true, false);

                    await _fileImport.UpdateState(key, datalist.FindIndex(a));

                    var newInfo = new WrongInfo { RowIndex = a.Index };
                    var canInsert = true;

                    if (!string.IsNullOrEmpty(a.EquipmentThirdIds))
                    {
                        a.EquipmentThirdIds = a.EquipmentThirdIds.Trim().Replace(" ", "");
                    }
                    Equipment equipment = null;

                    var group = (await _equipmentGroupRepository.GetListAsync()).FirstOrDefault(); ;

                    if (group == null)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"设备分组【{a.Name}】不已存在\r\n");
                    }
                    else
                    {
                        equipment = _equipmentRepository.Where(x => x.Name == a.Name && x.GroupId == group.Id).ToList().FirstOrDefault();
                        if (equipment == null)
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"设备【{a.Name}】不已存在\r\n");
                        }
                    }

                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    // 先查找是否已经存在绑定的数据，如果存在就替换，否则新建
                    var existBind = _alarmEquipmentBindIdsRepository
                        .Where(x => x.EquipmentId == equipment.Id)
                        .FirstOrDefault();

                    if (existBind == null)
                    {
                        var serviceRecord = new AlarmEquipmentIdBind(_guidGenerator.Create())
                        {
                            EquipmentId = equipment.Id,
                            EquipmentThirdIds = a.EquipmentThirdIds
                        };
                        await _alarmEquipmentBindIdsRepository.InsertAsync(serviceRecord);
                    }
                    else
                    {
                        existBind.EquipmentThirdIds = a.EquipmentThirdIds;
                        await _alarmEquipmentBindIdsRepository.UpdateAsync(existBind);
                    }


                    await uow.SaveChangesAsync();
                }
                await _fileImport.Complete(key);
            }
            // 处理错误信息
            if (wrongInfos.Any())
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), key, workbook.ConvertToBytes());
            }
        }


        public async Task<List<AlarmSimple>> GetAlarmEquipmentBindIdsByIds(List<RealAlarmVaryInfo> realAlarmVaryInfos)
        {
            var realAlarmInfos = new List<RealAlarmInfo>();
            var thirdIds = new List<string>();

            foreach (RealAlarmVaryInfo info in realAlarmVaryInfos)
            {
                realAlarmInfos.Add(info.RealAlarmInfo);
            }

            // 提取 设备第三方ids
            thirdIds = realAlarmInfos.Select(x => x.lDeviceId.ToString() + ",").ToList();
            if (thirdIds.Count == 0)
            {
                return new List<AlarmSimple>();
            }


            try
            {
                // 绑定信息
                var binds = _alarmEquipmentIdBindRepository
                   .Where(x => x.EquipmentThirdIds != null)
                   //.Where(x => x.EquipmentThirdIds.Contains("900230400029,"))
                   .ToList();

                // 设备ids
                var equipmentIds = binds.Select(x => x.EquipmentId).ToList();


                // 查询设备信息
                var equipments = _equipmentRepository
                    .WithDetails(
                        x => x.ComponentCategory.Parent.Parent,
                        x => x.InstallationSite.Station,
                        x => x.InstallationSite.Railway,
                        x => x.Group
                    )
                    .Where(x => equipmentIds.Contains(x.Id))
                    .ToList();

                var equipmentDtos = ObjectMapper.Map<List<Equipment>, List<EquipmentDto>>(equipments);

                var alarms = new List<AlarmSimple>();
                foreach (RealAlarmInfo info in realAlarmInfos)
                {
                    var bind = binds.Find(x =>
                        !string.IsNullOrEmpty(x.EquipmentThirdIds) &&
                        x.EquipmentThirdIds.Contains(info.lDeviceId.ToString() + ',')
                    );

                    var equipment = bind != null ? equipmentDtos.Find(x => x.Id == bind.EquipmentId) : null;
                    if (bind != null && equipment != null)
                    {
                        if (equipment.ComponentCategory != null && equipment.ComponentCategory.Parent != null)
                        {
                            equipment.ComponentCategory.Children = null;
                            equipment.ComponentCategory.Parent.Children = null;
                            if (equipment.ComponentCategory.Parent.Parent != null)
                            {
                                equipment.ComponentCategory.Parent.Parent = null;
                            }
                        }

                        var alarm = new AlarmSimple();
                        alarm.EquipmentId = equipment.Id;
                        alarm.Equipment = equipment;
                        alarm.Code = info.strFaultInfo;
                        alarm.Id = info.lId;
                        alarm.IsConfirm = info.bIsAck;
                        alarm.Level = info.nLevel;
                        alarm.Content = info.strDescription;
                        alarm.ActivedTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddMilliseconds(info.tTime);
                        alarms.Add(alarm);
                    }
                }

                alarms = alarms.OrderByDescending(x => x.ActivedTime).ToList();
                return alarms;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}