using Microsoft.AspNetCore.Authorization;
using SnAbp.Basic.Entities;
using SnAbp.CrPlan.Authorization;
using SnAbp.CrPlan.Dto.Statistical;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.IServices.Statistical;
using SnAbp.CrPlan.Repositories;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using SnAbp.Identity;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using SnAbp.CrPlan.Enums;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using SnAbp.CrPlan.Enumer;
using SnAbp.Utils.EnumHelper;

namespace SnAbp.CrPlan.Services
{
    /// <summary>
    /// 智能报表接口实现
    /// </summary>
    [Authorize]
    public class CrPlanStatisticalAppService : CrPlanAppService, ICrPlanStatisticalAppService
    {

        private readonly OrganizationManager _organizationRepository;
        private readonly IRepository<DailyPlan, Guid> _dailyPlanRepository;
        private readonly IRepository<PlanDetail, Guid> _planDetailRepository;
        private readonly IRepository<YearMonthPlan, Guid> _yearMonthPlan;//年月表计划
        private readonly IRepository<DailyPlanAlter, Guid> _dailyPlanAlterRepository;//计划变更
        private readonly IRepository<AlterRecord, Guid> _alterRecordRepository;//生产任务变更记录

        private readonly IRepository<SkylightPlan, Guid> _skylightRepository;//天窗
        private readonly IdentityUserManager _personnelRepository;//用户
        private readonly IRepository<WorkOrder, Guid> _workOrderRepository;//派工单
        private readonly IRepository<Worker, Guid> _workerRepository;//作业人员
        private readonly IRepository<WorkOrganization, Guid> _workOrganizationRepository;//作业单位
        private readonly IRepository<Station, Guid> _stationRepository;//处所位置
        private readonly ICrPlanStatistialRepository _crPlanStatistial;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;

        public CrPlanStatisticalAppService(
            OrganizationManager organizationRepository,
            IRepository<YearMonthPlan, Guid> yearMonthPlanRepository,
            IRepository<DailyPlan, Guid> dailyPlanRepository,
            IRepository<PlanDetail, Guid> planDetailRepository,
            IRepository<DailyPlanAlter, Guid> dailyPlanAlter,
            IRepository<AlterRecord, Guid> alterRecord,
            IRepository<SkylightPlan, Guid> skylightRepository,
            IdentityUserManager personnelRepository,
            IRepository<WorkOrder, Guid> workOrderRepository,
            IRepository<Worker, Guid> workerRepository,
            IRepository<WorkOrganization, Guid> workOrganizationRepository,
            IRepository<Station, Guid> stationRepository,
            ICrPlanStatistialRepository crPlanStatistial,
            IRepository<DataDictionary, Guid> dataDictionaries
            )
        {
            _organizationRepository = organizationRepository;
            _yearMonthPlan = yearMonthPlanRepository;
            _dailyPlanRepository = dailyPlanRepository;
            _planDetailRepository = planDetailRepository;
            _dailyPlanAlterRepository = dailyPlanAlter;
            _alterRecordRepository = alterRecord;
            _skylightRepository = skylightRepository;
            _personnelRepository = personnelRepository;
            _workOrderRepository = workOrderRepository;
            _workerRepository = workerRepository;
            _workOrganizationRepository = workOrganizationRepository;
            _stationRepository = stationRepository;
            _crPlanStatistial = crPlanStatistial;
            _dataDictionaries = dataDictionaries;
        }

        /// <summary>
        /// 根据用户组织机构Id获取整个段的年计划完成情况统计列表
        /// </summary>
        /// <param name="id">当前用户组织机构ID</param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_StatisticalCharts)]
        public async Task<List<YearStatisticalDto>> GetYearStatistical(Guid id, string repairTag)
        {
            try
            {
                Dictionary<string, decimal> rst = _crPlanStatistial.GetYearStatisticalBySql(id);
                List<YearStatisticalDto> res = new List<YearStatisticalDto>();
                if (rst?.Count > 0)
                {
                    foreach (var item in rst)
                    {
                        res.Add(new YearStatisticalDto() { OrganizationName = item.Key, Percentage = item.Value });
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

            //if (id == Guid.Empty) throw new UserFriendlyException("未找到组织机构");
            //List<YearStatisticalDto> res = new List<YearStatisticalDto>();
            ////获取用户所在组织机构下的所有车间
            //var orgList = GetWorkShopByUserOrgId(id);
            //if (orgList == null || orgList.Count == 0) return null;
            //var allYearMonthPlans = _yearMonthPlan.Where(x => x.PlanType == 1 && x.Year == DateTime.Now.Year);
            //var allDailyPlans = _dailyPlanRepository.ToList();
            //var allPlanDetails = _planDetailRepository.ToList();
            //try
            //{
            //    foreach (var org in orgList)
            //    {
            //        YearStatisticalDto dto = new YearStatisticalDto();
            //        decimal planCount = 0;    //计划数量
            //        decimal completeCount = 0;    //完成数量
            //        var ymPlans = allYearMonthPlans.Where(x => x.ResponsibleUnit == org.Id).ToList();
            //        if (ymPlans == null || ymPlans.Count == 0) continue;
            //        foreach (var plan in ymPlans)
            //        {
            //            var dailyPlans = allDailyPlans.Where(x => x.PlanId == plan.Id).ToList();
            //            if (dailyPlans?.Count > 0)
            //            {
            //                foreach (var daily in dailyPlans)
            //                {
            //                    var details = allPlanDetails.Where(x => x.DailyPlanId == daily.Id).ToList();
            //                    planCount += daily.Count;
            //                    if (details?.Count > 0)
            //                        completeCount = details.Sum(x => x.WorkCount);
            //                }
            //            }
            //        }
            //        dto.OrganizationName = org.Name;
            //        dto.Percentage = 0;
            //        if (completeCount > 0 && planCount > 0)
            //            dto.Percentage = Math.Round(completeCount / planCount, 3);
            //        res.Add(dto);
            //    }

            //    return res;
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(ex.Message);
            //}
        }

        /// <summary>
        /// 根据查询条件获取整个段的月计划完成情况统计列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_StatisticalCharts)]
        public async Task<List<MonthStatisticalDto>> GetMonthStatistical(StatisticalChartsSearchInputDto input)
        {
            try
            {
                int planType = 2;
                if (input.MonthPlanType == Enumer.YearMonthPlanStatisticalType.年表)
                {
                    planType = 3;
                }
                Dictionary<string, List<decimal>> rst = _crPlanStatistial.GetMonthStatistical(input.OrganizationId, planType, input.PlanTime);
                List<MonthStatisticalDto> res = new List<MonthStatisticalDto>();
                if (rst?.Count > 0)
                {
                    //Value 
                    foreach (var item in rst)
                    {
                        if (item.Value?.Count == 3)
                        {
                            MonthStatisticalDto dto = new MonthStatisticalDto();
                            dto.OrganizationName = item.Key;
                            dto.FinishCount = Math.Round(item.Value[1], 2);
                            dto.ChangeCount = Math.Round(item.Value[2], 2);
                            //未完成 = 总计划 - 已完成 - 变更
                            if (item.Value[0] >= dto.FinishCount + dto.ChangeCount)
                                dto.UnFinishedCount = item.Value[0] - dto.FinishCount - dto.ChangeCount;
                            if (dto.FinishCount == item.Value[0])
                                dto.isFinish = true;
                            res.Add(dto);
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

            //if (input == null || input.OrganizationId == Guid.Empty) return null;
            //List<MonthStatisticalDto> res = new List<MonthStatisticalDto>();
            ////获取用户所在组织机构下的所有车间
            //var orgList = GetWorkShopByUserOrgId(input.OrganizationId);
            //if (orgList == null || orgList.Count == 0) return null;
            //try
            //{
            //    var planType = Enumer.YearMonthPlanType.月表;
            //    if (input.MonthPlanType == Enumer.YearMonthPlanStatisticalType.年表)
            //    {
            //        planType = Enumer.YearMonthPlanType.年度月表;
            //    }
            //    foreach (var org in orgList)
            //    {
            //        MonthStatisticalDto dto = new MonthStatisticalDto();
            //        var ymPlans = _yearMonthPlan.Where(x => x.PlanType == planType.GetHashCode() && x.Year == input.PlanTime.Year && x.Month == input.PlanTime.Month).ToList();
            //        if (ymPlans == null || ymPlans.Count == 0) continue;
            //        decimal planCount = 0;    //总的计划数量
            //        foreach (var plan in ymPlans)
            //        {
            //            var dailyPlans = _dailyPlanRepository.Where(x => x.PlanId == plan.Id).ToList();
            //            if (dailyPlans?.Count > 0)
            //            {
            //                foreach (var daily in dailyPlans)
            //                {
            //                    //已完成数量
            //                    var details = _planDetailRepository.Where(x => x.DailyPlanId == daily.Id).ToList();
            //                    planCount += daily.Count;
            //                    if (details?.Count > 0)
            //                        details.ForEach(x => dto.FinishCount += x.WorkCount);
            //                    //变更数量
            //                    var alters = _dailyPlanAlterRepository.Where(x => x.DailyId == daily.Id).ToList();
            //                    if (alters?.Count > 0)
            //                        alters.ForEach(x =>
            //                        {
            //                            if (x.PlanCount >= x.AlterCount)
            //                                dto.ChangeCount += (x.PlanCount - x.AlterCount);
            //                        });
            //                }
            //            }
            //        }
            //        dto.FinishCount = Math.Round(dto.FinishCount, 2);
            //        dto.ChangeCount = Math.Round(dto.ChangeCount, 2);
            //        planCount = Math.Round(planCount);
            //        //未完成 = 总计划 - 已完成 - 变更
            //        if (planCount >= dto.FinishCount + dto.ChangeCount)
            //            dto.UnFinishedCount = planCount - dto.FinishCount - dto.ChangeCount;
            //        dto.OrganizationName = org.Name;
            //        if (dto.FinishCount == planCount)
            //            dto.isFinish = true;
            //        res.Add(dto);
            //    }
            //    return res;
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(ex.Message);
            //}
        }

        /// <summary>
        /// 根据查询条件获取设备类型的完成情况统计列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_StatisticalCharts)]
        public async Task<List<EquipmentStatisticalDto>> GetEquipmentStatistical(StatisticalChartsSearchInputDto input)
        {
            if (input == null || input.OrganizationId == Guid.Empty) return null;
            try
            {
                var planType = Enumer.YearMonthPlanType.月表;
                if (input.MonthPlanType == Enumer.YearMonthPlanStatisticalType.年表)
                {
                    planType = Enumer.YearMonthPlanType.年度月表;
                }
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                //查询本车间下所有月表
                var ymPlans = _yearMonthPlan.Where(x => x.RepairTagId == RepairTagId && x.ResponsibleUnit == input.OrganizationId && x.PlanType == planType.GetHashCode()).ToList();
                var allDailyPlans = _dailyPlanRepository.Where(x => x.RepairTagId == RepairTagId && x.Id != Guid.Empty);//全部日计划
                var allPlanDetails = _planDetailRepository.Where(x => x.RepairTagId == RepairTagId && x.Id != Guid.Empty);  //全部计划详情
                var allPlanAlters = _dailyPlanAlterRepository.Where(x => x.RepairTagId == RepairTagId && x.Id != Guid.Empty);   //全部变更
                if (ymPlans == null || ymPlans.Count == 0) return null;
                List<EquipmentStatisticalDto> res = new List<EquipmentStatisticalDto>();
                decimal planCount = 0;    //总的计划数量
                var typeList = ymPlans.GroupBy(x => x.RepairGroup); //维修项类型
                if (typeList?.Count() > 0)
                {
                    //第一层---维修项类型分组
                    foreach (var tp in typeList)
                    {
                        EquipmentStatisticalDto typeDto = new EquipmentStatisticalDto();
                        typeDto.Name = tp.Key;
                        typeDto.Children = new List<EquipmentStatisticalDto>();
                        var nameList = tp.GroupBy(x => x.DeviceName);
                        //第二层---维修项类型下的名称分组
                        foreach (var item in nameList)
                        {
                            EquipmentStatisticalDto nameDto = new EquipmentStatisticalDto();
                            nameDto.Name = item.Key;
                            nameDto.Children = new List<EquipmentStatisticalDto>();
                            //第三层---维修项名称下的维修内容
                            foreach (var content in item)
                            {
                                EquipmentStatisticalDto contentDto = new EquipmentStatisticalDto();
                                contentDto.Name = content.RepairContent;
                                var dailyPlans = allDailyPlans.Where(x => x.PlanId == content.Id).ToList();
                                if (dailyPlans?.Count > 0)
                                {
                                    foreach (var daily in dailyPlans)
                                    {
                                        //已完成数量
                                        var details = allPlanDetails.Where(x => x.DailyPlanId == daily.Id).ToList();
                                        planCount += daily.Count;
                                        if (details?.Count > 0)
                                            details.ForEach(x => contentDto.FinishCount += x.WorkCount);
                                        //变更数量
                                        var alters = allPlanAlters.Where(x => x.DailyId == daily.Id).ToList();
                                        if (alters?.Count > 0)
                                            alters.ForEach(x =>
                                            {
                                                if (x.PlanCount >= x.AlterCount)
                                                    contentDto.ChangeCount += x.PlanCount - x.AlterCount;
                                            });
                                    }
                                }
                                //contentDto.FinishCount = Math.Round(contentDto.FinishCount, 2);
                                //contentDto.ChangeCount = Math.Round(contentDto.ChangeCount, 2);
                                //planCount = Math.Round(planCount);
                                //未完成 = 总计划 - 已完成 - 变更
                                if (planCount >= contentDto.FinishCount + contentDto.ChangeCount)
                                    contentDto.UnFinishedCount = planCount - contentDto.FinishCount - contentDto.ChangeCount;
                                nameDto.Children.Add(contentDto);
                            }
                            if (nameDto.Children?.Count > 0)
                            {
                                nameDto.FinishCount = nameDto.Children.Sum(x => x.FinishCount);
                                nameDto.UnFinishedCount = nameDto.Children.Sum(x => x.UnFinishedCount);
                                nameDto.ChangeCount = nameDto.Children.Sum(x => x.ChangeCount);
                            }
                            typeDto.Children.Add(nameDto);
                        }
                        if (typeDto.Children?.Count > 0)
                        {
                            typeDto.FinishCount = typeDto.Children.Sum(x => x.FinishCount);
                            typeDto.UnFinishedCount = typeDto.Children.Sum(x => x.UnFinishedCount);
                            typeDto.ChangeCount = typeDto.Children.Sum(x => x.ChangeCount);
                        }
                        res.Add(typeDto);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// 根据查询条件获取计划状态追踪数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_PlanStateTracking)]
        public async Task<PlanStateTrackingDto> GetPlanStateTracking(PlanStateTrackingSearchInputDto input)
        {
            if (input == null || input.OrganizationId == Guid.Empty) return null;
            if (input.PlanTime.Year < 2000 && input.PlanTime.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                PlanStateTrackingDto dto = new PlanStateTrackingDto();
                await Task.Run(async () =>
               {
                   var planType = Enumer.YearMonthPlanType.月表;
                   if (input.MonthPlanType == Enumer.YearMonthPlanStatisticalType.年表)
                   {
                       planType = Enumer.YearMonthPlanType.年度月表;
                   }
                   int month = input.PlanTime.Month;
                   if (!string.IsNullOrEmpty(input.SequenceNumber))
                   {
                       var arrayNumbers = input.SequenceNumber.Split("-");
                       if (arrayNumbers.Length != 3) throw new UserFriendlyException("请输入正确格式年月表序号！"); ;
                       var newArray = new List<string>();
                       foreach (var item in arrayNumbers)
                       {
                           int intA = 0;
                           if (!int.TryParse(item, out intA)) return;
                           newArray.Add(int.Parse(item).ToString(new string('0', 3)));
                       }
                       input.SequenceNumber = string.Join("-", newArray);
                   }
                   //获取计划
                   var plan = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId &&
                           z.Year == input.PlanTime.Year && z.Month == input.PlanTime.Month && (input.OrganizationId != null && z.ResponsibleUnit == input.OrganizationId) &&
                           z.PlanType == planType.GetHashCode() && z.Number == input.SequenceNumber).FirstOrDefault();
                   if (plan != null)
                   {
                       dto.Id = plan.Id;
                       dto.DeviceName = plan.DeviceName;
                       dto.RepairContent = plan.RepairContent;
                       dto.PlanCompletionList = new List<PlanCompletionDto>();
                       dto.SkyligetType = plan.SkyligetType;
                       dto.Total = plan.Total;

                       var dailyPlanList = _dailyPlanRepository.Where(z => z.RepairTagId == RepairTagId && z.PlanId == plan.Id && z.PlanDate.Month == month).ToList();
                       if (dailyPlanList?.Count > 0)
                       {
                           #region 获取符合条件的数据库数据
                           //获取日任务列表
                           var ids = dailyPlanList.ConvertAll(x => x.Id);

                           //获取变更列表
                           var planAlterList = _dailyPlanAlterRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.DailyId)).ToList();

                           //获取变更记录
                           var alterRecordList = new List<AlterRecord>();
                           if (planAlterList?.Count > 0)
                           {
                               var alterIds = planAlterList.ConvertAll(m => m.AlterRecordId);
                               alterRecordList = _alterRecordRepository.Where(z => z.RepairTagId == RepairTagId && alterIds.Contains(z.Id)).ToList();
                           }
                           //获取计划详细信息列表
                           var planDailyList = _planDetailRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.DailyPlanId)).ToList();

                           var skylightList = new List<SkylightPlan>();//天窗列表
                           var orderList = new List<WorkOrder>();//派工单列表
                           var userList = new List<IdentityUser>();//用户列表
                           var organizationList = new List<Organization>();//组织机构列表
                           var workerList = new List<Worker>();//派工单作业人员列表
                           var stationList = new List<Station>();//站点列表
                           var workOrganizationList = new List<WorkOrganization>();//派工单作业单位列表
                           if (planDailyList?.Count > 0)
                           {

                               var skylightIds = planDailyList.ConvertAll(m => m.SkylightPlanId);
                               //获取天窗
                               skylightList = _skylightRepository.Where(z => z.RepairTagId == RepairTagId && skylightIds.Contains(z.Id)).ToList();
                               if (skylightList?.Count > 0)
                               {
                                   var stationIds = skylightList.ConvertAll(m => m.Station);
                                   stationList = _stationRepository.Where(z => stationIds.Contains(z.Id)).ToList();

                                   //获取派工单
                                   orderList = _workOrderRepository.Where(z => z.RepairTagId == RepairTagId && skylightIds.Contains(z.SkylightPlanId)).ToList();
                                   if (orderList?.Count > 0)
                                   {
                                       var orderIds = orderList.ConvertAll(m => m.Id);
                                       //获取作业人员
                                       workerList = _workerRepository.Where(z => z.RepairTagId == RepairTagId && orderIds.Contains(z.WorkOrderId) && z.Duty == Enums.UserDuty.WorkLeader).ToList();
                                       if (workerList?.Count > 0)
                                       {
                                           //获取用户
                                           var userIds = workerList.ConvertAll(m => m.UserId);
                                           userList = await _personnelRepository.GetUserListAsync(z => userIds.Contains(z.Id));
                                       }
                                       //获取作业单位
                                       workOrganizationList = _workOrganizationRepository.Where(z => z.RepairTagId == RepairTagId && orderIds.Contains(z.WorkOrderId)).ToList();
                                       if (workOrganizationList?.Count > 0)
                                       {
                                           //获取组织机构
                                           var organizationIds = workOrganizationList.ConvertAll(m => m.OrganizationId);
                                           organizationList = (await _organizationRepository.Where(z => organizationIds.Contains(z.Id))).ToList();
                                       }
                                   }
                               }
                           }
                           #endregion

                           var dailyCompletionList = new List<SkylightPlanDailyCompletionDto>();
                           //组织数据
                           dailyPlanList.ForEach(m =>
                          {
                              PlanCompletionDto planDto = new PlanCompletionDto();
                              planDto.PlanCount = m.Count;
                              planDto.PlanTime = m.PlanDate.ToString("yyyy-MM-dd");
                              planDto.IsLastMonthChange = false;
                              //明细
                              planDto.SkylightPlanDailyCompletionList = new List<SkylightPlanDailyCompletionDto>();

                              var detailList = planDailyList.FindAll(x => x.DailyPlanId == m.Id);
                              decimal planTotal = 0;
                              decimal workTotal = 0;
                              decimal unfinishedTotal = 0;
                              if (detailList?.Count > 0)
                              {
                                  foreach (var x in detailList)
                                  {
                                      var currentSkylight = _skylightRepository.FirstOrDefault(s => s.Id == x.SkylightPlanId);

                                      //if (currentSkylight != null && currentSkylight.PlanState == PlanState.Backout) continue;

                                      SkylightPlanDailyCompletionDto dailyDto = new SkylightPlanDailyCompletionDto();
                                      dailyDto.SkylightPlanId = x.SkylightPlanId;
                                      dailyDto.FinishCount = x.WorkCount;
                                      workTotal += x.WorkCount;
                                      dailyDto.PlanCount = x.PlanCount;
                                      planTotal += x.PlanCount;
                                      dailyDto.UnFinishedCount = x.PlanCount - x.WorkCount;
                                      //查询是否有该天窗数据，有则其他属性直接赋值，无则组织数据
                                      var model = dailyCompletionList.Find(y => y.SkylightPlanId == x.SkylightPlanId);
                                      if (model == null)
                                      {
                                          //获取天窗
                                          var skylight = skylightList.Find(y => y.Id == x.SkylightPlanId);
                                          var order = orderList.Find(y => y.SkylightPlanId == x.SkylightPlanId);
                                          if (skylight != null)
                                          {
                                              dailyDto.PlanState = skylight.PlanState;
                                              dailyDto.PlanTime = skylight.WorkTime.ToString("yyyy-MM-dd");
                                              dailyDto.PlanType = skylight.PlanType;
                                              //站点
                                              if (skylight.Station != null)
                                              {
                                                  var station = stationList.Find(y => y.Id == skylight.Station);
                                                  if (station != null)
                                                  {
                                                      dailyDto.StationName = station.Name;
                                                  }
                                              }
                                              //获取派工单相关数据
                                              if (order != null)
                                              {
                                                  if (order.OrderState == Enums.OrderState.Acceptance)
                                                  {
                                                      dailyDto.PlanState = Enums.PlanState.Complete;
                                                  }
                                                  //作业单位
                                                  var workUnitList = workOrganizationList.FindAll(y => y.WorkOrderId == order.Id);
                                                  if (workUnitList?.Count > 0)
                                                  {
                                                      workUnitList.ForEach(p =>
                                                      {
                                                          var orangetion = organizationList.Find(y => y.Id == p.OrganizationId);
                                                          if (p.Duty == Enums.Duty.Acceptance)
                                                              dailyDto.CommunicationUnit = orangetion?.Name;
                                                          else
                                                          {
                                                              dailyDto.MaintenanceUnit = orangetion?.Name;
                                                          }
                                                      });
                                                  }
                                                  //作业组长
                                                  var worker = workerList.Find(y => y.WorkOrderId == order.Id);
                                                  if (worker != null)
                                                  {
                                                      var user = userList.Find(y => y.Id == worker.UserId);
                                                      dailyDto.WorkLeader = user.Name;

                                                  }
                                                  //实际作业时间
                                                  if (order.StartRealityTime != DateTime.MinValue && order.OrderState != Enums.OrderState.Unfinished)
                                                  {
                                                      string timeString = order.StartRealityTime.ToString("yyyy.MM.dd HH:mm:ss") +
                                                      "-" + order.EndRealityTime.ToString("yyyy.MM.dd HH:mm:ss");
                                                      dailyDto.WorkTime = timeString;
                                                  }
                                                  else
                                                  {
                                                      dailyDto.WorkTime = string.Empty;
                                                  }
                                              }

                                              dailyCompletionList.Add(dailyDto);
                                          }
                                      }
                                      else
                                      {
                                          dailyDto.CommunicationUnit = model.CommunicationUnit;
                                          dailyDto.MaintenanceUnit = model.MaintenanceUnit;
                                          dailyDto.PlanState = model.PlanState;
                                          dailyDto.PlanTime = model.PlanTime;
                                          dailyDto.PlanType = model.PlanType;
                                          dailyDto.StationName = model.StationName;
                                          dailyDto.WorkLeader = model.WorkLeader;
                                          dailyDto.WorkTime = model.WorkTime;

                                      }
                                      //添加到明细列表
                                      planDto.SkylightPlanDailyCompletionList.Add(dailyDto);
                                  };
                                  //合计
                                  planDto.PlanDailyTotal = new PlanDailyTotalDto();
                                  planDto.PlanDailyTotal.FinishCount = workTotal;
                                  planDto.PlanDailyTotal.PlanCount = planTotal;
                                  planDto.PlanDailyTotal.UnFinishedCount = planTotal - workTotal;
                              }

                              //变更计划获取
                              planDto.PlanChangeList = new List<PlanChangeDto>();
                              var alterList = planAlterList.FindAll(y => y.DailyId == m.Id);
                              if (alterList?.Count > 0)
                              {
                                  planDto.IsLastMonthChange = true;
                                  alterList.ForEach(x =>
                              {
                                  var record = alterRecordList.Find(y => y.Id == x.AlterRecordId);
                                  PlanChangeDto changeDto = new PlanChangeDto();
                                  changeDto.ApprovalStatus = record.State;
                                  changeDto.ChangeCount = x.AlterCount;
                                  changeDto.ChangeReason = record.Reason;
                                  changeDto.ChangeTime = record.AlterTime.ToString("yyyy.MM.dd");
                                  planDto.PlanChangeList.Add(changeDto);

                              });

                              }
                              dto.PlanCompletionList.Add(planDto);

                          });
                       }


                   }

               });
                if (dto.PlanCompletionList != null)
                {
                    dto.PlanCompletionList = dto.PlanCompletionList.OrderBy(x => x.PlanTime).ToList();
                }
                return dto;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 根据查询条件获取月计划完成情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_PlanCompletion)]
        public async Task<List<MonthCompletionDto>> GetMonthCompletion(MonthFinishSearchInputDto input)
        {
            if (input.PlanTime.Year < 2000 && input.PlanTime.Year > 9999)
            {
                throw new UserFriendlyException("年份应在2000-9999间");
            }
            if (input == null || input.OrganizationId == Guid.Empty) return null;
            var organization = await _organizationRepository.Where(m => m.Id == input.OrganizationId);
            if (organization.Count() == 0) return null;
            var organizationIdList = new List<Guid>();
            var organizationIdChildrenList = new List<Guid>();
            //新增需求:添加段统计
            var isDuanOrganization = false;
            var singleOrganization = organization.FirstOrDefault();


            if (singleOrganization?.Type?.Key == "OrganizationType.Duan")
            {
                isDuanOrganization = true;
                organizationIdChildrenList = singleOrganization?.Children.Select(x => x.Id).ToList();
            }

            if (organization.First().Code.Length < 5)
            {
                //获取用户所在组织机构下的所有车间
                var organizationList = await GetWorkShopByUserOrgId(input.OrganizationId);
                if (organizationList == null || organizationList.Count == 0) return null;
                organizationIdList = organizationList.ConvertAll(m => m.Id);
            }
            else
            {
                organizationIdList = new List<Guid>();
                organizationIdList.Add(input.OrganizationId);
            }

            var resList = new List<MonthCompletionDto>();
            var list = new List<MonthCompletionDto>();
            var finishlist = new List<MonthCompletionDto>();
            var unFinishedList = new List<MonthCompletionDto>();

            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(() =>
                   {

                       var planType = Enumer.YearMonthPlanType.年表;
                       if (input.MonthPlanType == Enumer.YearMonthPlanStatisticalType.年表)
                       {
                           planType = Enumer.YearMonthPlanType.年度月表;
                       }
                       else
                       {
                           planType = Enumer.YearMonthPlanType.月表;
                       }

                       //获取月计划
                       var planList = new List<YearMonthPlan>();
                       if (isDuanOrganization)
                       {
                           planList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId && z.Year == input.PlanTime.Year && z.Month == input.PlanTime.Month && z.PlanType == planType.GetHashCode() && organizationIdChildrenList.Contains(z.ResponsibleUnit)).OrderBy(z => z.Number.Replace("-", "")).ToList();

                           //var groupData = from a in planList group a by a.Number into temp select new { temp.Key, List = temp.ToList() };
                       }
                       else
                       {
                           planList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId && z.Year == input.PlanTime.Year && z.Month == input.PlanTime.Month && (organizationIdList.Contains(z.ResponsibleUnit)) && z.PlanType == planType.GetHashCode())
                             .OrderBy(z => z.Number.Replace("-", "")).ToList();
                       }
                       if (planList?.Count > 0)
                       {
                           var ids = planList.ConvertAll(x => x.Id);
                           //获取日任务列表
                           var dailyPlanList = _dailyPlanRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.PlanId)).ToList();

                           //获取计划详细信息列表
                           var planDetailList = new List<PlanDetail>();
                           ids = dailyPlanList.ConvertAll(x => x.Id);
                           planDetailList = _planDetailRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.DailyPlanId)).ToList();
                           var skylightPlanIdList = new List<Guid>();
                           //获取派工单
                           var workOrderList = new List<WorkOrder>();
                           if (planDetailList?.Count > 0)
                           {
                               var skIds = planDetailList.ConvertAll(x => x.SkylightPlanId);
                               workOrderList = _workOrderRepository.Where(z => z.RepairTagId == RepairTagId && skIds.Contains(z.SkylightPlanId) && ((z.OrderType == Enums.OrderType.WorkOrder && z.OrderState == Enums.OrderState.Acceptance) || (z.OrderType == Enums.OrderType.OtherAssignments && z.OrderState == Enums.OrderState.Complete))).ToList();
                               skylightPlanIdList = workOrderList.ConvertAll(x => x.SkylightPlanId);
                           }
                           planList.ForEach(m =>
                           {
                               var bf = resList.Find(x => x.Number == m.Number);
                               if (bf == null)
                               {
                                   var plans = planList.FindAll(x => x.Number == m.Number);
                                   if (plans?.Count > 0)

                                   {
                                       var dto = new MonthCompletionDto();
                                       dto.Number = ConvertNumber(m.Number);
                                       //dto.Number = m.Number;
                                       dto.RepairContent = m.RepairContent;
                                       dto.DeviceName = m.DeviceName;
                                       dto.Unit = m.Unit;
                                       dto.DataType = 0;

                                       var finishdto = new MonthCompletionDto();
                                       finishdto.Number = m.Number;
                                       finishdto.DeviceName = m.DeviceName;
                                       finishdto.RepairContent = m.RepairContent;
                                       finishdto.Unit = m.Unit;
                                       finishdto.DataType = 1;
                                       int k = 0;
                                       string equipmentLocation = string.Empty;
                                       decimal finishTotal = 0;
                                       plans.ForEach(y =>
                                       {
                                           //设备处所
                                           var nameList = new List<string>();
                                           if (k == 0 && !string.IsNullOrEmpty(y.EquipmentLocation))
                                           {
                                               equipmentLocation = y.EquipmentLocation;
                                               nameList.Add(y.EquipmentLocation);
                                           }
                                           else
                                           {
                                               if (nameList.Find(x => x == y.EquipmentLocation && !string.IsNullOrEmpty(y.EquipmentLocation)) == null)
                                               {
                                                   equipmentLocation += "," + y.EquipmentLocation;
                                                   nameList.Add(y.EquipmentLocation);
                                               }
                                           }

                                           #region 计划数量

                                           dto.Col_1 += y.Col_1;
                                           dto.Col_2 += y.Col_2;
                                           dto.Col_3 += y.Col_3;
                                           dto.Col_4 += y.Col_4;
                                           dto.Col_5 += y.Col_5;
                                           dto.Col_6 += y.Col_6;
                                           dto.Col_7 += y.Col_7;
                                           dto.Col_8 += y.Col_8;
                                           dto.Col_9 += y.Col_9;
                                           dto.Col_10 += y.Col_10;
                                           dto.Col_11 += y.Col_11;
                                           dto.Col_12 += y.Col_12;
                                           dto.Col_13 += y.Col_13;
                                           dto.Col_14 += y.Col_14;
                                           dto.Col_15 += y.Col_15;
                                           dto.Col_16 += y.Col_16;
                                           dto.Col_17 += y.Col_17;
                                           dto.Col_18 += y.Col_18;
                                           dto.Col_19 += y.Col_19;
                                           dto.Col_20 += y.Col_20;
                                           dto.Col_21 += y.Col_21;
                                           dto.Col_22 += y.Col_22;

                                           dto.Col_23 += y.Col_23;
                                           dto.Col_24 += y.Col_24;
                                           dto.Col_25 += y.Col_25;
                                           dto.Col_26 += y.Col_26;
                                           dto.Col_27 += y.Col_27;
                                           dto.Col_28 += y.Col_28;
                                           dto.Col_29 += y.Col_29;
                                           dto.Col_30 += y.Col_30;
                                           dto.Col_31 += y.Col_31;

                                           int days = DateTime.DaysInMonth(y.Year, y.Month);
                                           for (int i = 1; i <= days; i++)
                                           {
                                               dto.Total += GetColVal(y, i);
                                           }
                                           #endregion


                                           #region 完成数量

                                           if (dailyPlanList?.Count > 0)
                                           {
                                               var dailys = dailyPlanList.FindAll(x => x.PlanId == y.Id);
                                               if (dailys?.Count > 0)
                                               {
                                                   dailys.ForEach(x =>
                                                   {
                                                       decimal finishCount = 0;
                                                       if (planDetailList?.Count > 0)
                                                       {
                                                           var planDetails = planDetailList.FindAll(p => p.DailyPlanId == x.Id);
                                                           if (planDetails?.Count > 0)
                                                           {

                                                               var skIds = planDetails.ConvertAll(p => p.SkylightPlanId);
                                                               if (workOrderList?.Count > 0)
                                                               {
                                                                   var orderList = workOrderList.FindAll(z => skIds.Contains(z.SkylightPlanId));
                                                                   if (orderList?.Count > 0)
                                                                   {
                                                                       orderList.ForEach(m =>
                                                                       {
                                                                           finishCount = 0;
                                                                           int day = m.StartRealityTime.Day;
                                                                           var details = planDetails.FindAll(x => x.SkylightPlanId == m.SkylightPlanId);
                                                                           details.ForEach(m =>
                                                                           {
                                                                               finishCount += m.WorkCount;
                                                                               finishTotal += m.WorkCount;
                                                                           });
                                                                           switch (day)
                                                                           {
                                                                               //完成数据
                                                                               case 1:
                                                                                   finishdto.Col_1 += finishCount; break;
                                                                               case 2:
                                                                                   finishdto.Col_2 += finishCount; break;
                                                                               case 3:
                                                                                   finishdto.Col_3 += finishCount; break;
                                                                               case 4:
                                                                                   finishdto.Col_4 += finishCount; break;

                                                                               case 5:
                                                                                   finishdto.Col_5 += finishCount; break;
                                                                               case 6:
                                                                                   finishdto.Col_6 += finishCount; break;
                                                                               case 7:
                                                                                   finishdto.Col_7 += finishCount; break;
                                                                               case 8:
                                                                                   finishdto.Col_8 += finishCount; break;
                                                                               case 9:
                                                                                   finishdto.Col_9 += finishCount; break;
                                                                               case 10:
                                                                                   finishdto.Col_10 += finishCount; break;
                                                                               case 11:
                                                                                   finishdto.Col_11 += finishCount; break;
                                                                               case 12:
                                                                                   finishdto.Col_12 += finishCount; break;
                                                                               case 13:
                                                                                   finishdto.Col_13 += finishCount; break;
                                                                               case 14:
                                                                                   finishdto.Col_14 += finishCount; break;
                                                                               case 15:
                                                                                   finishdto.Col_15 += finishCount; break;
                                                                               case 16:
                                                                                   finishdto.Col_16 += finishCount; break;
                                                                               case 17:
                                                                                   finishdto.Col_17 += finishCount; break;
                                                                               case 18:
                                                                                   finishdto.Col_18 += finishCount; break;
                                                                               case 19:
                                                                                   finishdto.Col_19 += finishCount; break;
                                                                               case 20:
                                                                                   finishdto.Col_20 += finishCount; break;
                                                                               case 21:
                                                                                   finishdto.Col_21 += finishCount; break;
                                                                               case 22:
                                                                                   finishdto.Col_22 += finishCount; break;
                                                                               case 23:
                                                                                   finishdto.Col_23 += finishCount; break;
                                                                               case 24:
                                                                                   finishdto.Col_24 += finishCount; break;
                                                                               case 25:
                                                                                   finishdto.Col_25 += finishCount; break;
                                                                               case 26:
                                                                                   finishdto.Col_26 += finishCount; break;
                                                                               case 27:
                                                                                   finishdto.Col_27 += finishCount; break;
                                                                               case 28:
                                                                                   finishdto.Col_28 += finishCount; break;
                                                                               case 29:
                                                                                   finishdto.Col_29 += finishCount; break;
                                                                               case 30:
                                                                                   finishdto.Col_30 += finishCount; break;
                                                                               case 31: finishdto.Col_31 += finishCount; break;
                                                                           }

                                                                       });
                                                                   }


                                                               }



                                                           }
                                                       }
                                                   });

                                               }
                                           }
                                           #endregion

                                           k++;
                                       });
                                       if (equipmentLocation != null)
                                       {
                                           dto.EquipmentLocation = equipmentLocation.TrimStart(',');
                                       }
                                       finishdto.EquipmentLocation = equipmentLocation;
                                       finishdto.Total = finishTotal;
                                       if (dto.Total > 0)
                                       {
                                           dto.PlanCount = dto.Total;
                                           finishdto.PlanCount = dto.Total;
                                           if (dto.Total != finishdto.Total)
                                           {
                                               unFinishedList.Add(dto);
                                               unFinishedList.Add(finishdto);
                                           }
                                           else
                                           {
                                               finishlist.Add(dto);
                                               finishlist.Add(finishdto);
                                           }
                                           resList.Add(dto);
                                           resList.Add(finishdto);
                                       }

                                   }

                               }

                               #region 原方法
                               //               //计划数据
                               //               var dto = ObjectMapper.Map<YearMonthPlan, MonthCompletionDto>(m);
                               //               dto.DataType = 0;

                               //               //已完成数据
                               //               var finishDto = ObjectMapper.Map<YearMonthPlan, MonthCompletionDto>(m);
                               //               finishDto.DataType = 1;
                               //               decimal finishTotal = 0;
                               //               for (var i = 1; i < 31; i++)
                               //               {
                               //                   ////decimal count = 0;
                               //                   decimal finishCount = 0;
                               //                   if (dailyPlanList?.Count > 0)
                               //                   {
                               //                       var plan = dailyPlanList.Find(x => x.PlanDate.Day == i);
                               //                       if (plan != null)
                               //                       {
                               //                           //count = plan.Count;
                               //                           if (planDetailList?.Count > 0)
                               //                           {
                               //                               var dailyList = planDetailList.FindAll(x => x.DailyPlanId == plan.Id);
                               //                               if (dailyList?.Count > 0)
                               //                               {
                               //                                   dailyList.ForEach(m =>
                               //                                   {
                               //                                       finishCount += m.WorkCount;
                               //                                   });
                               //                               }
                               //                           }
                               //                       }
                               //                   }
                               //                   //SetColVal(dto, i, count);
                               //                   SetColVal(finishDto, i, finishCount);
                               //                   finishTotal += finishCount;
                               //               }
                               //               finishDto.Total = finishTotal;
                               //               //if (finishDto.Total == dto.Total)
                               //               //{
                               //               //    finishlist.Add(dto);
                               //               //    finishlist.Add(finishDto);
                               //               //}
                               //               //else
                               //               //{
                               //               //    unFinishedList.Add(dto);
                               //               //    unFinishedList.Add(finishDto);
                               //               //}
                               //               list.Add(dto);
                               //               list.Add(finishDto);
                               //           }
                               //           );
                               //       }

                               //   });
                               //if (list?.Count > 0)
                               //{
                               //    list.ForEach(m =>
                               //    {
                               //        var bf = resList.Find(x => x.Number == m.Number);
                               //        if (bf == null)
                               //        {
                               //            var plans = list.FindAll(x => x.Number == m.Number);

                               //            var dto = new MonthCompletionDto();
                               //            dto.Number = m.Number;
                               //            dto.RepairContent = m.RepairContent;
                               //            dto.Unit = m.Unit;
                               //            dto.DataType = 0;

                               //            var finishdto = new MonthCompletionDto();
                               //            finishdto.Number = m.Number;
                               //            finishdto.RepairContent = m.RepairContent;
                               //            finishdto.Unit = m.Unit;
                               //            finishdto.DataType = 1;
                               //            int i = 0;
                               //            string equipmentLocation = string.Empty;
                               //            plans.ForEach(y =>
                               //            {
                               //                //设备处所
                               //                var nameList = new List<string>();
                               //                if (i == 0)
                               //                {
                               //                    equipmentLocation = y.EquipmentLocation;
                               //                    nameList.Add(y.EquipmentLocation);
                               //                }
                               //                else
                               //                {
                               //                    if (nameList.Find(x => x == y.EquipmentLocation) == null)
                               //                    {
                               //                        equipmentLocation += "," + y.EquipmentLocation;
                               //                        nameList.Add(y.EquipmentLocation);
                               //                    }
                               //                }
                               //                //计划数据
                               //                if (y.DataType == 0)
                               //                {
                               //                    dto.PlanCount += y.PlanCount;
                               //                    dto.Total += y.Total;
                               //                    dto.Col_1 += y.Col_1;
                               //                    dto.Col_2 += y.Col_2;
                               //                    dto.Col_3 += y.Col_3;
                               //                    dto.Col_4 += y.Col_4;
                               //                    dto.Col_5 += y.Col_5;
                               //                    dto.Col_6 += y.Col_6;
                               //                    dto.Col_7 += y.Col_7;
                               //                    dto.Col_8 += y.Col_8;
                               //                    dto.Col_9 += y.Col_9;
                               //                    dto.Col_10 += y.Col_10;
                               //                    dto.Col_11 += y.Col_11;
                               //                    dto.Col_12 += y.Col_12;
                               //                    dto.Col_13 += y.Col_13;
                               //                    dto.Col_14 += y.Col_14;
                               //                    dto.Col_15 += y.Col_15;
                               //                    dto.Col_16 += y.Col_16;
                               //                    dto.Col_17 += y.Col_17;
                               //                    dto.Col_18 += y.Col_18;
                               //                    dto.Col_19 += y.Col_19;
                               //                    dto.Col_20 += y.Col_20;
                               //                    dto.Col_21 += y.Col_21;
                               //                    dto.Col_22 += y.Col_22;

                               //                    dto.Col_23 += y.Col_23;
                               //                    dto.Col_24 += y.Col_24;
                               //                    dto.Col_25 += y.Col_25;
                               //                    dto.Col_26 += y.Col_26;
                               //                    dto.Col_27 += y.Col_27;
                               //                    dto.Col_28 += y.Col_28;
                               //                    dto.Col_29 += y.Col_29;
                               //                    dto.Col_30 += y.Col_30;
                               //                    dto.Col_31 += y.Col_31;
                               //                }
                               //                else
                               //                {
                               //                    //完成数据
                               //                    finishdto.PlanCount += y.PlanCount;
                               //                    finishdto.Total += y.Total;
                               //                    finishdto.Col_1 += y.Col_1;
                               //                    finishdto.Col_2 += y.Col_2;
                               //                    finishdto.Col_3 += y.Col_3;
                               //                    finishdto.Col_4 += y.Col_4;
                               //                    finishdto.Col_5 += y.Col_5;
                               //                    finishdto.Col_6 += y.Col_6;
                               //                    finishdto.Col_7 += y.Col_7;
                               //                    finishdto.Col_8 += y.Col_8;
                               //                    finishdto.Col_9 += y.Col_9;
                               //                    finishdto.Col_10 += y.Col_10;
                               //                    finishdto.Col_11 += y.Col_11;
                               //                    finishdto.Col_12 += y.Col_12;
                               //                    finishdto.Col_13 += y.Col_13;
                               //                    finishdto.Col_14 += y.Col_14;
                               //                    finishdto.Col_15 += y.Col_15;
                               //                    finishdto.Col_16 += y.Col_16;
                               //                    finishdto.Col_17 += y.Col_17;
                               //                    finishdto.Col_18 += y.Col_18;
                               //                    finishdto.Col_19 += y.Col_19;
                               //                    finishdto.Col_20 += y.Col_20;
                               //                    finishdto.Col_21 += y.Col_21;
                               //                    finishdto.Col_22 += y.Col_22;

                               //                    finishdto.Col_23 += y.Col_23;
                               //                    finishdto.Col_24 += y.Col_24;
                               //                    finishdto.Col_25 += y.Col_25;
                               //                    finishdto.Col_26 += y.Col_26;
                               //                    finishdto.Col_27 += y.Col_27;
                               //                    finishdto.Col_28 += y.Col_28;
                               //                    finishdto.Col_29 += y.Col_29;
                               //                    finishdto.Col_30 += y.Col_30;
                               //                    finishdto.Col_31 += y.Col_31;
                               //                }

                               //                i++;


                               //            });
                               //            dto.EquipmentLocation = equipmentLocation;
                               //            finishdto.EquipmentLocation = equipmentLocation; 
                               #endregion



                           });

                       }
                       if (input.State == Enumer.PlanFinishState.Complete)
                       {
                           list = new List<MonthCompletionDto>();
                           list.AddRange(finishlist);
                       }
                       else if (input.State == Enumer.PlanFinishState.Unfinished)
                       {
                           list = new List<MonthCompletionDto>();
                           list.AddRange(unFinishedList);
                       }
                       else
                       {
                           list = new List<MonthCompletionDto>();
                           list.AddRange(resList);
                       }
                   });
                return list;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }

        }

        /// <summary>
        /// 根据查询条件获取单项统计数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_StatisticalCharts)]
        public async Task<List<SingleCompleteDto>> GetSingleStatistical(SingleCompleteSearchInputDto input)
        {
            if (input.PlanTime.Year < 2000 && input.PlanTime.Year > 9999)
            {
                throw new UserFriendlyException("年份应在2000-9999间");
            }
            if (input == null || input.OrganizationId == Guid.Empty) return null;
            var organization = await _organizationRepository.Where(m => m.Id == input.OrganizationId);
            if (organization.Count() == 0) return null;
            var organizationIdList = new List<Guid>();
            if (organization.First().Code.Length < 5)
            {
                //获取用户所在组织机构下的所有车间
                var organizationList = await GetWorkShopByUserOrgId(input.OrganizationId);
                if (organizationList == null || organizationList.Count == 0) return null;
                organizationIdList = organizationList.ConvertAll(m => m.Id);
            }
            else
            {
                organizationIdList = new List<Guid>();
                organizationIdList.Add(input.OrganizationId);
            }
            var resList = new List<SingleCompleteDto>();

            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(() =>
                {
                    var planType = Enumer.YearMonthPlanType.月表;
                    if (input.MonthPlanType == Enumer.YearMonthPlanStatisticalType.年表)
                    {
                        planType = Enumer.YearMonthPlanType.年度月表;
                    }

                    //获取月计划
                    var planList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId &&
                     z.Year == input.PlanTime.Year && (organizationIdList.Contains(z.ResponsibleUnit)) && z.Number == input.Number &&
                     z.PlanType == planType.GetHashCode() && (z.Month == input.PlanTime.Month)).ToList();
                    if (planList?.Count > 0)
                    {
                        var ids = planList.ConvertAll(x => x.Id);
                        //获取日任务列表
                        var dailyPlanList = _dailyPlanRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.PlanId)).ToList();

                        //获取计划详细信息列表
                        var planDetailList = new List<PlanDetail>();
                        ids = dailyPlanList.ConvertAll(x => x.Id);
                        planDetailList = _planDetailRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.DailyPlanId)).ToList();
                        var skylightPlanIdList = new List<Guid>();
                        //获取派工单
                        var workOrderList = new List<WorkOrder>();
                        if (planDetailList?.Count > 0)
                        {
                            var skIds = planDetailList.ConvertAll(x => x.SkylightPlanId);
                            workOrderList = _workOrderRepository.Where(z => z.RepairTagId == RepairTagId && skIds.Contains(z.SkylightPlanId) && ((z.OrderType == Enums.OrderType.WorkOrder && z.OrderState == Enums.OrderState.Acceptance) || (z.OrderType == Enums.OrderType.OtherAssignments && z.OrderState == Enums.OrderState.Complete))).ToList();
                            skylightPlanIdList = workOrderList.ConvertAll(x => x.SkylightPlanId);
                        }
                        int days = DateTime.DaysInMonth(input.PlanTime.Year, input.PlanTime.Month);
                        for (int i = 1; i <= days; i++)
                        {
                            var dto = new SingleCompleteDto();
                            dto.Days = i;
                            resList.Add(dto);
                        }
                        planList.ForEach(m =>
                        {
                            #region 计划数量
                            resList[0].PlanCount += m.Col_1;
                            resList[1].PlanCount += m.Col_2;
                            resList[2].PlanCount += m.Col_3;
                            resList[3].PlanCount += m.Col_4;
                            resList[4].PlanCount += m.Col_5;
                            resList[5].PlanCount += m.Col_6;
                            resList[6].PlanCount += m.Col_7;
                            resList[7].PlanCount += m.Col_8;
                            resList[8].PlanCount += m.Col_9;
                            resList[9].PlanCount += m.Col_10;
                            resList[10].PlanCount += m.Col_11;
                            resList[11].PlanCount += m.Col_12;
                            resList[12].PlanCount += m.Col_13;
                            resList[13].PlanCount += m.Col_14;
                            resList[14].PlanCount += m.Col_15;
                            resList[15].PlanCount += m.Col_16;
                            resList[16].PlanCount += m.Col_17;
                            resList[17].PlanCount += m.Col_18;
                            resList[18].PlanCount += m.Col_19;
                            resList[19].PlanCount += m.Col_20;
                            resList[20].PlanCount += m.Col_21;
                            resList[21].PlanCount += m.Col_22;

                            resList[22].PlanCount += m.Col_23;
                            resList[23].PlanCount += m.Col_24;
                            resList[24].PlanCount += m.Col_25;
                            resList[25].PlanCount += m.Col_26;
                            resList[26].PlanCount += m.Col_27;
                            resList[27].PlanCount += m.Col_28;
                            if (resList.Count >= 29)
                            {
                                resList[28].PlanCount += m.Col_29;
                            }
                            if (resList.Count >= 30)
                            {
                                resList[29].PlanCount += m.Col_30;
                            }
                            if (resList.Count >= 31)
                            {
                                resList[30].PlanCount += m.Col_31;
                            }

                            #endregion

                            #region 完成数量
                            if (dailyPlanList?.Count > 0)
                            {
                                var dailys = dailyPlanList.FindAll(x => x.PlanId == m.Id);
                                if (dailys?.Count > 0)
                                {
                                    dailys.ForEach(x =>
                                    {
                                        decimal finishCount = 0;
                                        if (planDetailList?.Count > 0)
                                        {
                                            var planDetails = planDetailList.FindAll(p => p.DailyPlanId == x.Id);
                                            if (planDetails?.Count > 0)
                                            {
                                                var skIds = planDetails.ConvertAll(p => p.SkylightPlanId);
                                                if (workOrderList?.Count > 0)
                                                {
                                                    var orderList = workOrderList.FindAll(z => skIds.Contains(z.SkylightPlanId));
                                                    if (orderList?.Count > 0)
                                                    {
                                                        orderList.ForEach(m =>
                                                        {
                                                            int day = m.StartRealityTime.Day;
                                                            var details = planDetails.FindAll(x => x.SkylightPlanId == m.SkylightPlanId);
                                                            details.ForEach(m =>
                                                            {
                                                                finishCount += m.WorkCount;
                                                            });
                                                            switch (day)

                                                            {
                                                                //完成数据
                                                                case 1:
                                                                    resList[0].FinishCount += finishCount; break;
                                                                case 2:
                                                                    resList[1].FinishCount += finishCount; break;
                                                                case 3:
                                                                    resList[2].FinishCount += finishCount; break;
                                                                case 4:
                                                                    resList[3].FinishCount += finishCount; break;

                                                                case 5:
                                                                    resList[4].FinishCount += finishCount; break;
                                                                case 6:
                                                                    resList[5].FinishCount += finishCount; break;
                                                                case 7:
                                                                    resList[6].FinishCount += finishCount; break;
                                                                case 8:
                                                                    resList[7].FinishCount += finishCount; break;
                                                                case 9:
                                                                    resList[8].FinishCount += finishCount; break;
                                                                case 10:
                                                                    resList[9].FinishCount += finishCount; break;
                                                                case 11:
                                                                    resList[10].FinishCount += finishCount; break;
                                                                case 12:
                                                                    resList[11].FinishCount += finishCount; break;
                                                                case 13:
                                                                    resList[12].FinishCount += finishCount; break;
                                                                case 14:
                                                                    resList[13].FinishCount += finishCount; break;
                                                                case 15:
                                                                    resList[14].FinishCount += finishCount; break;
                                                                case 16:
                                                                    resList[15].FinishCount += finishCount; break;
                                                                case 17:
                                                                    resList[16].FinishCount += finishCount; break;
                                                                case 18:
                                                                    resList[17].FinishCount += finishCount; break;
                                                                case 19:
                                                                    resList[18].FinishCount += finishCount; break;
                                                                case 20:
                                                                    resList[19].FinishCount += finishCount; break;
                                                                case 21:
                                                                    resList[20].FinishCount += finishCount; break;
                                                                case 22:
                                                                    resList[21].FinishCount += finishCount; break;
                                                                case 23:
                                                                    resList[22].FinishCount += finishCount; break;
                                                                case 24:
                                                                    resList[23].FinishCount += finishCount; break;
                                                                case 25:
                                                                    resList[24].FinishCount += finishCount; break;
                                                                case 26:
                                                                    resList[25].FinishCount += finishCount; break;
                                                                case 27:
                                                                    resList[26].FinishCount += finishCount; break;
                                                                case 28:
                                                                    resList[27].FinishCount += finishCount; break;
                                                                case 29:
                                                                    resList[28].FinishCount += finishCount; break;
                                                                case 30:
                                                                    resList[29].FinishCount += finishCount; break;
                                                                case 31:
                                                                    resList[30].FinishCount += finishCount; break;
                                                            }

                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    });

                                }
                            }
                            #endregion

                        });


                    }
                });
                return resList;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 根据查询条件获取月计划完成率列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_PlanCompletion)]
        public async Task<List<MonthCompletionRatesDto>> GetMonthCompletionRates(MonthFinishSearchInputDto input)
        {
            if (input.PlanTime.Year < 2000 && input.PlanTime.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            if (input == null || input.OrganizationId == Guid.Empty) return null;
            var organization = await _organizationRepository.Where(m => m.Id == input.OrganizationId);
            if (organization.Count() == 0) return null;

            var isDuanOrganization = false;
            var singleOrganization = organization.FirstOrDefault();
            if (singleOrganization?.Type?.Key == "OrganizationType.Duan")
            {
                isDuanOrganization = true;
            }
            var organizationIdList = new List<Guid>();
            var organizationList = new List<Organization>();
            if (organization.First().Code.Length < 5)
            {
                //获取用户所在组织机构下的所有车间
                organizationList = await GetWorkShopByUserOrgId(input.OrganizationId);
                if (organizationList == null || organizationList.Count == 0) return null;
                organizationIdList = organizationList.ConvertAll(m => m.Id);
            }
            else
            {
                organizationList.Add(organization.First());
                organizationIdList = new List<Guid>();
                organizationIdList.Add(input.OrganizationId);
            }
            List<MonthCompletionRatesDto> list = new List<MonthCompletionRatesDto>();
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(() =>
                {
                    var planType = Enumer.YearMonthPlanType.月表;
                    if (input.MonthPlanType == Enumer.YearMonthPlanStatisticalType.年表)
                    {
                        planType = Enumer.YearMonthPlanType.年度月表;
                    }
                    //获取月计划
                    //新增需求：段统计
                    var planList = new List<YearMonthPlan>();
                    if (isDuanOrganization)
                    {
                        planList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId &&
                           z.Year == input.PlanTime.Year &&
                           z.PlanType == planType.GetHashCode() && z.Month == input.PlanTime.Month).OrderBy(z => z.Number.Replace("-", "")).ToList();
                    }
                    else
                    {
                        planList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId &&
                          z.Year == input.PlanTime.Year && (organizationIdList.Contains(z.ResponsibleUnit)) &&
                          z.PlanType == planType.GetHashCode() && z.Month == input.PlanTime.Month).OrderBy(z => z.Number.Replace("-", "")).ToList();
                    }

                    if (planList?.Count > 0)
                    {
                        var ids = planList.ConvertAll(x => x.Id);
                        //获取日任务列表
                        var dailyPlanList = _dailyPlanRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.PlanId)).ToList();
                        ids = dailyPlanList.ConvertAll(x => x.Id);
                        //获取计划详细信息列表
                        var planDailyList = new List<PlanDetail>();
                        planDailyList = _planDetailRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.DailyPlanId)).ToList();
                        //获取变更列表
                        var planAlterList = _dailyPlanAlterRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.DailyId)).ToList();

                        planList.ForEach(m =>
                        {
                            var bf = list.Find(x => x.Number == ConvertNumber(m.Number));
                            if (bf == null)
                            {
                                var plans = planList.FindAll(x => x.Number == m.Number);
                                if (plans?.Count > 0)
                                {
                                    var bfIds = plans.ConvertAll(m => m.Id);
                                    var dailyPlans = dailyPlanList.FindAll(x => bfIds.Contains(x.PlanId));
                                    MonthCompletionRatesDto dto = new MonthCompletionRatesDto();
                                    dto.Number = ConvertNumber(m.Number);
                                    //dto.Number = m.Number;
                                    dto.DeviceName = m.DeviceName;
                                    dto.RepairContent = m.RepairContent;
                                    dto.Unit = m.Unit;
                                    int k = 0;
                                    string equipmentLocation = string.Empty;
                                    dto.DetailList = new List<MonthStatisticalDto>();

                                    plans.ForEach(y =>
                                    {
                                        //详细数据
                                        MonthStatisticalDto monthDto = new MonthStatisticalDto();
                                        if (y.ResponsibleUnit != null)
                                        {
                                            var workUnit = organizationList.Find(x => x.Id == y.ResponsibleUnit);
                                            if (workUnit != null)
                                            {
                                                monthDto.OrganizationName = workUnit.Name;
                                            }
                                        }
                                        //设备处所
                                        var nameList = new List<string>();
                                        if (k == 0 && !string.IsNullOrEmpty(y.EquipmentLocation))
                                        {
                                            equipmentLocation = y.EquipmentLocation;
                                            nameList.Add(y.EquipmentLocation);
                                        }
                                        else
                                        {
                                            if (nameList.Find(x => x == y.EquipmentLocation && !string.IsNullOrEmpty(y.EquipmentLocation)) == null)
                                            {
                                                equipmentLocation += "," + y.EquipmentLocation;
                                                nameList.Add(y.EquipmentLocation);
                                            }
                                        }
                                        int days = DateTime.DaysInMonth(y.Year, y.Month);
                                        for (int i = 1; i <= days; i++)
                                        {
                                            dto.PlanCount += GetColVal(y, i);
                                        }

                                        //获取完成数量
                                        decimal finishTotal = 0;
                                        decimal alterTotal = 0;
                                        for (var i = 1; i <= days; i++)
                                        {
                                            if (dailyPlans?.Count > 0)
                                            {
                                                var plan = dailyPlans.FindAll(x => x.PlanDate.Day == i && x.PlanId == y.Id);
                                                var planIds = plan.ConvertAll(m => m.Id);
                                                if (plan != null)
                                                {
                                                    if (planDailyList?.Count > 0)
                                                    {
                                                        var dailyList = planDailyList.FindAll(x => planIds.Contains(x.DailyPlanId));
                                                        if (dailyList?.Count > 0)
                                                        {
                                                            dailyList.ForEach(m =>
                                                            {
                                                                finishTotal += m.WorkCount;
                                                            });
                                                        }
                                                    }
                                                    if (planAlterList?.Count > 0)
                                                    {
                                                        var alterList = planAlterList.FindAll(x => planIds.Contains(x.DailyId));
                                                        if (alterList?.Count > 0)
                                                        {
                                                            alterList.ForEach(m =>
                                                            {
                                                                alterTotal += m.AlterCount;
                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //总体+=统计数据
                                        dto.ChangeCount += alterTotal;
                                        if (dto.EquipmentLocation != null)
                                        {
                                            dto?.EquipmentLocation.TrimStart(',');
                                        }


                                        //var alterAfterTotal = y.Total - alterTotal;

                                        ////res.TotalFinishInfo.TotalCount = totalCount;//totalCount;
                                        //if (finishTotal >= alterAfterTotal)
                                        //{
                                        //    dto.FinishCount = alterAfterTotal;
                                        //}
                                        //else
                                        //{
                                        //    dto.FinishCount = finishTotal;
                                        //}

                                        dto.FinishCount += finishTotal;// - alterTotal;

                                        if (dto.FinishCount > dto.PlanCount)
                                        {
                                            dto.FinishCount = dto.PlanCount;
                                        }
                                        //详细信息=统计数据
                                        monthDto.FinishCount = finishTotal;
                                        monthDto.ChangeCount = alterTotal;
                                        monthDto.UnFinishedCount = y.Total - finishTotal - alterTotal;

                                        if (finishTotal != y.Total)
                                        {
                                            dto.DetailList.Add(monthDto);
                                        }
                                        k++;
                                    });
                                    dto.EquipmentLocation = equipmentLocation;
                                    dto.Gap = dto.PlanCount - dto.FinishCount;
                                    dto.UnFinishedCount = dto.Gap - dto.ChangeCount;

                                    if (dto.UnFinishedCount < 0)
                                    {
                                        dto.UnFinishedCount = 0;
                                    }


                                    if (dto.PlanCount > 0 && dto.FinishCount > 0)
                                    {
                                        dto.Percentage = Math.Round(dto.FinishCount / dto.PlanCount, 4, MidpointRounding.AwayFromZero) * 100;
                                    }
                                    else
                                    {
                                        dto.Percentage = 0;
                                    }
                                    if (dto.PlanCount > 0)
                                    {
                                        list.Add(dto);
                                    }
                                }
                            }

                        });
                    }

                });
                return list;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 根据查询条件获取年完成率列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.CrPlan_PlanCompletion)]
        public async Task<List<YearCompletionRatesDto>> GetYearCompletionRates(YearFinishSearchInputDto input)
        {
            if (input.PlanTime.Year < 2000 && input.PlanTime.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            if (input == null || input.OrganizationId == Guid.Empty) return null;
            var organization = await _organizationRepository.Where(m => m.Id == input.OrganizationId);
            if (organization.Count() == 0) return null;
            var organizationIdList = new List<Guid>();
            var organizationIdChildrenList = new List<Guid>();

            var isDuanOrganization = false;
            var singleOrganization = organization.FirstOrDefault();
            if (singleOrganization?.Type?.Key == "OrganizationType.Duan")
            {
                isDuanOrganization = true;
                organizationIdChildrenList = singleOrganization?.Children.Select(x => x.Id).ToList();
            }

            if (organization.First().Code.Length < 5)
            {
                //获取用户所在组织机构下的所有车间
                var organizationList = await GetWorkShopByUserOrgId(input.OrganizationId);
                if (organizationList == null || organizationList.Count == 0) return null;
                organizationIdList = organizationList.ConvertAll(m => m.Id);
            }
            else
            {
                organizationIdList = new List<Guid>();
                organizationIdList.Add(input.OrganizationId);
            }
            List<YearCompletionRatesDto> list = new List<YearCompletionRatesDto>();
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                await Task.Run(() =>
                    {
                        int month = input.PlanTime.Month;
                        if (input.RepairlType == Enumer.StatisticalRepairType.All)
                        {

                        }

                        var planList = new List<YearMonthPlan>();
                        if (isDuanOrganization)
                        {
                            planList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId &&
                           z.Year == input.PlanTime.Year && (input.RepairlType != Enumer.StatisticalRepairType.All ? z.RepairType == input.RepairlType.GetHashCode() : z.RepairType > 0) && z.PlanType == Enumer.YearMonthPlanType.年表.GetHashCode() && organizationIdChildrenList.Contains(z.ResponsibleUnit) &&
                           (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.RepairGroup.Contains(input.KeyWord))
                           ).OrderBy(z => z.Number.Replace("-", "")).ToList();
                        }
                        else
                        {
                            planList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId &&
                             z.Year == input.PlanTime.Year && (input.RepairlType != Enumer.StatisticalRepairType.All ? z.RepairType == input.RepairlType.GetHashCode() : z.RepairType > 0) &&
                             (organizationIdList.Contains(z.ResponsibleUnit)) && z.PlanType == Enumer.YearMonthPlanType.年表.GetHashCode() &&
                             (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.RepairGroup.Contains(input.KeyWord))
                             ).OrderBy(z => z.Number.Replace("-", "")).ToList();
                        }

                        if (planList?.Count > 0)
                        {
                            var ids = planList.ConvertAll(x => x.Id);
                            var childList = _yearMonthPlan.Where(z => z.RepairTagId == RepairTagId && z.ParentId.HasValue && ids.Contains(z.ParentId.Value) && z.PlanType == Enumer.YearMonthPlanType.年度月表.GetHashCode()).ToList();
                            if (childList?.Count > 0)
                            {
                                ids = childList.ConvertAll(x => x.Id);
                            }
                            //获取日任务列表
                            var dailyPlanList = _dailyPlanRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.PlanId) && z.PlanDate.Year == input.PlanTime.Year && z.PlanDate.Month <= month).ToList();
                            ids = dailyPlanList.ConvertAll(x => x.Id);
                            //获取计划详细信息列表
                            var planDailyList = new List<PlanDetail>();
                            planDailyList = _planDetailRepository.Where(z => z.RepairTagId == RepairTagId && ids.Contains(z.DailyPlanId)).ToList();

                            //planList = planList.GroupBy(x => x.Number).Select(x => x.First()).ToList();

                            planList.ForEach(m =>
                                {
                                    var bf = list.Find(x => x.Number == ConvertNumber(m.Number));
                                    if (bf == null)
                                    {
                                        var plans = planList.FindAll(x => x.Number == m.Number);
                                        if (plans?.Count > 0)
                                        {
                                            YearCompletionRatesDto dto = new YearCompletionRatesDto();
                                            dto.DeviceName = m.DeviceName;
                                            dto.Number = ConvertNumber(m.Number);
                                            //dto.Number = m.Number;
                                            dto.RepairContent = m.RepairContent;
                                            dto.RepairGroup = m.RepairGroup;
                                            dto.RepairType = (RepairType)Enum.ToObject(typeof(RepairType), m.RepairType);
                                            dto.Times = m.Times;
                                            dto.Unit = m.Unit;
                                            plans.ForEach(y =>
                                            {
                                                decimal realPlanCount = 0;
                                                for (int i = 1; i < 13; i++)
                                                {
                                                    realPlanCount += GetColVal(y, i);
                                                }
                                                dto.PlanCount += realPlanCount;

                                                decimal total = 0;
                                                decimal monthCount = 0;

                                                var childs = childList.FindAll(x => x.ParentId == y.Id);
                                                var childIds = childs.ConvertAll(x => x.Id);
                                                var totalPlanList = dailyPlanList.FindAll(x => childIds.Contains(x.PlanId));
                                                if (totalPlanList?.Count > 0)
                                                {
                                                    //累计日任务
                                                    var totalIds = totalPlanList.ConvertAll(x => x.Id);
                                                    var totalplanDailyList = planDailyList.FindAll(z => totalIds.Contains(z.DailyPlanId));

                                                    //本月日任务
                                                    var monthPlanList = totalPlanList.FindAll(x => x.PlanDate.Month == month);
                                                    var monthIds = monthPlanList.ConvertAll(x => x.Id);
                                                    var monthplanDailyList = planDailyList.FindAll(z => monthIds.Contains(z.DailyPlanId));
                                                    if (totalplanDailyList?.Count > 0)
                                                    {
                                                        totalplanDailyList.ForEach(x =>
                                                        {
                                                            if (monthplanDailyList?.Count > 0)
                                                            {
                                                                var daily = monthplanDailyList.Find(k => k.Id == x.Id);
                                                                if (daily != null)
                                                                {
                                                                    monthCount += daily.WorkCount;
                                                                }
                                                                //查询是否存在变更计划
                                                                //var alterPlans = _dailyPlanAlterRepository.FirstOrDefault(y => y.DailyId == x.DailyPlanId);
                                                                //if (alterPlans != null)
                                                                //{
                                                                //    monthCount -= alterPlans.AlterCount;
                                                                //}
                                                            }
                                                            total += x.WorkCount;
                                                        });

                                                    }
                                                }
                                                dto.CumulativeFinishedCount += total;
                                                dto.MonthFinishedCount += monthCount;
                                            });
                                            if (dto.PlanCount > 0)
                                            {
                                                if (dto.CumulativeFinishedCount > 0)
                                                {
                                                    dto.CumulativeFinishedPercentage = Math.Round(dto.CumulativeFinishedCount / dto.PlanCount, 4, MidpointRounding.AwayFromZero) * 100;
                                                }
                                                if (dto.MonthFinishedCount > 0)
                                                {
                                                    dto.MonthFinishedPercentage = Math.Round(dto.MonthFinishedCount / dto.PlanCount, 4, MidpointRounding.AwayFromZero) * 100;
                                                }
                                                list.Add(dto);
                                            }
                                            else
                                            {
                                                dto.CumulativeFinishedPercentage = 0;
                                                dto.MonthFinishedPercentage = 0;

                                            }
                                        }
                                    }


                                });


                        }
                    });

                return list;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        private static string ConvertNumber(string number)
        {
            var nums = number.Split('-');
            string newNum = "";
            foreach (var num in nums)
            {
                newNum += int.Parse(num).ToString() + "-";
            }
            newNum = newNum.TrimEnd('-');
            return newNum;
        }

        #region 私有方法
        /// <summary>
        /// 根据用户组织机构获取所有车间
        /// </summary>
        /// <param name="id">车间或段组织机构ID</param>
        /// <returns></returns>
        private async Task<List<Organization>> GetWorkShopByUserOrgId(Guid id)
        {
            var userOrg = (await _organizationRepository.Where(x => x.Id == id)).FirstOrDefault();
            if (userOrg == null || string.IsNullOrEmpty(userOrg.Code)) return null;
            try
            {
                //获取用户所在组织机构下的所有车间
                string topOrgCode = userOrg.Code.Substring(0, 4);
                var orgList = (await _organizationRepository.Where(x => x.Code.Length == 8 && x.Code.Substring(0, 4) == topOrgCode && x.Code != topOrgCode)).ToList();
                if (orgList == null || orgList.Count == 0) return null;
                return orgList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        private decimal GetColVal<T>(T model, int col)
        {
            var result = (string)null;
            PropertyInfo proInfo = model.GetType().GetProperty("Col_" + col);
            if (proInfo != null)
            {
                result = proInfo.GetValue(model, null).ToString();
            }
            else
            {
                return 0;
            }
            var val = (decimal)0;
            if (decimal.TryParse(result, out val)) return val;
            else throw new UserFriendlyException("数据获取失败");
        }

        /// <summary>
        /// 月完成情况统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Produces("application/octet-stream")]
        public async Task<Stream> ExportMonthCompletion(MonthFinishSearchInputDto input)
        {
            var mothPlanList = await GetMonthCompletion(input);//根据查询提交筛得到list

            if (mothPlanList?.Count <= 0 || mothPlanList == null)
            {
                return null;
            }

            var dataTableName = "";
            var organization = await _organizationRepository.Where(m => m.Id == input.OrganizationId);

            dataTableName = organization.FirstOrDefault()?.Name + input.PlanTime.ToString("yyyy年MM月") + "-月表完成情况统计表.xlsx";

            Stream st = null;
            byte[] subf;
            var row = (DataRow)null;
            var dataTable = new DataTable();

            //添加表头
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.序号.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.设备处所.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.设备名称.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.工作内容.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.单位.ToString()));
            dataTable.Columns.Add("计划数量");
            dataTable.Columns.Add("计划");
            dataTable.Columns.Add("合计");
            for (int i = 1; i <= 31; i++)
            {
                dataTable.Columns.Add(i.ToString());
            }

            var newRow = new MonthCompletionDto();

            newRow.DataType = 1;
            mothPlanList.AddFirst(newRow);

            //添加数据
            foreach (var mothPlan in mothPlanList)
            {
                //var samePlan = totalPlanList.Where(x => x.Number == mothPlan.Number.Replace("0", "")).FirstOrDefault();
                row = dataTable.NewRow();
                row[YearPlanExcelCol.序号.ToString()] = mothPlan.Number ?? "";
                row[YearPlanExcelCol.设备处所.ToString()] = mothPlan.EquipmentLocation ?? "";
                row[YearPlanExcelCol.设备名称.ToString()] = mothPlan.DeviceName ?? "";
                row[YearPlanExcelCol.工作内容.ToString()] = mothPlan.RepairContent ?? "";
                row[YearPlanExcelCol.单位.ToString()] = mothPlan.Unit ?? "";
                row["计划数量"] = mothPlan.PlanCount;
                row["计划"] = $"{(mothPlan.DataType == 1 ? "完成" : "计划")}";
                row["合计"] = mothPlan.Total;

                for (int i = 1; i <= 31; i++)
                {
                    row[i.ToString()] = GetColVal(mothPlan, i);
                }
                dataTable.Rows.Add(row);
            }
            subf = Commons.ExcelHepler.DataTableToExcel(dataTable, dataTableName, null, true); ;
            st = new MemoryStream(subf);
            return st;
        }

        public async Task<Stream> ExprortMonthCompletionRates(MonthFinishSearchInputDto input)
        {
            var mothRatesList = await GetMonthCompletionRates(input);
            Stream stream = null;

            if (mothRatesList?.Count <= 0 || mothRatesList == null)
            {
                return stream;
            }
            //创建dataTable
            var dataTable = new DataTable();
            byte[] subf;
            //创建表头
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.序号.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.设备处所.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.设备名称.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.工作内容.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.单位.ToString()));
            dataTable.Columns.Add("计划数量");
            dataTable.Columns.Add("完成数量");
            dataTable.Columns.Add("完成率");
            dataTable.Columns.Add("差额");
            dataTable.Columns.Add("变更");
            dataTable.Columns.Add("未完成");

            //添加数据
            foreach (var mothPlan in mothRatesList)
            {
                var row = dataTable.NewRow();
                row[YearPlanExcelCol.序号.ToString()] = mothPlan.Number ?? "";
                row[YearPlanExcelCol.设备处所.ToString()] = mothPlan.EquipmentLocation ?? "";
                row[YearPlanExcelCol.设备名称.ToString()] = mothPlan.DeviceName ?? "";
                row[YearPlanExcelCol.工作内容.ToString()] = mothPlan.RepairContent ?? "";
                row[YearPlanExcelCol.单位.ToString()] = mothPlan.Unit ?? "";
                row["计划数量"] = mothPlan.PlanCount;
                row["完成数量"] = mothPlan.FinishCount;
                row["完成率"] = mothPlan.Percentage + "%";
                row["差额"] = mothPlan.Gap;
                row["变更"] = mothPlan.ChangeCount;
                row["未完成"] = mothPlan.UnFinishedCount;
                dataTable.Rows.Add(row);
            }

            subf = Commons.ExcelHepler.DataTableToExcel(dataTable, "月表完成统计率.xlsx"); ;
            var st = new MemoryStream(subf);
            return st;
        }

        public async Task<Stream> ExprortYearCompletionRates(YearFinishSearchInputDto input)
        {
            var mothRatesList = await GetYearCompletionRates(input);
            Stream stream = null;

            if (mothRatesList?.Count <= 0 || mothRatesList == null)
            {
                return stream;
            }
            //创建dataTable
            var dataTable = new DataTable();
            byte[] subf;
            //创建表头
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.序号.ToString()));
            dataTable.Columns.Add("设备类型");
            dataTable.Columns.Add("类型");
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.设备名称.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.工作内容.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.单位.ToString()));
            dataTable.Columns.Add("每年次数");
            dataTable.Columns.Add("计划数量");
            dataTable.Columns.Add("累计完成数量");
            dataTable.Columns.Add("月完成数量");
            dataTable.Columns.Add("月完成率");
            dataTable.Columns.Add("累计完成率");

            //添加数据
            foreach (var mothPlan in mothRatesList)
            {
                var row = dataTable.NewRow();
                row[YearPlanExcelCol.序号.ToString()] = mothPlan.Number ?? "";
                row["设备类型"] = mothPlan.RepairGroup ?? "";
                row["类型"] = mothPlan.RepairType.GetDescription() ?? "";
                row[YearPlanExcelCol.设备名称.ToString()] = mothPlan.DeviceName ?? "";
                row[YearPlanExcelCol.工作内容.ToString()] = mothPlan.RepairContent ?? "";
                row[YearPlanExcelCol.单位.ToString()] = mothPlan.Unit ?? "";
                row["每年次数"] = mothPlan.Times;
                row["计划数量"] = mothPlan.PlanCount;
                row["累计完成数量"] = mothPlan.CumulativeFinishedCount;
                row["月完成数量"] = mothPlan.MonthFinishedCount;
                row["月完成率"] = mothPlan.MonthFinishedPercentage + "%";
                row["累计完成率"] = mothPlan.CumulativeFinishedPercentage + "%";
                dataTable.Rows.Add(row);
            }

            subf = Commons.ExcelHepler.DataTableToExcel(dataTable, "年表完成统计率.xlsx"); ;
            var st = new MemoryStream(subf);
            return st;
        }

        #endregion
    }
}
