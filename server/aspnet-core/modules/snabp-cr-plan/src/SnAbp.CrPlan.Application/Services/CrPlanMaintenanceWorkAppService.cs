using SnAbp.CrPlan.Dto;
using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Entities;
//using SnAbp.CrPlan.IServices.MaintenanceWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Newtonsoft.Json.Linq;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Enums;
using SnAbp.Bpm.Services;
using SnAbp.Identity;
using SnAbp.Utils.EnumHelper;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm;
using SnAbp.CrPlan.IServices.MaintenanceWork;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Dtos.MaintenanceWork;
using SnAbp.CrPlan.Enums;
using System.IO;
using System.Data;
using SnAbp.Utils.ExcelHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SnAbp.CrPlan.Commons;
using SnAbp.File;
using SnAbp.Message.Notice;
using Newtonsoft.Json;
using SnAbp.Message.MessageDefine;

namespace SnAbp.CrPlan.Services
{
    public class CrPlanMaintenanceWorkAppService : CrPlanAppService, ICrPlanMaintenanceWorkAppService
    {
        private readonly IRepository<MaintenanceWork, Guid> _maintenanceWorks;
        private readonly IRepository<MaintenanceWorkRltSkylightPlan, Guid> _maintenanceWorkRltSkylightPlanRepository;
        //private readonly CrPlanSkylightPlanAppService _skylightPlan;
        private readonly IRepository<SkylightPlan, Guid> _skylightPlanRepos;
        private readonly CrPlanManager _crPlanManager;
        private readonly BpmManager _bpmManager;
        private readonly IRepository<StationRltRailway, Guid> _stationRltRailways;
        private IGuidGenerator _generator;
        private IRepository<Station, Guid> _stations;
        private IRepository<Organization, Guid> _organizations;
        private readonly IRepository<Workflow, Guid> _workflows;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private IRepository<SkylightPlanRltWorkTicket, Guid> _skylightPlanRltWorkTicket;
        private IRepository<MaintenanceWorkRltFile, Guid> _maintenanceWorkRltFile;
        private IRepository<File.Entities.File, Guid> _fileRepos;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<SkylightPlanRltWorkTicket, Guid> _skylightRltTicket;
        private readonly IRepository<WorkTicketRltCooperationUnit, Guid> _workTicketRltCooperationUnit;
        private readonly IMessageNoticeProvider _messageNotice;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IRepository<WorkTicket, Guid> _workTickets;

        public CrPlanMaintenanceWorkAppService(
            IRepository<MaintenanceWork, Guid> maintenanceWorks,
            //CrPlanSkylightPlanAppService skylightPlan,
            IRepository<SkylightPlan, Guid> skylightPlanRepos,
            BpmManager bpmManager,
            CrPlanManager crPlanManager,
            IRepository<StationRltRailway, Guid> stationRltRailways,
            IGuidGenerator generator,
            IRepository<Station, Guid> stations,
            IRepository<Organization, Guid> organizations,
            IRepository<MaintenanceWorkRltSkylightPlan, Guid> maintenanceWorkRltSkylightPlanRepository,
            IRepository<Workflow, Guid> workflows,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IRepository<MaintenanceWorkRltSkylightPlan, Guid> maintenanceWork,
            IRepository<SkylightPlanRltWorkTicket, Guid> skylightPlanRltWorkTicket,
            IRepository<MaintenanceWorkRltFile, Guid> maintenanceWorkRltFile,
            IRepository<File.Entities.File, Guid> fileRepos,
            IHttpContextAccessor httpContextAccessor,
            IRepository<SkylightPlanRltWorkTicket, Guid> skylightRltTicket,
            IRepository<WorkTicketRltCooperationUnit, Guid> workTicketRltCooperationUnit,
            IMessageNoticeProvider messageNotice,
            IdentityUserManager identityUserManager,
            IRepository<WorkTicket, Guid> workTickets
            )
        {
            _maintenanceWorks = maintenanceWorks;
            //_skylightPlan = skylightPlan;
            _skylightPlanRepos = skylightPlanRepos;
            _bpmManager = bpmManager;
            _crPlanManager = crPlanManager;
            _stationRltRailways = stationRltRailways;
            _generator = generator;
            _stations = stations;
            _organizations = organizations;
            _maintenanceWorkRltSkylightPlanRepository = maintenanceWorkRltSkylightPlanRepository;
            _workflows = workflows;
            _dataDictionaries = dataDictionaries;
            _skylightPlanRltWorkTicket = skylightPlanRltWorkTicket;
            _maintenanceWorkRltFile = maintenanceWorkRltFile;
            _fileRepos = fileRepos;
            _httpContextAccessor = httpContextAccessor;
            _skylightRltTicket = skylightRltTicket;
            _workTicketRltCooperationUnit = workTicketRltCooperationUnit;
            _messageNotice = messageNotice;
            _identityUserManager = identityUserManager;
            _workTickets = workTickets;
        }

        /// <summary>
        /// 生成维修计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MaintenanceWorkDto> Create(MaintenanceWorkCreateDto input)
        {
            //if (input.SkylightPlanIds.Count == 0)
            //{
            //    throw new UserFriendlyException("未勾选天窗计划项");
            //}

            //var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //var dto = ObjectMapper.Map<MaintenanceWorkCreateDto, MaintenanceWorkDto>(input);

            //dto.Id = _generator.Create();
            //var maintenancePlan = ObjectMapper.Map<MaintenanceWorkDto, MaintenanceWork>(dto);

            ////修改提交的天窗的维修级别及计划状态
            //var levels = "";

            //foreach (var item in input.SkylightPlanIds)
            //{
            //    var skylightEnt = _skylightPlanRepos.Where(x => x.Id == item).FirstOrDefault();
            //    //添加维修计划的维修级别
            //    levels += skylightEnt.Level + ",";

            //    skylightEnt.PlanState = PlanState.Waitting;
            //    await _skylightPlanRepos.UpdateAsync(skylightEnt);
            //}

            //var levelList = levels.TrimEnd(',').Split(",").Distinct().ToList();

            //var level = "";
            //foreach (var item in levelList)
            //{
            //    level += int.Parse(item) + ",";
            //}
            //maintenancePlan.RepairLevel = level.TrimEnd(',');

            ////判断是否包含一级维修：Y:必须添加计划方案
            //if (level.TrimEnd(',').Contains("1") && input.MaintenanceWorkRltPlanFiles.Count == 0)
            //{
            //    throw new UserFriendlyException("存在I级计划,请添加计划方案文件");
            //}

            ////保存维修计划与关联关系
            //foreach (var item in input.SkylightPlanIds)
            //{
            //    var maintenanceWorkRltSkylightPlan = new MaintenanceWorkRltSkylightPlan(_generator.Create())
            //    {
            //        MaintenanceWorkId = maintenancePlan.Id,
            //        SkylightPlanId = item,
            //        WorkOrgAndDutyPerson = input.WorkOrgAndDutyPerson,
            //        SignOrganization = input.SignOrganization,
            //        Remark = input.Remark,
            //        FirstTrial = input.FirstTrial
            //    };
            //    await _maintenanceWorkRltSkylightPlanRepository.InsertAsync(maintenanceWorkRltSkylightPlan);
            //    //await CurrentUnitOfWork.SaveChangesAsync();
            //}

            //maintenancePlan.MaintenanceProject = "通信维修";
            //maintenancePlan.RepairTagId = RepairTagId;

            //await _maintenanceWorks.InsertAsync(maintenancePlan);

            //await CurrentUnitOfWork.SaveChangesAsync();
            ////维修计划提交审批
            //var maintenanceWorkSubmitDto = new MaintenanceWorkSubmitDto
            //{
            //    MaintenanceId = maintenancePlan.Id,
            //    MaintenanceWorkRltFiles = input?.MaintenanceWorkRltPlanFiles
            //};
            //var submitSuccess = await SumbitFirsrFlow(maintenanceWorkSubmitDto);
            //if (!submitSuccess)
            //{
            //    throw new UserFriendlyException("提交审批失败");
            //}

            //return ObjectMapper.Map<MaintenanceWork, MaintenanceWorkDto>(maintenancePlan);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="time"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string FormatTime(DateTime time, int length)
        {
            var endTime = time.AddMinutes(length);
            return $"{time.Hour.ToString().PadLeft(2, '0')}:{time.Minute.ToString().PadLeft(2, '0')}-{endTime.Hour.ToString().PadLeft(2, '0')}:{endTime.Minute.ToString().PadLeft(2, '0')}({length}分钟)";
        }

        /// <summary>
        /// 获取维修地点
        /// </summary>
        /// <returns></returns>
        private string GetRepaireLoaction(SkylightPlanDto skylightPlan)
        {
            var repaireLocation = "";

            switch (skylightPlan.StationRelateRailwayType)
            {
                case RelateRailwayType.SINGLELINK:
                    repaireLocation = ChecStation(skylightPlan);
                    break;
                case RelateRailwayType.UPLINK:
                    repaireLocation = "上行:" + "\n" + ChecStation(skylightPlan);
                    break;
                case RelateRailwayType.DOWNLINK:
                    repaireLocation = "下行:" + "\n" + ChecStation(skylightPlan);
                    break;
                case RelateRailwayType.UPANDDOWN:
                    var rltRailway = _stationRltRailways.Where(x =>
                        x.RailwayId == skylightPlan.RailwayId && x.StationId == skylightPlan.StationId).ToList();
                    foreach (var stationRltRailway in rltRailway)
                    {
                        switch (stationRltRailway.RailwayType)
                        {
                            case RelateRailwayType.UPLINK:
                                repaireLocation = "上行:" + "\n" + ChecStation(skylightPlan);
                                break;
                            case RelateRailwayType.DOWNLINK:
                                repaireLocation = "下行:" + "\n" + ChecStation(skylightPlan);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return repaireLocation;
        }

        /// <summary>
        /// 查找车站、区间
        /// </summary>
        private string ChecStation(SkylightPlanDto skylightPlan)
        {
            var repaireLocation = "";
            var rltRailway = _stationRltRailways.FirstOrDefault(x =>
                x.RailwayId == skylightPlan.RailwayId && x.StationId == skylightPlan.StationId);

            var station = _stations.FirstOrDefault(x => x.Id == rltRailway.StationId);
            if (station != null && station.Type == 1)//区间
            {
                // ReSharper disable once PossibleNullReferenceException
                var startMark = _stationRltRailways.FirstOrDefault(x =>
                    x.RailwayId == skylightPlan.RailwayId &&
                    x.StationId == station.SectionStartStationId).KMMark;
                // ReSharper disable once PossibleNullReferenceException
                var endMark = _stationRltRailways.FirstOrDefault(x =>
                    x.RailwayId == skylightPlan.RailwayId &&
                    x.StationId == station.SectionEndStationId).KMMark;

                repaireLocation = rltRailway.Station.Name + "\n" + startMark + "-" +
                                  endMark;
            }
            else
            {
                repaireLocation = rltRailway.Station.Name;// + "\n" + rltRailway.KMMark;//车站
            }
            return repaireLocation;
        }
        public async Task<List<MaintenanceWorkDto>> Get(Guid id)
        {
            List<MaintenanceWorkDto> res = new List<MaintenanceWorkDto>();
            return res;
        }

        /// <summary>
        /// 获取第一级审批维修计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MaintenanceWorkSimpleDto>> GetList(MaintenanceWorkSearchDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            PagedResultDto<MaintenanceWorkSimpleDto> result = new PagedResultDto<MaintenanceWorkSimpleDto>();

            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) return null;
            var StartOrgCode = "";
            var org = await _organizations.GetAsync(input.OrganizationId);
            if (org != null)
            {
                StartOrgCode = org.Code;
            }
            var maintenanceWork = _maintenanceWorks.WithDetails()
                 .Where(x => x.RepairTagId == RepairTagId && x.Organization.Code.StartsWith(StartOrgCode))
                 .WhereIf(input.PlanTime != null, x => x.StartTime.ToString().StartsWith(input.PlanTime) || x.EndTime.ToString().StartsWith(input.PlanTime));

            var dtos = ObjectMapper.Map<List<MaintenanceWork>, List<MaintenanceWorkSimpleDto>>(maintenanceWork.ToList());
            var resList = new List<MaintenanceWorkSimpleDto>();

            //筛选第一级审批状态
            var firstStepARKeys = dtos.Select(s => s.ARKey);
            var firstFlows = _workflows.Where(s => firstStepARKeys.Contains(s.Id));
            foreach (var item in dtos)
            {

                var temp = firstFlows.FirstOrDefault(s => s.Id == item.ARKey);

                if (temp != null)
                {
                    item.WorkflowState = temp.State;
                }

                if (input.WorkflowState != WorkflowState.All)
                {
                    if (temp != null && temp.State == input.WorkflowState)
                    {
                        resList.Add(item);
                    }
                }
                else
                {
                    resList = dtos;
                }
            }

            result.Items = resList.OrderBy(x => x.WorkflowState).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.TotalCount = resList.Count();
            return result;
        }

        /// <summary>
        /// 获取第二阶段审批的维修作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MaintenanceWorkSimpleDto>> GetListForSecondStep(MaintenanceWorkSearchDto input)
        {
            PagedResultDto<MaintenanceWorkSimpleDto> result = new PagedResultDto<MaintenanceWorkSimpleDto>();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) return null;
            var StartOrgCode = "";
            var org = await _organizations.GetAsync(input.OrganizationId);
            if (org != null)
            {
                StartOrgCode = org.Code;
            }
            var maintenanceWork = _maintenanceWorks.WithDetails()
             .Where(x => x.RepairTagId == RepairTagId && x.Organization.Code.StartsWith(StartOrgCode))
             .WhereIf(input.PlanTime != null, x => x.StartTime.ToString().StartsWith(input.PlanTime) || x.EndTime.ToString().StartsWith(input.PlanTime)).ToList();

            //筛选第一级审批通过的数据
            var firstStepARKeys = maintenanceWork.Select(s => s.ARKey);
            var flows = _workflows.Where(s => firstStepARKeys.Contains(s.Id));
            List<MaintenanceWork> tempResList = new List<MaintenanceWork>();
            foreach (var item in maintenanceWork)
            {
                var temp = flows.FirstOrDefault(s => s.Id == item.ARKey);
                if (temp != null && temp.State == WorkflowState.Finished)
                    tempResList.Add(item);
            }
            List<MaintenanceWorkSimpleDto> tempResDtoList = ObjectMapper.Map<List<MaintenanceWork>, List<MaintenanceWorkSimpleDto>>(tempResList);
            List<MaintenanceWorkSimpleDto> resList = new List<MaintenanceWorkSimpleDto>();


            //筛选第二级审批状态
            var secondStepARKeys = tempResDtoList.Select(s => s.SecondARKey);
            var secondFlows = _workflows.Where(s => secondStepARKeys.Contains(s.Id));
            foreach (var item in tempResDtoList)
            {
                var temp = secondFlows.FirstOrDefault(s => s.Id == item.SecondARKey);
                if (temp != null)
                {
                    item.SecondWorkflowState = temp.State;
                }
                if (input.WorkflowState != WorkflowState.All)
                {
                    if (temp != null && temp.State == input.WorkflowState)
                    {
                        resList.Add(item);
                    }
                }
                else
                {
                    resList = tempResDtoList;
                }
            }
            result.TotalCount = resList.Count;
            result.Items = resList.OrderBy(s => s.SecondWorkflowState).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }

        /// <summary>
        /// 提交维修计划的第二级审批
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SumbitSecondFlow(Guid id)
        {
            //if (id == null || id == Guid.Empty) throw new UserFriendlyException("id有误");
            //var ent = await _maintenanceWorks.GetAsync(id);
            //if (ent == null) throw new UserFriendlyException("此维修作业计划不存在");
            ////TODO  生成工作流

            //var workflowId = await SubmitProcessAsync(id, "MaintenanceWorkSecond", false);

            //if (Guid.Empty != workflowId)
            //{
            //    ent.SecondARKey = workflowId;
            //}
            //await _maintenanceWorks.UpdateAsync(ent);
            return true;
        }


        /// <summary>
        /// 维修计划第二级审批 审批通过
        /// </summary>
        /// <param name="id">审批流id</param>
        /// <param name="state"></param>
        /// <returns></returns>
        //public async Task<bool> FinishSecondFlow(Guid id, WorkflowState state)
        //{
        //    var data = _maintenanceWorks.FirstOrDefault(z => z.SecondARKey == id);
        //    if (data == null) throw new UserFriendlyException("未找到任何要审核的数据");
        //    var skylightPlan = _maintenanceWork.WithDetails().Where(s => s.MaintenanceWorkId == data.Id).Select(s => s.SkylightPlan).ToList();
        //    foreach (var item in skylightPlan)
        //    {
        //        item.PlanState = Enums.PlanState.UnDispatching;
        //        await _skylightPlanRepos.UpdateAsync(item);
        //    }

        //    return true;
        //}

        public async Task<bool> Delete(Guid id)
        {
            // 注：需要删除与天窗计划表里面的关联关系
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要删除的维修计划");
            var maintenanceWorks = _maintenanceWorks.WithDetails(x => x.MaintenanceWorkRltSkylightPlans).Where(x => x.Id == id).FirstOrDefault();
            if (maintenanceWorks == null) throw new UserFriendlyException("当前维修计划不存在");

            //更新天窗计划状态
            var skylight = _maintenanceWorkRltSkylightPlanRepository.WithDetails(x => x.SkylightPlan).Where(x => x.MaintenanceWorkId == id).ToList();
            foreach (var item in skylight)
            {
                item.SkylightPlan.PlanState = PlanState.UnSubmited;
                await _skylightPlanRepos.UpdateAsync(item.SkylightPlan);
            }
            // 删除关联表天窗计划
            if (maintenanceWorks.MaintenanceWorkRltSkylightPlans.Count > 0)
            {
                await _maintenanceWorkRltSkylightPlanRepository.DeleteAsync(x => x.MaintenanceWorkId == id);
            }
            // 删除当前表
            await _maintenanceWorks.DeleteAsync(id);
            return true;
        }

        //获取维修项对应的天窗计划项
        public async Task<MaintenanceWorkDetailDto> GetMaintenanceWork(Guid workflowId)
        {
            MaintenanceWorkDetailDto res = new MaintenanceWorkDetailDto();

            var maintenanceWork = _maintenanceWorks.WithDetails().FirstOrDefault(x => x.ARKey == workflowId);
            if (null == maintenanceWork)
            {
                throw new UserFriendlyException("维修计划不存在");
            }
            var maintenanceWorkRltSkylights = _maintenanceWorkRltSkylightPlanRepository.WithDetails().Where(x => x.MaintenanceWorkId == maintenanceWork.Id).ToList();

            var maintenanceWorkRltWorkTicketlist = new List<MaintenanceWorkRltWorkTicketDto>();

            foreach (var item in maintenanceWorkRltSkylights)
            {
                var stationRltRailway = _stationRltRailways.FirstOrDefault(x => x.RailwayId == item.SkylightPlan.RailwayId && x.StationId == item.SkylightPlan.Station);
                if (stationRltRailway == null)
                {
                    continue;
                }
                var railwayType = stationRltRailway.RailwayType.GetDescription();
                var skylightPlanDto = ObjectMapper.Map<SkylightPlan, SkylightPlanDto>(item.SkylightPlan);
                skylightPlanDto.StationId = item.SkylightPlan.Station;

                var maintenanceWorkRltWorkTicket = new MaintenanceWorkRltWorkTicketDto
                {
                    Id = item.SkylightPlan.Id,
                    PlanIndex = "",
                    Railway = item.SkylightPlan.Railway?.Name,
                    RailwayType = railwayType,
                    Level = item.SkylightPlan.Level,
                    MaintenanceProject = item.MaintenanceWork?.MaintenanceProject,
                    MaintenanceType = item.MaintenanceWork.MaintenanceType.GetDescription(),
                    MaintenanceLocation = GetRepaireLoaction(skylightPlanDto),
                    RepaireDate = item.SkylightPlan.WorkTime.ToShortDateString(),
                    RepaireTime = FormatTime(item.SkylightPlan.WorkTime, item.SkylightPlan.TimeLength),
                    Incidence = item.SkylightPlan.Incidence,
                    TrainInfo = "",
                    WorkOrgAndDutyPerson = item?.WorkOrgAndDutyPerson,
                    CooperationUnit = "",
                    SignOrganization = item?.SignOrganization,
                    FirstTrial = item?.FirstTrial,
                    Remark = item?.Remark,
                    PlanStartTime = item.SkylightPlan.WorkTime,
                    PlanEndTime = item.SkylightPlan.WorkTime.AddMinutes(item.SkylightPlan.TimeLength)
                };
                //关联工作票
                var workTickets = _skylightPlanRltWorkTicket.WithDetails().Where(x => x.SkylightPlanId == item.SkylightPlan.Id).ToList();
                if (workTickets.Count() > 0)
                {
                    foreach (var workTicket in workTickets)
                    {
                        var workTicketSimple = ObjectMapper.Map<WorkTicket, WorkTicketDto>(workTicket.WorkTicket);
                        workTicketSimple.SkylightPlanId = item.SkylightPlan.Id;
                        maintenanceWorkRltWorkTicket.WorkTickets.Add(workTicketSimple);
                    }
                }
                maintenanceWorkRltWorkTicketlist.Add(maintenanceWorkRltWorkTicket);
            }
            res.MaintenanceWorkRltWorkTickets = maintenanceWorkRltWorkTicketlist;

            //关联文件
            var filesList = _maintenanceWorkRltFile.WithDetails().Where(x => x.MaintenanceWorkId == maintenanceWork.Id).ToList();

            var files = (from a in filesList
                         group a by a.RelateFileId into t
                         select new
                         {
                             coverFileId = t.Key,
                             conentFiles = t.Select(s => s.File).ToList()
                         }).ToList();

            foreach (var item in files)
            {
                MaintenanceWorkRltFileSimpleDto fileDto = new MaintenanceWorkRltFileSimpleDto();
                var converFileEnt = filesList.FirstOrDefault(s => s.RelateFileId == item.coverFileId).RelateFile;
                fileDto.CoverFile = ObjectMapper.Map<File.Entities.File, File.Dtos.FileSimpleDto>(converFileEnt);
                fileDto.ContentFiles = ObjectMapper.Map<List<File.Entities.File>, List<File.Dtos.FileSimpleDto>>(item.conentFiles);
                res.MaintenanceWorkRltFileSimples.Add(fileDto);
            }

            return res;
        }

        /// <summary>
        /// 提交审批
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SumbitFirsrFlow(Guid skylightPlanId, List<FileDomainDto> files)
        {
            //2021.01.21 高铁需求变更 不需要添加封面直接添加方案即可
            //foreach (var item in input.MaintenanceWorkRltFiles)
            //{
            //    foreach (var contentFile in item.ContentFiles)
            //    {
            //        // 删除原有关联关系
            //        await _maintenanceWorkRltFile
            //            .DeleteAsync(x => x.MaintenanceWorkId == input.MaintenanceId && x.FileId == contentFile.Id && x.RelateFileId == item.CoverFile.Id);

            //        await CurrentUnitOfWork.SaveChangesAsync();

            //        //保存文件关联关系
            //        var maintenanceWorkRltfile = new MaintenanceWorkRltFile(_generator.Create())
            //        {
            //            MaintenanceWorkId = input.MaintenanceId,
            //            RelateFileId = item.CoverFile?.Id,
            //            FileId = contentFile.Id
            //        };

            //        await _maintenanceWorkRltFile.InsertAsync(maintenanceWorkRltfile);
            //        await CurrentUnitOfWork.SaveChangesAsync();
            //    }
            //}

            //提交审批
            var maintenanceId = _maintenanceWorkRltSkylightPlanRepository.FirstOrDefault(x => x.SkylightPlanId == skylightPlanId)?.MaintenanceWorkId;

            if (maintenanceId == null) return true;
            var workflowId = await SubmitProcessAsync(maintenanceId.GetValueOrDefault(), "MaintenanceWork", files, skylightPlanId);
            var maintenanceWork = _maintenanceWorks.WithDetails().Where(x => x.Id == maintenanceId).FirstOrDefault();

            if (Guid.Empty != workflowId)
            {
                maintenanceWork.ARKey = workflowId;
            }
            await _maintenanceWorks.UpdateAsync(maintenanceWork);

            //更新垂直天窗

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RepaireId">维修计划Id</param>
        /// <param name="key">工作流Key</param>
        /// <param name="files"></param>
        /// <param name="skylightId"></param>
        /// <param name="isFirst">是否为第一次提交</param>
        /// <returns></returns>
        private async Task<Guid> SubmitProcessAsync(Guid RepaireId, string key, List<FileDomainDto> files, Guid skylightId)
        {
            //提交审批
            var maintenanceWorkRltSkylightPlans = _maintenanceWorkRltSkylightPlanRepository
                .WithDetails()
                .Where(x => x.MaintenanceWorkId == RepaireId).ToList();
            var maintenanceWork = _maintenanceWorks.WithDetails().Where(x => x.Id == RepaireId).FirstOrDefault();
            if (maintenanceWork.RepairLevel.Contains("1") && files.Count == 0) throw new UserFriendlyException("I级计划需要上传计划方案");

            var skylightRltWorkTickets = _skylightPlanRltWorkTicket.Where(x => x.SkylightPlanId == skylightId).ToList();
            if (maintenanceWork.RepairLevel.Contains("1") && skylightRltWorkTickets.Count == 0) throw new UserFriendlyException("I级计划需要添加工作票");

            var level = GetLevel(maintenanceWork.RepairLevel);

            foreach (var item in maintenanceWorkRltSkylightPlans)
            {
                var skylightPlan = item.SkylightPlan;
                skylightPlan.PlanState = PlanState.Waitting;
                await _skylightPlanRepos.UpdateAsync(skylightPlan);
            }

            var value = new JObject();
            var organizationName = _organizations.FirstOrDefault(x => x.Id == maintenanceWork.OrganizationId)?.Name;
            var workTime = maintenanceWork.StartTime.ToString("yyyy年MM月dd日");
            //var tableName = organizationName + workTime + "高速铁路维修作业日计划表";

            // 将数据写入excel
            //var fileJObject = await _crPlanManager.JsonToDataTable(rows, tableName, true);
            var fileArray = new JArray();
            foreach (var item in files)
            {
                var fileJObject = new JObject
                {
                    ["id"] = item?.Id,
                    ["name"] = item?.Name,
                    ["type"] = item?.Type,
                    ["url"] = item?.Url,
                    ["size"] = item?.Size
                };
                fileArray.Add(fileJObject);
            }
            value["workTime"] = workTime;
            value["oranization"] = organizationName;
            value["repaireLevel"] = level;
            value["files"] = fileArray;

            //发起流程
            var userId = CurrentUser.Id.GetValueOrDefault();
            var workflow = await _bpmManager.CreateWorkflowByWorkflowTemplateKey(key, value.ToString(), userId);

            if (workflow == null)
            {
                throw new UserFriendlyException("提交失败");
            }

            return workflow.Id;
        }

        /// <summary>
        /// 第一级审批完成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<bool> FirstFinshProcrss(Guid id, WorkflowState state)
        {
            var data = _maintenanceWorks.FirstOrDefault(z => z.ARKey == id);
            if (data == null) throw new UserFriendlyException("未找到任何要审核的数据");
            var skylightPlans = _maintenanceWorkRltSkylightPlanRepository.WithDetails().Where(s => s.MaintenanceWorkId == data.Id).Select(s => s.SkylightPlan).ToList();
            //工作票添加流程审批节点人员信息
            //BpmManager.WorkflowGetProcessMember member = null;
            //if (state == WorkflowState.Finished)
            //{
            //    member = await _bpmManager.ProcessMember(id);
            //}
            foreach (var item in skylightPlans)
            {
                if (state == WorkflowState.Finished)
                {
                    item.PlanState = PlanState.Adopted;

                    //高铁办新增代办通知,即计划审批通过后，代办通知会发出
                    //1、查找天窗对应的工作票
                    var skylightRltWorkTickets = _skylightPlanRltWorkTicket.WithDetails(x => x.WorkTicket).Where(x => x.SkylightPlanId == item.Id).ToList();
                    foreach (var skylightRltWorkTicket in skylightRltWorkTickets)
                    {
                        if (skylightRltWorkTicket.WorkTicketId == null)
                        {
                            return false;
                        }
                        //2、更新工作票
                        var workticket = skylightRltWorkTicket.WorkTicket;
                        //workticket.SafetyDispatchCheckerId = member.SafeMemberId;
                        //workticket.TechnicalCheckerId = member.TechnicalMemberId;
                        await _workTickets.UpdateAsync(workticket);
                        //3、查找工作票对应的配合作业单位
                        var ticketRltCoomparetions = _workTicketRltCooperationUnit.Where(x => x.WorkTicketId == skylightRltWorkTicket.WorkTicketId).ToList();

                        //4、根据组织机构类型进行代办消息发送
                        foreach (var ticketRltCoomparetion in ticketRltCoomparetions)
                        {
                            if (ticketRltCoomparetion.Type == 1)
                            {
                                await PushMessageAsync(skylightRltWorkTicket.WorkTicketId, ticketRltCoomparetion.CooperateWorkShopId, SendModeType.Organization, ticketRltCoomparetion.CreatorId);
                            }
                        }

                    }
                }
                else
                {
                    item.PlanState = PlanState.UnAdopted;
                }
                await _skylightPlanRepos.UpdateAsync(item);
            }

            return true;
        }

        /// <summary>
        /// 删除垂直天窗与维修计划的关联关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> RemoveMaintenanceWorkRltSkylightPlan(RemoveMaintenanceWorkRltSkylightPlanDto input)
        {
            if (Guid.Empty == input.SkylightPlanId || Guid.Empty == input.WorkflowId)
            {
                throw new UserFriendlyException("id 有误");
            }

            var maintenanceWork = _maintenanceWorks.WithDetails().FirstOrDefault(x => x.ARKey == input.WorkflowId);

            var skylightPlan = _skylightPlanRepos.FirstOrDefault(x => x.Id == input.SkylightPlanId);
            skylightPlan.Opinion = input.Opinion;
            skylightPlan.PlanState = PlanState.UnAdopted;//0318修改:驻台联络员退回后天窗计划修改为未批复
            await _skylightPlanRepos.UpdateAsync(skylightPlan);

            await _maintenanceWorkRltSkylightPlanRepository
                .DeleteAsync(x => x.MaintenanceWorkId == maintenanceWork.Id && x.SkylightPlanId == input.SkylightPlanId);
            var maintenanceWorkCount = _maintenanceWorkRltSkylightPlanRepository.Where(x => x.MaintenanceWorkId == maintenanceWork.Id).Count();

            if (maintenanceWorkCount > 1)
            {
                await _maintenanceWorkRltSkylightPlanRepository
                    .DeleteAsync(x => x.SkylightPlanId == skylightPlan.Id && x.MaintenanceWorkId != maintenanceWork.Id);
            }
            else
            {
                await _workflows.DeleteAsync(x => x.Id == maintenanceWork.ARKey);

            }
            return true;
        }

        /// <summary>
        /// 导出维修计划
        /// </summary>
        /// <param name="workflowId">工作流Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<FileStreamResult> ExportMaintenanceWorkPlan(Guid workflowId)
        {

            var maintenanceWork = GetMaintenanceWorkByARKey(workflowId);

            var maintenanceWorkRltSkylightPlans = _maintenanceWorkRltSkylightPlanRepository
                .WithDetails()
                .Where(x => x.MaintenanceWorkId == maintenanceWork.Id).ToList();

            var dataTable = new DataTable();

            //添加表头
            var enumValues = Enum.GetValues(typeof(MaintenanceWorkEnum));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    var dataColumn = new DataColumn(Enum.GetName(typeof(MaintenanceWorkEnum), item));
                    dataTable.Columns.Add(dataColumn);
                }
            }
            //维修等级与给出模板样式一致

            var order = 0;//序号

            //添加数据
            foreach (var item in maintenanceWorkRltSkylightPlans)
            {
                var stationRltRailway = _stationRltRailways.FirstOrDefault(x => x.RailwayId == item.SkylightPlan.RailwayId && x.StationId == item.SkylightPlan.Station);

                if (stationRltRailway == null)
                {
                    continue;
                }

                //var railwayType = stationRltRailway.RailwayType.GetDescription();
                var level = GetLevel(item.SkylightPlan.Level, true);

                var railwayType = item.SkylightPlan?.StationRelateRailwayType.GetDescription();

                var SkylightPlanDto = ObjectMapper.Map<SkylightPlan, SkylightPlanDto>(item.SkylightPlan);
                SkylightPlanDto.StationId = item.SkylightPlan.Station;

                var dataRow = dataTable.NewRow();
                order++;
                dataRow[MaintenanceWorkEnum.序号.ToString()] = order.ToString();
                dataRow[MaintenanceWorkEnum.计划号.ToString()] = " ";
                dataRow[MaintenanceWorkEnum.线别.ToString()] = item.SkylightPlan.Railway.Name;
                dataRow[MaintenanceWorkEnum.等级.ToString()] = level;
                dataRow[MaintenanceWorkEnum.行别.ToString()] = railwayType;
                dataRow[MaintenanceWorkEnum.维修项目.ToString()] = maintenanceWork.MaintenanceProject;
                dataRow[MaintenanceWorkEnum.类型.ToString()] = maintenanceWork.MaintenanceType.GetDescription();
                dataRow[MaintenanceWorkEnum.维修地点.ToString()] = ChecStation(SkylightPlanDto);
                dataRow[MaintenanceWorkEnum.维修日期.ToString()] = item.SkylightPlan.WorkTime.ToShortDateString();
                dataRow[MaintenanceWorkEnum.维修时间.ToString()] = FormatTime(item.SkylightPlan.WorkTime, item.SkylightPlan.TimeLength);
                dataRow[MaintenanceWorkEnum.维修内容及影响范围.ToString()] = item.SkylightPlan.Incidence + "\n" + "登记地点:" + item.SkylightPlan.RegistrationPlace ?? "";
                dataRow[MaintenanceWorkEnum.路用列车信息.ToString()] = "";
                dataRow[MaintenanceWorkEnum.作业单位及负责人.ToString()] = item.WorkOrgAndDutyPerson;
                dataRow[MaintenanceWorkEnum.配合单位.ToString()] = "";
                dataRow[MaintenanceWorkEnum.签收单位.ToString()] = item.SignOrganization;
                dataRow[MaintenanceWorkEnum.初审部门.ToString()] = item.FirstTrial;
                dataRow[MaintenanceWorkEnum.备注.ToString()] = item.Remark;
                dataTable.Rows.Add(dataRow);
            }

            var organizationName = _organizations.FirstOrDefault(x => x.Id == maintenanceWork.OrganizationId)?.Name;
            var workTime = maintenanceWork.StartTime.ToString("yyyy年MM月dd日");
            var tableName = organizationName + workTime + "高速铁路维修作业日计划表";

            //构造维修计划作业表底部数据
            var members = await _bpmManager.ProcessMember(workflowId);
            var bootomData = new MaintenanceWorkPlanBootom();
            if (Guid.Empty != members.SafeMemberId && Guid.Empty != members.TechnicalMemberId && Guid.Empty != members.ChiefMemberId)
            {
                var createUser = await _identityUserManager.GetByIdAsync(maintenanceWork.CreatorId.GetValueOrDefault());
                var safeMember = await _identityUserManager.GetByIdAsync(members.SafeMemberId);
                var technicalMember = await _identityUserManager.GetByIdAsync(members.TechnicalMemberId);
                var chiefMember = await _identityUserManager.GetByIdAsync(members.ChiefMemberId);
                bootomData.CreateMember = createUser?.Name;
                bootomData.SafeMember = safeMember?.Name;
                bootomData.ChiefMember = chiefMember?.Name;
                bootomData.TechnicalMember = technicalMember?.Name;
            }

            var sbuf = ExcelHelper.DataTableToExcel(dataTable, tableName + ".xlsx", null, tableName, true, bootomData);
            var stream = new MemoryStream(sbuf);

            var result = (FileStreamResult)null;
            try
            {
                result = GetFile(stream, tableName + ".xlsx");
            }
            catch (Exception)
            {

                throw new UserFriendlyException("下载失败");
            }

            return await Task.FromResult(result);
        }
        /// <summary>
        /// 维修计划表底部数据
        /// </summary>
        public class MaintenanceWorkPlanBootom
        {
            /// <summary>
            /// 编制人
            /// </summary>
            public string CreateMember { get; set; } = "";
            /// <summary>
            /// 安全科负责人
            /// </summary>
            public string SafeMember { get; set; } = "";
            /// <summary>
            ///技术科负责人
            /// </summary>
            public string TechnicalMember { get; set; } = "";
            /// <summary>
            ///主管段长
            /// </summary>
            public string ChiefMember { get; set; } = "";
        }

        /// <summary>
        /// 导出工作票
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="isWorkflowId">True:工作流id False:天窗id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<FileStreamResult> ExportWorkTicket(Guid workflowId, bool isWorkflowId = true)
        {
            var StreamList = new Dictionary<string, Stream>();
            var startTime = "";
            var endTime = "";
            var organizationName = "";

            var workTicetRltCoomperations = new List<ExportCoomperaSimpleDto>();
            var worflowid = workflowId;

            if (!isWorkflowId)
            {
                var skylightRltWorkflowId = _maintenanceWorkRltSkylightPlanRepository.WithDetails(x => x.MaintenanceWork).FirstOrDefault(x => x.SkylightPlanId == workflowId).MaintenanceWork.ARKey;
                if (skylightRltWorkflowId != null)
                {
                    worflowid = skylightRltWorkflowId.GetValueOrDefault();
                }
                else
                {
                    throw new UserFriendlyException("未找到审批流程");
                }
            }

            var members = await _bpmManager.ProcessMember(worflowid);

            var safeMember = members.SafeMemberId == Guid.Empty ? null : await _identityUserManager.GetByIdAsync(members.SafeMemberId);
            var technicalMember = members.TechnicalMemberId == Guid.Empty ? null : await _identityUserManager.GetByIdAsync(members.TechnicalMemberId);

            //获取天窗计划
            if (isWorkflowId)
            {
                var maintenanceWork = GetMaintenanceWorkByARKey(workflowId);
                var maintenanceWorkRltSkylightPlans = _maintenanceWorkRltSkylightPlanRepository
                 .WithDetails(x => x.SkylightPlan)
                 .Where(x => x.MaintenanceWorkId == maintenanceWork.Id).ToList();
                var skylightPlans = maintenanceWorkRltSkylightPlans.Select(x => x.SkylightPlan).ToList();
                organizationName = _organizations.FirstOrDefault(x => x.Id == maintenanceWork.OrganizationId)?.Name;

                var index = 0;
                foreach (var item in skylightPlans)
                {
                    index++;
                    var skylightRltTickets = _skylightRltTicket.WithDetails(
                            x => x.WorkTicket.SafetyDispatchChecker,
                            z => z.WorkTicket.TechnicalChecker,
                            y => y.SkylightPlan)
                        .Where(x => x.SkylightPlanId == item.Id)
                        .ToList();

                    foreach (var tickets in skylightRltTickets)
                    {
                        var workTicketRltCooperationUnits = _workTicketRltCooperationUnit.Where(x => x.WorkTicketId == tickets.WorkTicketId).ToList();

                        foreach (var workTicketRltCooperationUnit in workTicketRltCooperationUnits)
                        {
                            var workTicetRltCoomperationDto = new ExportCoomperaSimpleDto
                            {
                                WorkTicketId = tickets.WorkTicketId,
                                OrganizationName = _organizations.FirstOrDefault(x => x.Id == workTicketRltCooperationUnit.CooperateWorkShopId)?.Name
                            };
                            workTicetRltCoomperations.Add(workTicetRltCoomperationDto);
                        }
                    }
                    StreamList.Add("工作票-" + index + ".docx", WordHelper.CreateDocxTable(item, skylightRltTickets, workTicetRltCoomperations, organizationName, safeMember?.Name, technicalMember?.Name));
                }
                startTime = maintenanceWork.StartTime.ToString("yyyy年MM月dd日");
                endTime = maintenanceWork.EndTime.ToString("yyyy年MM月dd日");
            }
            else
            {
                var skylightRltTickets = _skylightRltTicket.WithDetails().Where(x => x.SkylightPlanId == workflowId).ToList();
                foreach (var tickets in skylightRltTickets)
                {
                    var workTicketRltCooperationUnits = _workTicketRltCooperationUnit.Where(x => x.WorkTicketId == tickets.WorkTicketId).ToList();

                    foreach (var workTicketRltCooperationUnit in workTicketRltCooperationUnits)
                    {
                        var workTicetRltCoomperationDto = new ExportCoomperaSimpleDto
                        {
                            WorkTicketId = tickets.WorkTicketId,
                            OrganizationName = _organizations.FirstOrDefault(x => x.Id == workTicketRltCooperationUnit.CooperateWorkShopId)?.Name
                        };
                        workTicetRltCoomperations.Add(workTicetRltCoomperationDto);
                    }
                }
                var skylight = _skylightPlanRepos.FirstOrDefault(x => x.Id == workflowId);
                organizationName = _organizations.FirstOrDefault(x => x.Id == skylight.WorkUnit)?.Name;
                StreamList.Add("工作票-" + ".docx", WordHelper.CreateDocxTable(skylight, skylightRltTickets, workTicetRltCoomperations, organizationName, safeMember?.Name, technicalMember?.Name));
                startTime = skylight.WorkTime.ToString("yyyy年MM月dd日");
            }


            var stream = WordHelper.CreateDocxRAR(StreamList);

            var workTime = isWorkflowId ? startTime + "-" + endTime : startTime;
            var tableName = organizationName + workTime + "工作票";

            var result = (FileStreamResult)null;

            try
            {
                result = GetFile(stream, tableName + ".zip");
            }
            catch (Exception)
            {

                throw new UserFriendlyException("下载失败");
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 导出计划方案
        /// </summary>
        /// <param name="worlflowId"></param>
        /// <returns></returns>
        public Task<FileStreamResult> ExportPlan(Guid worlflowId)
        {
            var maintenanceWork = GetMaintenanceWorkByARKey(worlflowId);

            var maintenanceWorkRltFile = _maintenanceWorkRltFile.Where(x => x.MaintenanceWorkId == maintenanceWork.Id);
            throw new NotImplementedException();
        }
        /// <summary>
        /// 通过工作流Id 获取维修计划
        /// </summary>
        /// <param name="workflowId">工作流Id</param>
        /// <returns></returns>

        private MaintenanceWork GetMaintenanceWorkByARKey(Guid workflowId)
        {
            if (Guid.Empty == workflowId)
            {
                throw new UserFriendlyException("id is null");
            }

            var maintenanceWork = _maintenanceWorks.WithDetails().Where(x => x.ARKey == workflowId || x.SecondARKey == workflowId).FirstOrDefault();

            if (null == maintenanceWork)
            {
                throw new UserFriendlyException("维修计划不存在");
            }

            return maintenanceWork;
        }

        /// <summary>
        /// 返回文件流
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private FileStreamResult GetFile(Stream buffer, string tableName)
        {
            _httpContextAccessor.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            var actionresult = new FileStreamResult(buffer, new MediaTypeHeaderValue("application/octet-stream"));
            actionresult.FileDownloadName = tableName;
            return actionresult;
        }

        private string GetLevel(string level, bool isTemplate = false)
        {
            var levelList = level.Split(",");
            var levels = new List<string>();
            for (var i = 0; i < levelList.Length; i++)
            {
                if (levelList[i] == "1")
                {
                    if (isTemplate)
                    {
                        levels.Add("I");
                        break;
                    }
                    else
                    {
                        levels.Add("天窗点内I级维修");
                    }
                }
                else if (levelList[i] == "2")
                {
                    if (isTemplate)
                    {
                        levels.Add("II");
                    }
                    else
                    {
                        levels.Add("天窗点内II级维修");
                    }
                }
                else if (levelList[i] == "3")
                {
                    levels.Add("天窗点外I级维修");
                }
                else if (levelList[i] == "4")
                {
                    levels.Add("天窗点外II级维修");
                }
            }
            var levelInfo = "";
            foreach (var item in levels)
            {
                levelInfo += item + "、";
            }
            return levelInfo.TrimEnd('、');
        }

        public async Task PushMessageAsync(Guid? planId, Guid? userId, SendModeType sendModeType, Guid? creatorId, bool isSponsor = true)
        {
            //获取发起用户的组织机构信息
            var organization = await _identityUserManager.GetOrganizationsAsync(creatorId);
            var organizationName = organization.FirstOrDefault()?.Name;
            var user = await _identityUserManager.GetByIdAsync(creatorId.GetValueOrDefault());
            var userName = user?.Name;
            // 创建消息实例
            //message.SendType = sendModeType; //设置发送模式为用户，还有其他模式可以选择哦
            if (sendModeType == SendModeType.Organization && isSponsor)
            {
                var organizationRltUsers = await _identityUserManager.GetUsersInOrganizationAsync(userId.GetValueOrDefault());
                foreach (var item in organizationRltUsers)
                {
                    await SendMessageAsync(item.Id, planId, organizationName, userName, creatorId, isSponsor, userId);
                }
            }
            else
            {
                await SendMessageAsync(userId, planId, organizationName, userName, creatorId, isSponsor, null);
            }
        }

        private async Task SendMessageAsync(Guid? userId, Guid? planId, string organizationName, string userName, Guid? creatorId, bool isSponsor, Guid? organizationId)
        {
            var message = new NoticeMessage();
            message.SetUserId(userId.GetValueOrDefault());// 配置接收此消息的人员id
            var messagepData = new NoticeMessageContent
            {
                Content = JsonConvert.SerializeObject(new
                {
                    PlanId = planId.ToString(),
                    PlanContent = organizationName + "(" + userName + ")" + (isSponsor ? "提交的垂直天窗计划需要配合作业" : "已经确认你的配合作业请求"),
                    Type = "CrPlan",
                    Sponsor = isSponsor,
                    OrgId = organizationId
                }),
                SponsorId = creatorId.GetValueOrDefault()
            };
            // 给消息添加消息内容
            message.SetContent(messagepData);
            ////调用接口，发送消息,发送时需要调用GetBinary方法，将消息转换成二进制数据
            await _messageNotice.PushAsync(message.GetBinary());
        }

    }
}
