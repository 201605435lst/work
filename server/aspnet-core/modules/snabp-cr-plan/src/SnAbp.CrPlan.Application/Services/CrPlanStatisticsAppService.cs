using Microsoft.AspNetCore.Http;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.IServices.Statistics;
using SnAbp.Identity;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.CrPlan.Services
{
    public class CrPlanStatisticsAppService : CrPlanAppService, ICrPlanStatisticsAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OrganizationManager _orgManager; //组织机构
        private readonly IRepository<YearMonthPlan, Guid> _yearMonthPlan;//年月表计划
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<PlanDetail, Guid> _planDetail;
        private readonly IRepository<DailyPlan, Guid> _dailyPlan;
        private readonly IRepository<DailyPlanAlter, Guid> _dailyPlanAlter;
        private readonly IRepository<SkylightPlan, Guid> _skylightPlan;
        private readonly IRepository<AlterRecord, Guid> _alterRecords;
        private readonly IRepository<RepairGroup, Guid> _repairGroups;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private readonly IRepository<StatisticsEquipmentWorker, Guid> statisticsEquipmentWorkers;
        private readonly IRepository<StatisticsPieWorker, Guid> statisticsPieWorkers;

        public CrPlanStatisticsAppService(
            IHttpContextAccessor httpContextAccessor,
            OrganizationManager orgManager,
            IRepository<YearMonthPlan, Guid> yearMonthPlan,
            IRepository<Organization, Guid> organizationRepository,
            IRepository<PlanDetail, Guid> planDetail,
            IRepository<DailyPlanAlter, Guid> dailyPlanAlter,
            IRepository<DailyPlan, Guid> dailyPlan,
            IRepository<SkylightPlan, Guid> skylightPlan,
            IRepository<AlterRecord, Guid> alterRecords,
            IRepository<RepairGroup, Guid> repairGroups,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IRepository<StatisticsPieWorker, Guid> statisticsPieWorkers,
            IRepository<StatisticsEquipmentWorker, Guid> statisticsEquipmentWorkers
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _orgManager = orgManager;
            _yearMonthPlan = yearMonthPlan;
            _organizationRepository = organizationRepository;
            _planDetail = planDetail;
            _dailyPlan = dailyPlan;
            _skylightPlan = skylightPlan;
            _dailyPlanAlter = dailyPlanAlter;
            _alterRecords = alterRecords;
            _repairGroups = repairGroups;
            _dataDictionaries = dataDictionaries;
            this.statisticsPieWorkers = statisticsPieWorkers;
            this.statisticsEquipmentWorkers = statisticsEquipmentWorkers;
        }

        /// <summary>
        /// 获取年表完成进度
        /// </summary>
        /// <returns></returns>
        public async Task<List<YearPlanFinishProgressDto>> GetYearPlanProgress(YearMonthPlanInputSearchDto input)
        {
            try
            {

                List<YearPlanFinishProgressDto> result = new List<YearPlanFinishProgressDto>();

                List<Organization> allOrg = null;
                var allOrgIds = new List<Guid>();
                Guid orgId;

                if (input.IsLoginFree)
                {
                    var org = _organizationRepository.FirstOrDefault(x => x.CSRGCode == "08001");
                    if (org == null)
                    {
                        throw new UserFriendlyException("组织机构有误");
                    }
                    orgId = org.Id;
                }
                else
                {
                    orgId = new Guid(_httpContextAccessor.HttpContext.Request.Headers["OrganizationId"]);
                }
                if (Guid.Empty != orgId)
                {
                    var org = await _orgManager.GetAsync(orgId);
                    allOrg = _organizationRepository.Where(s => s.Code.StartsWith(org.Code)).ToList();
                    allOrgIds = allOrg.Select(s => s.Id).ToList();
                }

                //设备维修组
                var allRepairGroupName = _repairGroups.Where(s => s.ParentId == null && !string.IsNullOrEmpty(s.Name)).Select(s => s.Name).Distinct().ToList();
                List<RepairGroupFinishInfo> repairGroupsTemplate = new List<RepairGroupFinishInfo>();
                foreach (var item in allRepairGroupName)
                {
                    RepairGroupFinishInfo group = new RepairGroupFinishInfo(item);
                    repairGroupsTemplate.Add(group);
                }

                List<YearMonthPlan> allYMPlan = null;
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                if (input.Type == 1)
                {
                    var parentIds = _yearMonthPlan
                        .WhereIf(allOrgIds.Count > 0, x => allOrgIds.Contains(x.ResponsibleUnit))
                        .WhereIf(!input.IsLoginFree, x => x.RepairTagId == RepairTagId)
                        .Where(s =>
                            (s.PlanType == 1 && s.Year == input.Time.Year && s.ParentId == null)
                           && s.State == (int)Enumer.YearMonthPlanState.审核通过).Select(s => s.Id).ToList();

                    allYMPlan = _yearMonthPlan.
                        Where(s => (s.ParentId == null && parentIds.Contains(s.Id)) ||
                         (s.ParentId != null && parentIds.Contains((Guid)s.ParentId) && s.Month == input.Time.Month && s.State == (int)Enumer.YearMonthPlanState.审核通过))
                        .WhereIf(!input.IsLoginFree, x => x.RepairTagId == RepairTagId)
                       .ToList();
                }
                else if (input.Type == 2)
                {
                    allYMPlan = _yearMonthPlan
                        .WhereIf(allOrgIds.Count > 0, x => allOrgIds.Contains(x.ResponsibleUnit))
                        .WhereIf(!input.IsLoginFree, x => x.RepairTagId == RepairTagId)
                        .Where(s =>
                        (s.PlanType == 2 && s.Year == input.Time.Year && s.Month == input.Time.Month && s.State == (int)Enumer.YearMonthPlanState.审核通过)).ToList();
                }
                else
                {
                    throw new UserFriendlyException("类型有误");
                }

                var planIds = allYMPlan.Select(s => s.Id).ToList();
                var dailplans = _dailyPlan.Where(s => planIds.Contains(s.PlanId));


                //日计划与计划详情与计划变更连接
                List<PlanFinishInfo> PlanFinishInfos = GetPlanFinishInfos(input, allYMPlan, dailplans, RepairTagId, input.IsLoginFree);

                //符合组织机构筛选后的计划
                var belongOrgPlans = allYMPlan.Where(s => s.ParentId == null);

                //对组织机构分组并统计总的年计划完成比例
                var groupedOrgPlan = belongOrgPlans.GroupBy(s => s.ResponsibleUnit).Select(s => new { OrgId = s.Key, Plans = s.Select(s => s) }).ToList();
                foreach (var item in groupedOrgPlan)
                {
                    YearPlanFinishProgressDto res = new YearPlanFinishProgressDto();
                    decimal totalCount = 0;
                    decimal finishCount = 0;
                    decimal alterCount = 0;
                    foreach (var p in item.Plans)
                    {
                        var info = PlanFinishInfos.FirstOrDefault(s => s.PlanId == p.Id);
                        if (info != null)
                        {
                            totalCount += info.TotalCount;
                            finishCount += info.FinishCount;
                            alterCount += info.AlterCount;
                        }
                    }
                    //var res = result.FirstOrDefault(s => s.OrgId == item.OrgId);
                    var alterAfterTotal = totalCount - alterCount;
                    res.TotalFinishInfo.TotalCount = totalCount;//totalCount;
                    if (finishCount >= alterAfterTotal)
                    {
                        res.TotalFinishInfo.FinishCount = alterAfterTotal;
                    }
                    else
                    {
                        res.TotalFinishInfo.FinishCount = finishCount;
                    }
                    // - alterCount;
                    res.TotalFinishInfo.AlterCount = alterCount;
                    res.OrgId = item.OrgId;
                    res.OrgName = allOrg.FirstOrDefault(a => a.Id == item.OrgId)?.Name;
                    //只统计总数量大于0
                    if (res.TotalFinishInfo.TotalCount > 0)
                        result.Add(res);

                    //统计各个设备类型(RepairGroup)的完成情况
                    var grouped_repairGroup = item.Plans.GroupBy(s => s.RepairGroup).Select(s => new { RepaiGroup = s.Key, Plans = s.Select(a => a) });

                    foreach (var g in grouped_repairGroup)
                    {
                        RepairGroupFinishInfo tempGroup = new RepairGroupFinishInfo(g.RepaiGroup);
                        decimal singleRepairGroupTotalCount = 0;
                        decimal singleRepairGroupFinishCount = 0;
                        decimal singleRepairGroupAlterCount = 0;
                        foreach (var p in g.Plans)
                        {
                            var info = PlanFinishInfos.FirstOrDefault(s => s.PlanId == p.Id);
                            if (info != null)
                            {
                                singleRepairGroupTotalCount += info.TotalCount;
                                singleRepairGroupFinishCount += info.FinishCount;
                                singleRepairGroupAlterCount += info.AlterCount;
                            }
                        }
                        tempGroup.FinishInfo.AlterCount = singleRepairGroupAlterCount;

                        var singleAlterAfterTotal = singleRepairGroupTotalCount - singleRepairGroupAlterCount;

                        if (singleRepairGroupFinishCount >= singleAlterAfterTotal)
                        {
                            tempGroup.FinishInfo.FinishCount = singleAlterAfterTotal;
                        }
                        else
                        {
                            tempGroup.FinishInfo.FinishCount = singleRepairGroupFinishCount;
                        }

                        //tempGroup.FinishInfo.FinishCount = singleRepairGroupFinishCount - singleRepairGroupAlterCount;
                        tempGroup.FinishInfo.TotalCount = singleRepairGroupTotalCount;
                        //只统计总数量大于0的项
                        if (tempGroup.FinishInfo.TotalCount > 0)
                            res.RepairGroupFinishInfos.Add(tempGroup);
                    }

                }
                //由于分组是对所有年月计划的组织机构（未包含无计划的组织机构）进行的，所以部分组织机构维修分组并未赋值
                //对没有年月计划的组织机构进行维修分组赋值
                //foreach (var item in result.Where(s => s.RepairGroupFinishInfos.Count == 0))
                //{
                //    item.RepairGroupFinishInfos = repairGroupsTemplate;
                //}

                //return result.OrderByDescending(s => s.TotalFinishInfo.FinishPercent).ToList();
                return result.OrderByDescending(s => s.TotalFinishInfo.FinishPercent).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定组织机构，指定设备类型下的完成进度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<RepairGroupFinishInfo>> GetRepairGroupFinishData(RepairGroupInputSearchDto input)
        {
            if (input.OrgId == null || input.OrgId == Guid.Empty) throw new UserFriendlyException("组织机构有误");
            if (string.IsNullOrEmpty(input.GroupName)) throw new UserFriendlyException("设备类型有误");
            List<RepairGroupFinishInfo> result = new List<RepairGroupFinishInfo>();

            var parentGroup = _repairGroups.FirstOrDefault(s => s.Name == input.GroupName && s.Parent == null);
            if (parentGroup != null)
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var parentID = parentGroup.Id;
                var groupNames = _repairGroups.Where(s => s.Parent.Id == parentID).Select(s => s.Name).ToList();
                IQueryable<YearMonthPlan> allYMPlan = null;
                if (input.Type == 1)
                {
                    var parentIds = _yearMonthPlan.Where(s => s.RepairTagId == RepairTagId &&
                    s.ResponsibleUnit == input.OrgId &&
                    groupNames.Contains(s.DeviceName) &&
                    (s.PlanType == 1 && s.Year == input.Time.Year && s.ParentId == null)
                    && s.State == (int)Enumer.YearMonthPlanState.审核通过).Select(s => s.Id).ToList();
                    allYMPlan = _yearMonthPlan.Where(s => s.RepairTagId == RepairTagId && (s.ParentId == null && parentIds.Contains(s.Id)) ||
                    (s.ParentId != null && parentIds.Contains((Guid)s.ParentId) && s.Month == input.Time.Month)
                    && s.State == (int)Enumer.YearMonthPlanState.审核通过);
                }
                else if (input.Type == 2)
                {
                    allYMPlan = _yearMonthPlan.Where(s => s.RepairTagId == RepairTagId &&
                    s.ResponsibleUnit == input.OrgId &&
                    groupNames.Contains(s.DeviceName) &&
                    (s.PlanType == 2 && s.Year == input.Time.Year && s.Month == input.Time.Month)
                    && s.State == (int)Enumer.YearMonthPlanState.审核通过);
                }
                var allYMPlanList = allYMPlan.ToList();
                var planIds = allYMPlanList.Select(s => s.Id).ToList();
                var dailplans = _dailyPlan.Where(s => planIds.Contains(s.PlanId));

                //日计划与计划详情与计划变更连接
                List<PlanFinishInfo> PlanFinishInfos = GetPlanFinishInfos(input, allYMPlanList, dailplans, RepairTagId);
                //符合组织机构筛选后的计划
                var belongOrgPlans = allYMPlanList.Where(s => s.ParentId == null);

                //对设备名称分组并统计总的年划完成比例
                var groupedRepairPlan = belongOrgPlans.GroupBy(s => s.DeviceName).Select(s => new { GroupName = s.Key, Plans = s.Select(s => s) });
                foreach (var item in groupedRepairPlan)
                {
                    RepairGroupFinishInfo res = new RepairGroupFinishInfo(item.GroupName);
                    decimal totalCount = 0;
                    decimal finishCount = 0;
                    decimal alterCount = 0;
                    foreach (var p in item.Plans)
                    {
                        var info = PlanFinishInfos.FirstOrDefault(s => s.PlanId == p.Id);
                        if (info != null)
                        {
                            totalCount += info.TotalCount;
                            finishCount += info.FinishCount;
                            alterCount += info.AlterCount;
                        }
                    }
                    res.FinishInfo.TotalCount = totalCount;

                    var alterAfterTotal = totalCount - alterCount;

                    if (finishCount >= alterAfterTotal)
                    {
                        res.FinishInfo.FinishCount = alterAfterTotal;
                    }
                    else
                    {
                        res.FinishInfo.FinishCount = finishCount;
                    }

                    res.FinishInfo.AlterCount = alterCount;
                    if (res.FinishInfo.TotalCount > 0)
                    {
                        result.Add(res);
                    }
                }
            }
            return result;
        }


        public List<PlanFinishInfo> GetPlanFinishInfos(YearMonthPlanInputSearchDto input, List<YearMonthPlan> allYMPlanList, IQueryable<DailyPlan> dailplans, Guid? repairTagId, bool isFreeLogin = false)
        {
            var passedAlterRecordIds = _alterRecords
                .WhereIf(!isFreeLogin, x => x.RepairTagId == repairTagId)
                .Where(s => s.State == Enumer.YearMonthPlanState.审核通过).Select(s => s.Id).ToList();

            var dail_detail = (from da in dailplans
                               join de in _planDetail
                               .WhereIf(!isFreeLogin, x => x.RepairTagId == repairTagId)
                               on da.Id equals de.DailyPlanId
                               select new { da = da, de = de }).ToList().GroupBy(s => s.da.PlanId).Select(s => new { s.Key, Details = s.Select(a => a.de) });

            var dail_alter = (from da in dailplans
                              join al in _dailyPlanAlter
                              .WhereIf(!isFreeLogin, x => x.RepairTagId == repairTagId)
                              .Where(s => passedAlterRecordIds.Contains(s.AlterRecordId)) on da.Id equals al.DailyId
                              select new { da = da, al = al }).ToList().GroupBy(s => s.da.PlanId).Select(s => new { s.Key, Alters = s.Select(a => a.al) });

            //YearMonthPlan计划与上文关联结果关联
            var planRlts = (from p in allYMPlanList
                            join d in dail_detail on p.Id equals d.Key into res1
                            from r1 in res1.DefaultIfEmpty()
                            join a in dail_alter on p.Id equals a.Key into res2
                            from r2 in res2.DefaultIfEmpty()
                            select new { Plan = p, Details = res1.Select(s => s.Details), Alters = res2.Select(s => s.Alters) }).ToList();


            var planGroups = planRlts.GroupBy(s => s.Plan.ParentId).Select(s => new { PlanParentId = s.Key, Plans = s.Select(s => s) });
            List<PlanFinishInfo> PlanFinishInfos = new List<PlanFinishInfo>();
            //计划每条计划的完成数量
            if (input.Type == 1)
            {
                foreach (var groupP in planGroups)
                {
                    if (groupP.PlanParentId != null)
                    {
                        //父计划下的子计划
                        var childPlans = groupP.Plans.Where(s => s.Plan.ParentId == groupP.PlanParentId);
                        PlanFinishInfo flag = new PlanFinishInfo();
                        flag.PlanId = (Guid)groupP.PlanParentId;
                        foreach (var item in childPlans.WhereIf(!isFreeLogin, s => s.Plan.RepairTagId == repairTagId))
                        {
                            flag.TotalCount += item.Plan.Total;
                            foreach (var i in item.Details)
                            {
                                //var alterPlanId = i.Select(x => x.DailyPlanId).FirstOrDefault();
                                //var alterPlans = _dailyPlanAlter.Where(x => x.DailyId == alterPlanId).ToList();
                                flag.FinishCount += i.WhereIf(!isFreeLogin, s => s.RepairTagId == repairTagId).Sum(s => s.WorkCount);
                            }
                            foreach (var i in item.Alters)
                            {
                                flag.AlterCount += i.WhereIf(!isFreeLogin, s => s.RepairTagId == repairTagId).Sum(s => s.AlterCount);
                            }
                        }
                        PlanFinishInfos.Add(flag);
                    }
                }
            }
            else if (input.Type == 2)
            {
                foreach (var groupP in planGroups)
                {
                    foreach (var p in groupP.Plans.WhereIf(!isFreeLogin, s => s.Plan.RepairTagId == repairTagId))
                    {
                        PlanFinishInfo flag = new PlanFinishInfo();
                        flag.PlanId = p.Plan.Id;
                        //int times = 1;
                        //if (!int.TryParse(p.Plan.Times, out times)) times = 1;
                        //flag.TotalCount += p.Plan.Total * times;

                        flag.TotalCount = FieldSum(p.Plan, "Col_", input.Time);

                        foreach (var i in p.Details)
                        {
                            flag.FinishCount += i.WhereIf(!isFreeLogin, s => s.RepairTagId == repairTagId).Sum(s => s.WorkCount);
                        }
                        foreach (var i in p.Alters)
                        {
                            flag.AlterCount += i.WhereIf(!isFreeLogin, s => s.RepairTagId == repairTagId).Sum(s => s.AlterCount);
                        }
                        PlanFinishInfos.Add(flag);
                    }
                }
            }
            return PlanFinishInfos;
        }

        /// <summary>
        /// 统计月表中的总数量
        /// </summary>
        /// <typeparam name="YearMonthPlan"></typeparam>
        /// <param name="plan"></param>
        /// <param name="prefix"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private decimal FieldSum<YearMonthPlan>(YearMonthPlan plan, string prefix, DateTime time)
        {
            int days = DateTime.DaysInMonth(time.Year, time.Month);

            decimal sum = 0;
            var props = plan.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name.StartsWith($"{prefix}") && decimal.TryParse(prop.GetValue(plan).ToString(), out var result))
                {
                    var day = int.Parse(prop.Name.Split("_")[1]);
                    if (day <= days)
                    {
                        sum += result;
                    }
                }
            }
            return sum;
        }

        public Task<GetPieStatisticalDto> GetPieStatistical(YearMonthPlanInputSearchDto input)
        {
            var pieStatisticalDto = new GetPieStatisticalDto();

            //总体数据饼状图
            var pieStatistic = statisticsPieWorkers
                .FirstOrDefault(x => x.Type == input.Type
                && x.Month == input.Time.Month
                && x.Year == input.Time.Year
                && x.OrgizationName == null
                );

            //车间完成统计图
            var historgramStatistics = statisticsPieWorkers
                .Where(x => x.Type == input.Type
                && x.Month == input.Time.Month
                && x.Year == input.Time.Year
                && x.OrgizationId != null)
                .ToList();


            if (pieStatistic == null) return Task.FromResult(pieStatisticalDto);

            pieStatisticalDto.FinshedTotal = pieStatistic.Finshed;
            pieStatisticalDto.UnFinshedTotal = pieStatistic.UnFinshed;
            pieStatisticalDto.ChangeTotal = pieStatistic.Changed;

            foreach (var item in historgramStatistics)
            {
                var histogramStatisticDto = new GetHistogramStatisticDto()
                {
                    OrganizationName = item.OrgizationName,
                    FinshedTotal = item.Finshed,
                    UnFinshedTotal = item.UnFinshed,
                    ChangeTotal = item.Changed,
                    OrgId = item.OrgizationId
                };
                pieStatisticalDto.HistogramInfos.Add(histogramStatisticDto);
            }

            return Task.FromResult(pieStatisticalDto);
        }

        public Task<List<GetEquipmentStatisticDto>> GetEquipmentStatistics(SearchEquipmentDto input)
        {
            var equipmentStatistic = statisticsEquipmentWorkers
                .Where(x => x.OrgizationId == input.OrgId && x.Year == input.Time.Year && x.Month == input.Time.Month && x.Type == input.Type)
                .ToList();

            var res = ObjectMapper.Map<List<StatisticsEquipmentWorker>, List<GetEquipmentStatisticDto>>(equipmentStatistic);

            return Task.FromResult(res);
        }
    }
    public class PlanFinishInfo
    {
        public Guid PlanId { get; set; }
        public decimal TotalCount { get; set; }
        public decimal FinishCount { get; set; }
        public decimal AlterCount { get; set; }
    }


}
