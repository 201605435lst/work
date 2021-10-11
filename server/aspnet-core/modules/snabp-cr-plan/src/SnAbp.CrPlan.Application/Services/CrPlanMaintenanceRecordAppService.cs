using SnAbp.CrPlan.Dto.MaintenanceRecord;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.IServices.MaintenanceRecord;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SnAbp.Resource.Entities;
using System.Linq;
using SnAbp.CrPlan.Entities;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using SnAbp.Identity;
using Microsoft.AspNetCore.Authorization;
using SnAbp.CrPlan.Authorization;

namespace SnAbp.CrPlan.Services
{
    public class CrPlanMaintenanceRecordAppService : CrPlanAppService, ICrPlanMaintenanceRecordAppService
    {
        private readonly IRepository<RepairGroup, Guid> _repairGroupRepository;
        private readonly IRepository<RepairItem, Guid> _repairItemRepository;
        private readonly IRepository<RepairItemRltComponentCategory, Guid> _repairItemRltComponentCategoryRepository;
        private readonly IRepository<Equipment> _equipmentRepository;
        private readonly IRepository<EquipmentRltOrganization> _equipRltOrgRepository;
        private readonly OrganizationManager _orgRepository;
        private readonly IRepository<WorkOrganization> _workOrgRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<PlanDetail> _planDetailRepository;
        private readonly IRepository<PlanRelateEquipment> _planDetailRltEquipRepository;
        private readonly IRepository<EquipmentTestResult> _equipTestResRepository;
        private readonly IRepository<DailyPlan> _dailyPlansRepository;
        private readonly IRepository<YearMonthPlan> _yearMonthPlansRepository;
        private readonly IRepository<YearMonthPlanTestItem, Guid> _yearMonthPlanTestItemsRepository;
        private readonly IRepository<SkylightPlan> _skylightPlansRepository;
        private readonly IRepository<RepairUser> _repairUsersRepository;
        private readonly IdentityUserManager _usersRepository;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private readonly IRepository<SkylightPlanRltInstallationSite, Guid> skylightPlanRltInstallationSites;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CrPlanMaintenanceRecordAppService(
            IRepository<RepairGroup, Guid> repairGroupRepository,
            IRepository<RepairItem, Guid> repairItemRepository,
            IRepository<RepairItemRltComponentCategory, Guid> repairItemRltComponentCategoryRepository,
            IRepository<Equipment> equipmentRepository,
            IRepository<EquipmentRltOrganization> equipRltOrgRepository,
            OrganizationManager orgRepository,
            IRepository<WorkOrganization> workOrgRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<PlanDetail> planDetailRepository,
            IRepository<PlanRelateEquipment> planDetailRltEquipRepository,
            IRepository<EquipmentTestResult> equipTestResRepository,
            IRepository<YearMonthPlanTestItem, Guid> yearMonthPlanTestItemsRepository,
            IRepository<SkylightPlan> skylightPlansRepository,
            IRepository<RepairUser> repairUsersRepository,
            IdentityUserManager usersRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<YearMonthPlan> yearMonthPlansRepository,
            IRepository<DailyPlan> dailyPlansRepository,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IRepository<SkylightPlanRltInstallationSite, Guid> skylightPlanRltInstallationSites
            )
        {
            _repairGroupRepository = repairGroupRepository;
            _repairItemRepository = repairItemRepository;
            _repairItemRltComponentCategoryRepository = repairItemRltComponentCategoryRepository;
            _equipmentRepository = equipmentRepository;
            _equipRltOrgRepository = equipRltOrgRepository;
            _orgRepository = orgRepository;
            _workOrgRepository = workOrgRepository;
            _workOrderRepository = workOrderRepository;
            _planDetailRepository = planDetailRepository;
            _planDetailRltEquipRepository = planDetailRltEquipRepository;
            _equipTestResRepository = equipTestResRepository;
            _yearMonthPlanTestItemsRepository = yearMonthPlanTestItemsRepository;
            _skylightPlansRepository = skylightPlansRepository;
            _repairUsersRepository = repairUsersRepository;
            _usersRepository = usersRepository;
            _httpContextAccessor = httpContextAccessor;
            _yearMonthPlansRepository = yearMonthPlansRepository;
            _dailyPlansRepository = dailyPlansRepository;
            _dataDictionaries = dataDictionaries;
            this.skylightPlanRltInstallationSites = skylightPlanRltInstallationSites;
        }

        /// <summary>
        /// 获取检修计划设备列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MaintenanceRecordEquipDto>> GetListFore(MaintenanceRecordEquipSearchDto input)
        {
            PagedResultDto<MaintenanceRecordEquipDto> result = new PagedResultDto<MaintenanceRecordEquipDto>();
            await Task.Run(() =>
            {
                //var allRepairGroup = _repairGroupRepository.WithDetails().ToList();
                ////设备类型筛选   设备名称筛选
                //var groups = (from a in
                //                    allRepairGroup.Where(s => input.EquipTypeId == null || input.EquipTypeId == Guid.Empty || s.Id == input.EquipTypeId)
                //              join b in
                //              allRepairGroup.Where(s => s.ParentId != null && (input.EquipNameId == null || input.EquipNameId == Guid.Empty || s.Id == input.EquipNameId)) on a.Id equals b.ParentId
                //              select b).ToList();

                //var groupsId = groups.Select(s => s.Id);
                //var items = _repairItemRepository.Where(s => groupsId.Contains(s.GroupId));
                //var itemRltCompCate = _repairItemRltComponentCategoryRepository.Where(s => s.Id != null);
                //var equips = _equipmentRepository.WithDetails().Where(s => !s.IsDeleted &&
                //    (input.InstallationSiteId == null || input.InstallationSiteId == Guid.Empty || s.InstallationSiteId == input.InstallationSiteId) &&
                //    (string.IsNullOrEmpty(input.Keywords) || s.Name.Contains(input.Keywords) || s.Code.Contains(input.Keywords))).WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, s => s.EquipmentRltOrganizations.Any(r => r.Organization.Id == input.OrganizationId));

                //var asadas = (from g in groups
                //              join i in items
                //              on g.Id equals i.GroupId
                //              join iRltCom in itemRltCompCate
                //              on i.Id equals iRltCom.RepairItemId into foreTemp
                //              from t in foreTemp.DefaultIfEmpty()
                //              select new { Group = g, Item = i, Rlt = t });

                ////有维修项内的测试项未关联构件分类、有关联构件分类但未设备关联了对应的构件分类 去重后统一称为“未关联设备”
                //var tempEnts = (from f in asadas
                //                join e in equips
                //                on f.Rlt?.ComponentCategoryId equals e.ComponentCategoryId into temp
                //                from tt in temp.DefaultIfEmpty()
                //                select new TempEnt { Group = f.Group, Item = f.Item, Rlt = f.Rlt, Equip = tt });
                //tempEnts = tempEnts.Distinct(new TempEntComparer());

                //List<MaintenanceRecordEquipDto> resDto = new List<MaintenanceRecordEquipDto>();
                //if (input.OrganizationId != null && input.OrganizationId != Guid.Empty)
                //{
                //    tempEnts = tempEnts.Where(s => s.Equip != null);
                //}
                //result.TotalCount = tempEnts.Count();
                //tempEnts = tempEnts.Skip(input.SkipCount).Take(input.MaxResultCount);
                //foreach (var item in tempEnts)
                //{
                //    MaintenanceRecordEquipDto dto = new MaintenanceRecordEquipDto();
                //    if (item.Equip != null)
                //    {
                //        dto.ResourceEquipId = item.Equip.Id;
                //        dto.EquipModelCode = item.Equip.Code;
                //        dto.EquipModelNumber = item.Equip.Name;
                //        dto.InstallationSite = item.Equip.InstallationSite?.Name;
                //        string orgName = "";
                //        foreach (var rlt in item.Equip.EquipmentRltOrganizations)
                //        {
                //            orgName += rlt.Organization.Name + " / ";
                //        }
                //        dto.MaintenanceOrg = orgName != "" ? orgName.Substring(0, orgName.Length - 3) : "";
                //    }
                //    else
                //    {
                //        //维护单位有值时，不显示未关联设备项
                //        if (input.OrganizationId != null && input.OrganizationId != Guid.Empty) continue;
                //        dto.EquipModelNumber = "未关联设备";
                //    }
                //    dto.EquipName = item.Group.Name;
                //    dto.EquipNameId = item.Group.Id;
                //    dto.EquipType = item.Group.Parent?.Name;
                //    resDto.Add(dto);
                //}
                //result.Items = resDto;
            });
            return result;
        }

        public async Task<PagedResultDto<MaintenanceRecordEquipDto>> GetList(MaintenanceRecordEquipSearchDto input)
        {
            PagedResultDto<MaintenanceRecordEquipDto> result = new PagedResultDto<MaintenanceRecordEquipDto>();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(async () =>
            {
                //获取选择的组织机构及其下级id集合
                var org = (await _orgRepository.Where(s => s.Id == input.OrganizationId)).FirstOrDefault()?.Code;
                var orgIds = (await _orgRepository.Where(s => s.Code.StartsWith(org))).Select(s => s.Id).ToList();

                //var installStation = "";//作业处所
                ////选择可见的workOrder
                //var    = _workOrgRepository
                //    .Where(s => s.RepairTagId == RepairTagId && orgIds.Contains(s.OrganizationId))
                //    .Select(s => s.WorkOrderId)
                //    .Distinct();

                //var workOrders = _workOrderRepository
                //    .Where(a => a.RepairTagId == RepairTagId && workOrderIds.Contains(a.Id) && a.StartRealityTime >= input.StartTime && a.EndRealityTime <= input.EndTime)
                //    .Select(x => new { x.SkylightPlanId, x.Id })
                //    .ToList();

                //2021.06.03 检修记录修改
                //根据线路站点查询计划

                //var skylinghtIds = new List<Guid>();
                //var skylightRltInstalIds = new List<Guid>();

                var skylinghtIds = _skylightPlansRepository
                     .Where(x => x.WorkTime >= input.StartTime
                     && x.WorkTime <= input.EndTime
                     && orgIds.Contains(x.WorkUnit)
                     && x.PlanState == Enums.PlanState.Complete
                     && x.WorkContentType == Enums.WorkContentType.MonthYearPlan)
                     .WhereIf(input.RailwayId != null, x => x.RailwayId == input.RailwayId)
                     .WhereIf(input.StationId != null, x => x.Station == input.StationId || x.Station == input.StationId)
                     .Select(x => x.Id)
                     .ToList();

                //&& x.RailwayId != null
                //&& x.Station != null
                //&& (x.RailwayId == input.RailwayId || x.Station == input.StationId || x.EndStationId == input.StationId))

                ////.Select(x => new { x.Id, x.WorkSites })

                if (input.InstallationSiteId != null)
                {
                    skylinghtIds = skylightPlanRltInstallationSites
                       .Where(x => x.InstallationSiteId == input.InstallationSiteId
                           && x.SkylightPlan.WorkTime <= input.EndTime
                           && x.SkylightPlan.WorkTime >= input.StartTime
                           && x.SkylightPlan.WorkContentType == Enums.WorkContentType.MonthYearPlan
                           && x.SkylightPlan.PlanState == Enums.PlanState.Complete)
                       .Select(x => x.SkylightPlan.Id)
                       .ToList();
                }
                //var skylinghtIds = skylinghtPlans.Select(x => x.Id).ToList();

                var workOrders = _workOrderRepository
                    .Where(x => skylinghtIds.Contains(x.SkylightPlanId))
                    .Select(x => new { x.Id, x.SkylightPlanId });

                //var skylinghtIds = workOrders.
                //    Select(s => s.SkylightPlanId)
                //    .Distinct();

                var planDetials = _planDetailRepository
                    .Where(s => s.RepairTagId == RepairTagId && skylinghtIds.Contains(s.SkylightPlanId))
                    .Select(x => new { x.Id, x.DailyPlanId, x.SkylightPlanId })
                    .ToList();

                var planDetailIds = planDetials.Select(s => s.Id);

                var planRelateEquips = _planDetailRltEquipRepository
                    .Where(s => s.RepairTagId == RepairTagId && planDetailIds.Contains(s.PlanDetailId))
                    .Select(x => new { x.EquipmentId, x.PlanDetailId })
                    .ToList();

                var equipIds = planRelateEquips.Select(s => s.EquipmentId).ToList().Distinct();

                var equips = _equipmentRepository
                    .WithDetails()
                    .Where(s => equipIds.Contains(s.Id) && !s.IsDeleted &&
                    (input.InstallationSiteId == null || input.InstallationSiteId == Guid.Empty || s.InstallationSiteId == input.InstallationSiteId) &&
                    (string.IsNullOrEmpty(input.Keywords) || s.Name.Contains(input.Keywords) || s.Code.Contains(input.Keywords)))
                    .Select(y => new { y.Id, y.Code, y.Name, y.InstallationSite })
                    .ToList();

                var dailyPlanIds = planDetials.Select(s => s.DailyPlanId).ToList().Distinct();

                var dailPlans = _dailyPlansRepository
                    .Where(s => s.RepairTagId == RepairTagId && dailyPlanIds.Contains(s.Id))
                    .Select(x => new { x.Id, x.PlanId }).ToList();

                var yearMonthPlanIds = dailPlans.Select(s => s.PlanId).Distinct();

                var yearMonthPlans = _yearMonthPlansRepository
                    .Where(s => s.RepairTagId == RepairTagId && yearMonthPlanIds.Contains(s.Id))
                    .Select(x => new { x.RepairDetailsId, x.Id })
                    .ToList();

                var reapairDetailIds = yearMonthPlans.Select(s => s.RepairDetailsId).Distinct().ToList();

                var repairItems = _repairItemRepository.WithDetails()
                    .Where(s => reapairDetailIds.Contains(s.Id))
                    .WhereIf(input.EquipNameId != null && input.EquipNameId != Guid.Empty, s => s.GroupId == input.EquipNameId)
                    .WhereIf(input.EquipTypeId != null && input.EquipTypeId != Guid.Empty, g => g.Group.ParentId == input.EquipTypeId)
                    .Select(x => new { x.Id, x.Group })
                    .ToList();

                List<MaintenanceRecordEquipDto> res = new List<MaintenanceRecordEquipDto>();

                //var relatePlans = dailPlans.
                //    Where(x => dailyPlanIds.Contains(x.Id))
                //    .Select(x=>new {})
                //    .ToList();

                foreach (var planDetail in planDetials)
                {
                    //设备类型及名称
                    var dailPlan = dailPlans.FirstOrDefault(d => d.Id == planDetail.DailyPlanId);
                    if (dailPlan != null)
                    {
                        //派工作业
                        var workOrder = workOrders.FirstOrDefault(w => w.SkylightPlanId == planDetail.SkylightPlanId);

                        if (workOrder == null) continue;
                        //设备类型 设备名称
                        var yearMonthPlan = yearMonthPlans.FirstOrDefault(y => y.Id == dailPlan.PlanId);
                        var repairDetailId = yearMonthPlan.RepairDetailsId;//repairDetailId即为RepairItem的Id
                        var repairItem = repairItems.FirstOrDefault(r => r.Id == repairDetailId);

                        if (repairItem == null) continue;

                        //设备项
                        var planRltEquip = planRelateEquips.Where(s => s.PlanDetailId == planDetail.Id).Distinct();
                        foreach (var item in planRltEquip)
                        {
                            MaintenanceRecordEquipDto dto = new MaintenanceRecordEquipDto();

                            var installations = skylightPlanRltInstallationSites
                                  .WithDetails(x => x.InstallationSite)
                                  .Where(x => x.SkylightPlanId == planDetail.SkylightPlanId).ToList();
                            var installationlNames = "";


                            if (installations.Count > 0)
                            {
                                var names = installations.Select(x => x.InstallationSite.Name).ToList();
                                names.Sort();
                                installationlNames = string.Join("、", names);
                            }

                            dto.InstallationSite = installationlNames;
                            dto.WorkOrderId = workOrder.Id;
                            dto.EquipName = repairItem.Group?.Name;
                            dto.EquipNameId = repairItem.Group?.Id;
                            dto.EquipType = repairItem.Group?.Parent?.Name;
                            if (item.EquipmentId != null)
                            {

                                var equip = equips.FirstOrDefault(s => s.Id == item.EquipmentId);
                                if (equip == null) continue;    //由于设备已过滤 此处根据id未查找到设备则表示不符合条件
                                dto.EquipModelCode = equip.Code;
                                dto.EquipModelNumber = equip.Name;
                                dto.ResourceEquipId = equip.Id;
                            }
                            else
                            {
                                //存在查询条件 不显示未关联设备的数据
                                //if ((input.InstallationSiteId != null && input.InstallationSiteId != Guid.Empty) ||
                                //(string.IsNullOrEmpty(input.Keywords) == false)) continue;
                                dto.EquipModelNumber = "未关联设备";
                            }

                            if (
                            res.Any(r => r.EquipNameId == dto.EquipNameId
                            && r.ResourceEquipId == dto.ResourceEquipId
                            && r.EquipType == dto.EquipType))
                            {
                                var exitdto = res.FirstOrDefault(r => r.EquipNameId == dto.EquipNameId && r.ResourceEquipId == dto.ResourceEquipId && r.EquipType == dto.EquipType);
                                exitdto.MaintenanceRecordRltWorkOrders.Add(dto.WorkOrderId);
                            }
                            else
                            {
                                res.Add(dto);
                            }
                        }

                    }
                }
                result.TotalCount = res.Count;
                result.Items = res.OrderBy(s => s.EquipType).ToList();//.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            });
            return result;
        }

        /// <summary>
        /// 获取检修记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.MaintenanceRecord.Detail)]
        /// <summary>
        /// 获取检修记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.MaintenanceRecord.Detail)]
        public async Task<PagedResultDto<MaintenanceRecordDto>> MaintenanceRecord(MaintenanceRecordSearchDto input)
        {
            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty)
            {
                throw new UserFriendlyException("组织机构错误");
            }
            PagedResultDto<MaintenanceRecordDto> result = new PagedResultDto<MaintenanceRecordDto>();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(async () =>
            {
                //此处不使用登录用户id，用户可在搜索条件中查找下级            
                var allOrg = await _orgRepository.Where(s => s.Id != null);
                var org = allOrg.FirstOrDefault(s => s.Id == input.OrganizationId)?.Code;

                var inputworkOrderIds = input.WorkOrderIds.Distinct();

                var workOrders = _workOrderRepository
                    .Where(x => inputworkOrderIds.Contains(x.Id))
                    .ToList();
                result.TotalCount = workOrders.Count();
                if (!input.IsAll) workOrders = workOrders.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                var workOrderIds = workOrders
                    .Select(w => w.Id).ToList();

                var workOrgs = _workOrgRepository
                    .Where(w => w.RepairTagId == RepairTagId && workOrderIds.Contains(w.WorkOrderId))
                    .ToList();

                var skylightIds = workOrders.Select(s => s.SkylightPlanId);
                var planDetails = _planDetailRepository
                    .Where(s => s.RepairTagId == RepairTagId && skylightIds.Contains(s.SkylightPlanId))
                    .ToList();
                var planDetailIds = planDetails.Select(s => s.Id);

                var planDetailRltEquips = _planDetailRltEquipRepository
                    .Where(s => s.RepairTagId == RepairTagId && planDetailIds.Contains(s.PlanDetailId) && s.EquipmentId == input.EquipmentId)
                    .ToList();
                var planDetailRltEquipIds = planDetailRltEquips.Select(s => s.Id);

                var repairUsers = _repairUsersRepository
                    .Where(s => s.RepairTagId == RepairTagId && planDetailRltEquipIds.Contains(s.PlanRelateEquipmentId.Value))
                    .ToList();
                var repairUserIds = repairUsers.Select(s => s.UserId);
                var repairUserInfo = await _usersRepository.GetUserListAsync(s => repairUserIds.Contains(s.Id));

                //获取实际测试项内容以及测试结果 验收结果
                var testResult = _equipTestResRepository
                    .WithDetails(s => s.File)
                    .Where(w => w.RepairTagId == RepairTagId && planDetailRltEquipIds.Contains(w.PlanRelateEquipmentId))
                    .ToList();

                //获取对应的维修项id
                var testIds = testResult.Select(s => s.TestId);
                var yearMonthTestItem = _yearMonthPlanTestItemsRepository
                    .Where(s => s.RepairTagId == RepairTagId && testIds.Contains(s.Id))
                    .ToList();
                var yearMonthTestItemIds = yearMonthTestItem
                    .Select(s => s.RepairDetailsID)
                    .ToList();

                var stdRepaitItem = _repairItemRepository
                    .Where(s => s.GroupId == input.RepairGroupId && yearMonthTestItemIds.Contains(s.Id))
                    .ToList();

                List<MaintenanceRecordDto> res = new List<MaintenanceRecordDto>();

                foreach (var workOrder in workOrders)
                {
                    MaintenanceRecordDto dto = new MaintenanceRecordDto();
                    dto.WorkOrderRealEndTime = workOrder.EndRealityTime;
                    var singlePlanDetailIds = planDetails.Where(s => s.SkylightPlanId == workOrder.SkylightPlanId).Select(s => s.Id);
                    var detailRltEquipIds = planDetailRltEquips.Where(s => singlePlanDetailIds.Contains(s.PlanDetailId)).Select(s => s.Id);
                    var singleRepairUsers = repairUsers.Where(s => detailRltEquipIds.Contains(s.PlanRelateEquipmentId.Value)).ToList();
                    var singleTestRes = testResult.Where(s => detailRltEquipIds.Contains(s.PlanRelateEquipmentId)).ToList();

                    List<MaintenanceRepairGroup> record = new List<MaintenanceRepairGroup>();
                    //检修验收人组织机构
                    var testOrgId = workOrgs.FirstOrDefault(w => w.WorkOrderId == workOrder.Id && w.Duty == Enums.Duty.Recondition)?.OrganizationId;
                    var checkOrgId = workOrgs.FirstOrDefault(w => w.WorkOrderId == workOrder.Id && w.Duty == Enums.Duty.Acceptance)?.OrganizationId;

                    var testOrgName = testOrgId != null ? allOrg.FirstOrDefault(o => o.Id == testOrgId)?.Name : "";
                    var checkOrgName = checkOrgId != null ? allOrg.FirstOrDefault(o => o.Id == checkOrgId)?.Name : "";
                    foreach (var item in yearMonthTestItem)
                    {
                        MaintenanceRepairGroup data = null;
                        var repairItem = stdRepaitItem.FirstOrDefault(s => s.Id == item.RepairDetailsID);
                        if (repairItem == null) continue;
                        var existData = record.FirstOrDefault(s => s.RepairItemId == repairItem.Id);
                        if (existData != null)
                        {
                            data = existData;
                        }
                        else
                        {
                            data = new MaintenanceRepairGroup();
                            data.RepairItemId = repairItem.Id;
                            data.RepairItem = repairItem.Content;
                            record.Add(data);
                        }
                        MaintenanceRecordTestItem testItem = new MaintenanceRecordTestItem();
                        var tempTestRes = singleTestRes.FirstOrDefault(s => s.TestId == item.Id);
                        string testUserName = "";
                        string checkUserName = "";
                        if (tempTestRes != null)
                        {
                            var users = singleRepairUsers.Where(s => s.PlanRelateEquipmentId == tempTestRes.PlanRelateEquipmentId);
                            foreach (var user in users)
                            {
                                var info = repairUserInfo.FirstOrDefault(s => s.Id == user.UserId);
                                if (user.Duty == Enums.Duty.Recondition && info != null)
                                {
                                    testUserName += info.Name + " / ";
                                }
                                else if (user.Duty == Enums.Duty.Acceptance)
                                {
                                    checkUserName += info.Name + " / ";
                                    if (checkOrgId == Guid.Empty)
                                    {
                                        var acceptanceUser = await _usersRepository.GetByIdAsync(user.UserId);
                                        var acceptanceOrgId = acceptanceUser.Organizations.FirstOrDefault()?.OrganizationId;
                                        checkOrgName = allOrg.FirstOrDefault(o => o.Id == acceptanceOrgId)?.Name;
                                    }
                                }
                            }
                        }
                        data.TestUserName = string.IsNullOrEmpty(testUserName) ? "暂未填写" : testUserName.Substring(0, testUserName.Length - 3) + "(" + testOrgName + ")";
                        data.CheckUserName = string.IsNullOrEmpty(checkUserName) ? "暂未填写" : checkUserName.Substring(0, checkUserName.Length - 3) + "(" + checkOrgName + ")";
                        testItem.RepairTestItem = item.Name;
                        var testRes = singleTestRes.FirstOrDefault(s => s.TestId == item.Id);
                        testItem.TestType = testRes?.TestType;
                        testItem.TestResult = testRes?.TestResult;
                        if (testRes != null && testRes.FileId != Guid.Empty && testRes.FileId != null)
                        {
                            testItem.FileId = testRes.FileId;
                            testItem.File = ObjectMapper.Map<File.Entities.File, File.Dtos.FileSimpleDto>(testRes.File);
                        }
                        testItem.CheckResult = testRes?.CheckResult;
                        data.TestItems.Add(testItem);
                    }
                    //排序保证维修项以及其中测试项顺序一致
                    dto.RecordDatas.ForEach(a => { a.TestItems = a.TestItems.OrderBy(s => s.RepairTestItem).ToList(); });
                    dto.RecordDatas = record.OrderBy(s => s.RepairItemId).ToList();

                    res.Add(dto);
                }
                result.Items = res.OrderBy(s => s.WorkOrderRealEndTime).ToList();
            });
            return result;
        }

    }


    #region 辅助类
    public class TempEnt
    {
        public RepairGroup Group { get; set; }
        public RepairItem Item { get; set; }
        public RepairItemRltComponentCategory Rlt { get; set; }
        public Equipment Equip { get; set; }
    }

    public class TempEntComparer : IEqualityComparer<TempEnt>
    {
        public bool Equals(TempEnt x, TempEnt y)
        {
            return x.Group == y.Group
              && x.Equip == y.Equip;
        }

        public int GetHashCode(TempEnt obj)
        {
            return 1;
        }
    }
    #endregion
}
