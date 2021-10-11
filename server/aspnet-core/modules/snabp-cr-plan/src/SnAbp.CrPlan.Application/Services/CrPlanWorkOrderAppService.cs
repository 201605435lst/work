using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Basic.Entities;
using SnAbp.CrPlan.Commons;
using SnAbp.CrPlan.Dto.EquipmentTestResult;
using SnAbp.CrPlan.Dto.RepairUser;
using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Dto.Worker;
using SnAbp.CrPlan.Dto.WorkOrder;
using SnAbp.CrPlan.Dto.WorkOrganization;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Enums;
using SnAbp.CrPlan.IServices.SkylightPlan;
using SnAbp.CrPlan.IServices.WorkOrder;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SnAbp.File.Dtos;
using SnAbp.Identity;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SnAbp.CrPlan.Dtos;
using SnAbp.Bpm.Entities;
using Microsoft.AspNetCore.Http;
using SnAbp.StdBasic.Entities;
using NPOI.SS.UserModel;
using System.Text;

namespace SnAbp.CrPlan.Services
{
    /// <summary>
    /// 派工单相关接口实现
    /// </summary>
    [Authorize]
    public class CrPlanWorkOrderAppService : CrPlanAppService, ICrPlanWorkOrderAppService
    {
        private readonly IRepository<WorkOrder, Guid> _workOrder;//派工单
        private readonly IRepository<Worker, Guid> _worker;//作业人员
        private readonly IRepository<WorkOrganization, Guid> _workOrganization;//作业单位
        private readonly IRepository<EquipmentTestResult, Guid> _equipmentTestResult;//设备测试项
        private readonly IRepository<RepairUser, Guid> _repairUser;//检修人员
        private readonly IRepository<PlanRelateEquipment, Guid> _planRelateEquipment;//关联设备
        private readonly IRepository<PlanDetail, Guid> _planDetail;//计划详细信息
        private readonly IRepository<SkylightPlan, Guid> _skylight;//天窗
        private readonly IRepository<InstallationSite, Guid> _installationSite;//处所位置
        private readonly IRepository<Organization, Guid> _organizationsRepository;//组织机构
        private readonly IdentityUserManager _personnel;//用户    
        private readonly OrganizationManager _organization;//组织机构
        private readonly ICrPlanSkylightPlanAppService _skylightPlanAppService; //天窗计划接口实现
        private readonly IRepository<DailyPlan, Guid> _dailyPlans;
        private readonly IRepository<YearMonthPlan, Guid> _yearMonthPlans;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        //private static readonly object Lock = new object();
        private readonly IRepository<MaintenanceWorkRltSkylightPlan, Guid> _maintenanceWorkRltSkylightPlanRepository;//垂直天窗关联维修作业
        private readonly IRepository<Workflow, Guid> _workflowRepository;//工作流
        private readonly IRepository<WorkOrderTestAdditional, Guid> _workOrderTestAdditionals;
        private readonly IRepository<YearMonthPlanTestItem, Guid> _yearMonthPlanTestItems;
        private readonly IRepository<Station, Guid> stations;
        private readonly IRepository<SkylightPlanRltWorkTicket, Guid> skylightPlanRltWorkTickets;
        private readonly IRepository<WorkTicket, Guid> workTicketRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CrPlanWorkOrderAppService(IRepository<WorkOrder, Guid> workOrder,
            IRepository<Worker, Guid> worker,
            IRepository<WorkOrganization, Guid> workOrganization,
            IRepository<EquipmentTestResult, Guid> equipmentTestResult,
            IRepository<RepairUser, Guid> repairUser,
            IRepository<SkylightPlan, Guid> skylight,
            OrganizationManager organization,
            IdentityUserManager personnel,
            IRepository<InstallationSite, Guid> installationSite,
            IRepository<Organization, Guid> organizationsRepository,
            IRepository<PlanRelateEquipment, Guid> planRelateEquipment,
            IRepository<PlanDetail, Guid> planDetail,
            ICrPlanSkylightPlanAppService skylightPlanAppService,
            IRepository<DailyPlan, Guid> dailyPlans,
            IRepository<YearMonthPlan, Guid> yearMonthPlans,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IRepository<MaintenanceWorkRltSkylightPlan, Guid> maintenanceWorkRltSkylightPlanRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Workflow, Guid> workflowRepository,
            IRepository<WorkOrderTestAdditional, Guid> workOrderTestAdditionals,
            IRepository<YearMonthPlanTestItem, Guid> yearMonthPlanTestItems,
            IRepository<Station, Guid> stations,
            IRepository<SkylightPlanRltWorkTicket, Guid> skylightPlanRltWorkTickets,
            IRepository<WorkTicket, Guid> workTicketRepository
            //IRepository<RepairTestItem,Guid> repairTestItems
            )
        {
            _workOrder = workOrder;
            _worker = worker;
            _workOrganization = workOrganization;
            _equipmentTestResult = equipmentTestResult;
            _repairUser = repairUser;
            _planRelateEquipment = planRelateEquipment;
            _planDetail = planDetail;
            _skylight = skylight;
            _organization = organization;
            _personnel = personnel;
            _installationSite = installationSite;
            _organizationsRepository = organizationsRepository;
            _skylightPlanAppService = skylightPlanAppService;
            _dailyPlans = dailyPlans;
            _yearMonthPlans = yearMonthPlans;
            _dataDictionaries = dataDictionaries;
            _maintenanceWorkRltSkylightPlanRepository = maintenanceWorkRltSkylightPlanRepository;
            _httpContextAccessor = httpContextAccessor;
            _workflowRepository = workflowRepository;
            _workOrderTestAdditionals = workOrderTestAdditionals;
            _yearMonthPlanTestItems = yearMonthPlanTestItems;
            this.stations = stations;
            this.skylightPlanRltWorkTickets = skylightPlanRltWorkTickets;
            this.workTicketRepository = workTicketRepository;
        }

        /// <summary>
        /// 根据查询条件，获取已派工模块的主页显示数据列表
        /// 使用范围：车间及以上组织机构人员
        /// </summary>
        /// <param name="input">查询条件实体</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.HaveSentWorkers.Default)]
        public async Task<PagedResultDto<WorkOrderSimpleDto>> GetListSentedWorkOrders(SkylightSearchInputDto input)
        {
            if (input == null) throw new UserFriendlyException("无查询条件");
            var modelList = new List<WorkOrderSimpleDto>();
            PagedResultDto<WorkOrderSimpleDto> res = new PagedResultDto<WorkOrderSimpleDto>();
            var skylightList = new List<SkylightPlan>();
            try
            {

                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(async () =>
                {
                    var workUnitIdList = new List<Guid>();

                    if (input.WorkUnitId != null)
                    {

                        var organization = (await _organization.Where(x => x.Id == input.WorkUnitId)).FirstOrDefault();
                        if (organization != null)
                        {
                            workUnitIdList.Add(input.WorkUnitId);
                            if (organization.Code.Length < 5)
                            {
                                var ids = GetSubsetOrganizationIdList(input.WorkUnitId).Result;
                                workUnitIdList = new List<Guid>();
                                workUnitIdList.AddRange(ids);
                            }
                        }
                    }

                    //根据时间段及天窗计划状态获取天窗列表
                    var timeList = _skylight.Where(z => z.RepairTagId == RepairTagId &&
                    workUnitIdList.Contains(z.WorkUnit) &&
                    (z.PlanState == PlanState.Dispatching || z.PlanState == PlanState.Complete) &&
                    z.PlanType != PlanType.Other);
                    if (timeList?.Count() > 0)
                    {
                        skylightList = timeList.ToList();
                        //根据所传的作业机房查询
                        if (input.InstallationSiteIds != null || input.InstallationSiteIds.Count > 0)
                            skylightList = skylightList.FindAll(m => m.WorkSites.Any(s => input.InstallationSiteIds.Contains(s.InstallationSiteId)));
                        //根据天窗类型查询
                        if (input.PlanType != PlanType.All)
                            skylightList = skylightList.FindAll(m => m.PlanType == input.PlanType && m.PlanType != PlanType.Other);
                        //根据作业内容、作业里程查询
                        if (!string.IsNullOrEmpty(input.OtherConditions))
                            skylightList = skylightList.FindAll(m => (!string.IsNullOrEmpty(m.WorkArea) && m.WorkArea.Contains(input.OtherConditions)) || (!string.IsNullOrEmpty(m.WorkContent) && m.WorkContent.Contains(input.OtherConditions)));
                        //所得的符合所有查询条件的天窗列表
                        if (skylightList?.Count > 0)
                        {

                            //获取天窗Id列表
                            var skylightGuidList = skylightList.ConvertAll(m => m.Id);

                            //
                            //var siteIdList = skylightList.Select(m => m.WorkSites);

                            //天窗关联作业机房变更后的代码
                            //获取天窗作业机房列表
                            var siteIdList = new List<Guid>();
                            foreach (var item in skylightList)
                            {
                                if (item.WorkSites.Count > 0)
                                {
                                    var temp = item.WorkSites.Select(s => s.InstallationSiteId).ToList();
                                    siteIdList.AddRange(temp);
                                }
                            }


                            var siteList = _installationSite.Where(z => siteIdList.Contains(z.Id));
                            //根据天窗Id列表获取派工单列表
                            var orderList = _workOrder.Where(z => z.RepairTagId == RepairTagId && skylightGuidList.Contains(z.SkylightPlanId) && ((z.StartPlanTime >= input.StartPlanTime.Date && z.StartPlanTime <= input.EndPlanTime) || (z.EndPlanTime >= input.StartPlanTime && z.EndPlanTime <= input.EndPlanTime.Date)))?.ToList();
                            //获取派工单Id列表
                            var orderGuidList = orderList.ConvertAll(x => x.Id);

                            //根据派工单列表获取所有相关的检修、验收单位列表
                            var workOrganizationList = _workOrganization.Where(z => z.RepairTagId == RepairTagId && orderGuidList.Contains(z.WorkOrderId));
                            var organizationList = new List<Organization>();
                            if (workOrganizationList?.Count() > 0)
                            {
                                var organizationIdList = workOrganizationList.ToList().ConvertAll(m => m.OrganizationId);
                                organizationList = (await _organization.Where(z => organizationIdList.Contains(z.Id)))?.ToList();
                            }


                            //根据派工单列表获取所有相关的作业组长、作业人员列表
                            var workerList = _worker.Where(z => z.RepairTagId == RepairTagId && orderGuidList.Contains(z.WorkOrderId));
                            var userList = new List<IdentityUser>();
                            if (workerList?.Count() > 0)
                            {
                                var userIdList = workerList.ToList().ConvertAll(m => m.UserId);
                                userList = await _personnel.GetUserListAsync(z => userIdList.Contains(z.Id));
                            }
                            orderList = orderList.OrderBy(z => z.StartPlanTime).ToList();
                            //将获取的数据组织成要返回的Dto实体
                            orderList.ForEach(m =>
                            {
                                WorkOrderSimpleDto dto = new WorkOrderSimpleDto(m.Id);
                                //工作单位
                                string workUint = string.Empty;
                                if (workOrganizationList?.Count() > 0)
                                {
                                    var workUints = workOrganizationList.ToList().FindAll(x => x.WorkOrderId == m.Id);
                                    if (workUints?.Count > 0)
                                    {

                                        for (int i = 0; i < workUints.Count; i++)
                                        {
                                            var organization = organizationList.Find(x => x.Id == workUints[i].OrganizationId);
                                            if (i == 0)
                                            {

                                                workUint = organization.Name;
                                            }
                                            else
                                            {
                                                workUint += "," + organization.Name;
                                            }
                                        }
                                    }
                                }
                                dto.WorkUintString = workUint;
                                //工作人员
                                string workMembersString = string.Empty;
                                if (workerList?.Count() > 0)
                                {
                                    var workers = workerList.ToList().FindAll(x => x.WorkOrderId == m.Id && x.Duty == UserDuty.WorkMembers);
                                    if (workers?.Count > 0)
                                    {
                                        for (int i = 0; i < workers.Count; i++)
                                        {
                                            var user = userList.Find(x => x.Id == workers[i].UserId);
                                            if (user != null)
                                            {
                                                if (i == 0)
                                                {
                                                    workMembersString = user.Name;
                                                }
                                                else
                                                {
                                                    workMembersString += "," + user.Name;
                                                }
                                            }

                                        }
                                    }
                                }
                                dto.WorkMemberString = workMembersString;

                                //天窗相关属性赋值
                                var skylight = skylightList.Find(x => x.Id == m.SkylightPlanId);
                                dto.Level = skylight.Level;
                                dto.PlanTime = skylight.WorkTime;
                                dto.PlanType = skylight.PlanType;
                                dto.TimeLength = skylight.TimeLength;
                                dto.WorkArea = skylight.WorkArea;
                                dto.WorkContent = skylight.WorkContent;
                                //作业机房
                                if (siteList?.Count() > 0)
                                {
                                    if (skylight.WorkSites != null && skylight.WorkSites.Count > 0)
                                    {
                                        //var workSite = siteList.ToList().Find(z => z.Id == skylight.WorkSite);
                                        //变更后的代码
                                        var workSite = siteList.ToList().Find(z => skylight.WorkSites.Any(m => m.InstallationSiteId == z.Id));
                                        dto.WorkSite = workSite.Name;
                                    }
                                }
                                dto.OrderState = m.OrderState;
                                //作业时间
                                dto.WorkTime = m.StartPlanTime.ToString("yyyy.MM.dd HH:mm") + "-" + m.EndPlanTime.ToString("yyyy.MM.dd HH:mm");
                                //作业组长
                                var leader = workerList.ToList().Find(x => x.WorkOrderId == m.Id && x.Duty == UserDuty.WorkLeader);
                                if (leader != null)
                                {
                                    var user = userList.Find(x => x.Id == leader.UserId);
                                    if (user != null)
                                    {
                                        dto.WorkLeader = user.Name;
                                    }

                                }
                                modelList.Add(dto);
                            });
                            //天窗列表总数量
                            res.TotalCount = modelList.Count;
                            modelList = modelList.AsQueryable().PageBy(input.SkipCount, input.MaxResultCount).OrderBy(m => m.WorkTime).ToList();
                            res.Items = modelList;
                        }
                    }


                });
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }

            return res;
        }

        /// <summary>
        /// 根据查询条件，获取派工作业、已完成模块的主页显示数据列表
        /// </summary>
        /// <param name="input">查询条件实体</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.WorkOrder.Default)]
        //[Authorize(CrPlanPermissions.WorkOrderFinished.Default)]
        public async Task<PagedResultDto<WorkOrderSimpleDto>> GetList(WorkOrderSearchInputDto input)
        {
            if (input == null) throw new UserFriendlyException("无查询条件");
            var modelList = new List<WorkOrderSimpleDto>();
            PagedResultDto<WorkOrderSimpleDto> res = new PagedResultDto<WorkOrderSimpleDto>();
            var skylightList = new List<SkylightPlan>();
            var orgs = await _organizationsRepository.GetListAsync();
            //当前作业单位的组织机构code
            var organizationCode = _organizationsRepository.WhereIf(input.WorkUnitId != null && input.WorkUnitId != Guid.Empty,
                                                                    x => x.Id == input.WorkUnitId).FirstOrDefault()?.Code;
            var organizationIds = _organizationsRepository.WhereIf(!string.IsNullOrEmpty(organizationCode),
                                                                    x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();
            var timeList = new List<WorkOrder>();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(async () =>
            {
                //根据派工作业时间进行查询
                var workOrderList = new List<WorkOrder>();
                if (input.StartPlanTime.Date != DateTime.Parse("0001/1/1 0:00:00") && input.EndPlanTime.Date != DateTime.Parse("0001/1/1 0:00:00"))
                {
                    timeList = _workOrder.Where(z => z.RepairTagId == RepairTagId &&
                                                    (z.StartPlanTime.Date >= input.StartPlanTime.Date && z.StartPlanTime.Date <= input.EndPlanTime.Date)
                                                    || (z.EndPlanTime.Date >= input.StartPlanTime.Date && z.EndPlanTime.Date <= input.EndPlanTime.Date))
                                         .ToList();
                    if (input.IsDispatching == true)
                    {
                        timeList = _workOrder.Where(z => z.RepairTagId == RepairTagId &&
                                                        (z.StartRealityTime.Date >= input.StartPlanTime.Date && z.StartRealityTime.Date <= input.EndPlanTime.Date) ||
                                                        (z.EndRealityTime.Date >= input.StartPlanTime.Date && z.EndRealityTime.Date <= input.EndPlanTime.Date))
                                             .ToList();
                    }
                }
                else
                {
                    timeList = _workOrder.Where(z => z.RepairTagId == RepairTagId).ToList();
                }

                if (timeList?.Count() > 0)
                {
                    //获取符合条件的派工单Id列表
                    var workOrderIdList = timeList.ConvertAll(m => m.Id);
                    workOrderList = timeList;

                    if (input.WorkUnitId != null && input.WorkUnitId != Guid.Empty)
                    {
                        //根据组织机构列表及派工单Id查询派工单作业单位表中符合条件的数据
                        var workOrganizations = _workOrganization.Where(z => z.RepairTagId == RepairTagId &&
                                                                             workOrderIdList.Contains(z.WorkOrderId) &&
                                                                             organizationIds.Contains(z.OrganizationId))
                                                                 .ToList();//获取工作单位
                        if (workOrganizations?.Count > 0)
                        {
                            //根据符合条件的作业单位表数据，获取派工单列表
                            var workOrderIds = workOrganizations.ConvertAll(m => m.WorkOrderId);
                            workOrderList = workOrderList.FindAll(m => workOrderIds.Contains(m.Id));
                        }
                        else
                        {
                            workOrderList = new List<WorkOrder>();
                        }
                    }
                    if (workOrderList?.Count() > 0)
                    {
                        //已完成界面使用数据（只显示派工单状态为已验收的数据）
                        if (input.IsDispatching == true)
                        {
                            workOrderList = workOrderList.FindAll(m => m.OrderState == OrderState.Acceptance);
                        }
                        //获取天窗Id列表，因为作业机房、作业内容、作业里程等只存在于天窗数据中
                        var guidList = workOrderList.ConvertAll(m => m.SkylightPlanId);
                        //天窗Id列表
                        if (guidList?.Count > 0)
                        {
                            //获取天窗数据
                            var skylightList = _skylight.WithDetails().Where(z => z.RepairTagId == RepairTagId &&
                                                                                  guidList.Contains(z.Id) && z.PlanType != PlanType.Other)
                                                                      .WhereIf(input.RepairLevel != 0, x => x.Level.Contains(((int)input.RepairLevel).ToString()))
                                                                      .ToList();
                            //根据作业机房查询数据
                            if (input.InstallationSiteId != null && input.InstallationSiteId != Guid.Empty)

                                //skylightList = skylightList.FindAll(m => m.WorkSite == input.InstallationSiteId);
                                //变更后代码 刘娟娟 2020年12月18日14:17:06
                                skylightList = skylightList.FindAll(m => m.WorkSites.Any(s => s.InstallationSiteId == input.InstallationSiteId));

                            //根据作业内容、作业里程查询
                            if (!string.IsNullOrEmpty(input.OtherConditions))
                                skylightList = skylightList.FindAll(m => (!string.IsNullOrEmpty(m.WorkArea) && m.WorkArea.Contains(input.OtherConditions)) || (!string.IsNullOrEmpty(m.WorkContent) && m.WorkContent.Contains(input.OtherConditions)));

                            //符合条件的天窗列表
                            if (skylightList?.Count > 0)
                            {


                                var skylightGuidList = skylightList.ConvertAll(m => m.Id);
                                //获取天窗作业机房列表
                                //var siteIdList = skylightList.ConvertAll(m => m.WorkSite);

                                //天窗关联作业机房变更后的代码 刘娟娟 2020年12月18日14:19:15
                                //获取天窗作业机房列表
                                var siteIdList = new List<Guid>();
                                foreach (var item in skylightList)
                                {
                                    if (item.WorkSites.Count > 0)
                                    {
                                        var temp = item.WorkSites.Select(s => s.InstallationSiteId).ToList();
                                        siteIdList.AddRange(temp);
                                    }
                                }
                                var siteList = _installationSite.Where(z => siteIdList.Contains(z.Id)).ToList();

                                //获取符合条件的派工单列表
                                workOrderList = workOrderList.FindAll(z => skylightGuidList.Contains(z.SkylightPlanId));
                                var orderGuidList = workOrderList.ConvertAll(x => x.Id);

                                //根据派工单列表获取所有相关的检修、验收单位列表
                                var workOrganizationList = _workOrganization.Where(z => z.RepairTagId == RepairTagId && orderGuidList.Contains(z.WorkOrderId)).ToList();
                                var organizationList = new List<Organization>();
                                if (workOrganizationList?.Count() > 0)
                                {
                                    var organizationIdList = workOrganizationList.ConvertAll(m => m.OrganizationId);
                                    organizationList = (await _organization.Where(z => organizationIdList.Contains(z.Id)))?.ToList();
                                }

                                //根据派工单列表获取所有相关的作业组长、作业人员列表
                                var workerList = _worker.Where(z => z.RepairTagId == RepairTagId && orderGuidList.Contains(z.WorkOrderId)).ToList();
                                var userList = new List<IdentityUser>();
                                if (workerList?.Count() > 0)
                                {
                                    var userIdList = workerList.ConvertAll(m => m.UserId);
                                    userList = await _personnel.GetUserListAsync(z => userIdList.Contains(z.Id));
                                }
                                workOrderList = workOrderList.OrderBy(z => z.OrderState).ThenBy(z => z.StartPlanTime).ToList();
                                //将获取的数据组织成要返回的Dto实体
                                workOrderList.ForEach(async m =>
                                {
                                    WorkOrderSimpleDto dto = new WorkOrderSimpleDto(m.Id);
                                    //工作单位
                                    string workUint = string.Empty;
                                    if (workOrganizationList?.Count() > 0)
                                    {
                                        var workUints = workOrganizationList.FindAll(x => x.WorkOrderId == m.Id);
                                        if (workUints?.Count > 0)
                                        {

                                            for (int i = 0; i < workUints.Count; i++)
                                            {
                                                var organization = organizationList.Find(x => x.Id == workUints[i].OrganizationId);
                                                if (organization != null)
                                                    if (i == 0)
                                                    {
                                                        workUint = organization.Name;
                                                    }
                                                    else
                                                    {
                                                        workUint += "," + organization.Name;
                                                    }
                                            }
                                        }
                                    }
                                    dto.WorkUintString = workUint;
                                    //工作人员
                                    string workMembersString = string.Empty;
                                    if (workerList?.Count() > 0)
                                    {
                                        var workers = workerList.FindAll(x => x.WorkOrderId == m.Id && x.Duty == UserDuty.WorkMembers);
                                        if (workers?.Count > 0)
                                        {
                                            for (int i = 0; i < workers.Count; i++)
                                            {
                                                var user = userList.Find(x => x.Id == workers[i].UserId);
                                                //if (user == null)
                                                //{
                                                //    user = await _personnel.GetByIdAsync(workers[i].UserId);
                                                //}
                                                if (user != null)
                                                {
                                                    if (i == 0)
                                                    {
                                                        workMembersString = user.Name;
                                                    }
                                                    else
                                                    {
                                                        workMembersString += "," + user.Name;
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    dto.WorkMemberString = workMembersString;

                                    var skylight = skylightList.Find(x => x.Id == m.SkylightPlanId);
                                    dto.Level = skylight.Level;
                                    dto.OrderState = m.OrderState;
                                    dto.PlanTime = skylight.WorkTime;
                                    dto.PlanType = skylight.PlanType;
                                    dto.TimeLength = skylight.TimeLength;
                                    dto.WorkArea = skylight.WorkArea;
                                    dto.WorkContent = skylight.WorkContent;
                                    dto.CancelReason = skylight.Opinion;
                                    //作业机房
                                    if (siteList?.Count() > 0)
                                    {
                                        if (skylight.WorkSites != null && skylight.WorkSites.Count > 0)
                                        {
                                            //var workSite = siteList.ToList().Find(z => z.Id == skylight.WorkSite);

                                            //变更后代码 刘娟娟 2020年12月18日14:20:19
                                            var workSite = siteList.Find(z => skylight.WorkSites.Any(s => s.InstallationSiteId == z.Id));
                                            dto.WorkSite = workSite.Name;
                                        }
                                    }
                                    dto.WorkTime = m.StartPlanTime.ToString("yyyy.MM.dd HH:mm") + "-" + m.EndPlanTime.ToString("yyyy.MM.dd HH:mm");
                                    if (input.IsDispatching == true)
                                    {
                                        dto.WorkTime = m.StartRealityTime.ToString("yyyy.MM.dd HH:mm") + "-" + m.EndRealityTime.ToString("yyyy.MM.dd HH:mm");
                                    }
                                    //作业组长
                                    var leader = workerList.Find(x => x.WorkOrderId == m.Id && x.Duty == UserDuty.WorkLeader);
                                    if (leader != null)
                                    {
                                        var user = userList.Find(x => x.Id == leader.UserId);
                                        if (user != null)
                                        {
                                            dto.WorkLeader = user.Name;
                                        }
                                    }

                                    modelList.Add(dto);
                                });
                                res.TotalCount = modelList.Count;
                                //modelList = modelList.OrderBy(z => z.OrderState).OrderBy(z => z.PlanTime).ToList();
                                modelList = modelList
                                .AsQueryable()
                                .PageBy(input.SkipCount, input.MaxResultCount)
                                .OrderBy(m => m.OrderState)
                                .ThenBy(n => n.PlanTime)
                                .ToList();
                                res.Items = modelList;
                            }

                        }

                    }
                }

            });

            return res;
        }

        /// <summary>
        /// 根据查询条件获取其他作业列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.OtherAssignments.Default)]
        public async Task<PagedResultDto<OtherHomeworkDto>> GetOtherHomeworkList(OtherPlanSearchInputDto input)
        {
            PagedResultDto<OtherHomeworkDto> result = new PagedResultDto<OtherHomeworkDto>();
            if (input == null) return null;
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //当前作业单位的组织机构code
            var organizationCode = _organizationsRepository.WhereIf(input.WorkAreaId != null && input.WorkAreaId != Guid.Empty,
                                                                    x => x.Id == input.WorkAreaId).FirstOrDefault()?.Code;
            var organizationIds = _organizationsRepository.WhereIf(!string.IsNullOrEmpty(organizationCode),
                                                                    x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();
            await Task.Run(async () =>
            {
                var planResult = _skylight
                    .Where(x =>
                        x.RepairTagId == RepairTagId &&
                        x.PlanType == Enums.PlanType.Other &&
                        (input.WorkAreaId == null || organizationIds.Contains(x.WorkAreaId.Value)) &&
                        //(input.WorkAreaId == null || x.WorkAreaId == input.WorkAreaId) &&
                        (input.StartTime == null || x.WorkTime >= input.StartTime) &&
                        (input.EndTime == null || x.WorkTime <= input.EndTime) &&
                        (
                            string.IsNullOrEmpty(input.WorkContent) ||
                            !string.IsNullOrEmpty(x.WorkContent) && x.WorkContent.Contains(input.WorkContent)
                        )
                    )
                    .OrderBy(m => m.PlanState)
                    .ToList();



                if (planResult?.Count() > 0)
                {
                    var planList = planResult.FindAll(x => x.PlanState == PlanState.Issued || x.PlanState == PlanState.Complete || x.PlanState == PlanState.NaturalDisasterCancel || x.PlanState == PlanState.OrderCancel || x.PlanState == PlanState.OtherReasonCancel).ToList();
                    if (planList?.Count > 0)
                    {
                        var organizationIdList = planList.ConvertAll(m => m.WorkAreaId);
                        var organizationList = (await _organization.Where(z => organizationIdList.Contains(z.Id)))?.ToList();
                        var planIds = planList.ConvertAll(m => m.Id);
                        var orderlist = _workOrder.Where(m => m.RepairTagId == RepairTagId && planIds.Contains(m.SkylightPlanId)).ToList();
                        if (orderlist?.Count > 0)
                        {
                            //result.TotalCount = orderlist.Count;
                            var dtoList = new List<OtherHomeworkDto>();
                            orderlist.ForEach(m =>
                            {
                                OtherHomeworkDto dto = new OtherHomeworkDto(m.Id);
                                var plan = planList.Find(x => x.Id == m.SkylightPlanId);
                                dto.WorkTime = plan.WorkTime;
                                dto.PlanTime = m.StartPlanTime;
                                dto.OrderState = m.OrderState;
                                dto.WorkContent = plan.WorkContent;
                                dto.OrderNo = m.OrderNo;
                                dto.CreationTime = m.CreationTime;
                                var organization = organizationList.Find(x => x.Id == plan.WorkAreaId);
                                if (organization != null)
                                {
                                    dto.WorkUnit = organization.Name;
                                }
                                dtoList.Add(dto);
                            });
                            result.TotalCount = dtoList.Count;
                            dtoList = dtoList.OrderBy(z => z.OrderState).ThenBy(z => z.PlanTime).ToList();
                            dtoList = dtoList.AsQueryable().PageBy(input.SkipCount, input.MaxResultCount).OrderBy(m => m.OrderState).ThenBy(s => s.PlanTime).ToList();
                            result.Items = dtoList;
                        }

                    }


                }

            });

            return result;
        }

        /// <summary>
        /// 根据派工单ID获取派工单实体（无设备测试项）
        /// 根据天窗id 获取PlanDetailDto列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkOrderDto> Get(CommonGuidGetDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("id不正确");
            WorkOrderDto res = new WorkOrderDto();

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(async () =>
            {
                var ent = input.IsUpdate ? _workOrder.FirstOrDefault(m => m.RepairTagId == RepairTagId && m.Id == input.Id) :
                _workOrder.FirstOrDefault(m => m.RepairTagId == RepairTagId && m.SkylightPlanId == input.Id);

                if (ent == null) throw new Exception("对象不存在");
                res = ObjectMapper.Map<WorkOrder, WorkOrderDto>(ent);
                if (res.SendWorkersId != null && res.SendWorkersId != Guid.Empty)
                {
                    var user = (await _personnel.GetUserListAsync(m => m.Id == ent.SendWorkersId)).FirstOrDefault();
                    if (user != null)
                    {
                        res.SendWorkersName = user.Name;
                    }
                }

                if (res.CreatorId != null && res.CreatorId != Guid.Empty)
                {
                    var user = (await _personnel.GetUserListAsync(m => m.Id == ent.CreatorId)).FirstOrDefault();
                    if (user != null)
                    {
                        res.CreatorName = user.Name;
                    }
                }
                if (res.StartRealityTime == DateTime.MinValue || res.EndRealityTime == DateTime.MinValue)
                {
                    res.StartRealityTime = DateTime.Now;
                    res.EndRealityTime = DateTime.Now;
                }
                //根据派工单Id获取所有相关的检修、验收单位列表
                var workOrganizationList = _workOrganization.Where(z => z.RepairTagId == RepairTagId && z.WorkOrderId == ent.Id);
                var organizationList = new List<Organization>();
                if (workOrganizationList?.Count() > 0)
                {
                    var organizationIdList = workOrganizationList.ToList().ConvertAll(m => m.OrganizationId);
                    organizationList = (await _organization.Where(z => organizationIdList.Contains(z.Id)))?.ToList();
                    workOrganizationList.ToList().ForEach(m =>
                    {
                        var organization = organizationList.Find(x => x.Id == m.OrganizationId);
                        WorkOrganizationDto dto = new WorkOrganizationDto(m.Id);
                        dto.OrganizationId = m.OrganizationId;
                        dto.OrganizationName = organization?.Name;
                        dto.WorkOrderId = m.WorkOrderId;
                        dto.Duty = m.Duty;
                        if (m.Duty == Duty.Acceptance)
                        {

                            res.CommunicationUnit = dto;
                        }
                        else
                        {
                            res.MaintenanceUnit = dto;
                        }
                    });

                }
                res.FieldGuardList = new List<Dto.Worker.WorkerDto>();
                res.StationLiaisonOfficerList = new List<Dto.Worker.WorkerDto>();
                //res.WorkLeader = new Dto.Worker.WorkerDto();
                res.WorkMemberList = new List<Dto.Worker.WorkerDto>();
                //根据派工单Id获取所有相关的作业组长、作业人员列表
                var workerList = _worker.Where(z => z.RepairTagId == RepairTagId && z.WorkOrderId == input.Id);
                var userList = new List<IdentityUser>();
                if (workerList?.Count() > 0)
                {
                    var userIdList = workerList.ToList().ConvertAll(m => m.UserId);
                    userList = await _personnel.GetUserListAsync(z => userIdList.Contains(z.Id));
                    workerList.ToList().ForEach(m =>
                    {
                        var user = userList.Find(x => x.Id == m.UserId);
                        WorkerDto dto = new WorkerDto(m.Id);
                        dto.UserId = m.UserId;
                        dto.UserName = user != null ? user.Name : "";
                        dto.WorkOrderId = m.WorkOrderId;
                        dto.Duty = m.Duty;
                        switch (m.Duty)
                        {

                            case UserDuty.FieldGuard:
                                res.FieldGuardList.Add(dto);
                                break;
                            case UserDuty.StationLiaisonOfficer:
                                res.StationLiaisonOfficerList.Add(dto);
                                break;
                            case UserDuty.WorkLeader:
                                res.WorkLeader = dto;
                                break;
                            case UserDuty.WorkMembers:
                                res.WorkMemberList.Add(dto);
                                break;
                        }

                    });

                }
                //天窗内容
                Task<SkylightPlanDetailDto> skylightDto = _skylightPlanAppService.GetInWork(new CommonGuidGetDto { Id = res.SkylightPlanId, RepairTagKey = input.RepairTagKey });
                if (skylightDto != null && skylightDto.Result != null)
                {
                    res.WorkSiteName = skylightDto.Result.WorkSiteName;
                    if (skylightDto.Result.WorkContentType == WorkContentType.MonthYearPlan)
                    {
                        res.WorkContentType = skylightDto.Result.WorkContentType;
                        if (skylightDto.Result.PlanDetails?.Count > 0)
                        {
                            var list = skylightDto.Result.PlanDetails.OrderBy(z => z.DailyPlan.PlanTypeStr).ThenBy(z => z.DailyPlan.Number).ToList();
                            res.PlanDetailList = list;
                            foreach (var item in res.PlanDetailList)
                            {
                                var nums = item.DailyPlan.Number.Split('-');
                                string newNum = "";
                                foreach (var num in nums)
                                {
                                    newNum += int.Parse(num).ToString() + "-";
                                }
                                item.DailyPlan.Number = newNum.TrimEnd('-');

                            }
                        }
                    }
                    if (skylightDto.Result.WorkContentType == WorkContentType.OtherPlan)
                    {
                        res.WorkContentType = skylightDto.Result.WorkContentType;
                        res.WorkContent = skylightDto.Result.WorkContent;
                    }
                }


            });

            return res;
        }

        /// <summary>
        /// 根据派工单ID获取派工单实体（有设备测试项）
        ///根据天窗id 获取PlanDetailDto列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkOrderDetailDto> GetDetail(CommonGuidGetDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("id不正确");
            WorkOrderDetailDto res = new WorkOrderDetailDto();

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(async () =>
            {
                var ent = _workOrder.FirstOrDefault(m => m.RepairTagId == RepairTagId && m.Id == input.Id);

                if (ent == null) throw new Exception("对象不存在");
                res = ObjectMapper.Map<WorkOrder, WorkOrderDetailDto>(ent);
                if (res.StartRealityTime == DateTime.MinValue || res.EndRealityTime == DateTime.MinValue)
                {
                    res.StartRealityTime = res.StartPlanTime;
                    res.EndRealityTime = res.EndPlanTime;
                }
                //根据派工单Id获取所有相关的检修、验收单位列表
                var workOrganizationList = _workOrganization.Where(z => z.RepairTagId == RepairTagId && z.WorkOrderId == input.Id);
                var organizationList = new List<Organization>();
                if (workOrganizationList?.Count() > 0)
                {
                    var organizationIdList = workOrganizationList.ToList().ConvertAll(m => m.OrganizationId);
                    organizationList = (await _organization.Where(z => organizationIdList.Contains(z.Id)))?.ToList();
                    workOrganizationList.ToList().ForEach(m =>
                    {
                        var organization = organizationList.Find(x => x.Id == m.OrganizationId);
                        WorkOrganizationDto dto = new WorkOrganizationDto(m.Id);
                        dto.OrganizationId = m.OrganizationId;
                        dto.OrganizationName = organization?.Name;
                        dto.WorkOrderId = m.WorkOrderId;
                        dto.Duty = m.Duty;
                        if (m.Duty == Duty.Acceptance)
                        {

                            res.CommunicationUnit = dto;
                        }
                        else
                        {
                            res.MaintenanceUnit = dto;
                        }
                    });

                }
                res.FieldGuardList = new List<Dto.Worker.WorkerDto>();
                res.StationLiaisonOfficerList = new List<Dto.Worker.WorkerDto>();
                res.WorkLeader = new Dto.Worker.WorkerDto();
                res.WorkMemberList = new List<Dto.Worker.WorkerDto>();
                if (ent.Feedback != null)
                {
                    res.Feedback = ent.Feedback;
                }
                //根据派工单Id获取所有相关的作业组长、作业人员列表
                var workerList = _worker.Where(z => z.RepairTagId == RepairTagId && z.WorkOrderId == input.Id);
                var userList = new List<IdentityUser>();
                if (workerList?.Count() > 0)
                {
                    var userIdList = workerList.ToList().ConvertAll(m => m.UserId);
                    userList = await _personnel.GetUserListAsync(z => userIdList.Contains(z.Id));
                    workerList.ToList().ForEach(m =>
                    {
                        var user = userList.Find(x => x.Id == m.UserId);
                        WorkerDto dto = new WorkerDto(m.Id);
                        dto.UserId = m.UserId;
                        dto.UserName = user != null ? user.Name : "";
                        dto.WorkOrderId = m.WorkOrderId;
                        dto.Duty = m.Duty;
                        switch (m.Duty)
                        {

                            case UserDuty.FieldGuard:
                                res.FieldGuardList.Add(dto);
                                break;
                            case UserDuty.StationLiaisonOfficer:
                                res.StationLiaisonOfficerList.Add(dto);
                                break;
                            case UserDuty.WorkLeader:
                                res.WorkLeader = dto;
                                break;
                            case UserDuty.WorkMembers:
                                res.WorkMemberList.Add(dto);
                                break;
                        }

                    });

                }

                List<EquipmentTestResultDto> testResultList = new List<EquipmentTestResultDto>();
                List<Guid> testIdList = new List<Guid>();
                List<Guid> repairUserIdList = new List<Guid>();

                //测试人员
                var repairUserList = new List<RepairUser>();
                var skylight = _skylight.FirstOrDefault(x => x.Id == ent.SkylightPlanId);
                if (skylight.WorkContentType == WorkContentType.OtherPlan)
                {
                    repairUserList = _repairUser.Where(x => x.WorkerOrderId == ent.Id && x.WorkerOrderId != null).ToList();
                }
                else
                {
                    //获取测试项及测试人员
                    GetEquipTestOrUserId(res.SkylightPlanId, ref testIdList, ref repairUserIdList);
                    //测试项
                    //高铁新增需求：测试项的排序要与维修项的排序保持一致
                    if (testIdList?.Count > 0)
                    {
                        var testList = _equipmentTestResult.WithDetails(x => x.File).Where(z => z.RepairTagId == RepairTagId && testIdList.Contains(z.Id));
                        var yearMonthTestIds = testList.Select(x => x.TestId);

                        var yearMonthTests = _yearMonthPlanTestItems.Where(x => yearMonthTestIds.Contains(x.Id)).ToList();

                        if (testList?.Count() > 0)
                        {
                            //测试项对应上传的文件
                            // TODO 2020-7-23
                            //var allFileRelat = _fileRelationshipservice.GetListAsync(s => s.Id != null && testIdList.Contains(s.LinkId)).Result;
                            testList.ToList().ForEach(m =>
                            {
                                var unit = string.IsNullOrWhiteSpace(m.Unit) ? "" : "(" + m.Unit + ")";
                                EquipmentTestResultDto dto = new EquipmentTestResultDto(m.Id);
                                dto.PlanRelateEquipmentId = m.PlanRelateEquipmentId;
                                dto.TestId = m.TestId;
                                dto.TestName = unit + m.TestName;
                                dto.File = ObjectMapper.Map<File.Entities.File, FileSimpleDto>(m.File);
                                dto.FileId = m.FileId;
                                dto.TestResult = m.TestResult;
                                dto.TestType = m.TestType;
                                dto.CheckResult = m.CheckResult;
                                dto.Unit = m.Unit;
                                dto.Order = yearMonthTests.FirstOrDefault(x => x.Id == m.TestId)?.Order;
                                if (dto.TestType == RepairTestType.EXCEL)
                                {
                                    //var file = allFileRelat.Find(f => f.LinkId == m.Id);
                                    //dto.UploadFile = file != null ? ObjectMapper.Map<FileRelationship, FileRelationshipDto>(file) : null;
                                }
                                //预设值,如果有数据，则转为list，供前端枚举选择
                                if (m.PredictedValue != null && !string.IsNullOrEmpty(m.PredictedValue))
                                {
                                    var stringlist = m.PredictedValue.Split(',');
                                    if (stringlist.Length > 0)
                                    {
                                        dto.PredictedValue = stringlist.ToList();
                                    }
                                }
                                dto.MaxRated = m.MaxRated;
                                dto.MinRated = m.MinRated;
                                testResultList.Add(dto);
                            });
                        }
                    }
                    if (repairUserIdList?.Count > 0)
                    {
                        var repairUsers = _repairUser.Where(z => z.RepairTagId == RepairTagId && repairUserIdList.Contains(z.Id));
                        if (repairUsers?.Count() > 0)
                        {
                            repairUserList = repairUsers.ToList();
                        }
                    }
                }



                Task<SkylightPlanDetailDto> skylightDto = _skylightPlanAppService.GetInWork(new CommonGuidGetDto { Id = ent.SkylightPlanId, RepairTagKey = input.RepairTagKey });
                if (skylightDto != null && skylightDto.Result != null)
                {
                    if (skylightDto.Result.WorkContentType == WorkContentType.OtherPlan)
                    {
                        res.WorkContentType = skylightDto.Result.WorkContentType;
                        res.WorkContent = skylightDto.Result.WorkContent;
                        res.PlanDetailList = await SkylightToConversionAsync(null, repairUserList, null, skylightDto.Result.WorkContent);
                    }
                    else
                    {
                        res.WorkContentType = skylightDto.Result.WorkContentType;
                        res.PlanDetailList = await SkylightToConversionAsync(skylightDto.Result.PlanDetails, repairUserList, testResultList, skylightDto.Result.WorkContent);
                    }
                }

            });

            return res;
        }

        /// <summary>
        /// 添加派工单（状态默认未完成）
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <param name="isOtherPlan">是否其他计划</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.OtherAssignments.Create)]
        //[Authorize(CrPlanPermissions.WorkOrder.Create)]
        public async Task<WorkOrderCreateDto> Create(WorkOrderCreateDto input, bool isOtherPlan)
        {
            if (isOtherPlan)
            {
                if (input.SkylightPlanId == null || input.SkylightPlanId == Guid.Empty)
                    throw new UserFriendlyException("其他计划不能为空");
                // 检修工区
                if (input.MaintenanceUnitId == null || input.MaintenanceUnitId == Guid.Empty)
                    throw new UserFriendlyException("检修工区不能为空");
            }
            else
            {
                if (input.SkylightPlanId == null || input.SkylightPlanId == Guid.Empty)
                    throw new UserFriendlyException("天窗计划不能为空");
                //if (input.StartPlanTime < DateTime.Now || input.EndPlanTime < DateTime.Now)
                //    throw new UserFriendlyException("计划时间不能小于当前时间");
                if (input.EndPlanTime < input.StartPlanTime)
                    throw new UserFriendlyException("计划结束时间不能小于开始时间");
                //通信工区
                if (input.RepairTagKey != "RepairTag.RailwayHighSpeed")
                {
                    if (input.CommunicationUnitId == null || input.CommunicationUnitId == Guid.Empty)
                        throw new UserFriendlyException("通信工区不能为空");
                }
                //检修工区
                if (input.MaintenanceUnitId == null || input.MaintenanceUnitId == Guid.Empty)
                    throw new UserFriendlyException("检修工区不能为空");
                //通信工具检查情况
                //if (string.IsNullOrEmpty(input.ToolSituation) || string.IsNullOrEmpty(Regex.Replace(input.ToolSituation, @"\s", "")))
                //    throw new UserFriendlyException("通信工具检查情况不能为空");
                //作业注意事项
                if (string.IsNullOrEmpty(input.Announcements) || string.IsNullOrEmpty(Regex.Replace(input.Announcements, @"\s", "")))
                    throw new UserFriendlyException("作业注意事项不能为空");
                //派工人员
                if (input.SendWorkersId == null || input.SendWorkersId == Guid.Empty)
                    throw new UserFriendlyException("派工人员不能为空");
            }
            WorkOrderCreateDto order = new WorkOrderCreateDto();
            //添加派工单
            try
            {

                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(async () =>
                {
                    WorkOrder ent = new WorkOrder(Guid.NewGuid());
                    var skylightPlan = _skylight.FirstOrDefault(m => m.RepairTagId == RepairTagId && m.Id == input.SkylightPlanId);
                    if (skylightPlan == null)
                        throw new UserFriendlyException("计划Id不能为空");
                    ent.InfluenceScope = skylightPlan.Incidence;
                    var organization = (await _organization.Where(m => m.Id == input.MaintenanceUnitId)).FirstOrDefault();
                    if (organization == null)
                        throw new UserFriendlyException("检修工区不能为空");
                    var user = (await _personnel.GetUserListAsync(m => m.Id == input.SendWorkersId)).FirstOrDefault();
                    if (user == null)
                    {
                        if (input.RepairTagKey != "RepairTag.RailwayHighSpeed")
                        {
                            throw new UserFriendlyException("派工人员不存在");

                        }
                        else
                        {
                            throw new UserFriendlyException("工区工长不存在");
                        }
                    }

                    if (input.RepairTagKey != "RepairTag.RailwayHighSpeed")
                    {
                        var creator = (await _personnel.GetUserListAsync(m => m.Id == input.CreatorId)).FirstOrDefault();
                        if (creator == null)
                            throw new UserFriendlyException("发起人员不存在");
                        ent.CreatorId = creator.Id;
                    }

                    ent.Name = input.StartPlanTime.ToString("yyyy年MM月dd日") + organization.Name + "派工单";
                    ent.Announcements = input.Announcements;
                    ent.DispatchingTime = input.DispatchingTime;
                    ent.EndPlanTime = input.EndPlanTime;
                    ent.InfluenceScope = input.InfluenceScope;
                    ent.OrderState = Enums.OrderState.Unfinished;
                    ent.SendWorkersId = user.Id;
                    ent.SkylightPlanId = input.SkylightPlanId;
                    ent.StartPlanTime = input.StartPlanTime;
                    ent.ToolSituation = input.ToolSituation;
                    ent.OrderType = OrderType.WorkOrder;
                    ent.RepairTagId = RepairTagId;
                    if (isOtherPlan)
                    {
                        ent.OrderType = OrderType.OtherAssignments;
                    }

                    var startNowDate = Convert.ToDateTime(skylightPlan.WorkTime.ToString("yyyy-MM-dd 00:00:00"));
                    var endNowDate = startNowDate.AddDays(1).AddSeconds(-1);

                    //自动生成命令票号（TX-20210713Y001/20210713E001)
                    if (input.RepairTagKey == "RepairTag.RailwayHighSpeed" && !isOtherPlan)
                    {
                        var nowDate = "TX-" + startNowDate.ToString("yyyyMMdd");

                        var workOrders = _workOrder
                            .Where(x =>
                                x.OrderType == OrderType.WorkOrder
                                && x.OrderNo != null
                                && x.StartPlanTime >= startNowDate
                                && x.StartPlanTime <= endNowDate
                                )
                            .ToList();

                        var workOrderNumber = "";
                        if (skylightPlan.PlanType == PlanType.VerticalSkylight)
                        {
                            workOrderNumber = workOrders
                                            .WhereIf(skylightPlan.Level.Contains("1"), x => x.OrderNo.ToString().Contains("Y"))
                                            .WhereIf(skylightPlan.Level == "2", x => x.OrderNo.ToString().Contains("E"))
                                            .ToList()
                                            .OrderBy(x => x.CreationTime)
                                            ?.LastOrDefault()
                                            ?.OrderNo;
                            //.OrderBy(x => x.CreationTime)
                            //?.LastOrDefault()
                            //?.OrderNo;

                            var repaireLevel = nowDate + (skylightPlan.Level.Contains("1") ? "Y" : "E");

                            if (string.IsNullOrEmpty(workOrderNumber) || workOrderNumber == null)
                            {
                                ent.OrderNo = repaireLevel + "001";
                            }
                            else
                            {
                                //workOrderNumber = workOrders.OrderBy(x => x.CreationTime).LastOrDefault().OrderNo;

                                var number = "";
                                if (workOrderNumber.Contains("Y"))
                                {
                                    number = workOrderNumber.Split('Y')[1].ToString();
                                }
                                else
                                {
                                    number = workOrderNumber.Split('E')[1].ToString();
                                }
                                ent.OrderNo = repaireLevel + (int.Parse(number) + 1).ToString().PadLeft(3, '0');
                            }

                            //保存工作票命令票号
                            var workTickets = skylightPlanRltWorkTickets
                                .Where(x => x.SkylightPlanId == input.SkylightPlanId)
                                .Select(x => x.WorkTicket)
                                .ToList();
                            foreach (var item in workTickets)
                            {
                                item.OrderNumber = ent.OrderNo;
                                await workTicketRepository.UpdateAsync(item);
                            }
                        }
                        else if (skylightPlan.PlanType == PlanType.SkylightOutside)
                        {
                            workOrderNumber = workOrders
                                                .Where(x => x.OrderNo.Contains("DW"))
                                                .ToList()
                                                .OrderBy(x => x.CreationTime)
                                                ?.LastOrDefault()
                                                ?.OrderNo;

                            if (string.IsNullOrEmpty(workOrderNumber) || workOrderNumber == null)
                            {
                                ent.OrderNo = nowDate + "DW001";
                            }
                            else
                            {
                                var number = workOrderNumber.Split('W');
                                ent.OrderNo = nowDate + "DW" + (int.Parse(number[1]) + 1).ToString().PadLeft(3, '0');
                            }
                        }
                    }
                    await _workOrder.InsertAsync(ent);

                    //添加检修工区
                    if (input.MaintenanceUnitId != null)
                    {
                        WorkOrganization maintenance = new WorkOrganization(Guid.NewGuid());
                        maintenance.Duty = Enums.Duty.Recondition;
                        maintenance.WorkOrderId = ent.Id;
                        maintenance.OrganizationId = input.MaintenanceUnitId;
                        maintenance.RepairTagId = RepairTagId;
                        await _workOrganization.InsertAsync(maintenance);
                    }
                    //添加通信工区
                    if (input.CommunicationUnitId != null)
                    {
                        WorkOrganization acceptance = new WorkOrganization(Guid.NewGuid());
                        acceptance.Duty = Enums.Duty.Acceptance;
                        acceptance.WorkOrderId = ent.Id;
                        acceptance.OrganizationId = input.CommunicationUnitId;
                        acceptance.RepairTagId = RepairTagId;
                        await _workOrganization.InsertAsync(acceptance);
                    }
                    //修改天窗状态

                    skylightPlan.PlanState = PlanState.Dispatching;
                    if (isOtherPlan)
                    {
                        skylightPlan.PlanState = PlanState.Issued;
                    }
                    skylightPlan.RepairTagId = RepairTagId;
                    await _skylight.UpdateAsync(skylightPlan);
                    order = ObjectMapper.Map<WorkOrder, WorkOrderCreateDto>(ent);

                });
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return order;
        }

        /// <summary>
        /// 修改派工单（无天窗相关）
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.WorkOrder.Update)]
        public async Task<WorkOrderUpdateDto> Update(WorkOrderUpdateDto input)
        {
            //if (input.StartPlanTime < DateTime.Now || input.EndPlanTime < DateTime.Now)
            //    throw new UserFriendlyException("计划时间不能小于当前时间");
            if (input.EndPlanTime < input.StartPlanTime)
                throw new UserFriendlyException("计划结束时间不能小于开始时间");

            //作业组长
            if (input.WorkLeader == null || input.WorkLeader == null || input.WorkLeader == Guid.Empty)
                throw new UserFriendlyException("作业组长不能为空");
            ////现场防护员
            //if (input.FieldGuardList == null || input.FieldGuardList.Count < 1)
            //    throw new UserFriendlyException("现场防护员不能为空");
            ////驻站联络员
            //if (input.StationLiaisonOfficerList == null || input.StationLiaisonOfficerList.Count < 1)
            //    throw new UserFriendlyException("驻站联络员不能为空");
            ////作业人员
            //if (input.WorkMemberList == null || input.WorkMemberList.Count < 1)
            //    throw new UserFriendlyException("作业人员不能为空");

            WorkOrderUpdateDto res = new WorkOrderUpdateDto();
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(async () =>
                {
                    var oldEnt = _workOrder.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == input.Id);
                    if (oldEnt == null) throw new Exception("对象不存在");
                    oldEnt.EndPlanTime = input.EndPlanTime;
                    oldEnt.StartPlanTime = input.StartPlanTime;
                    oldEnt.RepairTagId = RepairTagId;
                    oldEnt.ToolSituation = input.ToolSituation;
                    //修改派工单
                    await _workOrder.UpdateAsync(oldEnt);

                    //lock (Lock)
                    //{
                    //删除所有相关人员
                    await _worker.DeleteAsync(s => s.WorkOrderId == oldEnt.Id);
                    //}

                    //添加作业组长
                    Worker leader = new Worker(Guid.NewGuid());
                    leader.WorkOrderId = oldEnt.Id;
                    leader.Duty = Enums.UserDuty.WorkLeader;
                    leader.UserId = input.WorkLeader;
                    leader.RepairTagId = RepairTagId;
                    await _worker.InsertAsync(leader);

                    //添加现场防护员
                    foreach (var item in input.FieldGuardList)
                    {
                        Worker people = new Worker(Guid.NewGuid());
                        people.WorkOrderId = oldEnt.Id;
                        people.Duty = Enums.UserDuty.FieldGuard;
                        people.UserId = item;
                        people.RepairTagId = RepairTagId;
                        await _worker.InsertAsync(people);
                    }

                    //添加驻站联络员
                    foreach (var item in input.StationLiaisonOfficerList)
                    {
                        Worker people = new Worker(Guid.NewGuid());
                        people.WorkOrderId = oldEnt.Id;
                        people.Duty = Enums.UserDuty.StationLiaisonOfficer;
                        people.UserId = item;
                        people.RepairTagId = RepairTagId;
                        await _worker.InsertAsync(people);
                    }
                    //添加作业组员
                    foreach (var item in input.WorkMemberList)
                    {
                        Worker people = new Worker(Guid.NewGuid());
                        people.WorkOrderId = oldEnt.Id;
                        people.Duty = Enums.UserDuty.WorkMembers;
                        people.UserId = item;
                        people.RepairTagId = RepairTagId;
                        await _worker.InsertAsync(people);
                    }
                    res = ObjectMapper.Map<WorkOrder, WorkOrderUpdateDto>(oldEnt);
                });
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return res;
        }

        /// <summary>
        /// 完成派工单
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <param name="issave">是否只保存数据</param>
        /// <param name="isOtherPlan">是否其他作业</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.WorkOrder.Finish)]
        //[Authorize(CrPlanPermissions.OtherAssignments.Finish)]
        public async Task<WorkOrderFinishDto> Finish(WorkOrderFinishDto input, bool issave, bool isOtherPlan)
        {
            //if (input.StartRealityTime > DateTime.Now || input.EndRealityTime > DateTime.Now)
            //    throw new UserFriendlyException("实际作业时间不能大于当前时间");
            var dataDic = await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey);
            var RepairTagId = dataDic?.Id;
            bool isHighRailWay = dataDic.Key == "RepairTag.RailwayHighSpeed";

            var oldEnt = _workOrder.FirstOrDefault(s => s.Id == input.Id);
            var oldSkylight = _skylight.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == oldEnt.SkylightPlanId);
            if (oldSkylight == null) throw new Exception("对象不存在");
            WorkOrderFinishDto res = new WorkOrderFinishDto();

            var isComplete = input.CashFeedBack == PlanState.Complete;
            //高铁新需求:添加兑现反馈
            if (isHighRailWay && !isComplete)
            {
                oldSkylight.PlanState = input.CashFeedBack;
                oldSkylight.Opinion = input.Feedback;
                await _skylight.UpdateAsync(oldSkylight);

                //return res;
            }

            if (input.StartRealityTime > input.EndRealityTime)
                throw new UserFriendlyException("实际结束时间不能小于开始时间");
            if (!isOtherPlan)
            {
                //完成情况
                if (string.IsNullOrEmpty(input.Feedback) || string.IsNullOrEmpty(Regex.Replace(input.Feedback, @"\s", "")))
                    throw new UserFriendlyException("完成情况不能为空");
                //命令票号
                if (string.IsNullOrEmpty(input.OrderNo) || string.IsNullOrEmpty(Regex.Replace(input.OrderNo, @"\s", "")))
                    throw new UserFriendlyException("命令票号不能为空");
            }
            var numberFinishs = input.EquipmentList.FindAll(z => z.WorkCount > z.PlanCount);
            if (numberFinishs?.Count > 0 && input.WorkContentType != WorkContentType.OtherPlan)
                throw new UserFriendlyException("有完成数量大于计划数量的数据");
            var userFinishs = input.EquipmentList.FindAll(z => z.WorkCount > 0 && (z.MaintenanceUserList == null || z.MaintenanceUserList.Count < 1));
            if (userFinishs?.Count > 0 && input.WorkContentType != WorkContentType.OtherPlan && isComplete && !issave)
                throw new UserFriendlyException("有填写完成数量，但未填写检修人员的数据");

            try
            {
                await Task.Run(async () =>
                {
                    //获取已传输过来的关联设备、设备测试项及检修人员列表
                    List<EquipmentTestResultUpdateDto> equipTestList = new List<EquipmentTestResultUpdateDto>();
                    List<RepairUser> userList = new List<RepairUser>();
                    List<Guid> idlist = new List<Guid>();
                    var equipmentIdList = new List<Guid>();
                    var equipmentList = new List<PlanRelateEquipment>();
                    List<PlanRelateEquipmentDto> relateEquipmentList = new List<PlanRelateEquipmentDto>();
                    if (input.EquipmentList?.Count > 0)
                    {
                        equipmentIdList = input.EquipmentList.ConvertAll(m => m.Id);
                        equipmentList = _planRelateEquipment.Where(m => m.RepairTagId == RepairTagId && equipmentIdList.Contains(m.Id)).ToList();
                        input.EquipmentList.ForEach(m =>
                        {

                            //if (isOtherPlan)
                            //{
                            var test = m.EquipmentTestResultList.Find(k => (k.FileId == null || k.FileId == Guid.Empty) && string.IsNullOrEmpty(k.TestResult));

                            if (test == null && m.PlanCount == m.WorkCount && m.MaintenanceUserList?.Count > 0)
                            {
                                idlist.Add(m.Id);
                            }

                            //}

                            //测试项
                            equipTestList.AddRange(m.EquipmentTestResultList);
                            //检测人
                            if (m.MaintenanceUserList?.Count > 0)
                            {
                                m.MaintenanceUserList.ForEach(x =>
                                {
                                    if (x != null && x != Guid.Empty)
                                    {
                                        if (userList.Find(z => z.UserId == x && z.PlanRelateEquipmentId == m.Id && z.Duty == Duty.Recondition) == null)
                                        {
                                            RepairUser user = new RepairUser(Guid.NewGuid())
                                            {
                                                WorkerOrderId = input.Id,
                                                PlanRelateEquipmentId = m.Id,
                                                UserId = x,
                                                Duty = Duty.Recondition,
                                            };
                                            userList.Add(user);
                                        }
                                    }

                                });

                            }
                        });

                        if (!issave)
                        {

                            var finishList = input.EquipmentList.FindAll(x => x.WorkCount == x.PlanCount);
                            if (finishList?.Count > idlist.Count && isComplete)
                                throw new UserFriendlyException("有未填写完成情况或未选检修人员，但已确认完成的数据");

                        }
                    }

                    if (oldEnt == null) throw new Exception("对象不存在");
                    oldEnt.StartRealityTime = input.StartRealityTime;
                    oldEnt.EndRealityTime = input.EndRealityTime;
                    oldEnt.Feedback = input.Feedback;
                    oldEnt.OrderNo = input.OrderNo;
                    oldEnt.RepairTagId = RepairTagId;
                    if (!issave || isOtherPlan)
                        oldEnt.OrderState = Enums.OrderState.Complete;

                    if (!issave)
                    {
                        //修改派工单
                        switch (input.CashFeedBack)
                        {
                            case PlanState.Complete:
                                oldEnt.OrderState = OrderState.Complete;
                                break;
                            case PlanState.OrderCancel:
                                oldEnt.OrderState = OrderState.OrderCancel;
                                break;
                            case PlanState.NaturalDisasterCancel:
                                oldEnt.OrderState = OrderState.NaturalDisasterCancel;
                                break;
                            case PlanState.OtherReasonCancel:
                                oldEnt.OrderState = OrderState.OtherReasonCancel;
                                break;
                            default:
                                break;
                        }
                    }

                    //if (input.CashFeedBack == PlanState.NaturalDisasterCancel)
                    //{
                    //    oldEnt.OrderState = OrderState.NaturalDisasterCancel;
                    //}
                    //else if()
                    //{
                    //    oldEnt.OrderState = OrderState.OrderCancel;
                    //}
                    //有线科补录数据：不改变原有状态
                    if (input.isAcceptance)
                    {
                        oldEnt.OrderState = OrderState.Acceptance;
                    }

                    await _workOrder.UpdateAsync(oldEnt);

                    if (input.WorkContentType == WorkContentType.OtherPlan)
                    {
                        //清空原检修人员
                        await _repairUser.DeleteAsync(z => z.WorkerOrderId == oldEnt.Id && z.Duty == Enums.Duty.Recondition);

                        //添加新检修人员
                        if (userList?.Count > 0)
                        {
                            userList.ForEach(async m =>
                            {
                                m.RepairTagId = RepairTagId;
                                await _repairUser.InsertAsync(m);
                            });
                        }

                        //高铁科完成时 直接验收
                        if (isHighRailWay && isComplete)
                        {
                            WorkOrderAcceptanceDto acceptanceDto = new WorkOrderAcceptanceDto();
                            acceptanceDto.Id = input.Id;
                            acceptanceDto.EquipmentList = input.EquipmentList;
                            acceptanceDto.RepairTagKey = input.RepairTagKey;
                            acceptanceDto.WorkContentType = input.WorkContentType;
                            await Acceptance(acceptanceDto, issave);
                        }
                    }
                    else
                    {

                        //修改设备测试项检验结果
                        List<Guid> oldEquiptestList = new List<Guid>();
                        List<Guid> oldUserIdList = new List<Guid>();

                        //获取测试项及测试人员
                        GetEquipTestOrUserId(oldEnt.SkylightPlanId, ref oldEquiptestList, ref oldUserIdList);
                        if (oldEquiptestList?.Count > 0)
                        {
                            var oldTestList = _equipmentTestResult.Where(z => z.RepairTagId == RepairTagId && oldEquiptestList.Contains(z.Id)).ToList();
                            if (oldTestList?.Count > 0)
                            {
                                oldTestList.ForEach(async m =>
                                {
                                    m.TestResult = null;
                                    if (equipTestList?.Count > 0)
                                    {
                                        var test = equipTestList.Find(x => x.Id == m.Id);
                                        {
                                            m.TestResult = test.TestResult;

                                            if (m.TestType == RepairTestType.EXCEL && test != null && test.FileId != null && test.FileId != Guid.Empty)
                                            {
                                                m.FileId = test.FileId;
                                            }
                                        }
                                    }
                                    m.RepairTagId = RepairTagId;
                                    await _equipmentTestResult.UpdateAsync(m);
                                });
                            }
                        }
                        //清空原检修人员

                        if (oldUserIdList?.Count > 0)
                        {
                            //lock (Lock)
                            //{
                            await _repairUser.DeleteAsync(z => oldUserIdList.Contains(z.Id) && z.Duty == Enums.Duty.Recondition);
                            //}

                        }
                        //添加新检修人员
                        if (userList?.Count > 0)
                        {
                            userList.ForEach(async m =>
                            {
                                m.RepairTagId = RepairTagId;
                                await _repairUser.InsertAsync(m);
                            });
                        }

                        //高铁科完成时 直接验收
                        if (isHighRailWay && isComplete)
                        {
                            WorkOrderAcceptanceDto acceptanceDto = new WorkOrderAcceptanceDto();
                            acceptanceDto.Id = input.Id;
                            acceptanceDto.EquipmentList = input.EquipmentList;
                            acceptanceDto.RepairTagKey = input.RepairTagKey;
                            await Acceptance(acceptanceDto, issave);
                        }
                        if (input.EquipmentList.Count > 0)
                        {
                            equipmentList.ForEach(async m =>
                            {
                                var equipment = input.EquipmentList.Find(x => x.Id == m.Id);
                                if (equipment != null)
                                {
                                    m.WorkCount = equipment.WorkCount;
                                }
                                if (isOtherPlan)
                                {
                                    //if (m.EquipmentId != null && m.EquipmentId != Guid.Empty)
                                    //{
                                    var id = idlist.Find(x => x == m.Id);
                                    if (id != null)
                                    {
                                        m.IsComplete = AcceptanceResults.Complete;
                                    }
                                }
                                //}
                                m.RepairTagId = RepairTagId;
                                await _planRelateEquipment.UpdateAsync(m);
                            });
                        }
                    }

                    //如果是其他作业，则进行日任务数据修改
                    if (isOtherPlan)
                    {

                        if (equipmentList?.Count > 0)
                        {
                            var planDetailIdList = equipmentList.ConvertAll(m => m.PlanDetailId);
                            var planDetailList = _planDetail.Where(m => planDetailIdList.Contains(m.Id)).ToList();
                            planDetailList.ForEach(async m =>
                            {
                                var list = equipmentList.FindAll(x => x.PlanDetailId == m.Id);
                                if (list?.Count > 0)
                                {
                                    decimal number = 0;
                                    list.ForEach(x =>
                                    {
                                        number += x.WorkCount;
                                    });
                                    m.WorkCount = number;
                                }
                                m.RepairTagId = RepairTagId;
                                await _planDetail.UpdateAsync(m);
                            });
                        }
                        oldSkylight.PlanState = input.CashFeedBack;
                        oldSkylight.RepairTagId = RepairTagId;
                        await _skylight.UpdateAsync(oldSkylight);
                    }
                });
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return res;

        }

        /// <summary>
        /// 验收派工单
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <param name="issave">是否只保存数据</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.WorkOrder.Acceptance)]
        public async Task<WorkOrderAcceptanceDto> Acceptance(WorkOrderAcceptanceDto input, bool issave)
        {
            WorkOrderAcceptanceDto res = new WorkOrderAcceptanceDto();
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var oldEnt = _workOrder.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == input.Id);
                if (oldEnt == null) throw new Exception("对象不存在");
                //获取已传输过来的关联设备、设备测试项及检修人员列表
                List<EquipmentTestResultUpdateDto> equipTestList = new List<EquipmentTestResultUpdateDto>();
                List<RepairUser> userList = new List<RepairUser>();
                List<Guid> idlist = new List<Guid>();

                if (input.EquipmentList?.Count > 0)
                {
                    input.EquipmentList.ForEach(m =>
                    {

                        var test = m.EquipmentTestResultList.Find(k => (k.FileId == null || k.FileId == Guid.Empty) && (string.IsNullOrEmpty(k.TestResult) || string.IsNullOrEmpty(k.CheckResult)));

                        if (test == null && m.PlanCount == m.WorkCount && m.AcceptanceUserList?.Count > 0 && m.MaintenanceUserList?.Count > 0)
                        {
                            idlist.Add(m.Id);
                        }

                        equipTestList.AddRange(m.EquipmentTestResultList);
                        if (m.AcceptanceUserList?.Count > 0)
                        {
                            m.AcceptanceUserList.ForEach(x =>
                            {
                                if (x != null && x != Guid.Empty)
                                {
                                    if (userList.Find(z => z.UserId == x && z.PlanRelateEquipmentId == m.Id && z.Duty == Duty.Acceptance) == null)
                                    {
                                        RepairUser user = new RepairUser(Guid.NewGuid());
                                        user.WorkerOrderId = input.Id;
                                        user.PlanRelateEquipmentId = m.Id;
                                        user.UserId = x;
                                        user.Duty = Duty.Acceptance;
                                        userList.Add(user);
                                    }
                                }

                            });

                        }

                    });
                }

                if (!issave)
                {
                    var userFinishs = input.EquipmentList.FindAll(z => z.WorkCount > 0 && (z.AcceptanceUserList == null || z.AcceptanceUserList.Count < 1 || z.AcceptanceUserList == null || z.AcceptanceUserList.Count < 1));
                    if (userFinishs?.Count > 0 && input.WorkContentType != WorkContentType.OtherPlan)
                        throw new UserFriendlyException("有已填写完成数量，但未填写验收人员的数据");
                    oldEnt.OrderState = Enums.OrderState.Acceptance;
                    var oldSkylight = _skylight.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == oldEnt.SkylightPlanId);
                    if (oldSkylight == null) throw new Exception("对象不存在");
                    oldSkylight.PlanState = PlanState.Complete;
                    await _skylight.UpdateAsync(oldSkylight);
                }
                //修改派工单
                oldEnt.RepairTagId = RepairTagId;
                await _workOrder.UpdateAsync(oldEnt);

                if (input.WorkContentType == WorkContentType.OtherPlan)
                {
                    //清空原验收人员
                    await _repairUser.DeleteAsync(z => z.WorkerOrderId == oldEnt.Id && z.Duty == Enums.Duty.Acceptance);

                    //添加新检修人员
                    if (userList?.Count > 0)
                    {
                        userList.ForEach(async m =>
                        {
                            m.RepairTagId = RepairTagId;
                            await _repairUser.InsertAsync(m);
                        });
                    }
                }
                else
                {
                    //修改设备测试项检验结果
                    List<Guid> oldEquiptestList = new List<Guid>();
                    List<Guid> oldUserIdList = new List<Guid>();

                    //获取测试项及测试人员
                    List<Guid> equipmentIdList = GetEquipTestOrUserId(oldEnt.SkylightPlanId, ref oldEquiptestList, ref oldUserIdList);
                    if (oldEquiptestList?.Count > 0)
                    {
                        var oldTestList = _equipmentTestResult.Where(z => z.RepairTagId == RepairTagId && oldEquiptestList.Contains(z.Id));
                        if (oldTestList?.Count() > 0)
                        {
                            oldTestList.ToList().ForEach(async m =>
                            {
                                m.CheckResult = null;
                                if (equipTestList?.Count > 0)
                                {
                                    var test = equipTestList.Find(x => x.Id == m.Id);
                                    {
                                        m.CheckResult = test.CheckResult;
                                    }
                                }
                                await _equipmentTestResult.UpdateAsync(m);
                            });
                        }
                    }
                    //清空原验收人员
                    if (oldUserIdList?.Count > 0)
                    {
                        //lock (Lock)
                        //{
                        await _repairUser.DeleteAsync(z => oldUserIdList.Contains(z.Id) && z.Duty == Enums.Duty.Acceptance);
                        //}

                    }
                    //添加新检修人员
                    if (userList?.Count > 0)
                    {
                        userList.ForEach(async m =>
                        {
                            m.RepairTagId = RepairTagId;
                            await _repairUser.InsertAsync(m);
                        });
                    }

                    //修改原设备完成状态 及日任务作业数量
                    if (input.EquipmentList?.Count > 0)
                    {
                        var equipIdList = input.EquipmentList.ConvertAll(m => m.Id);

                        var equipmentList = _planRelateEquipment.Where(m => m.RepairTagId == RepairTagId && equipIdList.Contains(m.Id)).ToList();

                        equipmentList.ForEach(async m =>
                        {
                            var equipment = input.EquipmentList.Find(x => x.Id == m.Id);
                            if (equipment != null)
                            {
                                m.WorkCount = equipment.WorkCount;
                            }
                            //if (m.EquipmentId != null && m.EquipmentId != Guid.Empty)
                            //{
                            var id = idlist.Find(x => x == m.Id);
                            if (id != null)
                            {
                                m.IsComplete = AcceptanceResults.Complete;
                            }
                            //}
                            m.RepairTagId = RepairTagId;
                            await _planRelateEquipment.UpdateAsync(m);
                        });
                        var planDetailIdList = equipmentList.ConvertAll(m => m.PlanDetailId);
                        var planDetailList = _planDetail.Where(m => m.RepairTagId == RepairTagId && planDetailIdList.Contains(m.Id)).ToList();
                        planDetailList.ForEach(async m =>
                        {
                            var list = equipmentList.FindAll(x => x.PlanDetailId == m.Id);
                            if (list?.Count > 0)
                            {
                                decimal number = 0;
                                list.ForEach(x =>
                                {
                                    number += x.WorkCount;
                                });
                                m.WorkCount = number;
                            }
                            m.RepairTagId = RepairTagId;
                            await _planDetail.UpdateAsync(m);
                        });
                    }
                }



                res = ObjectMapper.Map<WorkOrder, WorkOrderAcceptanceDto>(oldEnt);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return res;
        }

        /// <summary>
        /// 编辑派工单（已完成模块使用）
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.WorkOrderFinished.Update)]
        public async Task<WorkOrderFinishDto> UpdateDetail(WorkOrderFinishDto input)
        {
            //if (input.StartRealityTime > DateTime.Now || input.EndRealityTime > DateTime.Now)
            //    throw new UserFriendlyException("实际作业时间不能大于当前时间");
            if (input.StartRealityTime > input.EndRealityTime)
                throw new UserFriendlyException("实际结束时间不能小于开始时间");
            //完成情况
            //if (string.IsNullOrEmpty(input.Feedback))
            //    throw new UserFriendlyException("完成情况不能为空");
            ////命令票号
            //if (string.IsNullOrEmpty(input.OrderNo))
            //    throw new UserFriendlyException("命令票号不能为空");

            var numberFinishs = input.EquipmentList?.FindAll(z => z.WorkCount > z.PlanCount);
            if (numberFinishs?.Count > 0 && input.WorkContentType != WorkContentType.OtherPlan)
                throw new UserFriendlyException("有完成数量大于计划数量的数据");
            var userFinishs = input.EquipmentList?.FindAll(z => z.WorkCount > 0 && (z.MaintenanceUserList == null || z.MaintenanceUserList.Count < 1 || z.AcceptanceUserList == null || z.AcceptanceUserList.Count < 1));
            if (userFinishs?.Count > 0 && input.WorkContentType != WorkContentType.OtherPlan)
                throw new UserFriendlyException("有填写完成数量，但未填写检修或验收人员的数据");
            WorkOrderFinishDto res = new WorkOrderFinishDto();
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

                var oldEnt = _workOrder.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == input.Id);
                if (oldEnt == null) throw new Exception("对象不存在");

                //获取已传输过来的关联设备、设备测试项及检修人员列表
                List<EquipmentTestResultUpdateDto> equipTestList = new List<EquipmentTestResultUpdateDto>();
                List<RepairUser> userList = new List<RepairUser>();
                List<Guid> idlist = new List<Guid>();
                if (input.EquipmentList?.Count > 0)
                {
                    input.EquipmentList.ForEach(m =>
                    {
                        var test = m.EquipmentTestResultList.Find(k => (k.FileId == null || k.FileId == Guid.Empty) && (string.IsNullOrEmpty(k.CheckResult) || string.IsNullOrEmpty(k.TestResult)));

                        if (test == null && m.PlanCount == m.WorkCount && m.AcceptanceUserList?.Count > 0 && m.MaintenanceUserList?.Count > 0)
                        {
                            idlist.Add(m.Id);
                        }
                        equipTestList.AddRange(m.EquipmentTestResultList);
                        if (m.MaintenanceUserList?.Count > 0)
                        {
                            m.MaintenanceUserList.ForEach(x =>
                            {
                                RepairUser user = new RepairUser(Guid.NewGuid());
                                user.WorkerOrderId = input.Id;
                                user.PlanRelateEquipmentId = m.Id;
                                user.UserId = x;
                                user.Duty = Duty.Recondition;
                                userList.Add(user);

                            });

                        }
                        if (m.AcceptanceUserList?.Count > 0)
                        {
                            m.AcceptanceUserList.ForEach(x =>
                            {
                                RepairUser user = new RepairUser(Guid.NewGuid());
                                user.WorkerOrderId = input.Id;
                                user.PlanRelateEquipmentId = m.Id;
                                user.UserId = x;
                                user.Duty = Duty.Acceptance;
                                userList.Add(user);

                            });

                        }
                    });
                    var tempUserFinishs = input.EquipmentList.FindAll(z => z.WorkCount > 0 && (z.MaintenanceUserList == null || z.MaintenanceUserList.Count < 1 || z.AcceptanceUserList == null || z.AcceptanceUserList.Count < 1));
                    if (tempUserFinishs?.Count > 0)
                        throw new UserFriendlyException("有填写完成数量，但未填写检修或验收人员的数据");
                    var finishList = input.EquipmentList.FindAll(x => x.WorkCount == x.PlanCount);
                    if (finishList?.Count > idlist.Count)
                        throw new UserFriendlyException("有未填写完成或验收情况或未填写检修人或验收人，但已确认完成的数据");

                }

                oldEnt.StartRealityTime = input.StartRealityTime;
                oldEnt.EndRealityTime = input.EndRealityTime;
                oldEnt.Feedback = input.Feedback;
                oldEnt.OrderNo = input.OrderNo;
                oldEnt.RepairTagId = RepairTagId;
                //修改派工单
                await _workOrder.UpdateAsync(oldEnt);

                List<ResInfo> ResInfos = new List<ResInfo>();
                if (input.WorkContentType == WorkContentType.OtherPlan)
                {
                    //清空原检验、验收人员
                    if (input.EquipmentList?.Count > 0)
                    {
                        var oldUserIdList = new List<Guid>();
                        if (input.EquipmentList[0].MaintenanceUserList?.Count > 0)
                            oldUserIdList.AddRange(input.EquipmentList[0].MaintenanceUserList);
                        if (input.EquipmentList[0].AcceptanceUserList?.Count > 0)
                            oldUserIdList.AddRange(input.EquipmentList[0].AcceptanceUserList);
                        await _repairUser.DeleteAsync(z => oldUserIdList.Contains(z.Id));
                    }
                    //添加新检修、验收人员
                    if (userList?.Count > 0)
                    {
                        userList.ForEach(async m =>
                        {
                            m.RepairTagId = RepairTagId;
                            await _repairUser.InsertAsync(m);

                        });
                    }
                }
                else
                {
                    //修改设备测试项检验、验收结果
                    List<Guid> oldEquiptestList = new List<Guid>();
                    List<Guid> oldUserIdList = new List<Guid>();

                    //获取测试项及测试人员
                    List<Guid> equipmentIdList = GetEquipTestOrUserId(oldEnt.SkylightPlanId, ref oldEquiptestList, ref oldUserIdList);
                    if (oldEquiptestList?.Count > 0)
                    {
                        var oldTestList = _equipmentTestResult.Where(z => z.RepairTagId == RepairTagId && oldEquiptestList.Contains(z.Id));
                        if (oldTestList?.Count() > 0)
                        {
                            oldTestList.ToList().ForEach(async m =>
                            {
                                m.CheckResult = null;
                                if (equipTestList?.Count > 0)
                                {
                                    var test = equipTestList.Find(x => x.Id == m.Id);
                                    {
                                        m.CheckResult = test.CheckResult;
                                        m.TestResult = test.TestResult;
                                        if (m.TestType == RepairTestType.EXCEL && test.FileId != null && test.FileId != null && test.FileId != Guid.Empty)
                                        {
                                            ////删除原先文件
                                            //_fileRelationshipservice.Delete(s => s.LinkId != null && s.LinkId == m.Id);
                                            //FileRelationship fileRe = new FileRelationship(Guid.NewGuid());
                                            //fileRe.FileId = test.UploadFile.FileId;
                                            //fileRe.FileName = test.UploadFile.FileName;
                                            //fileRe.FileType = FileType.EquipmentTestResult;
                                            //fileRe.LinkId = m.Id;
                                            //fileRe.LinkTableName = "EquipmentTestResult";
                                            //fileRe.Remark = test.UploadFile.Remark;
                                            //_fileRelationshipservice.Create(fileRe);
                                        }
                                    }
                                }
                                m.RepairTagId = RepairTagId;
                                await _equipmentTestResult.UpdateAsync(m);
                            });
                        }
                    }
                    //清空原检验、验收人员
                    if (oldUserIdList?.Count > 0)
                    {
                        //lock (Lock)
                        //{
                        await _repairUser.DeleteAsync(z => oldUserIdList.Contains(z.Id));
                        //}

                    }
                    //添加新检修、验收人员
                    if (userList?.Count > 0)
                    {
                        userList.ForEach(async m =>
                        {
                            m.RepairTagId = RepairTagId;
                            await _repairUser.InsertAsync(m);

                        });
                    }

                    #region 修改关联设备及日任务数据
                    if (input.EquipmentList.Count > 0)
                    {
                        var allPlanDetail = _planDetail.Where(s => s.RepairTagId == RepairTagId && s.Id != null);

                        var equipIdList = input.EquipmentList.ConvertAll(m => m.Id);

                        var equipmentList = _planRelateEquipment.Where(m => m.RepairTagId == RepairTagId && equipIdList.Contains(m.Id)).ToList();
                        var planDetailIdList = equipmentList.ConvertAll(m => m.PlanDetailId);
                        var planDetailList = allPlanDetail.Where(m => planDetailIdList.Contains(m.Id)).ToList();
                        var dailPlanIds = planDetailList.Select(s => s.DailyPlanId);
                        var dailPlans = _dailyPlans.Where(s => s.RepairTagId == RepairTagId && dailPlanIds.Contains(s.Id)).ToList();
                        var yearMonthPlanIds = dailPlans.Select(s => s.PlanId);
                        var yearMonthPlans = _yearMonthPlans.Where(s => s.RepairTagId == RepairTagId && yearMonthPlanIds.Contains(s.Id));
                        var tempJoin = (from a in planDetailList
                                        join b in dailPlans on a.DailyPlanId equals b.Id
                                        select new
                                        {
                                            PlanDetail = a,
                                            DailyPlan = b
                                        }).ToList();
                        foreach (var m in equipmentList)
                        {
                            var tempDailP = tempJoin.FirstOrDefault(s => s.PlanDetail.Id == m.PlanDetailId)?.DailyPlan;
                            if (tempDailP == null) continue;
                            var MaxFinishCount = tempDailP.Count;
                            var sameDailPlans = allPlanDetail.Where(s => !planDetailIdList.Contains(s.Id) && s.DailyPlanId == tempDailP.Id);
                            var canFinishCount = MaxFinishCount - sameDailPlans.Sum(s => s.WorkCount);

                            var equipment = input.EquipmentList.Find(x => x.Id == m.Id);
                            if (equipment != null)
                            {
                                m.WorkCount = equipment.WorkCount > canFinishCount ? canFinishCount : equipment.WorkCount;
                                if (equipment.WorkCount > canFinishCount)
                                {
                                    var yearMonthPlan = yearMonthPlans.FirstOrDefault(s => s.Id == tempDailP.PlanId);
                                    if (yearMonthPlan != null)
                                    {
                                        var numP = yearMonthPlan.Number.Split('-').ToList();
                                        string newNum = "";
                                        numP.ForEach(i =>
                                        {
                                            newNum += int.Parse(i) + "-";
                                        });
                                        ResInfo inf = new ResInfo(yearMonthPlan.Number);
                                        inf.Content = newNum.TrimEnd('-') + "：该工作内容目前可完成数量的最大值为" + canFinishCount.ToString().TrimEnd('0') + "，已自动修改";
                                        ResInfos.Add(inf);
                                    }
                                }
                            }
                            //if (m.EquipmentId != null && m.EquipmentId != Guid.Empty)
                            //{
                            var id = idlist.Find(x => x == m.Id);
                            if (id != null)
                            {
                                m.IsComplete = AcceptanceResults.Complete;
                            }
                            //}
                            m.RepairTagId = RepairTagId;
                            await _planRelateEquipment.UpdateAsync(m);
                        }
                        //var planDetailIdList = equipmentList.ConvertAll(m => m.PlanDetailId);
                        //var planDetailList = _planDetail.Where(m => planDetailIdList.Contains(m.Id)).ToList();
                        planDetailList.ForEach(async m =>
                        {
                            var list = equipmentList.FindAll(x => x.PlanDetailId == m.Id);
                            if (list?.Count > 0)
                            {
                                decimal number = 0;
                                list.ForEach(x =>
                                {
                                    number += x.WorkCount;
                                });
                                m.WorkCount = number;
                            }
                            m.RepairTagId = RepairTagId;
                            await _planDetail.UpdateAsync(m);
                        });
                    }
                    #endregion
                }

                res = ObjectMapper.Map<WorkOrder, WorkOrderFinishDto>(oldEnt);
                res.FinishInfos = ResInfos.OrderBy(s => s.OrderNumber).ToList();
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return res;
        }

        /// <summary>
        /// 撤销派工单
        /// 删除派工单数据
        /// 对应天窗计划状态改为已发布
        /// 设备测试项清理检修结果、验收结果数据
        /// 相关设备检修人员数据删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        // [Authorize(CrPlanPermissions.OtherAssignments.Delete)]
        // [Authorize(CrPlanPermissions.WorkOrder.Delete)]
        public async Task<bool> Delete(WorkOrderDeleteDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("id不正确");
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(() =>
                {
                    var oldEnt = _workOrder.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == input.Id);
                    if (oldEnt == null) throw new Exception("对象不存在");
                    if (oldEnt.OrderState != OrderState.Unfinished)
                        throw new Exception("该作业已完成无法撤销");
                    var oldSkylightPlan = _skylight.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == oldEnt.SkylightPlanId);
                    if (oldSkylightPlan == null) throw new Exception("计划对象不存在");
                    //清空设备测试项检查、验收结果
                    List<Guid> oldEquiptestIdList = new List<Guid>();
                    List<Guid> oldUserIdList = new List<Guid>();

                    //获取测试项及测试人员
                    GetEquipTestOrUserId(oldEnt.SkylightPlanId, ref oldEquiptestIdList, ref oldUserIdList);
                    if (oldEquiptestIdList?.Count > 0)
                    {
                        var oldEquiptestList = _equipmentTestResult.Where(z => z.RepairTagId == RepairTagId && oldEquiptestIdList.Contains(z.Id));
                        if (oldEquiptestList?.Count() > 0)
                        {
                            oldEquiptestList.ToList().ForEach(m =>
                            {
                                if (m.TestType == RepairTestType.EXCEL)
                                {
                                    //删除原先文件
                                    //_fileRelationshipservice.Delete(s => s.LinkId == m.Id);
                                }
                                m.TestResult = null;
                                m.CheckResult = null;
                                m.RepairTagId = RepairTagId;
                                _equipmentTestResult.UpdateAsync(m);

                            });
                        }
                    }
                    //清空原检验、验收人员
                    if (oldUserIdList?.Count > 0)
                    {
                        _repairUser.DeleteAsync(z => oldUserIdList.Contains(z.Id));
                    }

                    #region 在已验收之后进行撤销使用
                    //日任务作业数量改为0
                    //var planDetailList = _planDetail.Where(z => z.SkylightPlanId == oldSkylightPlan.Id);
                    //if (planDetailList?.Count() > 0)
                    //{
                    //    var planDetailIdList = planDetailList.ToList().ConvertAll(m => m.Id);
                    //    //关联设备的设备状态改为未完成
                    //    var equipmentList = _planRelateEquipment.Where(z => planDetailIdList.Contains(z.PlanDetailId));
                    //    if (equipmentList?.Count() > 0)
                    //    {
                    //        equipmentList.ToList().ForEach(m =>
                    //        {
                    //            m.IsComplete = AcceptanceResults.Unfinished;
                    //            _planRelateEquipment.UpdateAsync(m);
                    //        });
                    //    }
                    //    planDetailList.ToList().ForEach(m =>
                    //    {
                    //        m.WorkCount = 0;
                    //        _planDetail.UpdateAsync(m);
                    //    });
                    //} 
                    #endregion
                    if (input.IsOtherPlan)
                    {
                        //对应其他计划状态改为已撤销
                        oldSkylightPlan.PlanState = Enums.PlanState.NotIssued;
                    }
                    else if (input.RepairTagKey == "RepairTag.RailwayHighSpeed" && !input.IsOtherPlan && oldSkylightPlan.PlanType == PlanType.VerticalSkylight)
                    {
                        oldSkylightPlan.PlanState = Enums.PlanState.Adopted;
                    }
                    else
                    {
                        //对应天窗计划状态改为已发布
                        oldSkylightPlan.PlanState = Enums.PlanState.UnDispatching;
                    }
                    oldSkylightPlan.RepairTagId = RepairTagId;
                    _skylight.UpdateAsync(oldSkylightPlan);
                    //删除派工单
                    _workOrder.DeleteAsync(oldEnt);
                    return true;
                });
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return false;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id">派工单Id</param>
        /// <param name="ismaintenance">True:导出检修表  False:导出派工单</param>
        /// <param name="repairTag">维修项标签key</param>
        /// <returns></returns>
        // [Authorize(CrPlanPermissions.WorkOrder.Export)]
        [Produces("application/octet-stream")]
        public async Task<Stream> Export(Guid id, bool ismaintenance, string repairTagKey)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("id不正确");
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == repairTagKey))?.Id;
            var workOrder = _workOrder.Where(x => x.RepairTagId == RepairTagId && x.Id == id).FirstOrDefault();
            if (workOrder == null) throw new UserFriendlyException("未找到派工单");
            Stream st = null;
            string fileName = "";
            //导出检修、验收表
            if (ismaintenance)
            {
                fileName = "检修、验收表.xlsx";
                byte[] bt = null;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    bt = await ExporToRepairAndCheck(workOrder.SkylightPlanId, fileName, repairTagKey, workOrder.EndRealityTime);
                }
                else if (repairTagKey == "RepairTag.RailwayHighSpeed")
                {
                    bt = await ExporToRepairWithCheck(workOrder.SkylightPlanId, fileName, repairTagKey);
                }
                //File.WriteAllBytes(@"d:/" + fileName, bt);
                st = new MemoryStream(bt);
            }
            else  //导出派工单
            {
                var detail = await GetDetail(new CommonGuidGetDto { Id = id, RepairTagKey = repairTagKey });
                var exportDto = await ConvertWorkOrderToExportDto(detail);
                if (exportDto == null) return null;
                fileName = exportDto.WorkArea + "派工单.xlsx";
                var bt = ExcelHepler.WorkOrderToExcel(exportDto, fileName);
                //File.WriteAllBytes(@"d:/" + fileName, bt);
                st = new MemoryStream(bt);
            }
            return st;
        }

        /// <summary>
        /// 派工单详情转导出Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<WorkOrderExportDto> ConvertWorkOrderToExportDto(WorkOrderDetailDto input)
        {
            if (input == null) return null;
            try
            {
                WorkOrderExportDto dto = new WorkOrderExportDto();
                var skylight = _skylight.WithDetails().Where(x => x.Id == input.SkylightPlanId).FirstOrDefault();
                if (skylight != null)
                    dto.WorkShop = (await _organization.Where(x => x.Id == skylight.WorkUnit)).FirstOrDefault()?.Name;
                dto.WorkArea = input.MaintenanceUnit?.OrganizationName;
                dto.OrderNo = input.OrderNo;
                dto.WorkLeader = input.WorkLeader?.UserName;

                //工区工长
                if (input.SendWorkersId != null && input.SendWorkersId != Guid.Empty)
                {
                    var user = (await _personnel.GetUserListAsync(m => m.Id == input.SendWorkersId)).FirstOrDefault();
                    if (user != null)
                    {
                        dto.WorkAreaLeader = user.Name;
                    }
                }

                //作业时间
                dto.WorkTime = input.StartRealityTime.ToString("MM月dd日HH时mm分") + "至" + input.EndRealityTime.ToString("MM月dd日HH时mm分");

                //天窗时间
                dto.SkylightTime = input.StartPlanTime.ToString("MM月dd日HH时mm分") + "至" + input.EndPlanTime.ToString("MM月dd日HH时mm分");

                //完成情况
                dto.CompletionStatus = input.Feedback;

                if (input.WorkMemberList?.Count > 0)
                {
                    input.WorkMemberList.ForEach(x => dto.WorkMemberList += x.UserName + "、");
                    dto.WorkMemberList = dto.WorkMemberList.Substring(0, dto.WorkMemberList.Length - 1);
                }
                if (input.StationLiaisonOfficerList?.Count > 0)
                {
                    input.StationLiaisonOfficerList.ForEach(x => dto.StationLiaisonOfficerList += x.UserName + "、");
                    dto.StationLiaisonOfficerList = dto.StationLiaisonOfficerList.Substring(0, dto.StationLiaisonOfficerList.Length - 1);
                }
                if (input.FieldGuardList?.Count > 0)
                {
                    input.FieldGuardList.ForEach(x => dto.FieldGuardList += x.UserName + "、");
                    dto.FieldGuardList = dto.FieldGuardList.Substring(0, dto.FieldGuardList.Length - 1);
                }
                dto.ToolSituation = input.ToolSituation;
                //作业地点内容影响,更改为 **机房年月表项目
                var installationNames = "";
                var str = "/";
                if (skylight.WorkSites != null && skylight.WorkSites.Count > 0)
                {
                    var siteIds = skylight.WorkSites.Select(x => x.InstallationSiteId).ToList();
                    var sites = _installationSite.Where(x => siteIds.Contains(x.Id)).ToList();
                    if (sites != null && sites.Count > 0)
                    {
                        foreach (var item in sites)
                        {
                            installationNames += item.Name + str;
                        }
                        installationNames = installationNames.Substring(0, installationNames.Length - str.Length);
                    }
                }

                dto.WorkSiteAndContent = skylight.WorkContentType == WorkContentType.MonthYearPlan ? installationNames + "年月表项目" : skylight.WorkContent;
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 导出检修、验收表有线
        /// </summary>
        /// <param name="id">天窗计划ID</param>
        /// <param name="fileName"></param>
        /// <param name="repairTagKey"></param>
        /// <param name="realEndTime"></param>
        /// <returns></returns>
        private async Task<byte[]> ExporToRepairAndCheck(Guid id, string fileName, string repairTagKey, DateTime realEndTime)
        {
            List<RepairCheckExportDto> repairList = new List<RepairCheckExportDto>();
            List<RepairCheckExportDto> checkList = new List<RepairCheckExportDto>();
            var sky = await _skylightPlanAppService.GetInWork(new CommonGuidGetDto { Id = id, RepairTagKey = repairTagKey });
            if (sky != null && sky.PlanDetails?.Count > 0)
            {
                repairList = await GetRepairExportDto(sky.PlanDetails, true);
                checkList = await GetRepairExportDto(sky.PlanDetails, false);
            }
            if (repairList == null || repairList.Count == 0) throw new UserFriendlyException("未找到检修表导出数据");
            if (checkList == null || checkList.Count == 0) throw new UserFriendlyException("未找到验收表导出数据");


            //添加作业处所及作业时间
            //1、计划作业车站区间是否为相邻
            string workPlace = "";
            //作业时间：作业实际完成时间
            string workTime = realEndTime == new DateTime(0) ? "暂未填写" : realEndTime.ToString("yyyy-MM-dd HH:mm:ss");

            if (sky.IsAdjacent)
            {
                var startStationName = stations.FirstOrDefault(x => x.Id == sky.StationId)?.Name;
                var endStationName = stations.FirstOrDefault(x => x.Id == sky.EndStationId)?.Name;
                workPlace = startStationName + "-" + endStationName;
            }
            else
            {

                workPlace = sky.WorkSiteIds.Count == 0 ? sky.WorkArea : sky.WorkSiteName;
            }

            var nullInfo = "合格";

            var dt = (DataTable)null;
            var row = (DataRow)null;
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("维护对象"));
            dt.Columns.Add(new DataColumn("类别"));
            dt.Columns.Add(new DataColumn("设备型号/编号"));
            dt.Columns.Add(new DataColumn("序号"));
            dt.Columns.Add(new DataColumn("维护项目"));
            dt.Columns.Add(new DataColumn("检修记录"));
            dt.Columns.Add(new DataColumn("检修人"));
            dt.Columns.Add(new DataColumn("备注"));

            //给数据按某一规则分组和排序
            var newLists = new List<RepairCheckExportDto>();
            var oldLists = new List<RepairCheckExportDto>();

            var planTypeList = repairList.GroupBy(x => x.PlanType);
            foreach (var planType in planTypeList)
            {
                var deviceList = planType.GroupBy(x => x.DeviceName);
                foreach (var item in deviceList)
                {
                    oldLists = item.OrderBy(x => x.WorkContent).ToList();
                    foreach (var ite in oldLists)
                    {
                        newLists.Add(ite);
                    }
                }
            }

            repairList = newLists;
            //添加数据
            newLists = newLists.OrderBy(x => x.DeviceName).ToList();
            foreach (var item in newLists)
            {
                row = dt.NewRow();
                row["维护对象"] = item.PlanType == null ? string.Empty : item.PlanType;
                row["类别"] = item.DeviceName == null ? string.Empty : item.DeviceName;
                row["设备型号/编号"] = item.EquipmentName == null ? string.Empty : item.EquipmentName;
                row["序号"] = "1";
                row["维护项目"] = item.WorkContent == null ? string.Empty : item.WorkContent;
                row["检修记录"] = item.RepairRecord == null ? nullInfo : item.RepairRecord;
                row["检修人"] = item.RepairUser == null ? string.Empty : item.RepairUser;
                row["备注"] = string.Empty;
                dt.Rows.Add(row);
            }
            //验收表
            var dt2 = (DataTable)null;
            var row2 = (DataRow)null;
            dt2 = new DataTable();

            dt2.Columns.Add(new DataColumn("维护对象"));
            dt2.Columns.Add(new DataColumn("类别"));
            dt2.Columns.Add(new DataColumn("序号"));
            dt2.Columns.Add(new DataColumn("维护项目"));
            dt2.Columns.Add(new DataColumn("验收情况"));
            dt2.Columns.Add(new DataColumn("验收人"));
            dt2.Columns.Add(new DataColumn("备注"));

            //给数据按某一规则排序
            newLists = new List<RepairCheckExportDto>();
            oldLists = new List<RepairCheckExportDto>();

            planTypeList = checkList.GroupBy(x => x.PlanType);
            foreach (var planType in planTypeList)
            {
                var deviceList = planType.GroupBy(x => x.DeviceName);
                foreach (var item in deviceList)
                {
                    oldLists = item.OrderBy(x => x.WorkContent).ToList();
                    foreach (var ite in oldLists)
                    {
                        newLists.Add(ite);
                    }
                }
            }
            checkList = newLists;
            //添加数据
            newLists = newLists.OrderBy(x => x.DeviceName).ToList();
            foreach (var item in newLists)
            {
                row2 = dt2.NewRow();
                row2["维护对象"] = item.PlanType ?? string.Empty;
                row2["类别"] = item.DeviceName ?? string.Empty;
                row2["序号"] = "1";
                row2["维护项目"] = item.WorkContent ?? string.Empty;
                row2["验收情况"] = item.RepairRecord != null ? item.RepairRecord : nullInfo;
                row2["验收人"] = item.RepairUser ?? string.Empty;
                row2["备注"] = string.Empty;
                dt2.Rows.Add(row2);
            }
            return ExcelHepler.ReparAndCheckToExcel(dt, dt2, fileName, workPlace, workTime);
        }

        /// <summary>
        /// 导出检修表，检修人与验收人不分两表高铁
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <param name="repairTagKey"></param>
        /// <returns></returns>
        private async Task<byte[]> ExporToRepairWithCheck(Guid id, string fileName, string repairTagKey)
        {
            List<RepairCheckExportDto> repairList = new List<RepairCheckExportDto>();
            List<RepairCheckExportDto> checkList = new List<RepairCheckExportDto>();
            var sky = await _skylightPlanAppService.GetInWork(new CommonGuidGetDto { Id = id, RepairTagKey = repairTagKey });

            if (sky.WorkContentType == WorkContentType.OtherPlan)
            {
                throw new UserFriendlyException("该计划内容为非年月表项目,暂无检修记录表");
            }
            var workTime = "";
            if (sky != null && sky.PlanDetails?.Count > 0)
            {
                workTime = sky.WorkTime.ToString("yyyy-MM-dd HH:mm:ss");
                repairList = await GetRepairExportDto(sky.PlanDetails, true);
                checkList = await GetRepairExportDto(sky.PlanDetails, false);
            }
            if (repairList == null || repairList.Count == 0)
            {
                throw new UserFriendlyException("未找到检修表导出数据");
            }
            if (checkList == null || checkList.Count == 0)
            {
                throw new UserFriendlyException("未找到验收表导出数据");
            }

            var dt = (DataTable)null;
            var row = (DataRow)null;
            dt = new DataTable();
            var workSiteName = sky.PlanDetails.FirstOrDefault().WorkSiteName;

            dt.Columns.Add(new DataColumn("维护对象"));
            dt.Columns.Add(new DataColumn("类别"));
            dt.Columns.Add(new DataColumn("设备型号/编号"));
            dt.Columns.Add(new DataColumn("序号"));
            dt.Columns.Add(new DataColumn("维护项目"));
            dt.Columns.Add(new DataColumn("检修记录"));
            dt.Columns.Add(new DataColumn("检修人"));
            dt.Columns.Add(new DataColumn("验收情况"));
            dt.Columns.Add(new DataColumn("验收人"));
            dt.Columns.Add(new DataColumn("备注"));

            //给数据按某一规则分组和排序
            var newLists = new List<RepairCheckExportDto>();
            var oldLists = new List<RepairCheckExportDto>();

            var planTypeList = repairList.GroupBy(x => x.PlanType);
            foreach (var planType in planTypeList)
            {
                var deviceList = planType.GroupBy(x => x.DeviceName);
                foreach (var item in deviceList)
                {
                    oldLists = item.OrderBy(x => x.WorkContent).ToList();
                    foreach (var ite in oldLists)
                    {
                        newLists.Add(ite);
                    }
                }
            }

            //添加数据
            for (int i = 0; i < newLists.Count; i++)
            {
                var item = newLists[i];
                row = dt.NewRow();
                row["维护对象"] = workSiteName ?? string.Empty;
                row["类别"] = item.DeviceName ?? string.Empty;
                row["设备型号/编号"] = item.EquipmentName ?? string.Empty;
                row["序号"] = "1";
                row["维护项目"] = item.WorkContent ?? string.Empty;
                row["检修记录"] = item.RepairRecord ?? string.Empty;
                row["检修人"] = item.RepairUser ?? string.Empty;
                var temp = checkList.FirstOrDefault(s => s.WorkContentNumber == item.WorkContentNumber);
                row["验收情况"] = temp == null ? string.Empty : temp.RepairRecord;
                row["验收人"] = temp == null ? string.Empty : temp.RepairUser;
                row["备注"] = string.Empty;
                dt.Rows.Add(row);
            }
            return ExcelHepler.RepairWithCheckToExcel(dt, fileName, workTime);
        }

        /// <summary>
        /// 组织检修导出内容
        /// </summary>
        /// <param name="dtos">天窗的计划内容（日计划）</param>
        /// <param name="isMaintain">是否维修表</param>
        /// <returns></returns>
        private async Task<List<RepairCheckExportDto>> GetRepairExportDto(List<PlanDetailDto> dtos, bool isMaintain)
        {
            if (dtos == null || dtos.Count == 0) return null;
            List<RepairCheckExportDto> result = new List<RepairCheckExportDto>();
            try
            {
                foreach (var dto in dtos)
                {
                    if (dto.RelateEquipments?.Count > 0)
                    {
                        foreach (var rlEquip in dto.RelateEquipments)
                        {
                            RepairCheckExportDto epDto = new RepairCheckExportDto();
                            if (dto.DailyPlan != null)
                            {
                                epDto.PlanType = dto.DailyPlan.PlanTypeStr;
                                epDto.DeviceName = dto.DailyPlan.EquipName;
                                epDto.DeviceNumber = int.Parse(dto.DailyPlan.Number.Split('-')[0]);
                                epDto.WorkContent = dto.DailyPlan.Content;
                                var nums = dto.DailyPlan.Number.Split('-');
                                string newNums = "";
                                foreach (var tem in nums)
                                {
                                    newNums += int.Parse(tem).ToString().PadLeft(3, '0');
                                }
                                epDto.WorkContentNumber = newNums;
                            }
                            epDto.EquipmentName = rlEquip.EquipmentName;
                            //检修记录/验收情况赋值
                            var testList = _equipmentTestResult.Where(x => x.PlanRelateEquipmentId == rlEquip.Id).ToList();
                            if (testList?.Count > 0)
                            {
                                if (isMaintain)
                                {
                                    testList.ForEach(x =>
                                    {
                                        if (x.TestType != RepairTestType.EXCEL && !string.IsNullOrEmpty(x.TestResult) && x.TestType != RepairTestType.NUMBER)
                                        {
                                            epDto.RepairRecord += x.TestResult + "、";
                                        }
                                        if (x.TestType == RepairTestType.NUMBER)
                                        {
                                            epDto.RepairRecord += "\n" + x.TestName + ":" + x.TestResult + "\n";
                                        }
                                    });
                                    if (!string.IsNullOrEmpty(epDto.RepairRecord))
                                        epDto.RepairRecord = epDto.RepairRecord.Substring(0, epDto.RepairRecord.Length - 1);
                                }
                                else  //""、合格、不合格
                                {
                                    foreach (var item in testList)
                                    {
                                        if (item.CheckResult == null)
                                        {
                                            epDto.RepairRecord = "";
                                            break;
                                        }
                                        else if (item.CheckResult == "不合格")
                                        {
                                            epDto.RepairRecord = "不合格";
                                            break;
                                        }
                                        if (testList.IndexOf(item) == testList.Count - 1)
                                        {
                                            epDto.RepairRecord = "合格";
                                        }
                                    }
                                    /* testList.ForEach(x =>
                                     {
                                         if (x.CheckResult == "不合格")
                                             epDto.RepairRecord = "不合格";
                                     });*/
                                }
                            }
                            //检修/验收人赋值
                            Duty userTp = isMaintain ? Duty.Recondition : Duty.Acceptance;
                            var rpUsers = _repairUser.Where(x => x.PlanRelateEquipmentId == rlEquip.Id && x.Duty == userTp).ToList();
                            if (rpUsers?.Count > 0)
                            {
                                foreach (var item in rpUsers)
                                {
                                    var user = (await _personnel.GetUserListAsync(m => m.Id == item.UserId)).FirstOrDefault();
                                    if (user != null)
                                    {
                                        epDto.RepairUser += user.Name + "、";
                                    }

                                }
                                //rpUsers.ForEach(async x => epDto.RepairUser += (await _personnel.GetUserListAsync(m => m.Id == x.UserId)).FirstOrDefault()?.Name + "、");
                                //if (!string.IsNullOrEmpty(epDto.RepairUser))
                                //    epDto.RepairUser = epDto.RepairUser.Substring(0, epDto.RepairUser.Length - 1);
                            }
                            if (!string.IsNullOrEmpty(epDto.RepairUser))
                                epDto.RepairUser = epDto.RepairUser.Substring(0, epDto.RepairUser.Length - 1);
                            result.Add(epDto);
                        }
                    }
                }
                if (isMaintain)
                    result = result.OrderBy(a => a.PlanType).ThenBy(x => x.DeviceNumber).ThenBy(b => b.WorkContentNumber).ToList();
                else
                    result = result.OrderBy(a => a.PlanType).ThenBy(x => x.DeviceNumber).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取要修改的设备测试表Id或检修人员Id列表(从数据库获取)
        /// </summary>
        /// <param name="skylightPlanId"></param>
        /// <param name="testIdList"></param>
        /// <param name="userIdList"></param>
        private List<Guid> GetEquipTestOrUserId(Guid skylightPlanId, ref List<Guid> testIdList, ref List<Guid> userIdList)
        {
            List<Guid> tests = new List<Guid>();
            List<Guid> users = new List<Guid>();
            List<Guid> equipIdList = new List<Guid>();
            var planDetailList = _planDetail.Where(z => z.SkylightPlanId == skylightPlanId);
            if (planDetailList?.Count() > 0)
            {
                var detailIdList = planDetailList.ToList().ConvertAll(m => m.Id);
                var equipList = _planRelateEquipment.Where(z => detailIdList.Contains(z.PlanDetailId));
                if (equipList?.Count() > 0)
                {
                    equipIdList = equipList.ToList().ConvertAll(m => m.Id);
                }
            }
            if (equipIdList?.Count > 0)
            {

                var userList = _repairUser.Where(z => equipIdList.Contains(z.PlanRelateEquipmentId.Value));
                if (userList?.Count() > 0)
                {
                    users = userList.ToList().ConvertAll(m => m.Id);
                }
                var testList = _equipmentTestResult.Where(z => equipIdList.Contains(z.PlanRelateEquipmentId));
                if (testList?.Count() > 0)
                {
                    tests = testList.ToList().ConvertAll(m => m.Id);
                }
            }
            testIdList = tests;
            userIdList = users;
            return equipIdList;
        }


        /// <summary>
        /// 将天窗计划的工作内容，转化为派工单界面使用工作内容
        /// 获取数据使用
        /// </summary>
        /// <param name="planList">天窗计划详细内容Dto列表</param>
        /// <param name="userList">检修、验收人员列表</param>
        /// <param name="testResultList">设备测试项列表</param>
        /// <param name="workContent">其他计划内容</param>
        /// <returns></returns>
        private async Task<List<JobContentDetailDto>> SkylightToConversionAsync(
            List<PlanDetailDto> planList,
            List<RepairUser> userList,
            List<EquipmentTestResultDto>
            testResultList,
            string workContent)
        {
            List<JobContentDetailDto> list = new List<JobContentDetailDto>();
            var identityUserList = new List<IdentityUser>();
            if (userList?.Count > 0)
            {
                var userIds = userList.ConvertAll(m => m.UserId);
                if (userIds?.Count > 0)
                {
                    identityUserList = await _personnel.GetUserListAsync(z => userIds.Contains(z.Id));
                }
            }
            if (planList?.Count > 0)
            {
                planList = planList.OrderBy(z => z.DailyPlan?.EquipName).ToList();

                planList.ForEach(m =>
                {
                    JobContentDetailDto dto = new JobContentDetailDto();
                    if (m.DailyPlan != null && m.DailyPlan.EquipName != null)

                        if (list.Find(x => x.DeviceName == m.DailyPlan.EquipName) == null)
                        {
                            //设备类型
                            dto.DeviceName = m.DailyPlan.EquipName;
                            dto.DeviceNumber = int.Parse(m.DailyPlan.Number.Split('-')[0]);

                            dto.JobContentEquipmentList = new List<JobContentEquipmentDetailDto>();
                            var detailList = planList.FindAll(x => x.DailyPlan.EquipName == m.DailyPlan.EquipName);

                            //所有工作内容
                            if (detailList?.Count > 0)
                            {
                                List<PlanRelateEquipmentDto> eqAllList = new List<PlanRelateEquipmentDto>();
                                detailList.ForEach(n =>
                                {
                                    if (n.RelateEquipments?.Count > 0)
                                    {
                                        eqAllList.AddRange(n.RelateEquipments);

                                    }
                                });

                                //根据设备
                                eqAllList.ForEach(p =>
                        {
                            if (p != null)
                            {
                                var befor = dto.JobContentEquipmentList.Find(x => x.EquipmentId == p.EquipmentId);
                                if (!p.EquipmentId.HasValue)
                                {


                                    befor = dto.JobContentEquipmentList.Find(x => x.EquipmentId == null || x.EquipmentId == Guid.Empty);
                                }


                                if (befor == null)
                                {
                                    List<PlanRelateEquipmentDto> seachList = eqAllList.FindAll(x => x.EquipmentId == p.EquipmentId);
                                    if (seachList == null || seachList.Count < 1)
                                    {
                                        if (p.EquipmentId == null || p.EquipmentId == Guid.Empty)
                                        {
                                            seachList = eqAllList.FindAll(x => x.EquipmentId == null || x.EquipmentId == Guid.Empty);
                                        }
                                    }
                                    if (seachList?.Count > 0)
                                    {
                                //一个设备
                                JobContentEquipmentDetailDto eqDto = new JobContentEquipmentDetailDto();
                                        if (p.EquipmentId.HasValue)
                                        {
                                            eqDto.EquipmentId = p.EquipmentId.Value;
                                        }
                                        eqDto.EquipmentName = p.EquipmentName;

                                //定义年月
                                eqDto.YearMonthDetailedList = new List<EquipmentYearMonthDetailDto>();
                                        int typeNO = Enum.GetNames(typeof(Enumer.YearMonthPlanType)).GetLength(0);
                                        var yearPlans = new List<PlanDetailDto>();
                                        for (int i = 0; i < 2; i++)
                                        {
                                    //年月表
                                    EquipmentYearMonthDetailDto detailed = new EquipmentYearMonthDetailDto();
                                            if (i == 0)
                                            {

                                                yearPlans = detailList.FindAll(x => x.DailyPlan.PlanTypeStr != Enumer.YearMonthPlanType.月表.ToString());

                                                detailed.YearMonthPlanType = Enumer.YearMonthPlanType.年表;
                                            }
                                            else
                                            {
                                                yearPlans = detailList.FindAll(x => x.DailyPlan.PlanTypeStr == Enumer.YearMonthPlanType.月表.ToString());
                                                detailed.YearMonthPlanType = Enumer.YearMonthPlanType.月表;
                                            }

                                            List<Guid> planGuidList = new List<Guid>();

                                    //根据年月表类型获取工作内容


                                    planGuidList = yearPlans.ConvertAll(y => y.Id);

                                    //根据工作内容获取关联设备Dto
                                    detailed.PlanDetailedList = new List<EquipmentPlanDetailDto>();
                                            var plans = seachList.FindAll(y => planGuidList.Contains(y.PlanDetailId));
                                            if (plans?.Count > 0)
                                            {
                                                plans = plans.OrderBy(x => x.EquipmentName).ToList();
                                        //根据关联设备列表获取测试项及作业人员
                                        plans.ForEach(q =>
                                {
                                                    if (detailed.PlanDetailedList.Find(k => k.Id == q.Id) == null)
                                                    {
                                                        var yearplan = yearPlans.Find(k => k.Id == q.PlanDetailId);
                                                        EquipmentPlanDetailDto equip = new EquipmentPlanDetailDto();
                                                        equip.Id = q.Id;
                                                        equip.IsComplete = q.IsComplete;
                                                        equip.PlanDetailId = q.PlanDetailId;

                                                        equip.PlanCount = q.PlanCount;
                                                        equip.WorkCount = q.WorkCount;
                                                        equip.Number = yearplan.DailyPlan.Number;

                                                        equip.WorkContent = detailList.Find(y => y.Id == equip.PlanDetailId).DailyPlan.Content;
                                                //测试项
                                                equip.EquipmentTestResultList = new List<EquipmentTestResultDto>();
                                                        var testList = testResultList.FindAll(y => y.PlanRelateEquipmentId == q.Id);
                                                        if (testList?.Count > 0)
                                                        {
                                                            equip.EquipmentTestResultList.AddRange(testList.OrderBy(x => x.Order));
                                                        }
                                                //验收人
                                                equip.AcceptanceUserList = new List<RepairUserDto>();
                                                //检修人
                                                equip.MaintenanceUserList = new List<RepairUserDto>();
                                                        var users = userList.FindAll(y => y.PlanRelateEquipmentId == q.Id);
                                                        if (users?.Count > 0)
                                                        {

                                                            users.ForEach(k =>
                                                    {
                                                                                RepairUserDto user = new RepairUserDto(k.Id);
                                                                                user.Duty = k.Duty;
                                                                                user.PlanRelateEquipmentId = k.PlanRelateEquipmentId.Value;
                                                                                user.Remark = k.Remark;
                                                                                user.UserId = k.UserId;
                                                                                var identUser = identityUserList.Find(y => y.Id == k.UserId);
                                                                                user.UserName = identUser != null ? identUser.Name : "";

                                                                                if (user.Duty == Duty.Acceptance)
                                                                                {
                                                                                    equip.AcceptanceUserList.Add(user);
                                                                                }
                                                                                else
                                                                                {
                                                                                    equip.MaintenanceUserList.Add(user);
                                                                                }

                                                                            });
                                                        }
                                                        detailed.PlanDetailedList.Add(equip);
                                                    }
                                                });
                                                if (detailed.PlanDetailedList?.Count > 0)
                                                {
                                            //左补0进行排序
                                            foreach (var item in detailed.PlanDetailedList)
                                                    {
                                                        var nums = item.Number.Split('-');
                                                        var newNums = "";
                                                        for (int t = 0; t < nums.Length; t++)
                                                        {
                                                            newNums += nums[t].PadLeft(3, '0') + "-";
                                                        }
                                                        item.Number = newNums.TrimEnd('-');
                                                    }
                                                    var orderList = detailed.PlanDetailedList.OrderBy(z => z.Number.Replace("-", "")).ThenBy(z => z.WorkContent).ToList();
                                                    detailed.PlanDetailedList = orderList;
                                            //恢复原有数据形式
                                            foreach (var item in detailed.PlanDetailedList)
                                                    {
                                                        var nums = item.Number.Split('-');
                                                        string newNum = "";
                                                        foreach (var num in nums)
                                                        {
                                                            newNum += int.Parse(num).ToString() + "-";
                                                        }
                                                        item.Number = newNum.TrimEnd('-');
                                                    }
                                                }
                                                eqDto.YearMonthDetailedList.Add(detailed);

                                            }

                                        }

                                        dto.JobContentEquipmentList.Add(eqDto);
                                    }

                                }
                            }

                        });

                            }
                            list.Add(dto);
                        }


                });

            }
            else
            {
                //验收人
                var acceptanceUserList = new List<RepairUserDto>();
                //检修人
                var maintenanceUserList = new List<RepairUserDto>();
                userList.ForEach(k =>
                {
                    RepairUserDto user = new RepairUserDto(k.Id);
                    user.Duty = k.Duty;
                    user.PlanRelateEquipmentId = k.PlanRelateEquipmentId.Value;
                    user.Remark = k.Remark;
                    user.UserId = k.UserId;
                    var identUser = identityUserList.Find(y => y.Id == k.UserId);
                    user.UserName = identUser != null ? identUser.Name : "";

                    if (user.Duty == Duty.Acceptance)
                    {
                        acceptanceUserList.Add(user);
                    }
                    else
                    {
                        maintenanceUserList.Add(user);
                    }

                });
                EquipmentPlanDetailDto equipmentPlanDetailDto = new EquipmentPlanDetailDto()
                {
                    WorkContent = workContent,
                    MaintenanceUserList = maintenanceUserList,
                    AcceptanceUserList = acceptanceUserList,
                };
                EquipmentYearMonthDetailDto YearMonthDetailed = new EquipmentYearMonthDetailDto()
                {
                    YearMonthPlanType = Enumer.YearMonthPlanType.年表,
                    PlanDetailedList = new List<EquipmentPlanDetailDto>(),
                };
                YearMonthDetailed.PlanDetailedList.Add(equipmentPlanDetailDto);
                JobContentEquipmentDetailDto jobContentEquipmentDetailDto = new JobContentEquipmentDetailDto()
                {
                    EquipmentId = Guid.Empty,
                    EquipmentName = null,
                    YearMonthDetailedList = new List<EquipmentYearMonthDetailDto>(),
                };
                jobContentEquipmentDetailDto.YearMonthDetailedList.Add(YearMonthDetailed);
                JobContentDetailDto jobContentDetailDto = new JobContentDetailDto()
                {
                    DeviceName = "",
                    DeviceNumber = 0,
                    JobContentEquipmentList = new List<JobContentEquipmentDetailDto>(),
                };
                jobContentDetailDto.JobContentEquipmentList.Add(jobContentEquipmentDetailDto);
                list.Add(jobContentDetailDto);
            }
            if (list?.Count > 0)
            {
                list = list.OrderBy(x => x.DeviceNumber).ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据组织机构ID获取组织机构及所有子集组织机构Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<Guid>> GetSubsetOrganizationIdList(Guid id)
        {
            List<Guid> idList = new List<Guid>();
            await Task.Run(async () =>
            {
                var organization = (await _organization.Where(x => x.Id == id)).FirstOrDefault();
                if (organization != null)
                {

                    idList.Add(organization.Id);
                    var childlist = (await _organization.Where(m => m.Code.Substring(0, organization.Code.Length) == organization.Code)).ToList();
                    if (childlist?.Count > 0)
                    {
                        childlist.ForEach(m =>
                        {
                            idList.Add(m.Id);
                        });
                    }
                }
            });
            return idList;

        }

        /// <summary>
        /// 根据用户组织机构获取所有车间
        /// </summary>
        /// <param name="id">车间或段组织机构ID</param>
        /// <returns></returns>
        private async Task<List<Organization>> GetWorkShopByUserOrgId(Guid id)
        {
            var userOrg = (await _organization.Where(x => x.Id == id)).FirstOrDefault();
            if (userOrg == null || string.IsNullOrEmpty(userOrg.Code)) return null;
            try
            {
                //获取用户所在组织机构下的所有车间
                string topOrgCode = userOrg.Code.Substring(0, 4);
                var orgList = (await _organization.Where(x => x.Code.Length == 8 && x.Code.Substring(0, 4) == topOrgCode && x.Code != topOrgCode)).ToList();
                if (orgList == null || orgList.Count == 0) return null;
                return orgList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据天窗id查询复验人员信息
        /// </summary>
        /// <param name="skylightPlanId">天窗计划id</param>
        /// <returns></returns>
        public async Task<MemberDto> CheckPlatformLiaisonOfficer(Guid skylightPlanId)
        {
            if (Guid.Empty == skylightPlanId)
            {
                throw new UserFriendlyException("天窗Id 有误");
            }

            var maintenanceWorkRltSkylightPaln = _maintenanceWorkRltSkylightPlanRepository
                .WithDetails(x => x.MaintenanceWork)
                .FirstOrDefault(x => x.SkylightPlanId == skylightPlanId);

            if (maintenanceWorkRltSkylightPaln == null)
            {
                throw new UserFriendlyException("天窗计划关联维修作业不存在");
            }

            var workflowId = maintenanceWorkRltSkylightPaln.MaintenanceWork?.ARKey;

            if (Guid.Empty == workflowId)
            {
                throw new UserFriendlyException("工作流不存在");
            }

            var creatorId = _workflowRepository.FirstOrDefault(x => x.Id == workflowId)?.CreatorId;

            if (creatorId == null)
            {
                throw new UserFriendlyException("id is null");
            }
            var userInfo = await _personnel.GetByIdAsync(creatorId.GetValueOrDefault());

            var member = new MemberDto()
            {
                Id = userInfo.Id,
                Name = userInfo.Name,
                Type = MemberType.User
            };

            return await Task.FromResult(member);
        }

        /// <summary>
        /// 获取派工单测试项的其他附加测试项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<string> GetWorkOrderTestAdditional(WorkOrderTestAdditionalDto input)
        {
            if (input.WorkOrderId == Guid.Empty || string.IsNullOrEmpty(input.Number))
            {
                throw new UserFriendlyException("参数为空");
            }

            var workOrderTestAdditional = _workOrderTestAdditionals.FirstOrDefault(x => x.WorkOrderId == input.WorkOrderId && x.Number == input.Number);

            //if (workOrderTestAdditional == null)
            //{
            //    throw new UserFriendlyException("无其他附加测试项");
            //}

            return Task.FromResult(workOrderTestAdditional?.TestConctent);
        }

        public async Task<bool> CreateWorkOrderTestAdditional(WorkOrderTestAdditionalDto input)
        {
            if (string.IsNullOrEmpty(input.TestConctent) || input.WorkOrderId == Guid.Empty || string.IsNullOrEmpty(input.Number))
            {
                throw new UserFriendlyException("参数为空");
            }
            var workOrderTestAdditionalDoman = _workOrderTestAdditionals.FirstOrDefault(x => x.WorkOrderId == input.WorkOrderId && x.Number == input.Number);
            var workOrderTestAdditional = ObjectMapper.Map<WorkOrderTestAdditionalDto, WorkOrderTestAdditional>(input);

            if (workOrderTestAdditionalDoman == null)
            {
                await _workOrderTestAdditionals.InsertAsync(workOrderTestAdditional);

            }
            else
            {
                workOrderTestAdditionalDoman.TestConctent = input.TestConctent;
                await _workOrderTestAdditionals.UpdateAsync(workOrderTestAdditionalDoman);
            }

            return true;
        }

        public async Task<bool> UploadWorkOrderTestAdditional(IFormFile file, Guid workOrderId, string number)
        {
            DataTable dataTable;
            if (file.Length == 0) throw new UserFriendlyException("导入文件为空");
            try
            {
                dataTable = ExcelHepler.ExcelToDataTable(file.OpenReadStream(), file.FileName, 1);//读取EXCEL
                var jsonData = ToJson(dataTable);

                var workOrderTestAdditional = _workOrderTestAdditionals.FirstOrDefault(x => x.WorkOrderId == workOrderId && x.Number == number);

                if (workOrderTestAdditional != null)
                {
                    workOrderTestAdditional.TestConctent = jsonData;
                    await _workOrderTestAdditionals.UpdateAsync(workOrderTestAdditional);
                }
                else
                {
                    var workOrderTestAdditionalEnt = new WorkOrderTestAdditional(Guid.NewGuid())
                    {
                        WorkOrderId = workOrderId,
                        Number = number,
                        TestConctent = jsonData
                    };
                    await _workOrderTestAdditionals.InsertAsync(workOrderTestAdditionalEnt);
                }
            }
            catch (Exception)
            {

                throw new UserFriendlyException("上传文件格式有误");
            }


            return true;

        }

        /// <summary>   
        /// Datatable转换为目标格式
        /// </summary>      
        /// <param name="table">Datatable对象</param>
        /// <returns>Json字符串</returns>    
        private static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                jsonString.Append("\"" + "key" + "\":" + "\"" + Guid.NewGuid().ToString() + "\"" + ",");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;

                    if (strKey == "验收记录") strKey = "checkResult";
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>     
        /// 格式化字符型、日期型、布尔型 
        /// </summary>     
        /// <param name="str"></param>   
        /// <param name="type"></param> 
        /// <returns></returns>     
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        /// <summary>     
        /// 过滤特殊字符    
        /// </summary>    
        /// <param name="s">字符串</param> 
        /// <returns>json字符串</returns> 
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
    }
}
