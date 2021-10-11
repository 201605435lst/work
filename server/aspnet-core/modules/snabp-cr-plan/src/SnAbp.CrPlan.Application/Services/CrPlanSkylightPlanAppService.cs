using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.OpenXml4Net.OPC;
using NPOI.XWPF.UserModel;
using SnAbp.Basic.Entities;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.Services;
using SnAbp.CrPlan.Authorization;
using SnAbp.CrPlan.Dto.AlterRecord;
using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Enumer;
using SnAbp.CrPlan.Enums;
using SnAbp.CrPlan.IServices.AlterRecord;
using SnAbp.CrPlan.IServices.SkylightPlan;
using SnAbp.File;
using SnAbp.Identity;
using SnAbp.Message.MessageDefine;
using SnAbp.Message.Notice;
using SnAbp.Message.Notice.Entities;
using SnAbp.Resource.Entities;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.CrPlan.Services
{
    /// <summary>
    /// 天窗、其他计划
    /// </summary>
    [Authorize]
    public class CrPlanSkylightPlanAppService : CrPlanAppService, ICrPlanSkylightPlanAppService, ITransientDependency
    {
        private readonly IRepository<SkylightPlan, Guid> _skylightPlanRepository;    //天窗计划
        private readonly IRepository<PlanDetail, Guid> _planDetailRepository;    //计划详细表
        private readonly IRepository<PlanRelateEquipment, Guid> _planRelateEquipmentRepository;  //计划内容关联设备
        private readonly IRepository<EquipmentTestResult, Guid> _equipmentTestRepository;    //设备测试项
        private readonly IRepository<RepairUser, Guid> _repairUserRepository;    //检修人员
        private readonly IRepository<DailyPlan, Guid> _dailyPlanRepository;  //日计划
        private readonly IRepository<YearMonthPlan, Guid> _yearMonthPlanRepository;  //年月表
        private readonly IRepository<Station, Guid> _stationRepository;  //工点
        private readonly IRepository<InstallationSite, Guid> _installationSiteRepository;    //机房
        private readonly IRepository<SkylightPlanRltInstallationSite, Guid> _skyRltInstallationSiteRepository;    //机房关联表
        private readonly IRepository<Equipment, Guid> _equipmentRepository;  //设备
        private readonly OrganizationManager _organization;//组织机构
        private readonly IRepository<Organization, Guid> _organizationRespository;//组织机构
        private readonly IRepository<WorkOrder, Guid> _workOrder;
        private readonly IRepository<WorkOrganization, Guid> _workOrganization;//作业单位
        private readonly IRepository<YearMonthPlanTestItem, Guid> _ymPlanTest;
        private readonly IRepository<RepairItemRltComponentCategory, Guid> _repDetailIfd; //维修项对应IFD
        private readonly IRepository<SnAbp.File.Entities.File, Guid> _fileRepository;
        private readonly IRepository<ComponentCategory, Guid> _componentCategoryRepository;
        private readonly ICrPlanAlterRecordAppService _alterRecordService;
        //IRepository<ComponentCategory, Guid> componentCategoryRepository
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private readonly IRepository<WorkTicket, Guid> _workTickets;
        private readonly IRepository<SkylightPlanRltWorkTicket, Guid> _skylightRltTicket;
        private readonly IRepository<MaintenanceWorkRltSkylightPlan, Guid> _maintenanceWorkRltSkylightPlanRepository;//垂直天窗关联维修作业
        private readonly IRepository<MaintenanceWork, Guid> _maintenanceWorks;
        private readonly IRepository<MaintenanceWorkRltFile, Guid> _maintenanceWorkRltFile;
        private readonly IRepository<SkylightPlanRltWorkTicket, Guid> _skylightPlanRltWorkTicket;
        private readonly IRepository<Workflow, Guid> _workflowRepository;
        private readonly IMessageNoticeProvider _messageNotice;
        private readonly IRepository<WorkTicketRltCooperationUnit, Guid> _workTicketRltCooperationUnit;
        private readonly IRepository<Notice, Guid> _notices;
        private readonly CrPlanMaintenanceWorkAppService _crPlanMaintenanceWorkAppService;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly BpmManager bpmManager;

        public CrPlanSkylightPlanAppService(IRepository<SkylightPlan, Guid> skylightPlanRepo, IRepository<PlanDetail, Guid> planDetailRepo,
            IRepository<PlanRelateEquipment, Guid> planRelateEquipmentRepo,
            IRepository<EquipmentTestResult, Guid> equipmentTestRepo,
            IRepository<RepairUser, Guid> repairUserRepo,
            IRepository<DailyPlan, Guid> dailyPlanRepo,
            IRepository<YearMonthPlan, Guid> yearMonthPlanRepo,
            IRepository<Station, Guid> stationRepo,
            IRepository<InstallationSite, Guid> installationSiteRepo,
            IRepository<SkylightPlanRltInstallationSite, Guid> skyRltInstallationSiteRepository,
            IRepository<Equipment, Guid> equipmentRepo,
            OrganizationManager organizationRepo,
            IRepository<Organization, Guid> organizationRespository,
            IRepository<WorkOrder, Guid> workOrderRepo,
            IRepository<WorkOrganization, Guid> workOrganization,
            IRepository<YearMonthPlanTestItem, Guid> yearMonthPlanTestItems,
            IRepository<RepairItemRltComponentCategory, Guid> repDetailIfd,
            IRepository<SnAbp.File.Entities.File, Guid> fileRepository,
            IRepository<ComponentCategory, Guid> componentCategoryRepository,
            ICrPlanAlterRecordAppService alterRecordService,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IRepository<WorkTicket, Guid> workTickets,
            IRepository<SkylightPlanRltWorkTicket, Guid> skylightRltTicket,
            IRepository<MaintenanceWorkRltSkylightPlan, Guid> maintenanceWorkRltSkylightPlanRepository,
            IRepository<MaintenanceWork, Guid> maintenanceWorks,
            IRepository<MaintenanceWorkRltFile, Guid> maintenanceWorkRltFile,
            IRepository<SkylightPlanRltWorkTicket, Guid> skylightPlanRltWorkTicket,
            IRepository<Workflow, Guid> workflowRepository,
            IMessageNoticeProvider messageNotice,
            IRepository<WorkTicketRltCooperationUnit, Guid> workTicketRltCooperationUnit,
            IRepository<Notice, Guid> notices,
            CrPlanMaintenanceWorkAppService crPlanMaintenanceWorkAppService,
            IdentityUserManager identityUserManager,
            IGuidGenerator guidGenerator,
            BpmManager bpmManager
            )
        {
            _skylightPlanRepository = skylightPlanRepo;
            _planDetailRepository = planDetailRepo;
            _planRelateEquipmentRepository = planRelateEquipmentRepo;
            _equipmentTestRepository = equipmentTestRepo;
            _repairUserRepository = repairUserRepo;
            _dailyPlanRepository = dailyPlanRepo;
            _yearMonthPlanRepository = yearMonthPlanRepo;
            _stationRepository = stationRepo;
            _installationSiteRepository = installationSiteRepo;
            _skyRltInstallationSiteRepository = skyRltInstallationSiteRepository;
            _equipmentRepository = equipmentRepo;
            _organization = organizationRepo;
            _organizationRespository = organizationRespository;
            _workOrder = workOrderRepo;
            _workOrganization = workOrganization;
            _ymPlanTest = yearMonthPlanTestItems;
            _repDetailIfd = repDetailIfd;
            _fileRepository = (IRepository<File.Entities.File, Guid>)fileRepository;
            _componentCategoryRepository = componentCategoryRepository;
            _alterRecordService = alterRecordService;
            _dataDictionaries = dataDictionaries;
            _workTickets = workTickets;
            _skylightRltTicket = skylightRltTicket;
            _maintenanceWorkRltSkylightPlanRepository = maintenanceWorkRltSkylightPlanRepository;
            _maintenanceWorks = maintenanceWorks;
            _maintenanceWorkRltFile = maintenanceWorkRltFile;
            _skylightPlanRltWorkTicket = skylightPlanRltWorkTicket;
            _workflowRepository = workflowRepository;
            _messageNotice = messageNotice;
            _workTicketRltCooperationUnit = workTicketRltCooperationUnit;
            _notices = notices;
            _crPlanMaintenanceWorkAppService = crPlanMaintenanceWorkAppService;
            _identityUserManager = identityUserManager;
            _guidGenerator = guidGenerator;
            this.bpmManager = bpmManager;
        }

        #region 计划管理
        /// <summary>
        /// 获取天窗计划列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.SkylightPlan.Default)]
        //[Authorize(CrPlanPermissions.ComprehensiveSkylightPlan.Default)]
        //[Authorize(CrPlanPermissions.OutsidePlan.Default)]
        public async Task<PagedResultDto<SkylightPlanDto>> GetList(SkylightPlanSearchInputDto input)
        {
            PagedResultDto<SkylightPlanDto> result = new PagedResultDto<SkylightPlanDto>();
            if (input == null || input.WorkUnit == null || input.WorkUnit == Guid.Empty) return result;
            var organizationCode = _organizationRespository.WhereIf(input.WorkUnit != null && input.WorkUnit != Guid.Empty,
                                                                     x => x.Id == input.WorkUnit).FirstOrDefault()?.Code;
            var organizationIds = _organizationRespository.WhereIf(!string.IsNullOrEmpty(organizationCode),
                                                                     x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(() =>
            {
                var allList = _skylightPlanRepository.WithDetails()
                                .WhereIf(organizationIds.Count > 0, x => organizationIds.Contains(x.WorkUnit))
                                .Where(x => x.RepairTagId == RepairTagId &&
                                  x.PlanType == input.PlanType &&
                                  (input.RepaireLevel == null || x.Level.Contains(((int)input.RepaireLevel).ToString())) &&
                                  (input.Station == null || x.Station == input.Station) &&
                                  (input.WorkSite == null || x.WorkSites.Any(m => m.InstallationSiteId == input.WorkSite)) &&
                                  (input.StartTime == null || x.WorkTime >= input.StartTime) &&
                                  (input.EndTime == null || x.WorkTime <= input.EndTime) &&
                                  (string.IsNullOrEmpty(input.ContentMileage) || (!string.IsNullOrEmpty(x.WorkContent) && x.WorkContent.Contains(input.ContentMileage)) ||
                                  (!string.IsNullOrEmpty(x.WorkArea) && x.WorkArea.Contains(input.ContentMileage))))
                                .WhereIf(input.RailwayId != null && input.RailwayId != Guid.Empty, x => x.RailwayId == input.RailwayId)
                                .WhereIf(input.RepaireLevel != null, x => x.Level.Contains(((int)input.RepaireLevel).ToString()))
                                .WhereIf(input.State != 0, x => x.PlanState == input.State)
                                .WhereIf(input.State == 0 && input.IsSearchData, x => x.PlanState != PlanState.Complete
                                     && x.PlanState != PlanState.NaturalDisasterCancel
                                     && x.PlanState != PlanState.OrderCancel
                                     && x.PlanState != PlanState.OtherReasonCancel
                                    )
                                .WhereIf(input.IsOnRoad != null, x => x.IsOnRoad == input.IsOnRoad)
                                .OrderBy(m => m.PlanState).ThenBy(s => s.WorkTime).ToList();

                if (allList?.Count > 0)
                {
                    result.TotalCount = allList.Count;
                    var dtos = ObjectMapper.Map<List<SkylightPlan>, List<SkylightPlanDto>>(allList);
                    if (!input.IsAll)
                    {
                        dtos = dtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                    }
                    //var resultItems = dtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                    //工点、机房名称赋值
                    foreach (var resItem in dtos)
                    {
                        var resEnt = allList.Find(x => x.Id == resItem.Id);
                        if (resEnt != null)
                        {
                            //获取计划方案
                            if (input.RepairTagKey == "RepairTag.RailwayHighSpeed")
                            {
                                var maintenanceWorkId = _maintenanceWorkRltSkylightPlanRepository
                                    .FirstOrDefault(x => x.SkylightPlanId == resItem.Id)?.MaintenanceWorkId;

                                if (maintenanceWorkId != null)
                                {
                                    var maintenanceRltFiles = _maintenanceWorkRltFile
                                        .WithDetails(x => x.File)
                                        .Where(x => x.MaintenanceWorkId == maintenanceWorkId)
                                        .Select(x => new { x.File, x.SchemeCoverName }).ToList();

                                    resItem.Files = ObjectMapper.Map<List<File.Entities.File>, List<FileDomainDto>>(maintenanceRltFiles.Select(x => x.File).ToList());
                                    resItem.SchemeCoverName = maintenanceRltFiles.FirstOrDefault()?.SchemeCoverName;
                                }
                            }

                            resItem.StationId = resEnt.Station;
                            resItem.WorkSiteIds = resEnt.WorkSites.ConvertAll(x => x.InstallationSiteId);
                            //机房名称赋值
                            string spltStr = " / ";
                            if (resEnt.WorkSites != null || resEnt.WorkSites.Count > 0)
                            {
                                foreach (var temp in resEnt.WorkSites)
                                {
                                    resItem.WorkSiteName += temp.InstallationSite.Name + spltStr;
                                }
                                resItem.WorkSiteName = string.IsNullOrEmpty(resItem.WorkSiteName) ?
                                    "" : resItem.WorkSiteName.Substring(0, resItem.WorkSiteName.Length - spltStr.Length);
                            }

                            var station = _stationRepository.FirstOrDefault(x => x.Id == resItem.StationId);
                            if (station != null && !resEnt.IsAdjacent)
                            {
                                resItem.StationName = station.Name;
                            }
                            if (resEnt.IsAdjacent)
                            { //添加非相邻区间
                                var endStation = _stationRepository.FirstOrDefault(x => x.Id == resItem.EndStationId);
                                resItem.StationName = station?.Name + "-" + endStation?.Name;
                            }

                            var workUnit = _organizationRespository.FirstOrDefault(x => x.Id == resItem.WorkUnit);
                            if (workUnit != null)
                            {
                                resItem.WorkUnitName = workUnit.Name;
                            }
                            //高铁需求：查询工作票 || 查询关联的维修计划的工作流Id
                            var workTickets = _skylightPlanRltWorkTicket.WithDetails().Where(x => x.SkylightPlanId == resItem.Id).ToList();
                            if (workTickets.Count() > 0)
                            {
                                foreach (var workTicket in workTickets)
                                {
                                    var workTicketSimple = ObjectMapper.Map<WorkTicket, WorkTicketDto>(workTicket.WorkTicket);
                                    workTicketSimple.SkylightPlanId = resItem.Id;
                                    var ticketsRltcooperation = _workTicketRltCooperationUnit.Where(x => x.WorkTicketId == workTicket.WorkTicketId).ToList();

                                    foreach (var rlt in ticketsRltcooperation)
                                    {
                                        var dto = ObjectMapper.Map<WorkTicketRltCooperationUnit, WorkTicketRltCooperationUnitDto>(rlt);
                                        dto.Id = rlt.CooperateWorkShopId;
                                        workTicketSimple.workTicketRltCooperationUnits.Add(dto);
                                        workTicketSimple.CooperateContent = rlt.CooperateContent;
                                    }
                                    resItem.SkylightPlanRltWorkTickets.Add(workTicketSimple);
                                }
                            }

                            var maintenanceWorkRltSkylightPlan = _maintenanceWorkRltSkylightPlanRepository
                                                                    .WithDetails(x => x.MaintenanceWork).OrderByDescending(x => x.MaintenanceWork.CreationTime)
                                                                    .FirstOrDefault(x => x.SkylightPlanId == resItem.Id);
                            resItem.WorkFlowId = maintenanceWorkRltSkylightPlan?.MaintenanceWork?.ARKey;
                        }
                    }
                    result.Items = dtos;
                }
            });
            return result;
        }
        /// <summary>
        /// 根据时间获取最后一次添加的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SkylightPlanDetailDto> GetLastPlan(CommonGuidGetDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var list = _skylightPlanRepository.WithDetails().Where(x => x.RepairTagId == RepairTagId)
                .WhereIf(input.PlanType.HasValue, x => x.PlanType == input.PlanType)
                .OrderBy(x => x.CreateTime).ToList();
            var dtos = ObjectMapper.Map<SkylightPlan, SkylightPlanDetailDto>(list.Count > 0 ? list.Last() : null);
            return dtos;
        }
        /// <summary>
        /// 获取其他计划列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.OtherPlan.Default)]
        public async Task<PagedResultDto<OtherPlanDto>> GetOtherPlanList(OtherPlanSearchInputDto input)
        {
            PagedResultDto<OtherPlanDto> result = new PagedResultDto<OtherPlanDto>();
            if (input == null || input.WorkUnitId == null || input.WorkUnitId == Guid.Empty) return result;
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var organizationCode = _organizationRespository.WhereIf(input.WorkUnitId != null && input.WorkUnitId != Guid.Empty,
                                                                     x => x.Id == input.WorkUnitId).FirstOrDefault()?.Code;
            var organizationIds = _organizationRespository.WhereIf(!string.IsNullOrEmpty(organizationCode),
                                                                     x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();
            await Task.Run(async () =>
            {
                var allList = _skylightPlanRepository.Where(x => x.RepairTagId == RepairTagId &&
                x.PlanType == Enums.PlanType.Other &&
                (input.WorkUnitId == null || organizationIds.Contains(x.WorkUnit)) &&
                (input.WorkAreaId == null || x.WorkAreaId == input.WorkAreaId) &&
                (input.StartTime == null || x.WorkTime >= input.StartTime) &&
                (input.EndTime == null || x.WorkTime <= input.EndTime) &&
                (string.IsNullOrEmpty(input.WorkContent) || (!string.IsNullOrEmpty(x.WorkContent) && x.WorkContent.Contains(input.WorkContent))))
                .WhereIf((int)input.PlanState != 0, x => x.PlanState == input.PlanState)
                .OrderByDescending(m => m.WorkTime).ToList();

                if (allList?.Count > 0)
                {
                    var organizationIdList = allList.ConvertAll(m => m.WorkAreaId);
                    //var organizationList = (await _organization.Where(z => organizationIdList.Contains(z.Id)))?.ToList();
                    result.TotalCount = allList.Count;
                    var modelList = new List<OtherPlanDto>();
                    allList.ForEach(m =>
                    {
                        OtherPlanDto dto = new OtherPlanDto(m.Id)
                        {
                            WorkTime = m.WorkTime,
                            PlanState = m.PlanState,
                            WorkContent = m.WorkContent,
                            IsChange = m.IsChange,
                            ChangTime = m.ChangTime,
                            Opinion = m.Opinion
                        };
                        var organization = _organizationRespository.FirstOrDefault(x => x.Id == m.WorkAreaId);
                        if (organization != null)
                        {
                            dto.WorkAreaName = organization.Name;
                        }
                        var workUnit = _organizationRespository.FirstOrDefault(x => x.Id == m.WorkUnit);
                        if (workUnit != null)
                        {
                            dto.WorkUnitName = workUnit.Name;
                        }
                        modelList.Add(dto);
                    });
                    result.Items = modelList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                }

            });
            return result;
        }

        /// <summary>
        /// 下发其他计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.OtherPlan.Release)]
        public async Task<bool> PublishOtherPlan(OtherPlanSearchInputDto input)
        {
            if (input == null) return false;

            //查找下发当天的其他下发计划
            var startNowDate = DateTime.Today;
            var endNowDate = DateTime.Today.AddDays(1).AddSeconds(-1);
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var allList = _skylightPlanRepository.Where(x => x.RepairTagId == RepairTagId && x.PlanType == Enums.PlanType.Other && x.PlanState == Enums.PlanState.NotIssued).ToList();
                if (input.WorkUnitId != null && allList?.Count > 0)
                    allList = allList.Where(x => x.WorkUnit == input.WorkUnitId).ToList();
                if (input.WorkAreaId != null && allList?.Count > 0)
                    allList = allList.FindAll(x => x.WorkAreaId != null && x.WorkAreaId == input.WorkAreaId);
                if (input.StartTime != null && allList?.Count > 0)
                    allList = allList.FindAll(x => x.WorkTime >= input.StartTime);
                if (input.EndTime != null && allList?.Count > 0)
                    allList = allList.FindAll(x => x.WorkTime <= input.EndTime);
                if (!string.IsNullOrEmpty(input.WorkContent) && !string.IsNullOrWhiteSpace(input.WorkContent) && allList?.Count > 0)
                    allList = allList.Where(x => !string.IsNullOrEmpty(x.WorkContent) && x.WorkContent.Contains(input.WorkContent)).ToList();

                if (allList?.Count > 0)
                {
                    foreach (var item in allList)
                    {
                        if (item.WorkAreaId == null) throw new UserFriendlyException("存在未指派工区的计划");
                        item.PlanState = Enums.PlanState.Issued;
                        await _skylightPlanRepository.UpdateAsync(item);

                        //添加派工单
                        WorkOrder ent = new WorkOrder(Guid.NewGuid());
                        ent.DispatchingTime = DateTime.Now;
                        ent.EndPlanTime = item.WorkTime;
                        ent.OrderState = Enums.OrderState.Unfinished;
                        ent.SkylightPlanId = item.Id;
                        ent.StartPlanTime = item.WorkTime;
                        ent.OrderType = Enums.OrderType.OtherAssignments;
                        ent.RepairTagId = RepairTagId;

                        if (input.RepairTagKey == "RepairTag.RailwayHighSpeed")
                        {
                            //高铁需求：其他作业在下发后自动生成命令票号(20210224-0001)
                            var otherWorkOrderNo = _workOrder.Where(x => x.OrderType == OrderType.OtherAssignments && x.CreationTime >= startNowDate && x.CreationTime <= endNowDate).ToList().OrderBy(x => x.OrderNo)?.LastOrDefault()?.OrderNo;
                            if (string.IsNullOrEmpty(otherWorkOrderNo) || otherWorkOrderNo == null)
                            {
                                ent.OrderNo = "0001";
                            }
                            else
                            {
                                ent.OrderNo = (int.Parse(otherWorkOrderNo) + 1).ToString().PadLeft(4, '0');
                            }
                        }
                        var addEnt = await _workOrder.InsertAsync(ent);
                        await CurrentUnitOfWork.SaveChangesAsync();
                        //添加检修工区
                        WorkOrganization maintenance = new WorkOrganization(Guid.NewGuid());
                        maintenance.Duty = Enums.Duty.Recondition;
                        maintenance.WorkOrderId = addEnt.Id;
                        maintenance.OrganizationId = (Guid)item.WorkAreaId;
                        maintenance.RepairTagId = RepairTagId;
                        await _workOrganization.InsertAsync(maintenance);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// 获取详情（天窗、其他计划）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SkylightPlanDetailDto> Get(CommonGuidGetDto input)
        {
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var ent = _skylightPlanRepository.WithDetails().FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == input.Id);
                if (ent == null) return null;
                SkylightPlanDetailDto result = new SkylightPlanDetailDto();
                result = ObjectMapper.Map<SkylightPlan, SkylightPlanDetailDto>(ent);
                result.StationId = ent.Station;
                //result.WorkSiteId = ent.WorkSite;

                //变更后代码 刘娟娟 2020年12月18日14:51:06
                result.WorkSiteIds = ent.WorkSites.ConvertAll(x => x.InstallationSiteId);
                //if (ent.WorkSite == Guid.Empty) result.WorkSiteId = null;
                result.OrganizationId = ent.WorkUnit;
                //站点名称赋值
                if (ent.Station != Guid.Empty)
                {
                    var sta = _stationRepository.FirstOrDefault(x => x.Id == ent.Station);
                    if (sta != null) result.StationName = sta.Name;
                }

                //机房名称赋值
                string spltStr = " / ";
                if (ent.WorkSites != null || ent.WorkSites.Count > 0)
                {
                    //变更后代码 刘娟娟 2020年12月18日14:51:06
                    foreach (var temp in ent.WorkSites)
                    {
                        result.WorkSiteName += temp.InstallationSite.Name + spltStr;
                    }
                    result.WorkSiteName = string.IsNullOrEmpty(result.WorkSiteName) ? ""
                        : result.WorkSiteName.Substring(0, result.WorkSiteName.Length - spltStr.Length);
                }

                //获取维修作业内容

                var maintenanceWorkRltSky = _maintenanceWorkRltSkylightPlanRepository.FirstOrDefault(x => x.SkylightPlanId == input.Id);
                if (maintenanceWorkRltSky != null)
                {
                    result.SignOrganization = maintenanceWorkRltSky.SignOrganization;
                    result.Remark = maintenanceWorkRltSky.Remark;
                    result.FirstTrial = maintenanceWorkRltSky.FirstTrial;
                    result.WorkOrgAndDutyPerson = maintenanceWorkRltSky.WorkOrgAndDutyPerson;
                }


                //计划内容组织
                result.PlanDetails = new List<PlanDetailDto>();
                var detailPlans = _planDetailRepository.Where(x => x.RepairTagId == RepairTagId && x.SkylightPlanId == input.Id).ToList();
                if (detailPlans == null || detailPlans.Count == 0) return result;
                AlterRecordGetListDto t = new AlterRecordGetListDto();
                t.Ids = detailPlans.Select(s => s.DailyPlanId).ToList();
                t.RepairTagKey = input.RepairTagKey;
                var selectableDtos = await _alterRecordService.ForSelectablePlanByIds(t);
                var tempDailyDetailIds = detailPlans.Select(s => s.Id);
                var tempDailyPlanIds = detailPlans.Select(s => s.DailyPlanId);
                var tempAllDailyPlans = _dailyPlanRepository.Where(s => s.RepairTagId == RepairTagId && tempDailyPlanIds.Contains(s.Id)).ToList();
                var tempAllYearMonthPlanIds = tempAllDailyPlans.Select(s => s.PlanId);
                var tempAllYearMonthPlan = _yearMonthPlanRepository.Where(s => s.RepairTagId == RepairTagId && tempAllYearMonthPlanIds.Contains(s.Id)).ToList();
                var tempAllRepairDetailIds = tempAllYearMonthPlan.Select(s => s.RepairDetailsId);
                var tempAllRepairDetails = _repDetailIfd.Where(x => tempAllRepairDetailIds.Contains(x.RepairItemId)).ToList();
                var tempAllPlanRelateEquips = _planRelateEquipmentRepository.Where(s => s.RepairTagId == RepairTagId && tempDailyDetailIds.Contains(s.PlanDetailId)).ToList();
                var tempAllEquipIds = tempAllPlanRelateEquips.Select(s => s.EquipmentId);
                var tempAllEquips = _equipmentRepository.Where(s => tempAllEquipIds.Contains(s.Id)).ToList();
                //获取所有日计划
                foreach (var detail in detailPlans)
                {
                    PlanDetailDto detailPlanDto = ObjectMapper.Map<PlanDetail, PlanDetailDto>(detail);
                    //组织日计划
                    //var dailyEnt = _dailyPlanRepository.FirstOrDefault(x => x.Id == detail.DailyPlanId);
                    var dailyEnt = tempAllDailyPlans.FirstOrDefault(x => x.Id == detail.DailyPlanId);
                    YearMonthPlan yearMonthPlan = null;
                    if (dailyEnt != null)
                        //yearMonthPlan = _yearMonthPlanRepository.FirstOrDefault(x => x.Id == dailyEnt.PlanId);
                        yearMonthPlan = tempAllYearMonthPlan.FirstOrDefault(x => x.Id == dailyEnt.PlanId);
                    if (yearMonthPlan != null)
                    {
                        var temp = selectableDtos.FirstOrDefault(s => s.Id == detail.DailyPlanId);
                        detailPlanDto.DailyPlan = new DailyPlanSelectableDto()
                        {
                            Number = yearMonthPlan.Number,
                            EquipName = yearMonthPlan.DeviceName,
                            Content = yearMonthPlan.RepairContent,
                            PlanDate = dailyEnt.PlanDate,
                            Unit = yearMonthPlan.Unit,
                            Count = dailyEnt.Count,
                            IFDCodes = new List<string>(),
                            UnFinishCount = temp != null ? temp.UnFinishCount : 0
                        };
                        if (yearMonthPlan.PlanType == 2)
                        {
                            detailPlanDto.DailyPlan.PlanTypeStr = "月表";
                            detailPlanDto.DailyPlan.PlanType = SelectablePlanType.Month;
                        }
                        else if (yearMonthPlan.PlanType == 1 || yearMonthPlan.PlanType == 3)
                        {
                            detailPlanDto.DailyPlan.PlanTypeStr = "年表";
                            var times = yearMonthPlan.Times.Trim();
                            if (times == "2") detailPlanDto.DailyPlan.PlanType = SelectablePlanType.HalfYaer;
                            else if (times == "4") detailPlanDto.DailyPlan.PlanType = SelectablePlanType.QuarterYear;
                            else detailPlanDto.DailyPlan.PlanType = SelectablePlanType.Year;
                        }
                        //detailPlanDto.DailyPlan.PlanTypeStr = Common.EnumHelper.GetDescription(detailPlanDto.DailyPlan.PlanType);
                        //维修项IFD列表赋值
                        //var ifdCodes = _repDetailIfd.Where(x => x.RepairItemId != null && x.RepairItemId == yearMonthPlan.RepairDetailsId).ToList();
                        var ifdCodes = tempAllRepairDetails.Where(x => x.RepairItemId == yearMonthPlan.RepairDetailsId).ToList();
                        if (ifdCodes?.Count > 0)
                        {
                            var ifdIds = ifdCodes.Select(x => x.ComponentCategoryId).ToList();
                            detailPlanDto.IFDCodeList = ifdIds;
                            detailPlanDto.DailyPlan.IFDCodes = _componentCategoryRepository.Where(s => ifdIds.Contains(s.Id)).Select(s => s.Code).ToList();
                        }
                    }

                    //组织关联设备
                    if (ent.PlanType != Enums.PlanType.Other)
                    {
                        detailPlanDto.RelateEquipments = new List<PlanRelateEquipmentDto>();
                        //var relEquips = _planRelateEquipmentRepository.Where(x => x.PlanDetailId == detail.Id).ToList();
                        var relEquips = tempAllPlanRelateEquips.Where(x => x.PlanDetailId == detail.Id).ToList();
                        if (relEquips?.Count > 0)
                        {
                            foreach (var equip in relEquips)
                            {
                                PlanRelateEquipmentDto relEquipDto = ObjectMapper.Map<PlanRelateEquipment, PlanRelateEquipmentDto>(equip);
                                if (equip.EquipmentId != null && equip.EquipmentId != Guid.Empty)
                                {
                                    //var equipmentEnt = _equipmentRepository.FirstOrDefault(x => x.Id == equip.EquipmentId);
                                    var equipmentEnt = tempAllEquips.FirstOrDefault(x => x.Id == equip.EquipmentId);
                                    if (equipmentEnt != null)
                                        relEquipDto.EquipmentName = equipmentEnt.Name;

                                }
                                detailPlanDto.RelateEquipments.Add(relEquipDto);
                            }
                        }
                        if (detailPlanDto.RelateEquipments.Any(s => s.EquipmentId == null))
                            detailPlanDto.RelateEquipments = new List<PlanRelateEquipmentDto>();
                    }
                    result.PlanDetails.Add(detailPlanDto);
                }
                result.PlanDetails = result.PlanDetails.OrderBy(s => s.DailyPlan.Number.Replace("-", "")).ToList();
                foreach (var item in result.PlanDetails)
                {
                    var nums = item.DailyPlan.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.DailyPlan.Number = newNum.TrimEnd('-');
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// 获取详情（派工作业专用）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SkylightPlanDetailDto> GetInWork(CommonGuidGetDto input)
        {
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var ent = _skylightPlanRepository.WithDetails().FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == input.Id);
                if (ent == null) return null;
                SkylightPlanDetailDto result = new SkylightPlanDetailDto();
                result = ObjectMapper.Map<SkylightPlan, SkylightPlanDetailDto>(ent);
                result.StationId = ent.Station;
                //result.WorkSiteId = ent.WorkSite;

                //变更后代码 刘娟娟 2020年12月18日14:51:06
                //机房id集合
                result.WorkSiteIds = ent.WorkSites.ConvertAll(x => x.InstallationSiteId);
                result.OrganizationId = ent.WorkUnit;
                //站点名称赋值
                if (ent.Station != Guid.Empty)
                {
                    var sta = _stationRepository.FirstOrDefault(x => x.Id == ent.Station);
                    if (sta != null) result.StationName = sta.Name;
                }
                InstallationSite site = null;
                //机房名称赋值
                //if (ent.WorkSite != Guid.Empty)
                //{
                //    site = _installationSiteRepository.FirstOrDefault(x => x.Id == ent.WorkSite);
                //    if (site != null) result.WorkSiteName = site.Name;
                //}

                //机房名称赋值
                string spltStr = " / ";
                if (ent.WorkSites != null || ent.WorkSites.Count > 0)
                {
                    foreach (var temp in ent.WorkSites)
                    {
                        result.WorkSiteName += temp.InstallationSite.Name + spltStr;
                    }
                    result.WorkSiteName = string.IsNullOrEmpty(result.WorkSiteName) ? "" : result.WorkSiteName.Substring(0, result.WorkSiteName.Length - spltStr.Length);
                }


                //计划内容组织
                result.PlanDetails = new List<PlanDetailDto>();
                var detailPlans = _planDetailRepository.Where(x => x.RepairTagId == RepairTagId && x.SkylightPlanId == input.Id).ToList();
                if (detailPlans == null || detailPlans.Count == 0) return result;

                //获取所有日计划
                foreach (var detail in detailPlans)
                {
                    PlanDetailDto detailPlanDto = ObjectMapper.Map<PlanDetail, PlanDetailDto>(detail);
                    //组织日计划
                    var dailyEnt = _dailyPlanRepository.FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == detail.DailyPlanId);
                    YearMonthPlan yearMonthPlan = null;
                    if (dailyEnt != null)
                        yearMonthPlan = _yearMonthPlanRepository.FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == dailyEnt.PlanId);
                    if (yearMonthPlan != null)
                    {
                        detailPlanDto.DailyPlan = new DailyPlanSelectableDto()
                        {
                            Number = yearMonthPlan.Number,
                            EquipName = yearMonthPlan.DeviceName,
                            Content = yearMonthPlan.RepairContent,
                            PlanDate = dailyEnt.PlanDate,
                            Unit = yearMonthPlan.Unit,
                            Count = dailyEnt.Count,
                            IFDCodes = new List<string>(),
                        };
                        if (result.WorkSiteIds != null || result.WorkSiteIds.Count > 0)
                        {
                            detailPlanDto.WorkSiteIds = result.WorkSiteIds;
                            detailPlanDto.WorkSiteName = result.WorkSiteName;
                        }
                        if (yearMonthPlan.PlanType == 2)
                        {
                            detailPlanDto.DailyPlan.PlanTypeStr = "月表";
                            detailPlanDto.DailyPlan.PlanType = SelectablePlanType.Month;
                        }
                        else if (yearMonthPlan.PlanType == 1 || yearMonthPlan.PlanType == 3)
                        {
                            detailPlanDto.DailyPlan.PlanTypeStr = "年表";
                            var times = yearMonthPlan.Times.Trim();
                            if (times == "2") detailPlanDto.DailyPlan.PlanType = SelectablePlanType.HalfYaer;
                            else if (times == "4") detailPlanDto.DailyPlan.PlanType = SelectablePlanType.QuarterYear;
                            else detailPlanDto.DailyPlan.PlanType = SelectablePlanType.Year;
                        }
                        //detailPlanDto.DailyPlan.PlanTypeStr = Common.EnumHelper.GetDescription(detailPlanDto.DailyPlan.PlanType);
                        //维修项IFD列表赋值
                        var ifdCodes = _repDetailIfd.Where(x => x.RepairItemId != null && x.RepairItemId == yearMonthPlan.RepairDetailsId).ToList();
                        if (ifdCodes?.Count > 0)
                        {
                            var ifdIds = ifdCodes.Select(x => x.ComponentCategoryId).ToList();
                            detailPlanDto.IFDCodeList = ifdIds;
                            detailPlanDto.DailyPlan.IFDCodes = _componentCategoryRepository.Where(s => ifdIds.Contains(s.Id)).Select(s => s.Code).ToList();
                        }
                    }

                    //组织关联设备
                    detailPlanDto.RelateEquipments = new List<PlanRelateEquipmentDto>();
                    var relEquips = _planRelateEquipmentRepository.Where(x => x.RepairTagId == RepairTagId && x.PlanDetailId == detail.Id).ToList();
                    if (relEquips?.Count > 0)
                    {
                        foreach (var equip in relEquips)
                        {
                            PlanRelateEquipmentDto relEquipDto = ObjectMapper.Map<PlanRelateEquipment, PlanRelateEquipmentDto>(equip);
                            if (equip.EquipmentId != null && equip.EquipmentId != Guid.Empty)
                            {
                                var equipmentEnt = _equipmentRepository.FirstOrDefault(x => x.Id == equip.EquipmentId);
                                if (equipmentEnt != null)
                                    relEquipDto.EquipmentName = equipmentEnt.Name;

                            }
                            detailPlanDto.RelateEquipments.Add(relEquipDto);
                        }
                    }
                    result.PlanDetails.Add(detailPlanDto);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        /// <summary>
        /// 发布天窗计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.SkylightPlan.Release)]
        //[Authorize(CrPlanPermissions.ComprehensiveSkylightPlan.Release)]
        //[Authorize(CrPlanPermissions.OutsidePlan.Release)]
        //public async Task<bool> PublishPlan(SkylightPlanSearchInputDto input)
        //{
        //    if (input == null) return false;
        //    try
        //    {
        //        var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
        //        var allList = _skylightPlanRepository.Where(x => x.RepairTagId == RepairTagId &&
        //        x.PlanState == Enums.PlanState.NoPublish &&
        //        x.PlanType == input.PlanType &&
        //        (input.WorkUnit == null || x.WorkUnit == input.WorkUnit) &&
        //        (input.Station == null || x.Station == input.Station) &&
        //        (input.WorkSite == null || x.WorkSite == input.WorkSite) &&
        //        (input.StartTime == null || x.WorkTime >= input.StartTime) &&
        //        (input.EndTime == null || x.WorkTime <= input.EndTime) &&
        //        (string.IsNullOrEmpty(input.ContentMileage) || (!string.IsNullOrEmpty(x.WorkContent) && x.WorkContent.Contains(input.ContentMileage)) ||
        //        (!string.IsNullOrEmpty(x.WorkArea) && x.WorkArea.Contains(input.ContentMileage)))).ToList();

        //        if (allList?.Count > 0)
        //        {
        //            foreach (var item in allList)
        //            {
        //                item.PlanState = Enums.PlanState.Publish;
        //                await _skylightPlanRepository.UpdateAsync(item);
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}

        /// <summary>
        /// 撤销天窗计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.SkylightPlan.Update)]
        //[Authorize(CrPlanPermissions.ComprehensiveSkylightPlan.Update)]
        //[Authorize(CrPlanPermissions.OutsidePlan.Update)]
        [Authorize(CrPlanPermissions.OtherPlan.Update)]
        public async Task<bool> BackoutPlan(CommonGuidGetDto input)
        {
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var ent = _skylightPlanRepository.FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == input.Id);
                if (ent == null) return false;
                if (ent.PlanState == PlanState.Dispatching)
                {
                    var workOrder = _workOrder.FirstOrDefault(x => x.SkylightPlanId == ent.Id);
                    if (workOrder == null) return false;
                    ent.PlanState = PlanState.UnDispatching;
                    await _workOrder.DeleteAsync(workOrder.Id);
                }
                else
                { //添加高铁科天窗计划
                    //更新状态
                    ent.PlanState = PlanState.Revoke;
                    //删除维修作业关联关系
                    var maintenanceWorkRltSkylightPlan = _maintenanceWorkRltSkylightPlanRepository.WithDetails(x => x.MaintenanceWork).Where(x => x.SkylightPlanId == ent.Id).FirstOrDefault();

                    if (maintenanceWorkRltSkylightPlan == null)
                    {
                        throw new UserFriendlyException("维修作业关联不存在");
                    }

                    var maintenanceWorkCount = _maintenanceWorkRltSkylightPlanRepository.Where(x => x.MaintenanceWorkId == maintenanceWorkRltSkylightPlan.MaintenanceWorkId).ToList();

                    //if (maintenanceWorkCount.Count == 1)
                    //{
                    // 则删除维修计划 及 工作流
                    await _workflowRepository.DeleteAsync(x => x.Id == maintenanceWorkRltSkylightPlan.MaintenanceWork.ARKey);
                    //await _maintenanceWorks.DeleteAsync(x => x.Id == maintenanceWorkRltSkylightPlan.MaintenanceWork.Id);
                    //}
                    if (maintenanceWorkCount.Count > 1)
                    {
                        await _maintenanceWorkRltSkylightPlanRepository
                            .DeleteAsync(x => x.SkylightPlanId == ent.Id && x.MaintenanceWorkId != maintenanceWorkRltSkylightPlan.MaintenanceWork.Id);
                    }
                }
                await _skylightPlanRepository.UpdateAsync(ent);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        private IQueryable<DailyPlan> _allDailyPlans;// = new List<DailyPlan>();
        private IQueryable<YearMonthPlan> _allYearMonthPlans;// = new List<YearMonthPlan>();
        private IQueryable<YearMonthPlanTestItem> _allYearMonthPlanTests;// = new List<YearMonthPlanTestItem>();
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isOther">True:其他计划 False:天窗计划</param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.SkylightPlan.Create)]
        //[Authorize(CrPlanPermissions.ComprehensiveSkylightPlan.Create)]
        //[Authorize(CrPlanPermissions.SkylightOutsidePlan.Create)]
        //[Authorize(CrPlanPermissions.OutsidePlan.Create)]
        [Authorize(CrPlanPermissions.OtherPlan.Create)]
        public async Task<bool> Create(SkylightPlanCreateDto input, bool isOther)
        {
            if (input == null) throw new UserFriendlyException("修改内容错误");
            if (isOther)
            {
                if (input.WorkAreaId == null || input.WorkAreaId == Guid.Empty) throw new UserFriendlyException("请选择作业工区");
                input.PlanType = Enums.PlanType.Other;
            }
            else
            {
                if (input.StationId == null || input.StationId == Guid.Empty) throw new UserFriendlyException("车站/区间未选择，无法添加");
                if (input.TimeLength <= 0) throw new UserFriendlyException("时长输入错误，无法添加");
            }
            if (input.WorkContentType == WorkContentType.MonthYearPlan && (input.PlanDetails == null || input.PlanDetails.Count == 0))
                throw new UserFriendlyException("未添加计划内容");
            if (input.WorkContentType == WorkContentType.OtherPlan && string.IsNullOrEmpty(input.WorkContent))
                throw new UserFriendlyException("未添加计划内容");
            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) throw new UserFriendlyException("作业单位（组织机构）为空，无法添加");

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //试着提升效率
            _allDailyPlans = _dailyPlanRepository.Where(s => s.RepairTagId == RepairTagId && s.Id != null);
            _allYearMonthPlans = _yearMonthPlanRepository.Where(s => s.RepairTagId == RepairTagId && s.Id != null);
            _allYearMonthPlanTests = _ymPlanTest.Where(s => s.RepairTagId == RepairTagId && s.Id != null && !s.IsDeleted);
            //_allRelationShips = _fileRepository.Where(s => s.Id != null);

            input.Id = Guid.NewGuid();
            var ent = ObjectMapper.Map<SkylightPlanCreateDto, SkylightPlan>(input);
            #region 作业内容
            //var dailyPlanIds = input.PlanDetails.Select(x => x.DailyPlanId).ToList();
            List<KeyValuePair<Guid, decimal>> planDetails = new List<KeyValuePair<Guid, decimal>>();
            input.PlanDetails.ForEach(m => planDetails.Add(new KeyValuePair<Guid, decimal>(m.DailyPlanId, m.PlanCount)));
            string workContent = GetWorkContent(planDetails);
            ent.WorkContent = input.WorkContentType == WorkContentType.OtherPlan ? input.WorkContent : workContent;
            #endregion
            ent.Station = input.StationId;
            ent.StationRelateRailwayType = input.StationRelateRailwayType;
            //ent.WorkSite = input.WorkSiteId == null ? Guid.Empty : (Guid)input.WorkSiteId;

            //变更代码 刘娟娟 2020年12月18日15:08:46
            ent.WorkSites = new List<SkylightPlanRltInstallationSite>();
            input.WorkSiteIds = input.WorkSiteIds == null ? new List<Guid>() : input.WorkSiteIds;
            // 重新保存关联机房信息
            foreach (var id in input.WorkSiteIds)
            {
                ent.WorkSites.Add(new SkylightPlanRltInstallationSite(Guid.NewGuid())
                {
                    SkylightPlanId = ent.Id,
                    InstallationSiteId = id,
                });
            }

            ent.WorkUnit = input.OrganizationId;
            //ent.WorkTime = DateTime.Parse(input.WorkTime.ToShortDateString());
            ent.WorkTime = input.WorkTime;
            ent.CreateTime = DateTime.Now;
            ent.RepairTagId = RepairTagId;
            ent.RailwayId = input.RailwayId;
            if (input.IsAdjacent)
            {
                ent.IsAdjacent = input.IsAdjacent;
                ent.EndStationId = input.EndStationId;
                ent.EndStationRelateRailwayType = input.EndStationRelateRailwayType;
            }
            if (isOther)
                ent.PlanState = PlanState.NotIssued;
            else
            {
                ent.PlanState = PlanState.UnDispatching;
                if (input.RepairTagKey == "RepairTag.RailwayHighSpeed" && input.PlanType == PlanType.VerticalSkylight)
                {
                    //ent.PlanState = PlanState.UnSubmited;
                    ent.PlanState = PlanState.UnDispatching;
                    ent.RegistrationPlace = input.RegistrationPlace;
                }
            }

            SkylightPlan resSkyPlanEnt = await _skylightPlanRepository.InsertAsync(ent);

            //if (input.RepairTagKey == "RepairTag.RailwayHighSpeed" && !isOther)
            //{
            //    //保存维修作业内容
            //    var maintenanceWorkEnt = new MaintenanceWork(_guidGenerator.Create())
            //    {
            //        OrganizationId = input.OrganizationId,
            //        StartTime = input.WorkTime,
            //        MaintenanceProject = "通信维修",
            //        MaintenanceType = input.PlanType,
            //        RepairTagId = RepairTagId,
            //        RepairLevel = input.Level,
            //    };

            //    await _maintenanceWorks.InsertAsync(maintenanceWorkEnt);

            //    //保存维修作业关联垂直天窗计划
            //    var maintenanceWorkRltSkylightPlan = new MaintenanceWorkRltSkylightPlan(_guidGenerator.Create())
            //    {
            //        MaintenanceWorkId = maintenanceWorkEnt.Id,
            //        SkylightPlanId = resSkyPlanEnt.Id,
            //        WorkOrgAndDutyPerson = input.MaintenanceQueryParams.WorkOrgAndDutyPerson,
            //        SignOrganization = input.MaintenanceQueryParams.SignOrganization,
            //        FirstTrial = input.MaintenanceQueryParams.FirstTrial,
            //        Remark = input.MaintenanceQueryParams.Remark
            //    };

            //    await _maintenanceWorkRltSkylightPlanRepository.InsertAsync(maintenanceWorkRltSkylightPlan);
            //}

            if (input.PlanDetails == null || input.PlanDetails.Count == 0) return true;
            //计划详情
            int i = 1;
            foreach (var detailDto in input.PlanDetails)
            {
                var dailyPlan = _allDailyPlans.FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == detailDto.DailyPlanId);
                if (dailyPlan == null) throw new UserFriendlyException("第" + i + "条计划内容对应日计划不存在，请删除此条内容");
                //if (detailDto.PlanCount > dailyPlan.Count) throw new UserFriendlyException("作业数量不可大于计划数量");
                detailDto.Id = Guid.NewGuid();
                detailDto.SkylightPlanId = resSkyPlanEnt.Id;
                var detailEnt = ObjectMapper.Map<PlanDetailCreateDto, PlanDetail>(detailDto);
                detailEnt.RepairTagId = RepairTagId;
                PlanDetail resDetailEnt = await _planDetailRepository.InsertAsync(detailEnt);
                //关联设备
                if (detailDto.RelateEquipments?.Count > 0)
                {
                    foreach (var relEquip in detailDto.RelateEquipments)
                    {
                        relEquip.Id = Guid.NewGuid();
                        relEquip.PlanDetailId = resDetailEnt.Id;
                        //relEquip.PlanCount = detailDto.PlanCount;
                        relEquip.IsComplete = Enums.AcceptanceResults.Unfinished;
                        var relEquipEnt = ObjectMapper.Map<PlanRelateEquipmentCreateDto, PlanRelateEquipment>(relEquip);
                        relEquipEnt.RepairTagId = RepairTagId;
                        var planRelEquipEnt = await _planRelateEquipmentRepository.InsertAsync(relEquipEnt);
                        //创建设备测试项数据
                        if (planRelEquipEnt != null)
                            await AddRelEquipTest(resDetailEnt.DailyPlanId, planRelEquipEnt.Id, RepairTagId);
                    }
                }
                //未关联则添加一条设备ID为空的数据
                else
                {
                    PlanRelateEquipment emptyEnt = new PlanRelateEquipment(Guid.NewGuid())
                    {
                        PlanDetailId = resDetailEnt.Id,
                        EquipmentId = null,
                        PlanCount = detailDto.PlanCount,
                        IsComplete = Enums.AcceptanceResults.Unfinished,
                        RepairTagId = RepairTagId
                    };
                    var planRelEquipEnt = await _planRelateEquipmentRepository.InsertAsync(emptyEnt);
                    //创建设备测试项数据
                    if (planRelEquipEnt != null)
                        await AddRelEquipTest(resDetailEnt.DailyPlanId, planRelEquipEnt.Id, RepairTagId);
                }
                i++;
            }

            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.SkylightPlan.Delete)]
        //[Authorize(CrPlanPermissions.ComprehensiveSkylightPlan.Delete)]
        //[Authorize(CrPlanPermissions.OutsidePlan.Delete)]
        public async Task<bool> Remove(CommonGuidGetDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var findDelEnt = _skylightPlanRepository.FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == input.Id);
            if (findDelEnt == null) throw new UserFriendlyException("删除的计划不存在");
            try
            {
                var detailPlans = _planDetailRepository.Where(x => x.RepairTagId == RepairTagId && x.SkylightPlanId == input.Id).ToList();
                if (detailPlans?.Count > 0)
                {
                    var detailPlanIds = detailPlans.Select(x => x.Id);
                    var relateEquips = _planRelateEquipmentRepository.Where(x => x.RepairTagId == RepairTagId && detailPlanIds.Contains(x.PlanDetailId)).ToList();

                    if (relateEquips?.Count > 0)
                    {
                        var relEquipIds = relateEquips.Select(x => x.Id);
                        //删除设备测试项
                        var equipTests = _equipmentTestRepository.Where(x => x.RepairTagId == RepairTagId && relEquipIds.Contains(x.PlanRelateEquipmentId)).ToList();
                        if (equipTests?.Count > 0)
                        {
                            foreach (var eqTest in equipTests)
                            {
                                var relFiles = _fileRepository.Where(x => x.Id == eqTest.Id).ToList();
                                if (relFiles?.Count > 0)
                                {
                                    foreach (var file in relFiles)
                                    {
                                        await _fileRepository.DeleteAsync(file.Id);
                                    }
                                }
                                await _equipmentTestRepository.DeleteAsync(eqTest.Id);
                            }
                        }
                        //删除关联设备检修人员
                        var repairUsers = _repairUserRepository.Where(x => x.RepairTagId == RepairTagId && relEquipIds.Contains(x.PlanRelateEquipmentId.Value)).ToList();
                        if (repairUsers?.Count > 0)
                        {
                            foreach (var rpUser in repairUsers)
                            {
                                await _repairUserRepository.DeleteAsync(rpUser.Id);
                            }
                        }
                        //删除关联设备
                        foreach (var rlEq in relateEquips)
                        {
                            await _planRelateEquipmentRepository.DeleteAsync(rlEq.Id);
                        }
                    }
                    //删除计划详情
                    foreach (var item in detailPlans)
                    {
                        await _planDetailRepository.DeleteAsync(item.Id);
                    }
                    //await Task.Run(() => detailPlans.ForEach(x => _planDetailRepository.DeleteAsync(x.Id)));
                }
                //删除天窗计划
                await _skylightPlanRepository.DeleteAsync(input.Id);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isOther">True:其他计划 False:天窗计划</param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.SkylightPlan.Update)]
        //[Authorize(CrPlanPermissions.ComprehensiveSkylightPlan.Update)]
        //[Authorize(CrPlanPermissions.OutsidePlan.Update)]
        [Authorize(CrPlanPermissions.OtherPlan.Update)]
        public async Task<bool> Update(SkylightPlanUpdateDto input, bool isOther)
        {
            if (input == null) throw new UserFriendlyException("修改内容错误");
            if (isOther)
            {
                if (input.WorkAreaId == null || input.WorkAreaId == Guid.Empty) throw new UserFriendlyException("请选择作业工区");
            }
            else
            {
                if (input.StationId == null || input.StationId == Guid.Empty) throw new UserFriendlyException("车站/区间未选择，无法添加");
                if (input.TimeLength <= 0) throw new UserFriendlyException("时长输入错误，无法添加");
            }
            if (input.WorkContentType == WorkContentType.MonthYearPlan && (input.PlanDetails == null || input.PlanDetails.Count == 0))
                throw new UserFriendlyException("未添加计划内容");
            if (input.WorkContentType == WorkContentType.OtherPlan && string.IsNullOrEmpty(input.WorkContent))
                throw new UserFriendlyException("未添加计划内容");
            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) throw new UserFriendlyException("作业单位（组织机构）为空");
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                //试着提升效率
                _allDailyPlans = _dailyPlanRepository.Where(s => s.RepairTagId == RepairTagId && s.Id != null);
                _allYearMonthPlans = _yearMonthPlanRepository.Where(s => s.RepairTagId == RepairTagId && s.Id != null);
                _allYearMonthPlanTests = _ymPlanTest.Where(s => s.RepairTagId == RepairTagId && s.Id != null && !s.IsDeleted);
                //_allRelationShips = _fileRepository.Where(s => s.Id != null);
                var allPlanDetails = _planDetailRepository.Where(s => s.RepairTagId == RepairTagId && s.Id != null);
                var allPlanRelateEquips = _planRelateEquipmentRepository.Where(s => s.RepairTagId == RepairTagId && s.Id != null);

                var skyPlanEnt = ObjectMapper.Map<SkylightPlanUpdateDto, SkylightPlan>(input);
                List<KeyValuePair<Guid, decimal>> planDetails = new List<KeyValuePair<Guid, decimal>>();

                #region 更新维修作业
                var maintenanceWorkSky = _maintenanceWorkRltSkylightPlanRepository.FirstOrDefault(x => x.SkylightPlanId == skyPlanEnt.Id);
                if (maintenanceWorkSky != null)
                {
                    var maintenanceWork = _maintenanceWorks.FirstOrDefault(x => x.Id == maintenanceWorkSky.MaintenanceWorkId);
                    if (maintenanceWork != null)
                    {
                        maintenanceWork.StartTime = input.WorkTime;
                        maintenanceWork.RepairLevel = input.Level;

                        await _maintenanceWorks.UpdateAsync(maintenanceWork);
                    }

                    maintenanceWorkSky.SignOrganization = input.MaintenanceQueryParams.SignOrganization;
                    maintenanceWorkSky.WorkOrgAndDutyPerson = input.MaintenanceQueryParams.WorkOrgAndDutyPerson;
                    maintenanceWorkSky.FirstTrial = input.MaintenanceQueryParams.FirstTrial;
                    maintenanceWorkSky.Remark = input.MaintenanceQueryParams.Remark;

                    await _maintenanceWorkRltSkylightPlanRepository.UpdateAsync(maintenanceWorkSky);
                }

                #endregion
                //高铁新增需求：添加兑现反馈：若反馈为未完成，则该计划可以进行变更（保留原有计划）
                SkylightPlan newSkyPlanEnt = new SkylightPlan();
                if (input.IsChange)
                {
                    List<KeyValuePair<Guid, decimal>> details = new List<KeyValuePair<Guid, decimal>>();
                    input.PlanDetails.ForEach(m => planDetails.Add(new KeyValuePair<Guid, decimal>(m.DailyPlanId, m.PlanCount)));
                    string newWorkContent = GetWorkContent(planDetails);

                    input.Id = Guid.NewGuid();
                    newSkyPlanEnt = ObjectMapper.Map<SkylightPlanUpdateDto, SkylightPlan>(input);
                    newSkyPlanEnt.CreateTime = DateTime.Now;
                    newSkyPlanEnt.RepairTagId = RepairTagId;
                    newSkyPlanEnt.WorkUnit = input.OrganizationId;
                    newSkyPlanEnt.Station = input.StationId;
                    newSkyPlanEnt.EndStationId = input.EndStationId;
                    newSkyPlanEnt.EndStationRelateRailwayType = input.EndStationRelateRailwayType;
                    newSkyPlanEnt.IsAdjacent = input.IsAdjacent;
                    //newSkyPlanEnt.WorkAreaId = skyPlanEnt.WorkAreaId;
                    newSkyPlanEnt.PlanState = isOther ? PlanState.NotIssued : PlanState.UnDispatching;
                    //input.PlanType != PlanType.VerticalSkylight ? PlanState.UnDispatching : PlanState.UnSubmited;
                    newSkyPlanEnt.WorkContent = string.IsNullOrWhiteSpace(newWorkContent) ? input.WorkContent : newWorkContent;
                    newSkyPlanEnt.WorkContentType = input.WorkContentType;
                    newSkyPlanEnt.IsChange = false;
                    newSkyPlanEnt.RegistrationPlace = input.RegistrationPlace;
                    await _skylightPlanRepository.InsertAsync(newSkyPlanEnt);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    #region 更新维修作业
                    if (maintenanceWorkSky != null)
                    {
                        maintenanceWorkSky.SkylightPlanId = newSkyPlanEnt.Id;
                        await _maintenanceWorkRltSkylightPlanRepository.UpdateAsync(maintenanceWorkSky);
                    }
                    #endregion

                    // 重新保存关联机房信息
                    input.WorkSiteIds = input.WorkSiteIds == null ? new List<Guid>() : input.WorkSiteIds;
                    foreach (var id in input.WorkSiteIds)
                    {
                        //skyPlanEnt.WorkSites.Add(new SkylightPlanRltInstallationSite(Guid.NewGuid())
                        //{
                        //    SkylightPlanId = skyPlanEnt.Id,
                        //    InstallationSiteId = id,
                        //});

                        var skylightPlanRltInstallationSite = new SkylightPlanRltInstallationSite(Guid.NewGuid())
                        {
                            SkylightPlanId = newSkyPlanEnt.Id,
                            InstallationSiteId = id,
                        };
                        await _skyRltInstallationSiteRepository.InsertAsync(skylightPlanRltInstallationSite);
                    }

                    //更新工作票
                    var workTickets = _skylightPlanRltWorkTicket.Where(x => x.SkylightPlanId == skyPlanEnt.Id);
                    foreach (var item in workTickets)
                    {
                        item.SkylightPlanId = newSkyPlanEnt.Id;
                        await _skylightPlanRltWorkTicket.UpdateAsync(item);
                    }

                    var oldSkyPlan = _skylightPlanRepository.FirstOrDefault(x => x.Id == skyPlanEnt.Id);
                    oldSkyPlan.WorkContent = "(计划已变更)" + oldSkyPlan.WorkContent;
                    oldSkyPlan.IsChange = true;
                    oldSkyPlan.ChangTime = input.WorkTime.ToString("yyyy-MM-dd HH:mm:ss");
                    await _skylightPlanRepository.UpdateAsync(oldSkyPlan);
                    //如果变更为非年月表计划,则需删除之前计划关联的年月表计划
                    if (input.WorkContentType == WorkContentType.OtherPlan)
                    {
                        var oldDetailsPlanIds = _planDetailRepository.Where(x => x.SkylightPlanId == oldSkyPlan.Id).Select(x => x.Id);
                        var oldPlanRltEqument = _planRelateEquipmentRepository.Where(y => oldDetailsPlanIds.Contains(y.PlanDetailId)).Select(x => x.Id);
                        await _planRelateEquipmentRepository.DeleteAsync(x => oldPlanRltEqument.Contains(x.Id));
                        await _planDetailRepository.DeleteAsync(x => oldDetailsPlanIds.Contains(x.Id));
                    }
                }
                else
                {
                    skyPlanEnt.Station = input.StationId;
                    skyPlanEnt.StationRelateRailwayType = input.StationRelateRailwayType;
                    //skyPlanEnt.WorkSite = input.WorkSiteId == null ? Guid.Empty : (Guid)input.WorkSiteId;

                    //变更代码 刘娟娟 2020年12月18日15:08:46

                    // 清楚之前关联机房信息
                    await _skyRltInstallationSiteRepository.DeleteAsync(x => x.SkylightPlanId == skyPlanEnt.Id);
                    //skyPlanEnt.WorkSites = new List<SkylightPlanRltInstallationSite>();
                    input.WorkSiteIds = input.WorkSiteIds == null ? new List<Guid>() : input.WorkSiteIds;
                    // 重新保存关联机房信息
                    foreach (var id in input.WorkSiteIds)
                    {
                        //skyPlanEnt.WorkSites.Add(new SkylightPlanRltInstallationSite(Guid.NewGuid())
                        //{
                        //    SkylightPlanId = skyPlanEnt.Id,
                        //    InstallationSiteId = id,
                        //});

                        var skylightPlanRltInstallationSite = new SkylightPlanRltInstallationSite(Guid.NewGuid())
                        {
                            SkylightPlanId = skyPlanEnt.Id,
                            InstallationSiteId = id,
                        };
                        await _skyRltInstallationSiteRepository.InsertAsync(skylightPlanRltInstallationSite);

                    }

                    skyPlanEnt.WorkUnit = input.OrganizationId;
                    //skyPlanEnt.WorkTime = DateTime.Parse(input.WorkTime.ToShortDateString());
                    skyPlanEnt.WorkTime = input.WorkTime;
                    skyPlanEnt.CreateTime = DateTime.Now;
                    skyPlanEnt.RepairTagId = RepairTagId;
                    skyPlanEnt.RailwayId = input.RailwayId;
                    skyPlanEnt.PlanState = input.PlanState;
                    skyPlanEnt.WorkContentType = input.WorkContentType;
                    skyPlanEnt.RegistrationPlace = input.RegistrationPlace ?? "";
                    skyPlanEnt.EndStationId = input.EndStationId;
                    skyPlanEnt.EndStationRelateRailwayType = input.EndStationRelateRailwayType;
                    skyPlanEnt.IsAdjacent = input.IsAdjacent;

                    #region 作业内容
                    //var dailyPlanIds = input.PlanDetails.Select(x => x.DailyPlanId).ToList();
                    input.PlanDetails.ForEach(m => planDetails.Add(new KeyValuePair<Guid, decimal>(m.DailyPlanId, m.PlanCount)));
                    string workContent = GetWorkContent(planDetails);
                    skyPlanEnt.WorkContent = input.WorkContentType == WorkContentType.OtherPlan ? input.WorkContent : workContent;
                    #endregion
                    await _skylightPlanRepository.UpdateAsync(skyPlanEnt);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    //删除已删除的计划详情
                    IQueryable<PlanDetail> delPlanDetails;// = new List<PlanDetail>();
                    if (input.PlanDetails == null || input.PlanDetails.Count == 0)
                    {
                        delPlanDetails = allPlanDetails.Where(x => x.SkylightPlanId == input.Id);
                    }
                    else
                    {
                        delPlanDetails = allPlanDetails.Where(x => x.SkylightPlanId == input.Id && !input.PlanDetails.Select(m => m.Id).Contains(x.Id));
                    }
                    if (delPlanDetails.Count() > 0)
                    {
                        await DelPlanDetail(delPlanDetails.Select(x => x.Id).ToList());
                    }
                }
                int i = 1;
                foreach (var detailDto in input.PlanDetails)
                {
                    detailDto.SkylightPlanId = skyPlanEnt.Id;
                    var dailyPlan = _allDailyPlans.FirstOrDefault(x => x.RepairTagId == RepairTagId && x.Id == detailDto.DailyPlanId);
                    if (dailyPlan == null) throw new UserFriendlyException("第" + i + "条计划内容对应日计划不存在，请删除此条内容");
                    //if (dailyPlan != null) detailDto.PlanCount = dailyPlan.Count;
                    if (detailDto.PlanCount > dailyPlan.Count) throw new UserFriendlyException("作业数量不可大于计划数量");
                    var bfDetailEnt = allPlanDetails.FirstOrDefault(x => x.Id == detailDto.Id);
                    PlanDetail savedPlanDetail = null;
                    //新添的计划内容
                    if (detailDto.Id == Guid.Empty || bfDetailEnt == null)
                    {
                        if (input.IsChange)
                        {
                            detailDto.SkylightPlanId = newSkyPlanEnt.Id;
                        }
                        detailDto.Id = Guid.NewGuid();
                        var detailEnt = ObjectMapper.Map<PlanDetailUpdateDto, PlanDetail>(detailDto);
                        detailEnt.RepairTagId = RepairTagId;
                        savedPlanDetail = await _planDetailRepository.InsertAsync(detailEnt);
                    }
                    //已存在的计划内容
                    else if (bfDetailEnt != null)
                    {
                        if (input.IsChange)
                        {
                            bfDetailEnt.SkylightPlanId = newSkyPlanEnt.Id;
                        }
                        bfDetailEnt.PlanCount = detailDto.PlanCount;
                        bfDetailEnt.WorkCount = detailDto.WorkCount;
                        bfDetailEnt.RepairTagId = RepairTagId;
                        savedPlanDetail = await _planDetailRepository.UpdateAsync(bfDetailEnt);
                    }
                    //计划内容关联设备
                    if (detailDto.RelateEquipments?.Count > 0)
                    {
                        //删除之前添加的空关联设备数据
                        var emptyRelEquips = allPlanRelateEquips.Where(x => x.PlanDetailId == detailDto.Id && x.EquipmentId == null);
                        //if (emptyRelEquips.Count() > 0)
                        //{
                        //    foreach (var emptyRelEquip in emptyRelEquips)
                        //    {
                        //        await _planRelateEquipmentRepository.DeleteAsync(emptyRelEquip.Id);
                        //    }
                        //}
                        await _planRelateEquipmentRepository.DeleteAsync(x => emptyRelEquips.Select(y => y.Id).Contains(x.Id));
                        //删除已删除的关联设备
                        var delRelEquips = allPlanRelateEquips.Where(x => x.PlanDetailId == detailDto.Id && !detailDto.RelateEquipments.Select(m => m.Id).Contains(x.Id));
                        if (delRelEquips.Count() > 0)
                        {
                            await DelRelEquip(delRelEquips.Select(x => x.Id).ToList());
                        }
                        foreach (var relEquipDto in detailDto.RelateEquipments)
                        {
                            relEquipDto.PlanDetailId = detailDto.Id;
                            var bfRelEquipEnt = allPlanRelateEquips.FirstOrDefault(x => x.Id == relEquipDto.Id);
                            //新添的关联设备
                            if (relEquipDto.Id == Guid.Empty || bfRelEquipEnt == null)
                            {
                                relEquipDto.Id = Guid.NewGuid();
                                //relEquipDto.PlanCount = detailDto.PlanCount;
                                var relEquipEnt = ObjectMapper.Map<PlanRelateEquipmentUpdateDto, PlanRelateEquipment>(relEquipDto);
                                relEquipEnt.RepairTagId = RepairTagId;
                                var savedRelEquip = await _planRelateEquipmentRepository.InsertAsync(relEquipEnt);
                                //添加关联设备测试项
                                if (savedRelEquip != null)
                                    await AddRelEquipTest(savedPlanDetail.DailyPlanId, savedRelEquip.Id, RepairTagId);
                            }
                            //已存在的关联设备
                            else if (bfRelEquipEnt != null)
                            {
                                bfRelEquipEnt.PlanCount = relEquipDto.PlanCount;
                                bfRelEquipEnt.WorkCount = relEquipDto.WorkCount;
                                bfRelEquipEnt.RepairTagId = RepairTagId;
                                await _planRelateEquipmentRepository.UpdateAsync(bfRelEquipEnt);
                            }
                        }
                    }
                    //未关联设备 添加一条空的内容
                    else if (detailDto.RelateEquipments == null || detailDto.RelateEquipments.Count == 0)
                    {
                        //删除之前添加的空关联设备数据
                        var emptyRelEquips = allPlanRelateEquips.Where(x => x.PlanDetailId == detailDto.Id && x.EquipmentId == null).ToList();
                        if (emptyRelEquips.Count > 0)
                        {
                            foreach (var emptyRelEquip in emptyRelEquips)
                            {
                                await _planRelateEquipmentRepository.DeleteAsync(emptyRelEquip.Id);
                            }
                        }
                        PlanRelateEquipment emptyEnt = new PlanRelateEquipment(Guid.NewGuid())
                        {
                            PlanDetailId = savedPlanDetail.Id,
                            EquipmentId = null,
                            PlanCount = detailDto.PlanCount,
                            IsComplete = Enums.AcceptanceResults.Unfinished,
                            RepairTagId = RepairTagId
                        };
                        var savedRelEquip = await _planRelateEquipmentRepository.InsertAsync(emptyEnt);
                        //添加关联设备测试项
                        if (savedRelEquip != null)
                            await AddRelEquipTest(savedPlanDetail.DailyPlanId, savedRelEquip.Id, RepairTagId);
                    }
                    i++;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// 天窗简单编辑 目前用于高铁科维修作业二次审批编辑计划时
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SimpleUpdate(SkylightPlanSimpleUpdateDto input)
        {
            if (input.PlanDate == null) throw new UserFriendlyException("计划日期有误");
            if (input.TimeLength <= 0) throw new UserFriendlyException("计划时长有误");
            var oldSkylightPlan = await _skylightPlanRepository.GetAsync(input.Id);
            oldSkylightPlan.WorkTime = input.PlanDate;
            oldSkylightPlan.TimeLength = input.TimeLength;
            await _skylightPlanRepository.UpdateAsync(oldSkylightPlan);

            //删除不再关联的工作内容
            var oldPlanDetails = _planDetailRepository.Where(s => s.SkylightPlanId == input.Id && s.RepairTagId == oldSkylightPlan.RepairTagId && s.Id != null).ToList();
            var delPlanDetails = oldPlanDetails.Where(s => !input.SavedPlanDetialIds.Contains(s.Id));
            if (delPlanDetails.Count() > 0)
            {
                await DelPlanDetail(delPlanDetails.Select(x => x.Id).ToList());
            }
            return true;
        }
        #endregion

        #region 工作票
        [Produces("application/octet-stream")]
        public async Task<Stream> Mulexport()
        {

            throw new UserFriendlyException("::");
            //return WordHelper.CreateDocxRAR();
        }

        /// <summary>
        /// 创建工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkTicketDto> CreateWorkTicket(WorkTicketCreateDto input)
        {
            if (input.SkylightPlanId == null || input.SkylightPlanId == Guid.Empty)
                throw new UserFriendlyException("关联的垂直天窗id有误");
            var skylightPlan = await _skylightPlanRepository.GetAsync(input.SkylightPlanId);
            if (skylightPlan == null) throw new UserFriendlyException("关联的垂直天窗不存在");
            if (string.IsNullOrEmpty(input.WorkTitle)) throw new UserFriendlyException("作业名称不能为空");
            if (string.IsNullOrEmpty(input.WorkPlace)) throw new UserFriendlyException("作业地点不能为空");
            if (string.IsNullOrEmpty(input.WorkContent)) throw new UserFriendlyException("作业内容不能为空");
            if (string.IsNullOrEmpty(input.SecurityMeasuresAndAttentions)) throw new UserFriendlyException("安全技术措施及注意事项不能为空");
            if (string.IsNullOrEmpty(input.PaperMaker)) throw new UserFriendlyException("制表人不能为空");
            if (string.IsNullOrEmpty(input.PersonInCharge)) throw new UserFriendlyException("作业负责人不能为空");

            var ent = ObjectMapper.Map<WorkTicketCreateDto, WorkTicket>(input);
            ent.RepairLevel = skylightPlan.Level;
            ent.FillInTime = DateTime.Now;
            var resEnt = await _workTickets.InsertAsync(ent);

            var rlt = new SkylightPlanRltWorkTicket(Guid.NewGuid());
            rlt.SkylightPlanId = input.SkylightPlanId;
            rlt.WorkTicketId = resEnt.Id;
            await _skylightRltTicket.InsertAsync(rlt);
            await CurrentUnitOfWork.SaveChangesAsync();
            //保存工作票与配合单位关联关系
            foreach (var item in input.WorkTicketRltCooperationUnits)
            {
                var workTicketRltCooperation = new WorkTicketRltCooperationUnit(Guid.NewGuid())
                {
                    WorkTicketId = ent.Id,
                    Type = item.Type,
                    CooperateWorkShopId = item.Id,
                    CooperateContent = input.CooperateContent,
                    MainWorkShopId = skylightPlan.WorkUnit,
                    State = WorkTicketRltCooperationUnitState.UnFinish
                };
                await _workTicketRltCooperationUnit.InsertAsync(workTicketRltCooperation);
            }

            return ObjectMapper.Map<WorkTicket, WorkTicketDto>(resEnt);
        }

        /// <summary>
        /// 获取工作票
        /// </summary>
        /// <param name="id">垂直天窗id</param>
        /// <returns></returns>
        public async Task<List<WorkTicketDto>> GetWorkTickets(Guid id)
        {
            List<WorkTicketDto> result = new List<WorkTicketDto>();
            var tickets = await GetTicketsBySkylightPlanId(id);


            //result = ObjectMapper.Map<List<WorkTicket>, List<WorkTicketDto>>(tickets);
            foreach (var item in tickets)
            {
                var workTicketDto = ObjectMapper.Map<WorkTicket, WorkTicketDto>(item);
                var ticketsRltcooperation = _workTicketRltCooperationUnit.Where(x => x.WorkTicketId == item.Id);
                foreach (var rlt in ticketsRltcooperation)
                {
                    var dto = ObjectMapper.Map<WorkTicketRltCooperationUnit, WorkTicketRltCooperationUnitDto>(rlt);
                    dto.Id = rlt.CooperateWorkShopId;
                    var organizationName = _organizationRespository.FirstOrDefault(x => x.Id == rlt.CooperateWorkShopId)?.Name;
                    workTicketDto.Completion += organizationName + "：" + (string.IsNullOrEmpty(rlt.Completion) ? "配合作业未完成" : rlt.Completion) + "\n";
                    workTicketDto.CooperateContent = rlt.CooperateContent;
                    workTicketDto.workTicketRltCooperationUnits.Add(dto);
                }
                result.Add(workTicketDto);
            }
            return result;
        }

        /// <summary>
        /// 获取工作票完成数量情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkTicketFinishInfoDto> GetWorkTicketFinishInfo(Guid id)
        {
            WorkTicketFinishInfoDto result = new WorkTicketFinishInfoDto();
            var tickets = await GetTicketsBySkylightPlanId(id);
            var ticketDtos = ObjectMapper.Map<List<WorkTicket>, List<WorkTicketDto>>(tickets);
            result.TotalCount = ticketDtos.Count;
            result.FinishCount = ticketDtos.Count(s => s.IsFinish);
            return result;
        }

        /// <summary>
        /// 编辑工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkTicketDto> UpdateWorkTicket(WorkTicketUpdateDto input)
        {
            if (input.SkylightPlanId == null || input.SkylightPlanId == Guid.Empty)
                throw new UserFriendlyException("关联的垂直天窗id有误");
            var skylightPlan = await _skylightPlanRepository.GetAsync(input.SkylightPlanId);
            if (skylightPlan == null) throw new UserFriendlyException("关联的垂直天窗不存在");
            var oldTicket = await _workTickets.GetAsync(input.Id);
            if (oldTicket == null) throw new UserFriendlyException("此工作票不存在");

            if (string.IsNullOrEmpty(input.WorkTitle)) throw new UserFriendlyException("作业名称不能为空");
            if (string.IsNullOrEmpty(input.WorkPlace)) throw new UserFriendlyException("作业地点不能为空");
            if (string.IsNullOrEmpty(input.WorkContent)) throw new UserFriendlyException("作业内容不能为空");
            if (string.IsNullOrEmpty(input.SecurityMeasuresAndAttentions)) throw new UserFriendlyException("安全技术措施及注意事项不能为空");
            if (string.IsNullOrEmpty(input.PaperMaker)) throw new UserFriendlyException("制表人不能为空");
            if (string.IsNullOrEmpty(input.PersonInCharge)) throw new UserFriendlyException("作业负责人不能为空");

            oldTicket.FillInTime = oldTicket?.FillInTime;
            oldTicket.FinishContent = input.FinishContent;
            oldTicket.InfluenceRange = input.InfluenceRange;
            oldTicket.OrderNumber = input.OrderNumber;
            oldTicket.PaperMaker = input.PaperMaker;
            oldTicket.PersonInCharge = input.PersonInCharge;
            oldTicket.PlanFinishTime = input.PlanFinishTime;
            oldTicket.PlanStartTime = input.PlanStartTime;
            oldTicket.RealFinsihTime = input.RealFinsihTime;
            oldTicket.RealStartTime = input.RealStartTime;
            //oldTicket.RepairLevel = input.RepairLevel;
            oldTicket.SafetyDispatchCheckerId = input.SafetyDispatchCheckerId;
            oldTicket.SecurityMeasuresAndAttentions = input.SecurityMeasuresAndAttentions;
            oldTicket.TechnicalCheckerId = input.TechnicalCheckerId;
            oldTicket.WorkContent = input.WorkContent;
            oldTicket.WorkPlace = input.WorkPlace;
            oldTicket.WorkTitle = input.WorkTitle;
            oldTicket.SafeGuard = input.SafeGuard;
            var resEnt = await _workTickets.UpdateAsync(oldTicket);

            //保存配合单位的关联关系
            var rltCoomperations = _workTicketRltCooperationUnit.DeleteAsync(x => x.WorkTicketId == input.Id);
            foreach (var item in input.WorkTicketRltCooperationUnits)
            {
                var workTicketRltCooperationUnit = new WorkTicketRltCooperationUnit(Guid.NewGuid())
                {
                    CooperateWorkShopId = item.Id,
                    Type = item.Type,
                    WorkTicketId = input.Id,
                    CooperateContent = input.CooperateContent,
                    State = WorkTicketRltCooperationUnitState.UnFinish
                };

                await _workTicketRltCooperationUnit.InsertAsync(workTicketRltCooperationUnit);
            }

            return ObjectMapper.Map<WorkTicket, WorkTicketDto>(resEnt);
        }

        /// <summary>
        /// 完成工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkTicketDto> FinishWorkTicket(WorkTicketFinishDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty)
                throw new UserFriendlyException("id有误");
            var oldTicket = await _workTickets.GetAsync(input.Id);
            if (oldTicket == null) throw new UserFriendlyException("此工作票不存在");
            if (input.RealFinsihTime < input.RealStartTime) throw new UserFriendlyException("作业结束时间应大于开始时间");
            oldTicket.OrderNumber = input.OrderNumber;
            oldTicket.InfluenceRange = input.InfluenceRange;
            oldTicket.FinishContent = input.FinishContent;
            oldTicket.RealStartTime = input.RealStartTime;
            oldTicket.RealFinsihTime = input.RealFinsihTime;
            oldTicket.FinishTime = DateTime.Now;
            oldTicket.IsFine = input.IsFine;
            var resEnt = await _workTickets.UpdateAsync(oldTicket);
            return ObjectMapper.Map<WorkTicket, WorkTicketDto>(resEnt);

        }

        /// <summary>
        /// 删除工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> DeleteWorkTicket(WorkTicketDeleteDto input)
        {
            if (input.TicketId == Guid.Empty ||
                input.TicketId == null || input.TicketId == Guid.Empty)
                throw new UserFriendlyException("id有误");

            var oldTicket = await _workTickets.GetAsync(input.TicketId);
            if (oldTicket == null) throw new UserFriendlyException("此工作票不存在");
            await _skylightRltTicket.DeleteAsync(s => s.WorkTicketId == input.TicketId);
            await _workTickets.DeleteAsync(input.TicketId);
            return true;
        }

        #endregion

        #region 配合作业
        /// <summary>
        /// 配合单位收到消息通知进行确认回复
        /// </summary>
        /// <param name="MessageId">代办消息Id</param>
        /// <returns></returns>
        public async Task<bool> ConfirmTodoMessage(Guid MessageId)
        {
            if (Guid.Empty == MessageId)
            {
                throw new UserFriendlyException("消息id有误");
            }

            var messageNotice = _notices.Where(x => x.Id == MessageId).FirstOrDefault();

            if (null == messageNotice)
            {
                throw new UserFriendlyException("消息不存在");
            }
            var content = JToken.Parse(messageNotice.Content);

            var isSponsor = content["Sponsor"].ToString();
            if (isSponsor == "False")
            {
                messageNotice.Process = true;
                await _notices.UpdateAsync(messageNotice);
                return true;
            }

            //通知消息发起人，配合作业消息已经确认
            var organizationId = Guid.Parse(content["OrgId"].ToString());
            var creatorId = CurrentUser.Id.GetValueOrDefault();
            await _crPlanMaintenanceWorkAppService.PushMessageAsync(null, messageNotice.CreatorId, SendModeType.Organization, creatorId, false);

            //同组织机构下的其他人将未读消息变为已读
            var organizationRltUsers = await _identityUserManager.GetUsersInOrganizationAsync(organizationId);

            var sameOrganizationUserIds = organizationRltUsers.Select(x => x.Id).ToList();

            //获取当前登录人信息
            var currentUserId = CurrentUser.Id.GetValueOrDefault();
            if (!sameOrganizationUserIds.Contains(currentUserId))
            {
                return false;
            }

            var messageNotices = _notices.Where(x => x.CreatorId == messageNotice.CreatorId && sameOrganizationUserIds.Contains(x.UserId)).ToList();
            var planId = content["PlanId"];

            foreach (var item in messageNotices)
            {
                var itemContent = JToken.Parse(messageNotice.Content);
                var itemPanId = content["PlanId"];

                if (itemPanId == planId)
                {
                    item.Process = true;
                    await _notices.UpdateAsync(item);
                }

            }

            return true;
        }
        /// <summary>
        /// 根据待办消息查询配合作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<WorkTicketSimpleDto>> GetWorkTicketList(WorkTicketSearchDto input)
        {
            var result = new PagedResultDto<WorkTicketSimpleDto>();

            if (input.WorkUnit == null || input.WorkUnit == Guid.Empty) return Task.FromResult(result);

            var organizationCode = _organizationRespository.WhereIf(input.WorkUnit != null && input.WorkUnit != Guid.Empty,
                                                                    x => x.Id == input.WorkUnit).FirstOrDefault()?.Code;
            var organizationIds = _organizationRespository.WhereIf(!string.IsNullOrEmpty(organizationCode),
                                                                     x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();

            var workTicketRltCooperationUnitIds = _workTicketRltCooperationUnit.Where(x => organizationIds.Contains(x.CooperateWorkShopId)).OrderBy(x => x.CreationTime).Select(x => x.WorkTicketId).ToList();

            var workTickets = _workTickets.Where(x => workTicketRltCooperationUnitIds.Contains(x.Id) && x.PlanStartTime >= input.StartTime && x.PlanFinishTime <= input.EndTime).ToList();

            var workTicketsDtos = ObjectMapper.Map<List<WorkTicket>, List<WorkTicketSimpleDto>>(workTickets).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var cooperations = new List<WorkTicketSimpleDto>();
            foreach (var item in workTicketsDtos)
            {
                var workTicketRltCooperationUnit = _workTicketRltCooperationUnit.FirstOrDefault(x => x.WorkTicketId == item.Id && x.CooperateWorkShopId == input.WorkUnit);
                if (workTicketRltCooperationUnit == null)
                {
                    continue;
                }
                var workTicketRltSkylight = _skylightPlanRltWorkTicket.FirstOrDefault(x => x.WorkTicketId == item.Id);

                var skylightPlanPlanState = _skylightPlanRepository.FirstOrDefault(x => x.Id == workTicketRltSkylight.SkylightPlanId)?.PlanState;
                if (
                    skylightPlanPlanState == PlanState.UnSubmited ||
                    skylightPlanPlanState == PlanState.Waitting ||
                    skylightPlanPlanState == PlanState.Revoke ||
                    skylightPlanPlanState == PlanState.Backed ||
                    skylightPlanPlanState == PlanState.UnAdopted
                    )
                {
                    continue;
                }
                item.CooperateContent = workTicketRltCooperationUnit.CooperateContent;
                item.MainWorkShopName = _organizationRespository.FirstOrDefault(x => x.Id == workTicketRltCooperationUnit.MainWorkShopId)?.Name;
                item.SkylightPlanId = workTicketRltSkylight.SkylightPlanId;
                item.WorkTicketRltCooperationUnitId = workTicketRltCooperationUnit.Id;
                item.CooperateRealStartTime = workTicketRltCooperationUnit.CooperateRealStartTime;
                item.CooperateRealFinishTime = workTicketRltCooperationUnit.CooperateRealFinishTime;
                item.Completion = workTicketRltCooperationUnit.Completion;
                item.State = workTicketRltCooperationUnit.State;

                cooperations.Add(item);
            }

            result.Items = cooperations;
            result.TotalCount = workTickets.Count();
            return Task.FromResult(result);
        }

        /// <summary>
        /// 完成配合作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> FinishCooperationWork(FinsCooperationWorkDto input)
        {
            if (input.CooperationWorkId == null || input.CooperationWorkId == Guid.Empty)
            {
                throw new UserFriendlyException("id is null");
            }
            else if (input.CooperateRealStartTime == null || input.CooperateRealFinishTime == null)
            {
                throw new UserFriendlyException("实际时间不可为空");
            }
            else if (string.IsNullOrEmpty(input.Completion))
            {
                throw new UserFriendlyException("完成情况不可为空");
            }

            var cooperationWork = _workTicketRltCooperationUnit.FirstOrDefault(x => x.Id == input.CooperationWorkId);

            if (cooperationWork == null)
            {
                throw new UserFriendlyException("配合作业不存在");
            }

            cooperationWork.CooperateRealStartTime = input.CooperateRealStartTime;
            cooperationWork.CooperateRealFinishTime = input.CooperateRealFinishTime;
            cooperationWork.Completion = input.Completion;
            cooperationWork.State = WorkTicketRltCooperationUnitState.Finish;

            await _workTicketRltCooperationUnit.UpdateAsync(cooperationWork);

            return true;
        }
        #endregion

        /// <summary>
        /// 保存工作方案
        /// </summary>
        /// <param name="fileIds"></param>
        /// <param name="skylightPlanId"></param>
        /// <param name="schemeCoverName"></param>
        /// <returns></returns>
        public async Task<bool> SaveFileRltMaintenanceWork(List<Guid> fileIds, Guid skylightPlanId, string schemeCoverName)
        {
            if (fileIds.Count == 0) throw new UserFriendlyException("请选择文件");

            var maintenanceWorkId = _maintenanceWorkRltSkylightPlanRepository.FirstOrDefault(x => x.SkylightPlanId == skylightPlanId)?.MaintenanceWorkId;

            if (maintenanceWorkId == null) throw new UserFriendlyException("该计划未关联维修计划");

            //判断维修作业是否已经上传方案
            var maintenances = _maintenanceWorkRltFile.FirstOrDefault(x => x.MaintenanceWorkId == maintenanceWorkId);
            if (maintenances != null)
            {
                await _maintenanceWorkRltFile.DeleteAsync(x => x.MaintenanceWorkId == maintenanceWorkId);

            }

            foreach (var item in fileIds)
            {
                var maintenanceWorkRltFile = new MaintenanceWorkRltFile(_guidGenerator.Create())
                {
                    MaintenanceWorkId = maintenanceWorkId.GetValueOrDefault(),
                    FileId = item,
                    SchemeCoverName = schemeCoverName
                };
                await _maintenanceWorkRltFile.InsertAsync(maintenanceWorkRltFile);
            }

            return true;
        }

        /// <summary>
        /// 取消计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skylightId"></param>
        /// <param name="type"></param>
        /// <param name="cancelReason"></param>
        /// <param name="isWorkOrder"></param>
        /// <returns></returns>
        public async Task<bool> CancelPlan(Guid id, string type, string cancelReason, bool isWorkOrder)
        {
            if (id == Guid.Empty) throw new UserFriendlyException("计划标识有误");


            if (!isWorkOrder)
            {
                var planState = PlanState.OrderCancel;
                switch (type)
                {
                    case "2":
                        planState = PlanState.NaturalDisasterCancel;
                        break;
                    case "3":
                        planState = PlanState.OtherReasonCancel;
                        break;
                }
                var skylightPlan = _skylightPlanRepository.FirstOrDefault(x => x.Id == id);

                if (skylightPlan == null) throw new UserFriendlyException("计划不存在");


                skylightPlan.PlanState = planState;
                skylightPlan.Opinion = cancelReason;
                await _skylightPlanRepository.UpdateAsync(skylightPlan);
            }
            else
            {
                var planState = PlanState.OrderCancel;
                var orderState = OrderState.OrderCancel;
                switch (type)
                {
                    case "2":
                        orderState = OrderState.NaturalDisasterCancel;
                        planState = PlanState.NaturalDisasterCancel;
                        break;
                    case "3":
                        orderState = OrderState.OtherReasonCancel;
                        planState = PlanState.OtherReasonCancel;
                        break;
                }
                var workOrder = _workOrder.FirstOrDefault(x => x.Id == id);
                if (workOrder == null) throw new UserFriendlyException("作业不存在");

                workOrder.OrderState = orderState;

                var skylightPlan = _skylightPlanRepository.FirstOrDefault(x => x.Id == workOrder.SkylightPlanId);
                skylightPlan.PlanState = planState;
                skylightPlan.Opinion = cancelReason;
                await _skylightPlanRepository.UpdateAsync(skylightPlan);
            }
            return true;
        }

        /// <summary>
        /// 跨月变更计划
        /// </summary>
        /// <param name="skylightPlanId"></param>
        /// <returns></returns>
        public async Task<bool> CrossMonthChange(Guid? skylightPlanId)
        {
            if (skylightPlanId == null) throw new UserFriendlyException("id有误");
            var skylightPlan = _skylightPlanRepository.FirstOrDefault(x => x.Id == skylightPlanId);
            if (skylightPlan == null) throw new UserFriendlyException("计划不存在");

            var repairTagId = skylightPlan.RepairTagId;
            try
            {
                var detailPlans = _planDetailRepository.Where(x => x.RepairTagId == repairTagId && x.SkylightPlanId == skylightPlanId).ToList();
                if (detailPlans?.Count > 0)
                {
                    var detailPlanIds = detailPlans.Select(x => x.Id);
                    var relateEquips = _planRelateEquipmentRepository.Where(x => x.RepairTagId == repairTagId && detailPlanIds.Contains(x.PlanDetailId)).ToList();

                    if (relateEquips?.Count > 0)
                    {
                        var relEquipIds = relateEquips.Select(x => x.Id);
                        //删除设备测试项
                        var equipTests = _equipmentTestRepository.Where(x => x.RepairTagId == repairTagId && relEquipIds.Contains(x.PlanRelateEquipmentId)).ToList();
                        if (equipTests?.Count > 0)
                        {
                            foreach (var eqTest in equipTests)
                            {
                                var relFiles = _fileRepository.Where(x => x.Id == eqTest.Id).ToList();
                                if (relFiles?.Count > 0)
                                {
                                    foreach (var file in relFiles)
                                    {
                                        await _fileRepository.DeleteAsync(file.Id);
                                    }
                                }
                                await _equipmentTestRepository.DeleteAsync(eqTest.Id);
                            }
                        }
                        //删除关联设备检修人员
                        var repairUsers = _repairUserRepository.Where(x => x.RepairTagId == repairTagId && relEquipIds.Contains(x.PlanRelateEquipmentId.Value)).ToList();
                        if (repairUsers?.Count > 0)
                        {
                            foreach (var rpUser in repairUsers)
                            {
                                await _repairUserRepository.DeleteAsync(rpUser.Id);
                            }
                        }
                        //删除关联设备
                        foreach (var rlEq in relateEquips)
                        {
                            await _planRelateEquipmentRepository.DeleteAsync(rlEq.Id);
                        }
                    }
                    //删除计划详情
                    foreach (var item in detailPlans)
                    {
                        await _planDetailRepository.DeleteAsync(item.Id);
                    }
                    //await Task.Run(() => detailPlans.ForEach(x => _planDetailRepository.DeleteAsync(x.Id)));
                }

                //更新天窗计划
                skylightPlan.IsChange = true;
                skylightPlan.ChangTime = "其他月份";
                skylightPlan.WorkContent = "(计划已变更)" + skylightPlan.WorkContent;
                await _skylightPlanRepository.UpdateAsync(skylightPlan);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        /// <summary>
        /// 一键生成月计划
        /// </summary>
        /// <param name="workUnitId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<bool> OneTouchMonthPlan(Guid workUnitId, DateTime month)
        {
            if (workUnitId == null || workUnitId == Guid.Empty)
            {
                return false;
            }

            //获取生成时间DateTime.DaysInMonth(dtNow.Year ,dtNow.Month)
            var startTime = new DateTime(month.Year, 1, 1);
            var endTime = new DateTime(month.Year, 1, DateTime.DaysInMonth(month.Year, month.Month)).AddDays(1).AddSeconds(-1);

            //删除原有计划
            var oldStartTime = new DateTime(month.Year, month.Month, 1);
            var oldEndTime = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month)).AddDays(1).AddSeconds(-1);

            var oldSkylightPlanIds = _skylightPlanRepository.Where(x => x.WorkUnit == workUnitId
                && oldStartTime <= x.WorkTime
                && x.WorkTime <= oldEndTime && x.PlanType == PlanType.Other).Select(x => x.Id).ToList();

            if (oldSkylightPlanIds.Count() > 0)
            {
                var oldDetailsPlanIds = _planDetailRepository.Where(x => oldSkylightPlanIds.Contains(x.SkylightPlanId)).Select(x => x.Id);
                var oldPlanRltEqument = _planRelateEquipmentRepository.Where(y => oldDetailsPlanIds.Contains(y.PlanDetailId)).Select(x => x.Id);
                await _equipmentTestRepository.DeleteAsync(x => oldPlanRltEqument.Contains(x.PlanRelateEquipmentId));
                await _planRelateEquipmentRepository.DeleteAsync(x => oldPlanRltEqument.Contains(x.Id));
                await _planDetailRepository.DeleteAsync(x => oldDetailsPlanIds.Contains(x.Id));
                await _skyRltInstallationSiteRepository.DeleteAsync(x => oldSkylightPlanIds.Contains(x.SkylightPlanId));
                await _skylightPlanRepository.DeleteAsync(x => oldSkylightPlanIds.Contains(x.Id));
            }

            var yearEndTime = new DateTime(month.Year, 12, 31).AddDays(1).AddSeconds(-1);

            var allDailyPlans = _dailyPlanRepository.Where(x => x.PlanDate >= startTime && x.PlanDate <= yearEndTime && x.PlanType == 2);

            _allYearMonthPlanTests = _ymPlanTest.Where(s => s.PlanYear == month.Year && !s.IsDeleted);

            //_allYearMonthPlans = _yearMonthPlanRepository.Where(s => s.Id != null && s.Year == month.Year && s.Month == month.Month);
            //查找当月与一月份对比的计划
            var skylightPlans = _skylightPlanRepository
                .WithDetails(x => x.WorkSites)
                .Where(x => x.WorkUnit == workUnitId && startTime <= x.WorkTime && x.WorkTime <= endTime && x.PlanType == PlanType.Other)
                //.Select(x => new { x.WorkContent, x.WorkTime })
                .ToList();

            if (skylightPlans.Count == 0)
            {
                throw new UserFriendlyException("请制定本年度一月份的其他计划");
            }

            foreach (var skylightPlan in skylightPlans)
            {
                if (!skylightPlan.WorkContent.Contains("月表")) continue;

                var details = new List<PlanDetail>();

                var newWorkTime = skylightPlan.WorkTime.AddMonths(month.Month - 1);
                var newOtherPlanDto = ObjectMapper.Map<SkylightPlan, SkylightPlanDto>(skylightPlan);

                var newWorkContentArry = skylightPlan.WorkContent.Split('。');

                string newWorkContent = "";
                foreach (var item in newWorkContentArry)
                {
                    if (item.Contains("年表") || string.IsNullOrEmpty(item)) continue;
                    newWorkContent = item;
                }

                newOtherPlanDto.Id = _guidGenerator.Create();
                newOtherPlanDto.WorkTime = newWorkTime;
                newOtherPlanDto.PlanState = PlanState.NotIssued;

                var newOtherPlan = ObjectMapper.Map<SkylightPlanDto, SkylightPlan>(newOtherPlanDto);
                newOtherPlan.CreateTime = DateTime.Now;
                newOtherPlan.WorkAreaId = skylightPlan.WorkAreaId.GetValueOrDefault();
                newOtherPlan.WorkContent = newWorkContent;

                await _skylightPlanRepository.InsertAsync(newOtherPlan);
                await CurrentUnitOfWork.SaveChangesAsync();

                //关联机房，保存机房信息
                foreach (var item in skylightPlan.WorkSites)
                {
                    var skylightRltInstallationSite = new SkylightPlanRltInstallationSite(_guidGenerator.Create())
                    {
                        SkylightPlanId = newOtherPlan.Id,
                        InstallationSiteId = item.InstallationSiteId
                    };

                    await _skyRltInstallationSiteRepository.InsertAsync(skylightRltInstallationSite);
                }

                //查找详细计划
                var planDetailDailys = _planDetailRepository
                    .Where(x => x.SkylightPlanId == skylightPlan.Id)
                    .Select(x => new { x.DailyPlanId, x.Id, x.PlanCount, x.RepairTagId, x.WorkCount })
                    .ToList();

                var planDetailDailyIds = planDetailDailys.
                    Select(x => x.DailyPlanId)
                    .ToList();

                var dailyPlans = allDailyPlans
                    .Where(x => planDetailDailyIds.Contains(x.Id))
                    .ToList();

                foreach (var dailyPlan in dailyPlans)
                {
                    var yearMonthPlan = _yearMonthPlanRepository.FirstOrDefault(x => x.Id == dailyPlan.PlanId);

                    var yearMonthPlanNumber = yearMonthPlan.Number;
                    var yearMonthRepairDetailsId = yearMonthPlan.RepairDetailsId;

                    if (string.IsNullOrWhiteSpace(yearMonthPlanNumber)) continue;

                    var sameYearMonthPlan = _yearMonthPlanRepository.
                      FirstOrDefault(x => x.Number == yearMonthPlanNumber && x.Month == month.Month
                      && x.Year == month.Year && x.ResponsibleUnit == workUnitId
                      && (bool)x.IsImport && x.PlanType == 2);

                    if (sameYearMonthPlan == null) continue;

                    var newDailyPlan = allDailyPlans.FirstOrDefault(x => x.PlanId == sameYearMonthPlan.Id &&
                        x.PlanDate.Day == dailyPlan.PlanDate.Day &&
                        x.RepairTagId == dailyPlan.RepairTagId && x.PlanType == 2);

                    //判断这条计划有没有被其他的类型计划使用
                    //var isExistDetail = _planDetailRepository.Any(x => x.DailyPlanId == newDailyPlan.Id); || isExistDetail 

                    if (newDailyPlan == null) continue;

                    var oldDetailPlan = planDetailDailys.FirstOrDefault(x => x.DailyPlanId == dailyPlan.Id);

                    if (oldDetailPlan.WorkCount == 0) continue;

                    var newDetailPlan = new PlanDetail(_guidGenerator.Create())
                    {
                        SkylightPlanId = newOtherPlan.Id,
                        DailyPlanId = newDailyPlan.Id,
                        PlanCount = oldDetailPlan.PlanCount,
                        RepairTagId = oldDetailPlan.RepairTagId,
                        WorkCount = 0,
                    };
                    await _planDetailRepository.InsertAsync(newDetailPlan);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    details.Add(newDetailPlan);

                    var planRltEquipment = new PlanRelateEquipment(_guidGenerator.Create())
                    {
                        PlanDetailId = newDetailPlan.Id,
                        PlanCount = oldDetailPlan.PlanCount,
                        WorkCount = 0,
                        RepairTagId = oldDetailPlan.RepairTagId,
                        IsComplete = AcceptanceResults.Unfinished
                    };

                    await _planRelateEquipmentRepository.InsertAsync(planRltEquipment);

                    var standTests = _allYearMonthPlanTests
                        .Where(x => x.RepairTagId == newDetailPlan.RepairTagId
                        && x.RepairDetailsID == yearMonthRepairDetailsId
                        && x.PlanYear == month.Year && !x.IsDeleted).ToList();

                    if (standTests.Count() > 0)
                    {
                        foreach (var test in standTests)
                        {
                            Guid testId = Guid.NewGuid();
                            EquipmentTestResult testEnt = new EquipmentTestResult(testId)
                            {
                                PlanRelateEquipmentId = planRltEquipment.Id,
                                TestId = test.Id,
                                TestName = test.Name,
                                TestType = (RepairTestType)test.TestType,
                                PredictedValue = test.PredictedValue,
                                MaxRated = test.MaxRated == null ? 0 : (decimal)test.MaxRated,
                                MinRated = test.MinRated == null ? 0 : (decimal)test.MinRated,
                                FileId = test.FileId,
                                RepairTagId = newDetailPlan.RepairTagId,
                                Unit = test.TestUnit
                            };
                            await _equipmentTestRepository.InsertAsync(testEnt);
                        }
                    }
                }

                //判断计划中是否存在月表计划，若不存在，则删除计划
                if (details.Count == 0) await _skylightPlanRepository.DeleteAsync(x => x.Id == newOtherPlan.Id);
            }
            return true;
        }

        /// <summary>
        /// 天窗计划数据统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<SkylightPalnDataStatisticDto> GetSkylightDataStatistic(SkylightPlanSearchInputDto input)
        {
            if (Guid.Empty == input.WorkUnit) throw new UserFriendlyException("请选择执行车间");


            var organizationCode = _organizationRespository.WhereIf(input.WorkUnit != null && input.WorkUnit != Guid.Empty,
                                                                      x => x.Id == input.WorkUnit).FirstOrDefault()?.Code;
            var organizationIds = _organizationRespository.WhereIf(!string.IsNullOrEmpty(organizationCode),
                                                                     x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();

            SkylightPalnDataStatisticDto result = new SkylightPalnDataStatisticDto();

            var skylightPlans = _skylightPlanRepository
                .Where(x => organizationIds.Contains(x.WorkUnit)
                    && x.WorkTime.Month == input.StartTime.GetValueOrDefault().Month
                    && x.PlanType != PlanType.Other)
                .WhereIf(input.RailwayId != Guid.Empty && input.RailwayId != null, x => x.RailwayId == input.RailwayId)
                .WhereIf(input.IsOnRoad != null, x => x.IsOnRoad == input.IsOnRoad)
                .ToList();

            //垂直兑现率
            result.VerticalSkylightCount.VerticalSkylightCompletionRate = GetCompletionRate(input, organizationIds, PlanType.VerticalSkylight);

            //点外兑现率
            result.SkylightOutsideCount.SkylightOutsideCompletionRate = GetCompletionRate(input, organizationIds, PlanType.SkylightOutside);

            if (skylightPlans.Count() == 0) return Task.FromResult(result);

            //垂直天窗I
            result.VerticalSkylightCount.LeveIRepairInSkylightFinshedCount = skylightPlans
                .Where(x => x.Level.Contains("1") && x.PlanType == PlanType.VerticalSkylight && x.PlanState == PlanState.Complete)
                .Count();

            result.VerticalSkylightCount.LeveIRepairInSkylightUnFinshedCount = skylightPlans
              .Where(x => x.Level.Contains("1") && x.PlanType == PlanType.VerticalSkylight && x.PlanState != PlanState.Complete)
              .Count();

            //垂直天窗II
            result.VerticalSkylightCount.LeveIIRepairInSkylightFinshedCount = skylightPlans
                .Where(x => x.Level.Contains("2") && x.PlanType == PlanType.VerticalSkylight && x.PlanState == PlanState.Complete)
                .Count();

            result.VerticalSkylightCount.LeveIIRepairInSkylightUnFinshedCount = skylightPlans
              .Where(x => x.Level.Contains("2") && x.PlanType == PlanType.VerticalSkylight && x.PlanState != PlanState.Complete)
              .Count();

            //天窗点外
            result.SkylightOutsideCount.FinshedCount = skylightPlans
              .Where(x => x.PlanType == PlanType.SkylightOutside && x.PlanState == PlanState.Complete)
              .Count();

            result.SkylightOutsideCount.UnFinshedCount = skylightPlans
            .Where(x => x.PlanType == PlanType.SkylightOutside && x.PlanState != PlanState.Complete)
            .Count();

            return Task.FromResult(result);
        }

        /// <summary>
        /// 获取垂直天窗具体数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<SkylightPlanSpecificDataStatisticsDto> GetSkylightSpecificData(SkylightPlanSearchInputDto input)
        {
            if (Guid.Empty == input.WorkUnit) throw new UserFriendlyException("请选择执行车间");

            var skylightSpecificData = new SkylightPlanSpecificDataStatisticsDto();

            var organizationCode = _organizationRespository.WhereIf(input.WorkUnit != null && input.WorkUnit != Guid.Empty,
                                                                            x => x.Id == input.WorkUnit).FirstOrDefault()?.Code;
            var organizationIds = _organizationRespository.WhereIf(!string.IsNullOrEmpty(organizationCode),
                                                                     x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();

            var level = ((int)input.RepaireLevel.GetValueOrDefault()).ToString();

            var skylightPlans = _skylightPlanRepository
                .Where(x => organizationIds.Contains(x.WorkUnit) && x.WorkTime.Month == input.StartTime.GetValueOrDefault().Month)
                .WhereIf(level == "1" || level == "2", x => x.Level.Contains(level))
                .WhereIf(level == "0", x => x.PlanType == PlanType.SkylightOutside)
                .WhereIf(input.RailwayId != null, x => x.RailwayId == input.RailwayId)
                .WhereIf(input.IsOnRoad != null, x => x.IsOnRoad == input.IsOnRoad)
                .Select(x => new { x.PlanState })
                .ToList();

            skylightSpecificData.NaturalCancelCount = skylightPlans
                .Where(x => x.PlanState == PlanState.NaturalDisasterCancel)
                .Count();

            skylightSpecificData.OrderCancelCount = skylightPlans
                .Where(x => x.PlanState == PlanState.OrderCancel)
                .Count();

            skylightSpecificData.OtherCancelCount = skylightPlans
                .Where(x => x.PlanState == PlanState.OtherReasonCancel)
                .Count();

            skylightSpecificData.CompleteCount = skylightPlans
                .Where(x => x.PlanState == PlanState.Complete)
                .Count();

            skylightSpecificData.ProcessingCount = skylightPlans
                .Where(x => x.PlanState != PlanState.Complete &&
                    x.PlanState != PlanState.NaturalDisasterCancel &&
                    x.PlanState != PlanState.OrderCancel &&
                    x.PlanState != PlanState.OtherReasonCancel
                    )
                .Count();

            return Task.FromResult(skylightSpecificData);
        }

        /// <summary>
        /// 下载计划方案封面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanCoverInfoDto> DownLoadPlanCover(Guid id)
        {
            if (id == Guid.Empty) throw new UserFriendlyException("计划不存在!");

            var maintenanceWork = _maintenanceWorkRltSkylightPlanRepository.WithDetails(x => x.MaintenanceWork).FirstOrDefault(x => x.SkylightPlanId == id).MaintenanceWork;

            if (maintenanceWork == null) throw new UserFriendlyException("维修计划不存在!");

            var members = await bpmManager.ProcessMember(maintenanceWork.ARKey.GetValueOrDefault());

            //编制人
            var createMember = await _identityUserManager.GetByIdAsync(maintenanceWork.CreatorId.GetValueOrDefault());
            var createTime = bpmManager.ProcessTime(maintenanceWork.ARKey.GetValueOrDefault(), createMember.Id);

            //审核人
            var auditor = await _identityUserManager.GetByIdAsync(members.TechnicalMemberId);
            var auditorTime = bpmManager.ProcessTime(maintenanceWork.ARKey.GetValueOrDefault(), createMember.Id);

            //批准人
            var approveMember = await _identityUserManager.GetByIdAsync(members.ChiefMemberId);
            var approveTime = bpmManager.ProcessTime(maintenanceWork.ARKey.GetValueOrDefault(), createMember.Id);

            var planInfo = new PlanCoverInfoDto()
            {
                CreateName = createMember?.Name,
                Auditor = auditor?.Name,
                ApproveName = approveMember?.Name,
                CreateTime = createTime?.ToString("yyyy年MM月dd日"),
                AuditorTime = auditorTime?.ToString("yyyy年MM月dd日"),
                ApproveTime = approveTime?.ToString("yyyy年MM月dd日"),
                PlanCoverName = _maintenanceWorkRltFile.FirstOrDefault(x => x.MaintenanceWorkId == maintenanceWork.Id)?.SchemeCoverName
            };


            return planInfo;
        }

        #region 私有方法

        /// <summary>
        /// 拼接工作内容
        /// </summary>
        /// <param name="planDetails">key:日计划ID value:作业数量</param>
        /// <returns></returns>
        private string GetWorkContent(List<KeyValuePair<Guid, decimal>> planDetails)
        {
            int yearPlanCount = 0; int monthPlanCount = 0;
            Dictionary<string, decimal> yearPlanContents = new Dictionary<string, decimal>();
            Dictionary<string, decimal> monthPlanContents = new Dictionary<string, decimal>();
            try
            {
                var yearMonthPlanList = _allYearMonthPlans.OrderBy(x => x.PlanType).ThenBy(m => m.Number.Replace("-", ""));
                planDetails.ForEach(m =>
                {
                    //日计划-年月计划（作业内容）
                    var dailyPlan = _allDailyPlans.FirstOrDefault(x => x.Id == m.Key);
                    YearMonthPlan yearMonthPlan = null;
                    if (dailyPlan != null)
                    {
                        //m.PlanCount = dailyPlan.Count;
                        yearMonthPlan = yearMonthPlanList.FirstOrDefault(x => x.Id == dailyPlan.PlanId);
                    }
                    if (yearMonthPlan != null)
                    {
                        var nums = yearMonthPlan.Number.Split('-');
                        string newNum = "";
                        foreach (var num in nums)
                        {
                            newNum += int.Parse(num).ToString() + "-";
                        }
                        newNum = newNum.TrimEnd('-');

                        string curContent = newNum + "." + yearMonthPlan.RepairContent;
                        if ((YearMonthPlanType)yearMonthPlan.PlanType == YearMonthPlanType.年表 || (YearMonthPlanType)yearMonthPlan.PlanType == YearMonthPlanType.年度月表)
                        {
                            if (!yearPlanContents.ContainsKey(curContent))
                            {
                                yearPlanCount++;
                                yearPlanContents.Add(curContent, m.Value);
                            }
                            else
                            {
                                var yearContent = yearPlanContents.FirstOrDefault(x => x.Key == curContent);
                                yearPlanContents.Remove(yearContent.Key);
                                yearPlanContents.Add(yearContent.Key, yearContent.Value + m.Value);
                            }
                        }
                        else if ((YearMonthPlanType)yearMonthPlan.PlanType == YearMonthPlanType.月表)
                        {
                            if (!monthPlanContents.ContainsKey(curContent))
                            {
                                monthPlanCount++;
                                monthPlanContents.Add(curContent, m.Value);
                            }
                            else
                            {
                                var monthContent = monthPlanContents.FirstOrDefault(x => x.Key == curContent);
                                monthPlanContents.Remove(monthContent.Key);
                                monthPlanContents.Add(monthContent.Key, monthContent.Value + m.Value);
                            }
                        }
                    }
                });
                string workContent = "";
                if (yearPlanCount > 0)
                {
                    var ycontentOrders = yearPlanContents.OrderBy(x => x.Key.Substring(0, x.Key.IndexOf('.')));
                    workContent = "年表" + yearPlanCount + "项：";
                    for (int i = 1; i <= ycontentOrders.Count(); i++)
                    {
                        var curContent = ycontentOrders.ElementAt(i - 1);
                        if (i == ycontentOrders.Count())
                            workContent += curContent.Key + "(" + curContent.Value + ")。";
                        else
                            workContent += curContent.Key + "(" + curContent.Value + ")；";
                    }
                }
                if (monthPlanCount > 0)
                {
                    var mcontentOrders = monthPlanContents.OrderBy(x => x.Key.Substring(0, x.Key.IndexOf('.')));
                    workContent += "月表" + monthPlanCount + "项：";
                    for (int i = 1; i <= mcontentOrders.Count(); i++)
                    {
                        var curContent = mcontentOrders.ElementAt(i - 1);
                        if (i == mcontentOrders.Count())
                            workContent += curContent.Key + "(" + curContent.Value + ")。";
                        else
                            workContent += curContent.Key + "(" + curContent.Value + ")；";
                    }
                }
                return workContent;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 添加关联设备测试项
        /// </summary>
        /// <param name="dailyPlanId">关联的日计划ID</param>
        /// <param name="planRelEquipId">关联设备表ID</param>
        /// <returns></returns>
        private async Task<bool> AddRelEquipTest(Guid dailyPlanId, Guid planRelEquipId, Guid? repairTagId)
        {
            try
            {
                var yearMonthPlanId = _allDailyPlans.FirstOrDefault(x => x.RepairTagId == repairTagId && x.Id == dailyPlanId)?.PlanId;
                YearMonthPlan yearMonthPlan = _allYearMonthPlans.FirstOrDefault(x => x.RepairTagId == repairTagId && x.Id == yearMonthPlanId);
                //创建设备测试项数据
                if (yearMonthPlan != null && yearMonthPlan.RepairDetailsId != Guid.Empty)
                {
                    var repairDetialGuid = yearMonthPlan.RepairDetailsId;
                    int year = yearMonthPlan.Year;
                    var standTests = _allYearMonthPlanTests
                        .Where(x => x.RepairTagId == repairTagId && x.RepairDetailsID == repairDetialGuid && x.PlanYear == year && !x.IsDeleted)
                        .ToList();
                    if (standTests.Count() > 0)
                    {
                        //查找年月表测试项表格
                        //var fileRels = _allRelationShips.Where(x => x.ty == Basic.Enums.FileType.YearMonthPlanTest).ToList();
                        foreach (var test in standTests)
                        {
                            Guid testId = Guid.NewGuid();
                            EquipmentTestResult testEnt = new EquipmentTestResult(testId)
                            {
                                PlanRelateEquipmentId = planRelEquipId,
                                TestId = test.Id,
                                TestName = test.Name,
                                TestType = (RepairTestType)test.TestType,
                                PredictedValue = test.PredictedValue,
                                MaxRated = test.MaxRated == null ? 0 : (decimal)test.MaxRated,
                                MinRated = test.MinRated == null ? 0 : (decimal)test.MinRated,
                                FileId = test.FileId,
                                RepairTagId = repairTagId,
                                Unit = test.TestUnit
                            };
                            await _equipmentTestRepository.InsertAsync(testEnt);

                            //添加测试项Excel
                            //    if (test.TestType == 3 && fileRels.Count() > 0)
                            //    {
                            //        var guid = test.Id;
                            //        var files = fileRels.Where(x => x.LinkId == guid);
                            //        if (files.Count() > 0)
                            //        {
                            //            foreach (var file in files)
                            //            {
                            //                FileRelationship newFile = new FileRelationship(Guid.NewGuid());
                            //                newFile.FileName = file.FileName;
                            //                newFile.FileId = file.FileId;
                            //                newFile.FileType = Basic.Enums.FileType.EquipmentTestResult;
                            //                newFile.LinkId = testId;
                            //                newFile.LinkTableName = Basic.Enums.LinkTableNameType.SnCrPlan_EquipmentTestResult.ToString();
                            //                await _fileRepository.InsertAsync(newFile);
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 单独删除计划详情及关联表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private async Task<bool> DelPlanDetail(List<Guid> ids)
        {
            try
            {
                var relateEquips = _planRelateEquipmentRepository.Where(x => ids.Contains(x.PlanDetailId)).ToList();

                if (relateEquips.Count() > 0)
                {
                    var relEquipIds = relateEquips.Select(x => x.Id);
                    //删除设备测试项
                    //var equipTests = _equipmentTestRepository.Where(x => relEquipIds.Contains(x.PlanRelateEquipmentId));
                    //if (equipTests.Count() > 0)
                    //{
                    //    foreach (var eqTest in equipTests)
                    //    {
                    //        //var relFiles = _fileRepository.Where(x => x.Id == eqTest.FileId);
                    //        //if (relFiles.Count() > 0)
                    //        //{
                    //        //    foreach (var file in relFiles)
                    //        //    {
                    //        //        await _fileRepository.DeleteAsync(file.Id);
                    //        //    }
                    //        //}
                    //        await _equipmentTestRepository.DeleteAsync(eqTest.Id);
                    //    }
                    //}

                    await _equipmentTestRepository.DeleteAsync(x => relEquipIds.Contains(x.PlanRelateEquipmentId));

                    //删除关联设备检修人员
                    var repairUsers = _repairUserRepository.Where(x => relEquipIds.Contains(x.PlanRelateEquipmentId.Value));
                    if (repairUsers.Count() > 0)
                    {
                        foreach (var rpUser in repairUsers)
                        {
                            await _repairUserRepository.DeleteAsync(rpUser.Id);
                        }
                    }
                    //删除关联设备
                    foreach (var item in relateEquips)
                    {
                        await _planRelateEquipmentRepository.DeleteAsync(item.Id);
                    }

                }
                //删除计划详情
                foreach (var id in ids)
                {
                    await _planDetailRepository.DeleteAsync(id);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 单独删除关联设备及关联表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private async Task<bool> DelRelEquip(List<Guid> ids)
        {
            try
            {
                var equipTests = _equipmentTestRepository.Where(x => ids.Contains(x.PlanRelateEquipmentId)).ToList();
                if (equipTests.Count > 0)
                {
                    //await _fileRepository.DeleteAsync(x => equipTests.Select(y=>y.FileId).Contains(x.Id));
                    await _equipmentTestRepository.DeleteAsync(x => equipTests.Select(y => y.Id).Contains(x.Id));
                }
                //删除关联设备检修人员
                var repairUsers = _repairUserRepository.Where(x => ids.Contains(x.PlanRelateEquipmentId.Value)).ToList();
                if (repairUsers.Count > 0)
                {
                    foreach (var rpUser in repairUsers)
                    {
                        await _repairUserRepository.DeleteAsync(rpUser.Id);
                    }
                }
                //删除关联设备
                foreach (var id in ids)
                {
                    await _planRelateEquipmentRepository.DeleteAsync(id);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据垂直天窗id获取工作票
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<WorkTicket>> GetTicketsBySkylightPlanId(Guid id)
        {
            if (id == null || id == Guid.Empty)
                throw new UserFriendlyException("id有误");
            try
            {
                var skylightPlan = await _skylightPlanRepository.GetAsync(id);

                if (skylightPlan == null) throw new UserFriendlyException("该天窗不存在");
            }
            catch (Exception)
            {

                throw new UserFriendlyException("计划不存在");
            }

            var resEnts = _skylightRltTicket.WithDetails(s => s.WorkTicket).Where(s => s.SkylightPlanId == id).Select(s => s.WorkTicket).ToList();

            //var resId = _skylightRltTicket.Where(s => s.SkylightPlanId == id).Select(s => s.WorkTicketId).ToList();
            //var resEnts = _workTickets.Where(s => resId.Contains(s.Id)).ToList();
            return resEnts;
        }

        /// <summary>
        /// 计算天窗统计率
        /// </summary>
        /// <param name="input"></param>
        /// <param name="orgIds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<CompletionRateRltMonth> GetCompletionRate(SkylightPlanSearchInputDto input, List<Guid> orgIds, PlanType type)
        {
            List<CompletionRateRltMonth> completionRates = new List<CompletionRateRltMonth>();

            //查询当年全部数据
            var skylightPlans = _skylightPlanRepository
              .Where(x => orgIds.Contains(x.WorkUnit)
                  && x.WorkTime.Year == input.StartTime.GetValueOrDefault().Year
                  && x.PlanType != PlanType.Other
                  && (x.PlanState == PlanState.Complete ||
                      x.PlanState == PlanState.NaturalDisasterCancel ||
                      x.PlanState == PlanState.OrderCancel ||
                      x.PlanState == PlanState.OtherReasonCancel)
                  && x.PlanType == type
                  )
              .WhereIf(input.RailwayId != Guid.Empty && input.RailwayId != null, x => x.RailwayId == input.RailwayId)
              .WhereIf(input.IsOnRoad != null, x => x.IsOnRoad == input.IsOnRoad)
              .Select(x => new { x.PlanState, x.PlanType, x.WorkTime })
              .ToList();

            var nowTime = DateTime.Now;

            var nowMonth = nowTime.Month;

            var nowMonthFirstDay = new DateTime(nowTime.Year,nowTime.Month, nowTime.AddDays(1 - (nowTime.Day)).Day);


            for (int i = 1; i <= nowMonth; i++)
            {
                CompletionRateRltMonth completionRateRltMonth = new CompletionRateRltMonth();

                decimal verticalSkylightTotal = 0;
                decimal verticalSkylightComplete = 0;
                //天窗兑现率
                if (i < nowMonth)
                {
                    verticalSkylightTotal = skylightPlans
                      .Where(x => x.WorkTime.Month == i)
                      .Count();

                    verticalSkylightComplete = skylightPlans
                       .Where(x => x.PlanState == PlanState.Complete && x.WorkTime.Month == i)
                       .Count();
                }
                else
                {
                    verticalSkylightTotal = skylightPlans
                        .Where(x => x.WorkTime >= nowMonthFirstDay && x.WorkTime <= nowTime)
                        .Count();

                    verticalSkylightComplete = skylightPlans
                      .Where(x => x.PlanState == PlanState.Complete && x.WorkTime >= nowMonthFirstDay && x.WorkTime <= nowTime)
                      .Count();
                }

                completionRateRltMonth.Month = i;

                if (verticalSkylightTotal == 0 || verticalSkylightComplete == 0)
                {
                    completionRateRltMonth.CompletionRate = 0;
                }
                else
                {
                    var completionRate = verticalSkylightComplete / verticalSkylightTotal;
                    completionRateRltMonth.CompletionRate = Math.Round(completionRate * 100, 2);
                }

                completionRates.Add(completionRateRltMonth);
            }

            return completionRates;
        }

        #endregion

    }
}