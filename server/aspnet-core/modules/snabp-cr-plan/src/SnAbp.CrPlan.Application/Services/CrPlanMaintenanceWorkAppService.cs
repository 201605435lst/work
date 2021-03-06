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
        /// ??????????????????
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MaintenanceWorkDto> Create(MaintenanceWorkCreateDto input)
        {
            //if (input.SkylightPlanIds.Count == 0)
            //{
            //    throw new UserFriendlyException("????????????????????????");
            //}

            //var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //var dto = ObjectMapper.Map<MaintenanceWorkCreateDto, MaintenanceWorkDto>(input);

            //dto.Id = _generator.Create();
            //var maintenancePlan = ObjectMapper.Map<MaintenanceWorkDto, MaintenanceWork>(dto);

            ////???????????????????????????????????????????????????
            //var levels = "";

            //foreach (var item in input.SkylightPlanIds)
            //{
            //    var skylightEnt = _skylightPlanRepos.Where(x => x.Id == item).FirstOrDefault();
            //    //?????????????????????????????????
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

            ////?????????????????????????????????Y:????????????????????????
            //if (level.TrimEnd(',').Contains("1") && input.MaintenanceWorkRltPlanFiles.Count == 0)
            //{
            //    throw new UserFriendlyException("??????I?????????,???????????????????????????");
            //}

            ////?????????????????????????????????
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

            //maintenancePlan.MaintenanceProject = "????????????";
            //maintenancePlan.RepairTagId = RepairTagId;

            //await _maintenanceWorks.InsertAsync(maintenancePlan);

            //await CurrentUnitOfWork.SaveChangesAsync();
            ////????????????????????????
            //var maintenanceWorkSubmitDto = new MaintenanceWorkSubmitDto
            //{
            //    MaintenanceId = maintenancePlan.Id,
            //    MaintenanceWorkRltFiles = input?.MaintenanceWorkRltPlanFiles
            //};
            //var submitSuccess = await SumbitFirsrFlow(maintenanceWorkSubmitDto);
            //if (!submitSuccess)
            //{
            //    throw new UserFriendlyException("??????????????????");
            //}

            //return ObjectMapper.Map<MaintenanceWork, MaintenanceWorkDto>(maintenancePlan);
            throw new NotImplementedException();
        }

        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="time"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string FormatTime(DateTime time, int length)
        {
            var endTime = time.AddMinutes(length);
            return $"{time.Hour.ToString().PadLeft(2, '0')}:{time.Minute.ToString().PadLeft(2, '0')}-{endTime.Hour.ToString().PadLeft(2, '0')}:{endTime.Minute.ToString().PadLeft(2, '0')}({length}??????)";
        }

        /// <summary>
        /// ??????????????????
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
                    repaireLocation = "??????:" + "\n" + ChecStation(skylightPlan);
                    break;
                case RelateRailwayType.DOWNLINK:
                    repaireLocation = "??????:" + "\n" + ChecStation(skylightPlan);
                    break;
                case RelateRailwayType.UPANDDOWN:
                    var rltRailway = _stationRltRailways.Where(x =>
                        x.RailwayId == skylightPlan.RailwayId && x.StationId == skylightPlan.StationId).ToList();
                    foreach (var stationRltRailway in rltRailway)
                    {
                        switch (stationRltRailway.RailwayType)
                        {
                            case RelateRailwayType.UPLINK:
                                repaireLocation = "??????:" + "\n" + ChecStation(skylightPlan);
                                break;
                            case RelateRailwayType.DOWNLINK:
                                repaireLocation = "??????:" + "\n" + ChecStation(skylightPlan);
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
        /// ?????????????????????
        /// </summary>
        private string ChecStation(SkylightPlanDto skylightPlan)
        {
            var repaireLocation = "";
            var rltRailway = _stationRltRailways.FirstOrDefault(x =>
                x.RailwayId == skylightPlan.RailwayId && x.StationId == skylightPlan.StationId);

            var station = _stations.FirstOrDefault(x => x.Id == rltRailway.StationId);
            if (station != null && station.Type == 1)//??????
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
                repaireLocation = rltRailway.Station.Name;// + "\n" + rltRailway.KMMark;//??????
            }
            return repaireLocation;
        }
        public async Task<List<MaintenanceWorkDto>> Get(Guid id)
        {
            List<MaintenanceWorkDto> res = new List<MaintenanceWorkDto>();
            return res;
        }

        /// <summary>
        /// ?????????????????????????????????
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

            //???????????????????????????
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
        /// ???????????????????????????????????????
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

            //????????????????????????????????????
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


            //???????????????????????????
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
        /// ????????????????????????????????????
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SumbitSecondFlow(Guid id)
        {
            //if (id == null || id == Guid.Empty) throw new UserFriendlyException("id??????");
            //var ent = await _maintenanceWorks.GetAsync(id);
            //if (ent == null) throw new UserFriendlyException("??????????????????????????????");
            ////TODO  ???????????????

            //var workflowId = await SubmitProcessAsync(id, "MaintenanceWorkSecond", false);

            //if (Guid.Empty != workflowId)
            //{
            //    ent.SecondARKey = workflowId;
            //}
            //await _maintenanceWorks.UpdateAsync(ent);
            return true;
        }


        /// <summary>
        /// ??????????????????????????? ????????????
        /// </summary>
        /// <param name="id">?????????id</param>
        /// <param name="state"></param>
        /// <returns></returns>
        //public async Task<bool> FinishSecondFlow(Guid id, WorkflowState state)
        //{
        //    var data = _maintenanceWorks.FirstOrDefault(z => z.SecondARKey == id);
        //    if (data == null) throw new UserFriendlyException("?????????????????????????????????");
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
            // ?????????????????????????????????????????????????????????
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("?????????????????????????????????");
            var maintenanceWorks = _maintenanceWorks.WithDetails(x => x.MaintenanceWorkRltSkylightPlans).Where(x => x.Id == id).FirstOrDefault();
            if (maintenanceWorks == null) throw new UserFriendlyException("???????????????????????????");

            //????????????????????????
            var skylight = _maintenanceWorkRltSkylightPlanRepository.WithDetails(x => x.SkylightPlan).Where(x => x.MaintenanceWorkId == id).ToList();
            foreach (var item in skylight)
            {
                item.SkylightPlan.PlanState = PlanState.UnSubmited;
                await _skylightPlanRepos.UpdateAsync(item.SkylightPlan);
            }
            // ???????????????????????????
            if (maintenanceWorks.MaintenanceWorkRltSkylightPlans.Count > 0)
            {
                await _maintenanceWorkRltSkylightPlanRepository.DeleteAsync(x => x.MaintenanceWorkId == id);
            }
            // ???????????????
            await _maintenanceWorks.DeleteAsync(id);
            return true;
        }

        //???????????????????????????????????????
        public async Task<MaintenanceWorkDetailDto> GetMaintenanceWork(Guid workflowId)
        {
            MaintenanceWorkDetailDto res = new MaintenanceWorkDetailDto();

            var maintenanceWork = _maintenanceWorks.WithDetails().FirstOrDefault(x => x.ARKey == workflowId);
            if (null == maintenanceWork)
            {
                throw new UserFriendlyException("?????????????????????");
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
                //???????????????
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

            //????????????
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
        /// ????????????
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SumbitFirsrFlow(Guid skylightPlanId, List<FileDomainDto> files)
        {
            //2021.01.21 ?????????????????? ?????????????????????????????????????????????
            //foreach (var item in input.MaintenanceWorkRltFiles)
            //{
            //    foreach (var contentFile in item.ContentFiles)
            //    {
            //        // ????????????????????????
            //        await _maintenanceWorkRltFile
            //            .DeleteAsync(x => x.MaintenanceWorkId == input.MaintenanceId && x.FileId == contentFile.Id && x.RelateFileId == item.CoverFile.Id);

            //        await CurrentUnitOfWork.SaveChangesAsync();

            //        //????????????????????????
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

            //????????????
            var maintenanceId = _maintenanceWorkRltSkylightPlanRepository.FirstOrDefault(x => x.SkylightPlanId == skylightPlanId)?.MaintenanceWorkId;

            if (maintenanceId == null) return true;
            var workflowId = await SubmitProcessAsync(maintenanceId.GetValueOrDefault(), "MaintenanceWork", files, skylightPlanId);
            var maintenanceWork = _maintenanceWorks.WithDetails().Where(x => x.Id == maintenanceId).FirstOrDefault();

            if (Guid.Empty != workflowId)
            {
                maintenanceWork.ARKey = workflowId;
            }
            await _maintenanceWorks.UpdateAsync(maintenanceWork);

            //??????????????????

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RepaireId">????????????Id</param>
        /// <param name="key">?????????Key</param>
        /// <param name="files"></param>
        /// <param name="skylightId"></param>
        /// <param name="isFirst">????????????????????????</param>
        /// <returns></returns>
        private async Task<Guid> SubmitProcessAsync(Guid RepaireId, string key, List<FileDomainDto> files, Guid skylightId)
        {
            //????????????
            var maintenanceWorkRltSkylightPlans = _maintenanceWorkRltSkylightPlanRepository
                .WithDetails()
                .Where(x => x.MaintenanceWorkId == RepaireId).ToList();
            var maintenanceWork = _maintenanceWorks.WithDetails().Where(x => x.Id == RepaireId).FirstOrDefault();
            if (maintenanceWork.RepairLevel.Contains("1") && files.Count == 0) throw new UserFriendlyException("I?????????????????????????????????");

            var skylightRltWorkTickets = _skylightPlanRltWorkTicket.Where(x => x.SkylightPlanId == skylightId).ToList();
            if (maintenanceWork.RepairLevel.Contains("1") && skylightRltWorkTickets.Count == 0) throw new UserFriendlyException("I??????????????????????????????");

            var level = GetLevel(maintenanceWork.RepairLevel);

            foreach (var item in maintenanceWorkRltSkylightPlans)
            {
                var skylightPlan = item.SkylightPlan;
                skylightPlan.PlanState = PlanState.Waitting;
                await _skylightPlanRepos.UpdateAsync(skylightPlan);
            }

            var value = new JObject();
            var organizationName = _organizations.FirstOrDefault(x => x.Id == maintenanceWork.OrganizationId)?.Name;
            var workTime = maintenanceWork.StartTime.ToString("yyyy???MM???dd???");
            //var tableName = organizationName + workTime + "????????????????????????????????????";

            // ???????????????excel
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

            //????????????
            var userId = CurrentUser.Id.GetValueOrDefault();
            var workflow = await _bpmManager.CreateWorkflowByWorkflowTemplateKey(key, value.ToString(), userId);

            if (workflow == null)
            {
                throw new UserFriendlyException("????????????");
            }

            return workflow.Id;
        }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<bool> FirstFinshProcrss(Guid id, WorkflowState state)
        {
            var data = _maintenanceWorks.FirstOrDefault(z => z.ARKey == id);
            if (data == null) throw new UserFriendlyException("?????????????????????????????????");
            var skylightPlans = _maintenanceWorkRltSkylightPlanRepository.WithDetails().Where(s => s.MaintenanceWorkId == data.Id).Select(s => s.SkylightPlan).ToList();
            //?????????????????????????????????????????????
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

                    //???????????????????????????,????????????????????????????????????????????????
                    //1?????????????????????????????????
                    var skylightRltWorkTickets = _skylightPlanRltWorkTicket.WithDetails(x => x.WorkTicket).Where(x => x.SkylightPlanId == item.Id).ToList();
                    foreach (var skylightRltWorkTicket in skylightRltWorkTickets)
                    {
                        if (skylightRltWorkTicket.WorkTicketId == null)
                        {
                            return false;
                        }
                        //2??????????????????
                        var workticket = skylightRltWorkTicket.WorkTicket;
                        //workticket.SafetyDispatchCheckerId = member.SafeMemberId;
                        //workticket.TechnicalCheckerId = member.TechnicalMemberId;
                        await _workTickets.UpdateAsync(workticket);
                        //3?????????????????????????????????????????????
                        var ticketRltCoomparetions = _workTicketRltCooperationUnit.Where(x => x.WorkTicketId == skylightRltWorkTicket.WorkTicketId).ToList();

                        //4???????????????????????????????????????????????????
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
        /// ????????????????????????????????????????????????
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> RemoveMaintenanceWorkRltSkylightPlan(RemoveMaintenanceWorkRltSkylightPlanDto input)
        {
            if (Guid.Empty == input.SkylightPlanId || Guid.Empty == input.WorkflowId)
            {
                throw new UserFriendlyException("id ??????");
            }

            var maintenanceWork = _maintenanceWorks.WithDetails().FirstOrDefault(x => x.ARKey == input.WorkflowId);

            var skylightPlan = _skylightPlanRepos.FirstOrDefault(x => x.Id == input.SkylightPlanId);
            skylightPlan.Opinion = input.Opinion;
            skylightPlan.PlanState = PlanState.UnAdopted;//0318??????:??????????????????????????????????????????????????????
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
        /// ??????????????????
        /// </summary>
        /// <param name="workflowId">?????????Id</param>
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

            //????????????
            var enumValues = Enum.GetValues(typeof(MaintenanceWorkEnum));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    var dataColumn = new DataColumn(Enum.GetName(typeof(MaintenanceWorkEnum), item));
                    dataTable.Columns.Add(dataColumn);
                }
            }
            //???????????????????????????????????????

            var order = 0;//??????

            //????????????
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
                dataRow[MaintenanceWorkEnum.??????.ToString()] = order.ToString();
                dataRow[MaintenanceWorkEnum.?????????.ToString()] = " ";
                dataRow[MaintenanceWorkEnum.??????.ToString()] = item.SkylightPlan.Railway.Name;
                dataRow[MaintenanceWorkEnum.??????.ToString()] = level;
                dataRow[MaintenanceWorkEnum.??????.ToString()] = railwayType;
                dataRow[MaintenanceWorkEnum.????????????.ToString()] = maintenanceWork.MaintenanceProject;
                dataRow[MaintenanceWorkEnum.??????.ToString()] = maintenanceWork.MaintenanceType.GetDescription();
                dataRow[MaintenanceWorkEnum.????????????.ToString()] = ChecStation(SkylightPlanDto);
                dataRow[MaintenanceWorkEnum.????????????.ToString()] = item.SkylightPlan.WorkTime.ToShortDateString();
                dataRow[MaintenanceWorkEnum.????????????.ToString()] = FormatTime(item.SkylightPlan.WorkTime, item.SkylightPlan.TimeLength);
                dataRow[MaintenanceWorkEnum.???????????????????????????.ToString()] = item.SkylightPlan.Incidence + "\n" + "????????????:" + item.SkylightPlan.RegistrationPlace ?? "";
                dataRow[MaintenanceWorkEnum.??????????????????.ToString()] = "";
                dataRow[MaintenanceWorkEnum.????????????????????????.ToString()] = item.WorkOrgAndDutyPerson;
                dataRow[MaintenanceWorkEnum.????????????.ToString()] = "";
                dataRow[MaintenanceWorkEnum.????????????.ToString()] = item.SignOrganization;
                dataRow[MaintenanceWorkEnum.????????????.ToString()] = item.FirstTrial;
                dataRow[MaintenanceWorkEnum.??????.ToString()] = item.Remark;
                dataTable.Rows.Add(dataRow);
            }

            var organizationName = _organizations.FirstOrDefault(x => x.Id == maintenanceWork.OrganizationId)?.Name;
            var workTime = maintenanceWork.StartTime.ToString("yyyy???MM???dd???");
            var tableName = organizationName + workTime + "????????????????????????????????????";

            //???????????????????????????????????????
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

                throw new UserFriendlyException("????????????");
            }

            return await Task.FromResult(result);
        }
        /// <summary>
        /// ???????????????????????????
        /// </summary>
        public class MaintenanceWorkPlanBootom
        {
            /// <summary>
            /// ?????????
            /// </summary>
            public string CreateMember { get; set; } = "";
            /// <summary>
            /// ??????????????????
            /// </summary>
            public string SafeMember { get; set; } = "";
            /// <summary>
            ///??????????????????
            /// </summary>
            public string TechnicalMember { get; set; } = "";
            /// <summary>
            ///????????????
            /// </summary>
            public string ChiefMember { get; set; } = "";
        }

        /// <summary>
        /// ???????????????
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="isWorkflowId">True:?????????id False:??????id</param>
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
                    throw new UserFriendlyException("?????????????????????");
                }
            }

            var members = await _bpmManager.ProcessMember(worflowid);

            var safeMember = members.SafeMemberId == Guid.Empty ? null : await _identityUserManager.GetByIdAsync(members.SafeMemberId);
            var technicalMember = members.TechnicalMemberId == Guid.Empty ? null : await _identityUserManager.GetByIdAsync(members.TechnicalMemberId);

            //??????????????????
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
                    StreamList.Add("?????????-" + index + ".docx", WordHelper.CreateDocxTable(item, skylightRltTickets, workTicetRltCoomperations, organizationName, safeMember?.Name, technicalMember?.Name));
                }
                startTime = maintenanceWork.StartTime.ToString("yyyy???MM???dd???");
                endTime = maintenanceWork.EndTime.ToString("yyyy???MM???dd???");
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
                StreamList.Add("?????????-" + ".docx", WordHelper.CreateDocxTable(skylight, skylightRltTickets, workTicetRltCoomperations, organizationName, safeMember?.Name, technicalMember?.Name));
                startTime = skylight.WorkTime.ToString("yyyy???MM???dd???");
            }


            var stream = WordHelper.CreateDocxRAR(StreamList);

            var workTime = isWorkflowId ? startTime + "-" + endTime : startTime;
            var tableName = organizationName + workTime + "?????????";

            var result = (FileStreamResult)null;

            try
            {
                result = GetFile(stream, tableName + ".zip");
            }
            catch (Exception)
            {

                throw new UserFriendlyException("????????????");
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// ??????????????????
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
        /// ???????????????Id ??????????????????
        /// </summary>
        /// <param name="workflowId">?????????Id</param>
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
                throw new UserFriendlyException("?????????????????????");
            }

            return maintenanceWork;
        }

        /// <summary>
        /// ???????????????
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
                        levels.Add("????????????I?????????");
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
                        levels.Add("????????????II?????????");
                    }
                }
                else if (levelList[i] == "3")
                {
                    levels.Add("????????????I?????????");
                }
                else if (levelList[i] == "4")
                {
                    levels.Add("????????????II?????????");
                }
            }
            var levelInfo = "";
            foreach (var item in levels)
            {
                levelInfo += item + "???";
            }
            return levelInfo.TrimEnd('???');
        }

        public async Task PushMessageAsync(Guid? planId, Guid? userId, SendModeType sendModeType, Guid? creatorId, bool isSponsor = true)
        {
            //???????????????????????????????????????
            var organization = await _identityUserManager.GetOrganizationsAsync(creatorId);
            var organizationName = organization.FirstOrDefault()?.Name;
            var user = await _identityUserManager.GetByIdAsync(creatorId.GetValueOrDefault());
            var userName = user?.Name;
            // ??????????????????
            //message.SendType = sendModeType; //???????????????????????????????????????????????????????????????
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
            message.SetUserId(userId.GetValueOrDefault());// ??????????????????????????????id
            var messagepData = new NoticeMessageContent
            {
                Content = JsonConvert.SerializeObject(new
                {
                    PlanId = planId.ToString(),
                    PlanContent = organizationName + "(" + userName + ")" + (isSponsor ? "?????????????????????????????????????????????" : "????????????????????????????????????"),
                    Type = "CrPlan",
                    Sponsor = isSponsor,
                    OrgId = organizationId
                }),
                SponsorId = creatorId.GetValueOrDefault()
            };
            // ???????????????????????????
            message.SetContent(messagepData);
            ////???????????????????????????,?????????????????????GetBinary??????????????????????????????????????????
            await _messageNotice.PushAsync(message.GetBinary());
        }

    }
}
