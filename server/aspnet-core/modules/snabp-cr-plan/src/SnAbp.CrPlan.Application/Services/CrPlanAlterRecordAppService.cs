using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SnAbp.Bpm;
using SnAbp.Bpm.Services;
using SnAbp.CrPlan.Authorization;
using SnAbp.CrPlan.Dto.AlterRecord;
using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Enumer;
using SnAbp.CrPlan.Enums;
using SnAbp.CrPlan.IServices.AlterRecord;
using SnAbp.Identity;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Utils.EnumHelper;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SnAbp.File.Services;
using SnAbp.CrPlan.Dtos;
using SnAbp.Bpm.Entities;

namespace SnAbp.CrPlan.Services
{
    //[Authorize]
    public class CrPlanAlterRecordAppService : CrPlanAppService, ICrPlanAlterRecordAppService
    {
        private readonly IRepository<AlterRecord, Guid> _alterRecordRepository;          //任务变更
        private readonly IRepository<DailyPlanAlter, Guid> _dailyPlan_AlterRepository;   //日任务 变更 关联
        private readonly OrganizationManager _orgRepository;
        private readonly IRepository<Organization, Guid> _organiztionRepository;

        private readonly IRepository<DailyPlan, Guid> _dailyPlansRepository;             //日任务
        private readonly IRepository<YearMonthPlan, Guid> _yearMonthPlansRepository;     //年月计划表
        private readonly IRepository<PlanDetail, Guid> _planDetailsRepository;           //实际计划 日计划 关系 计划详细信息
        private readonly IRepository<SkylightPlan, Guid> _skylightPlansRepository;       //实际天窗计划

        private readonly IRepository<RepairItemRltComponentCategory, Guid> _repairDetailsIFDRepository;
        private readonly IRepository<ComponentCategory, Guid> _componentCategoryRepository;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private readonly IRepository<RepairItemRltOrganizationType, Guid> _repairItemsRltOrgs;
        private readonly IRepository<WorkflowData, Guid> workflowDataRepository;
        private readonly BpmManager _bpmManager;
        private readonly CrPlanManager _crPlanManager;

        public CrPlanAlterRecordAppService(
            IRepository<AlterRecord, Guid> alterRecordRep,
            IRepository<DailyPlanAlter, Guid> dailyPlanAlterRep,
            OrganizationManager orgRep,
            IRepository<Organization, Guid> organiztionRepository,
            IRepository<DailyPlan, Guid> dailyPlansRep,
            IRepository<YearMonthPlan, Guid> yearMonthPlansRep,
            IRepository<PlanDetail, Guid> planDetailsRep,
            IRepository<SkylightPlan, Guid> skylightPlansRep,
            IRepository<RepairItemRltComponentCategory, Guid> repairDetailsIFDRep,
            IRepository<ComponentCategory, Guid> componentCategoryRepository,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IRepository<RepairItemRltOrganizationType, Guid> repairItemsRltOrgs,
            IRepository<WorkflowData, Guid> workflowDataRepository,
            BpmManager bpmManager,
            CrPlanManager crPlanManager
            )
        {
            _alterRecordRepository = alterRecordRep;
            _dailyPlan_AlterRepository = dailyPlanAlterRep;
            _orgRepository = orgRep;
            _organiztionRepository = organiztionRepository;
            _dailyPlansRepository = dailyPlansRep;
            _yearMonthPlansRepository = yearMonthPlansRep;
            _planDetailsRepository = planDetailsRep;
            _skylightPlansRepository = skylightPlansRep;
            _repairDetailsIFDRepository = repairDetailsIFDRep;
            _componentCategoryRepository = componentCategoryRepository;
            _dataDictionaries = dataDictionaries;
            _repairItemsRltOrgs = repairItemsRltOrgs;
            this.workflowDataRepository = workflowDataRepository;
            _bpmManager = bpmManager;
            _crPlanManager = crPlanManager;
        }

        public async Task<AlterRecordDetailDto> Get(CommonGuidGetDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("id不正确");
            AlterRecordDetailDto res = new AlterRecordDetailDto();
            try
            {
                await Task.Run(async () =>
                {
                    var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                    var ent = _alterRecordRepository.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == input.Id);
                    if (ent == null) throw new UserFriendlyException("对象不存在");
                    res = ObjectMapper.Map<AlterRecord, AlterRecordDetailDto>(ent);
                    res = (await SetDetailInfo(new List<AlterRecordDetailDto>() { res }, ent))[0];
                });
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return res;
        }

        public async Task<PagedResultDto<AlterRecordSimpleDto>> GetList(AlterRecordSearchDto input)
        {
            PagedResultDto<AlterRecordSimpleDto> res = new PagedResultDto<AlterRecordSimpleDto>();
            var isDuanOrganization = false;
            var organization = _organiztionRepository.WithDetails(x => x.Type).FirstOrDefault(x => x.Id == input.OrganizationId);
            if (organization?.Type?.Key == "OrganizationType.Duan")
            {
                isDuanOrganization = true;
            }
            try
            {
                await Task.Run(async () =>
                {
                    var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                    var ents = new List<AlterRecord>();
                    if (isDuanOrganization)
                    {
                        ents = _alterRecordRepository.Where(s => s.RepairTagId == RepairTagId &&
                          (input.AlterType == null || s.AlterType == input.AlterType) &&
                          (input.State == null || s.State == input.State) &&
                          //(input.OrganizationId == null || input.OrganizationId == Guid.Empty || s.OrganizationId == input.OrganizationId) &&
                          (s.PlanTime <= input.PlanEndTime && s.PlanTime >= input.PlanStartTime || s.AlterTime >= input.AlterStartTime && s.AlterTime <= input.AlterEndTime) &&
                          (string.IsNullOrEmpty(input.Keyword) || s.Reason.Contains(input.Keyword))
                          ).ToList().OrderBy(s => s.State).ThenByDescending(s => s.PlanTime).ToList();
                        res.TotalCount = ents.Count;
                    }
                    else
                    {
                        ents = _alterRecordRepository.Where(s => s.RepairTagId == RepairTagId &&
                         (input.AlterType == null || s.AlterType == input.AlterType) &&
                         (input.State == null || s.State == input.State) &&
                         (input.OrganizationId == null || input.OrganizationId == Guid.Empty || s.OrganizationId == input.OrganizationId) &&
                          (s.PlanTime <= input.PlanEndTime && s.PlanTime >= input.PlanStartTime || s.AlterTime >= input.AlterStartTime && s.AlterTime <= input.AlterEndTime) &&
                         (string.IsNullOrEmpty(input.Keyword) || s.Reason.Contains(input.Keyword))
                         ).ToList().OrderBy(s => s.State).ThenByDescending(s => s.PlanTime).ToList();
                        res.TotalCount = ents.Count;
                    }
                    if (!input.IsAll)
                    {
                        ents = ents.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                    }
                    List<AlterRecordSimpleDto> dtos = ObjectMapper.Map<List<AlterRecord>, List<AlterRecordSimpleDto>>(ents);
                    dtos = SetSimpleInfo(dtos);
                    res.Items = dtos;
                });
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return res;
        }

        /// <summary>
        /// 获取待选计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DailyPlanSelectableDto>> GetSelectablePlans(SelectablePlansSearchDto input)
        {
            PagedResultDto<DailyPlanSelectableDto> result = new PagedResultDto<DailyPlanSelectableDto>();
            List<DailyPlanSelectableDto> res = new List<DailyPlanSelectableDto>();
            if (string.IsNullOrEmpty(input.OrgId.ToString()) || Guid.Empty == input.OrgId) throw new UserFriendlyException("负责单位未选择");
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(() =>
            {
                List<DailyPlan> ents = new List<DailyPlan>();
                IQueryable<YearMonthPlan> yearMonthPlans = null;// = new List<YearMonthPlan>();
                string[] planType = GetConvertEnumStr(input.SkylightType);
                //计划类型 年表、半年表、季度表筛选
                bool? isMonthType = null;
                if (input.Type != null)
                {
                    //月表判断
                    isMonthType = input.Type == SelectablePlanType.Month;
                    if ((bool)isMonthType)
                    {
                        yearMonthPlans = _yearMonthPlansRepository.Where(s =>
                                                                         s.RepairTagId == RepairTagId &&
                                                                         s.ResponsibleUnit == input.OrgId &&
                                                                         ((bool)isMonthType ? s.PlanType == 2 : (s.PlanType == 1 || s.PlanType == 3)) &&
                                                                         (string.IsNullOrEmpty(input.Keyword) ||
                                                                            s.DeviceName.Contains(input.Keyword) ||
                                                                            s.RepairContent.Contains(input.Keyword)
                                                                         ));
                    }
                    //年表、半年表、季度表判断  
                    else if (input.SkylightType == null)
                    {
                        switch (input.Type)
                        {
                            case SelectablePlanType.Year:
                                if (input.RepairTagKey == "RepairTag.RailwayWired")
                                {
                                    yearMonthPlans = _yearMonthPlansRepository.Where(s =>
                                                                                 s.RepairTagId == RepairTagId &&
                                                                                 s.ResponsibleUnit == input.OrgId &&
                                                                                 (s.PlanType == 1 || s.PlanType == 3) &&
                                                                                 s.Times.Trim() != "2" && s.Times.Trim() != "4" &&
                                                                                (string.IsNullOrEmpty(input.Keyword) ||
                                                                                    s.DeviceName.Contains(input.Keyword) ||
                                                                                    s.RepairContent.Contains(input.Keyword)
                                                                                 ));
                                }
                                else
                                {
                                    yearMonthPlans = _yearMonthPlansRepository.Where(s =>
                                                                                s.RepairTagId == RepairTagId &&
                                                                                s.ResponsibleUnit == input.OrgId &&
                                                                                (s.PlanType == 1 || s.PlanType == 3) &&
                                                                               (string.IsNullOrEmpty(input.Keyword) ||
                                                                                   s.DeviceName.Contains(input.Keyword) ||
                                                                                   s.RepairContent.Contains(input.Keyword)
                                                                                ));
                                }

                                break;
                            case SelectablePlanType.HalfYaer:
                                yearMonthPlans = _yearMonthPlansRepository.Where(s =>
                                                                                 s.RepairTagId == RepairTagId &&
                                                                                 s.ResponsibleUnit == input.OrgId &&
                                                                                 (s.PlanType == 1 || s.PlanType == 3) &&
                                                                                 s.Times.Trim() == "2" &&
                                                                                 (string.IsNullOrEmpty(input.Keyword) ||
                                                                                    s.DeviceName.Contains(input.Keyword) ||
                                                                                    s.RepairContent.Contains(input.Keyword)
                                                                                  ));
                                break;
                            case SelectablePlanType.QuarterYear:
                                yearMonthPlans = _yearMonthPlansRepository.Where(s =>
                                                                                 s.RepairTagId == RepairTagId &&
                                                                                 s.ResponsibleUnit == input.OrgId &&
                                                                                 (s.PlanType == 1 || s.PlanType == 3) &&
                                                                                 s.Times.Trim() == "4" &&
                                                                                 (string.IsNullOrEmpty(input.Keyword) ||
                                                                                     s.DeviceName.Contains(input.Keyword) ||
                                                                                     s.RepairContent.Contains(input.Keyword)
                                                                                  ));
                                break;
                        }
                    }
                    else
                    {
                        yearMonthPlans = _yearMonthPlansRepository.Where(s =>
                                                                         s.RepairTagId == RepairTagId &&
                                                                         s.ResponsibleUnit == input.OrgId &&
                                                                         (s.PlanType == 1 || s.PlanType == 3) &&
                                                                         (string.IsNullOrEmpty(input.Keyword) ||
                                                                            s.DeviceName.Contains(input.Keyword) ||
                                                                            s.RepairContent.Contains(input.Keyword)
                                                                          ));
                    }
                }
                else
                {
                    yearMonthPlans = _yearMonthPlansRepository.Where(s =>
                                                                     s.RepairTagId == RepairTagId &&
                                                                     s.ResponsibleUnit == input.OrgId &&
                                                                     (string.IsNullOrEmpty(input.Keyword) ||
                                                                        s.DeviceName.Contains(input.Keyword) ||
                                                                        s.RepairContent.Contains(input.Keyword)
                                                                      ));
                }

                var dailyPlanEnts = (from yearMonthPlan in
                                yearMonthPlans.Where(s =>
                                (input.SkylightType == null ||
                                (planType.Length == 1 && s.SkyligetType != null && s.SkyligetType.Contains(planType[0])) ||
                                (planType.Length == 2 && s.SkyligetType != null && (s.SkyligetType.Contains(planType[0]) || s.SkyligetType.Contains(planType[1]))))
                                )
                                     join dailyPlan in _dailyPlansRepository.Where(s =>
                                                                                   s.RepairTagId == RepairTagId &&
                                                                                   s.PlanDate >= input.StartTime &&
                                                                                   s.PlanDate <= input.EndTime)
                                     .WhereIf(input.IsChange != 2, s => s.State == input.IsChange)
                                     on yearMonthPlan.Id equals dailyPlan.PlanId
                                     select dailyPlan).ToList();

                //满足要求的日计划的详细信息
                var detialRe = _planDetailsRepository.Where(s => s.RepairTagId == RepairTagId && dailyPlanEnts.Select(d => d.Id).Contains(s.DailyPlanId)).ToList();
                //详细信息天窗信息
                var skylights = _skylightPlansRepository.Where(s => s.RepairTagId == RepairTagId && detialRe.Select(d => d.SkylightPlanId).Contains(s.Id)).ToList();
                //需减去的数量
                Dictionary<Guid, decimal> data = new Dictionary<Guid, decimal>();
                var alterEnts = _alterRecordRepository.Where(s => s.RepairTagId == RepairTagId && s.State != YearMonthPlanState.审核驳回);
                var alterRecords = _dailyPlan_AlterRepository.Where(s => s.RepairTagId == RepairTagId && alterEnts.Select(s => s.Id).Contains(s.AlterRecordId)).ToList();
                foreach (var item in detialRe)
                {
                    //已完成天窗计划数量
                    var skylight = skylights.FirstOrDefault(s => s.Id == item.SkylightPlanId);
                    if (skylight != null)
                    {
                        decimal count = skylight.PlanState == Enums.PlanState.Complete ? item.WorkCount : item.PlanCount;
                        if (data.Keys.Contains(item.DailyPlanId))
                        {
                            data[item.DailyPlanId] += count;
                        }
                        else
                        {
                            data.Add(item.DailyPlanId, count);
                        }
                    }
                }
                foreach (var item in alterRecords)
                {
                    //变更了的计划的数量
                    decimal alterCount = item.AlterCount;
                    if (data.Keys.Contains(item.DailyId))
                    {
                        data[item.DailyId] += alterCount;
                    }
                    else
                    {
                        data.Add(item.DailyId, alterCount);
                    }
                }
                List<DailyPlanSelectableDto> DailyPlans = new List<DailyPlanSelectableDto>();
                var allRepairIFD = _repairDetailsIFDRepository.Where(s => s.Id != null).ToList();
                var allIFD = _componentCategoryRepository.Where(s => s.Id != null && s.Code != null).ToList();
                var tempDailyPlanEnts = dailyPlanEnts.Distinct(new MonthYearCompare());
                var dailyPlanWithYearPlan = (from a in tempDailyPlanEnts
                                             join b in yearMonthPlans on a.PlanId equals b.Id
                                             select new
                                             {
                                                 DailyPlan = a,
                                                 YearMonthPlan = b
                                             }).ToList();
                foreach (var t in dailyPlanWithYearPlan)
                {
                    var item = t.DailyPlan;
                    var plan = t.YearMonthPlan;
                    DailyPlanSelectableDto dto = new DailyPlanSelectableDto();
                    //var plan = yearMonthPlans.FirstOrDefault(p => p.Id == item.PlanId);
                    if (isMonthType != null && (bool)isMonthType)
                    {
                        dto.PlanType = SelectablePlanType.Month;
                    }
                    else
                    {
                        if (plan.PlanType == 2)
                        {
                            dto.PlanType = SelectablePlanType.Month;
                        }
                        else if (plan.PlanType == 1 || plan.PlanType == 3)
                        {
                            var times = plan.Times.Trim();
                            if (times == "2") dto.PlanType = SelectablePlanType.HalfYaer;
                            else if (times == "4") dto.PlanType = SelectablePlanType.QuarterYear;
                            else dto.PlanType = SelectablePlanType.Year;
                        }
                    }
                    //天窗类型筛选 后 半年表、季度表、年表 统称为年表
                    if (input.SkylightType != null)
                    {
                        switch (dto.PlanType)
                        {
                            case SelectablePlanType.HalfYaer:
                            case SelectablePlanType.QuarterYear:
                                dto.PlanType = SelectablePlanType.Year;
                                break;
                        }
                    }
                    dto.PlanId = item.Id;
                    dto.PlanTypeStr = EnumHelper.GetDescription(dto.PlanType);
                    dto.Id = item.Id;
                    dto.IsChange = item.State == 1;
                    if (plan != null)
                    {
                        string newNum = "";
                        var nums = plan.Number.Split("-");
                        foreach (var i in nums)
                        {
                            newNum += int.Parse(i).ToString().PadLeft(3, '0') + "-";
                        }
                        newNum = newNum.TrimEnd('-');
                        dto.Number = newNum;
                        dto.EquipName = plan.DeviceName;
                        dto.Content = plan.RepairContent;
                        dto.Unit = plan.Unit;
                        dto.PlanRepairDetailId = plan.RepairDetailsId;
                    }
                    dto.PlanDate = item.PlanDate;
                    var samePlanDateYearMonthPlan = dailyPlanEnts.Where(p => p.PlanId == item.PlanId && p.PlanDate == item.PlanDate && p.PlanType == item.PlanType).ToList();
                    dto.Count = samePlanDateYearMonthPlan.Sum(s => s.Count);
                    if (data.Where(s => s.Key == dto.PlanId) != null && data.Where(s => s.Key == dto.PlanId).Count() > 0)
                        dto.UnFinishCount = dto.Count - data[dto.PlanId];
                    else dto.UnFinishCount = dto.Count;
                    if (dto.UnFinishCount <= 0) continue;
                    DailyPlans.Add(dto);
                }

                res = DailyPlans.OrderBy(s => s.PlanType).ThenBy(s => s.Number.Replace("-", "")).ThenBy(s => s.PlanDate).ToList();
                result.TotalCount = res.Count;
                result.Items = res.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                foreach (var item in result.Items)
                {
                    var repair_ifds = allRepairIFD.Where(s => s.RepairItemId == item.PlanRepairDetailId);//.ToList();
                    if (repair_ifds != null && repair_ifds.Count() > 0)
                    {
                        var ifdIds = repair_ifds.Select(s => s.ComponentCategoryId);
                        item.IFDCodes = allIFD.Where(s => ifdIds.Contains(s.Id)).Select(s => s.Code).ToList();
                    }
                    var nums = item.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.Number = newNum.TrimEnd('-');
                }
            });
            return result;
        }

        /// <summary>
        /// 获取单条待选计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DailyPlanSelectableDto> GetSelectablePlan(CommonGuidGetDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            return await Task.Run(() =>
            {
                var dailyPlanEnts = _dailyPlansRepository.FirstOrDefault(s => s.Id == input.Id && s.RepairTagId == RepairTagId);
                if (dailyPlanEnts == null) throw new UserFriendlyException("该计划不存在");
                //需减去的数量
                decimal reduceCount = 0;
                //满足要求的日计划的详细信息
                var detialRe = ObjectMapper.Map<PlanDetail, PlanDetailDto>(_planDetailsRepository.FirstOrDefault(s => s.RepairTagId == RepairTagId && dailyPlanEnts.Id == s.DailyPlanId));
                //详细信息天窗信息
                SkylightPlanDto skylight = null;
                if (detialRe != null)
                {
                    skylight = ObjectMapper.Map<SkylightPlan, SkylightPlanDto>(_skylightPlansRepository.FirstOrDefault(s => s.RepairTagId == RepairTagId && detialRe.SkylightPlanId == s.Id));
                    if (skylight != null)
                    {
                        decimal count = skylight.PlanState == Enums.PlanState.Complete ? detialRe.WorkCount : detialRe.PlanCount;
                        reduceCount += count;
                    }
                }
                var alterEnts = _alterRecordRepository.Where(s => s.RepairTagId == RepairTagId && s.State != YearMonthPlanState.审核驳回);
                var alterRecords = _dailyPlan_AlterRepository.Where(s => s.RepairTagId == RepairTagId && alterEnts.Select(s => s.Id).Contains(s.AlterRecordId));
                //变更了的计划的数量
                var alterEnt = alterRecords.Where(s => s.DailyId == input.Id);
                if (alterEnt.Count() > 0)
                {
                    reduceCount += alterEnt.Sum(s => s.AlterCount);
                }
                DailyPlanSelectableDto dto = new DailyPlanSelectableDto();
                var plan = _yearMonthPlansRepository.FirstOrDefault(p => p.RepairTagId == RepairTagId && p.Id == dailyPlanEnts.PlanId);
                if (plan.PlanType == 2)
                {
                    dto.PlanType = SelectablePlanType.Month;
                }
                else if (plan.PlanType == 1 || plan.PlanType == 3)
                {
                    var times = plan.Times.Trim();
                    if (times == "2") dto.PlanType = SelectablePlanType.HalfYaer;
                    else if (times == "4") dto.PlanType = SelectablePlanType.QuarterYear;
                    else dto.PlanType = SelectablePlanType.Year;
                }
                dto.PlanId = dailyPlanEnts.Id;
                dto.PlanTypeStr = EnumHelper.GetDescription(dto.PlanType);
                dto.Id = dailyPlanEnts.Id;
                dto.Number = plan != null ? plan.Number : "0";
                dto.EquipName = plan.DeviceName;
                dto.Content = plan.RepairContent;
                dto.PlanDate = dailyPlanEnts.PlanDate;
                var samePlanDateYearMonthPlan = _dailyPlansRepository.Where(p => p.RepairTagId == RepairTagId && p.PlanId == dailyPlanEnts.PlanId && p.PlanDate == dailyPlanEnts.PlanDate && p.PlanType == dailyPlanEnts.PlanType);
                dto.Count = samePlanDateYearMonthPlan.Sum(s => s.Count);
                dto.UnFinishCount = dto.Count - reduceCount;
                dto.Unit = plan.Unit;
                dto.PlanRepairDetailId = plan.RepairDetailsId;
                dto.IsChange = plan.State == 1;
                var repair_ifds = _repairDetailsIFDRepository.Where(s => s.RepairItemId == dto.PlanRepairDetailId).ToList();
                if (repair_ifds != null && repair_ifds.Count > 0)
                {
                    var ifdIds = repair_ifds.Select(s => s.ComponentCategoryId);
                    dto.IFDCodes = _componentCategoryRepository.Where(s => ifdIds.Contains(s.Id)).Select(s => s.Code).ToList();
                }
                return dto;
            });
        }

        /// <summary>
        ///选择待选计划时增加全选按钮 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public async Task<List<Guid>> GetSelectableAllPlans(SelectablePlansSearchDto input)
        //{
        //    var res = new List<Guid>();
        //    if (string.IsNullOrEmpty(input.OrgId.ToString()) || Guid.Empty == input.OrgId) throw new UserFriendlyException("负责单位未选择");
        //    var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
        //    await Task.Run(() =>
        //    {
        //        List<DailyPlan> ents = new List<DailyPlan>();
        //        IQueryable<YearMonthPlan> yearMonthPlans = null;// = new List<YearMonthPlan>();
        //        string[] planType = GetConvertEnumStr(input.SkylightType);
        //        //计划类型 年表、半年表、季度表筛选
        //        bool? isMonthType = null;
        //        if (input.Type != null)
        //        {
        //            //月表判断
        //            isMonthType = input.Type == SelectablePlanType.Month;
        //            if ((bool)isMonthType)
        //            {
        //                yearMonthPlans = _yearMonthPlansRepository.Where(s =>
        //                                                                 s.RepairTagId == RepairTagId &&
        //                                                                 s.ResponsibleUnit == input.OrgId &&
        //                                                                 ((bool)isMonthType ? s.PlanType == 2 : (s.PlanType == 1 || s.PlanType == 3)) &&
        //                                                                 (string.IsNullOrEmpty(input.Keyword) ||
        //                                                                    s.DeviceName.Contains(input.Keyword) ||
        //                                                                    s.RepairContent.Contains(input.Keyword)
        //                                                                 ));
        //            }
        //            //年表、半年表、季度表判断  
        //            else if (input.SkylightType == null)
        //            {
        //                switch (input.Type)
        //                {
        //                    case SelectablePlanType.Year:
        //                        yearMonthPlans = _yearMonthPlansRepository.Where(s =>
        //                                                                         s.RepairTagId == RepairTagId &&
        //                                                                         s.ResponsibleUnit == input.OrgId &&
        //                                                                         (s.PlanType == 1 || s.PlanType == 3) &&
        //                                                                         s.Times.Trim() != "2" && s.Times.Trim() != "4" &&
        //                                                                        (string.IsNullOrEmpty(input.Keyword) ||
        //                                                                            s.DeviceName.Contains(input.Keyword) ||
        //                                                                            s.RepairContent.Contains(input.Keyword)
        //                                                                         ));
        //                        break;
        //                    case SelectablePlanType.HalfYaer:
        //                        yearMonthPlans = _yearMonthPlansRepository.Where(s =>
        //                                                                         s.RepairTagId == RepairTagId &&
        //                                                                         s.ResponsibleUnit == input.OrgId &&
        //                                                                         (s.PlanType == 1 || s.PlanType == 3) &&
        //                                                                         s.Times.Trim() == "2" &&
        //                                                                         (string.IsNullOrEmpty(input.Keyword) ||
        //                                                                            s.DeviceName.Contains(input.Keyword) ||
        //                                                                            s.RepairContent.Contains(input.Keyword)
        //                                                                          ));
        //                        break;
        //                    case SelectablePlanType.QuarterYear:
        //                        yearMonthPlans = _yearMonthPlansRepository.Where(s =>
        //                                                                         s.RepairTagId == RepairTagId &&
        //                                                                         s.ResponsibleUnit == input.OrgId &&
        //                                                                         (s.PlanType == 1 || s.PlanType == 3) &&
        //                                                                         s.Times.Trim() == "4" &&
        //                                                                         (string.IsNullOrEmpty(input.Keyword) ||
        //                                                                             s.DeviceName.Contains(input.Keyword) ||
        //                                                                             s.RepairContent.Contains(input.Keyword)
        //                                                                          ));
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                yearMonthPlans = _yearMonthPlansRepository.Where(s =>
        //                                                                 s.RepairTagId == RepairTagId &&
        //                                                                 s.ResponsibleUnit == input.OrgId &&
        //                                                                 (s.PlanType == 1 || s.PlanType == 3) &&
        //                                                                 (string.IsNullOrEmpty(input.Keyword) ||
        //                                                                    s.DeviceName.Contains(input.Keyword) ||
        //                                                                    s.RepairContent.Contains(input.Keyword)
        //                                                                  ));
        //            }
        //        }
        //        else
        //        {
        //            yearMonthPlans = _yearMonthPlansRepository.Where(s =>
        //                                                             s.RepairTagId == RepairTagId &&
        //                                                             s.ResponsibleUnit == input.OrgId &&
        //                                                             (string.IsNullOrEmpty(input.Keyword) ||
        //                                                                s.DeviceName.Contains(input.Keyword) ||
        //                                                                s.RepairContent.Contains(input.Keyword)
        //                                                              ));
        //        }

        //        var dailyPlanEnts = (from yearMonthPlan in
        //                        yearMonthPlans.Where(s =>
        //                        (input.SkylightType == null ||
        //                        (planType.Length == 1 && s.SkyligetType != null && s.SkyligetType.Contains(planType[0])) ||
        //                        (planType.Length == 2 && s.SkyligetType != null && (s.SkyligetType.Contains(planType[0]) || s.SkyligetType.Contains(planType[1]))))
        //                        )
        //                             join dailyPlan in _dailyPlansRepository.Where(s =>
        //                                                                           s.RepairTagId == RepairTagId &&
        //                                                                           s.PlanDate >= input.StartTime &&
        //                                                                           s.PlanDate <= input.EndTime) on yearMonthPlan.Id equals dailyPlan.PlanId
        //                             select dailyPlan).ToList();

        //        //满足要求的日计划的详细信息
        //        var detialRe = _planDetailsRepository.Where(s => s.RepairTagId == RepairTagId && dailyPlanEnts.Select(d => d.Id).Contains(s.DailyPlanId)).ToList();
        //        //详细信息天窗信息
        //        var skylights = _skylightPlansRepository.Where(s => s.RepairTagId == RepairTagId && detialRe.Select(d => d.SkylightPlanId).Contains(s.Id)).ToList();
        //        //需减去的数量
        //        Dictionary<Guid, decimal> data = new Dictionary<Guid, decimal>();
        //        var alterEnts = _alterRecordRepository.Where(s => s.RepairTagId == RepairTagId && s.State != YearMonthPlanState.审核驳回);
        //        var alterRecords = _dailyPlan_AlterRepository.Where(s => s.RepairTagId == RepairTagId && alterEnts.Select(s => s.Id).Contains(s.AlterRecordId)).ToList();
        //        foreach (var item in detialRe)
        //        {
        //            //已完成天窗计划数量
        //            var skylight = skylights.FirstOrDefault(s => s.Id == item.SkylightPlanId);
        //            if (skylight != null)
        //            {
        //                decimal count = skylight.PlanState == Enums.PlanState.Complete ? item.WorkCount : item.PlanCount;
        //                if (data.Keys.Contains(item.DailyPlanId))
        //                {
        //                    data[item.DailyPlanId] += count;
        //                }
        //                else
        //                {
        //                    data.Add(item.DailyPlanId, count);
        //                }
        //            }
        //        }
        //        foreach (var item in alterRecords)
        //        {
        //            //变更了的计划的数量
        //            decimal alterCount = item.AlterCount;
        //            if (data.Keys.Contains(item.DailyId))
        //            {
        //                data[item.DailyId] += alterCount;
        //            }
        //            else
        //            {
        //                data.Add(item.DailyId, alterCount);
        //            }
        //        }
        //        List<DailyPlanSelectableDto> DailyPlans = new List<DailyPlanSelectableDto>();
        //        var allRepairIFD = _repairDetailsIFDRepository.Where(s => s.Id != null).ToList();
        //        var allIFD = _componentCategoryRepository.Where(s => s.Id != null && s.Code != null).ToList();
        //        var tempDailyPlanEnts = dailyPlanEnts.Distinct(new MonthYearCompare());
        //        var dailyPlanWithYearPlan = (from a in tempDailyPlanEnts
        //                                     join b in yearMonthPlans on a.PlanId equals b.Id
        //                                     select new
        //                                     {
        //                                         DailyPlan = a,
        //                                         YearMonthPlan = b
        //                                     }).ToList();
        //        res = dailyPlanWithYearPlan.Select(x => x.DailyPlan).Select(y => y.Id).ToList();
        //    });

        //    return res;
        //}

        /// <summary>
        /// 获取多条待选计划
        /// </summary>
        /// <returns></returns>
        public async Task<List<DailyPlanSelectableDto>> ForSelectablePlanByIds(AlterRecordGetListDto input)
        {
            List<DailyPlanSelectableDto> res = new List<DailyPlanSelectableDto>();
            //return await Task.Run(() =>
            //{
            #region OldCode
            //var allDailyPlanEnts = dailyPlansRepository.Where(s => ids.Contains(s.Id)).ToList();
            //var planIds = allDailyPlanEnts.Select(s => s.PlanId).ToList();
            //var allYearMonthPlans = yearMonthPlansRepository.Where(s => planIds.Contains(s.Id));
            //var allPlanDetails = planDetailsRepository.Where(s => ids.Contains(s.DailyPlanId));
            //var allSkylightPlans = skylightPlansRepository.Where(s => s.Id != null);
            //var alterEnts = alterRecordRepository.Where(s => s.State != YearMonthPlanState.审核驳回);
            //var allDailyPlans = dailyPlansRepository.Where(s => s.Id != null);
            //var alterRecords = dailyPlan_AlterRepository.Where(s => alterEnts.Select(s => s.Id).Contains(s.AlterRecordId));
            //var allRepairDetails = repairDetailsIFDRepository.Where(s => s.Id != null);
            //var allIFDCode = iFDCodesRepository.Where(s => s.Id != null);
            //foreach (var id in ids)
            //{
            //    var dailyPlanEnts = allDailyPlanEnts.FirstOrDefault(s => s.Id == id);
            //    if (dailyPlanEnts == null) throw new UserFriendlyException("该计划不存在");
            //    //需减去的数量
            //    decimal reduceCount = 0;
            //    //满足要求的日计划的详细信息
            //    var detialRe = ObjectMapper.Map<PlanDetail, PlanDetailDto>(allPlanDetails.FirstOrDefault(s => dailyPlanEnts.Id == s.DailyPlanId));
            //    //详细信息天窗信息
            //    SkylightPlanDto skylight = null;
            //    if (detialRe != null)
            //    {
            //        skylight = ObjectMapper.Map<SkylightPlan, SkylightPlanDto>(allSkylightPlans.FirstOrDefault(s => detialRe.SkylightPlanId == s.Id));
            //        if (skylight != null && skylight.PlanState != PlanState.Backout)
            //        {
            //            decimal count = skylight.PlanState == Enums.PlanState.Complete ? detialRe.WorkCount : detialRe.PlanCount;
            //            reduceCount += count;
            //        }
            //    }
            //    //变更了的计划的数量
            //    var alterEnt = alterRecords.Where(s => s.DailyId == id);
            //    if (alterEnt.Count() > 0)
            //    {
            //        reduceCount += alterEnt.Sum(s => s.AlterCount);
            //    }
            //    DailyPlanSelectableDto dto = new DailyPlanSelectableDto();
            //    var plan = allYearMonthPlans.FirstOrDefault(p => p.Id == dailyPlanEnts.PlanId);
            //    if (plan.PlanType == 2)
            //    {
            //        dto.PlanType = SelectablePlanType.Month;
            //    }
            //    else if (plan.PlanType == 1 || plan.PlanType == 3)
            //    {
            //        var times = plan.Times.Trim();
            //        if (times == "2") dto.PlanType = SelectablePlanType.HalfYaer;
            //        else if (times == "4") dto.PlanType = SelectablePlanType.QuarterYear;
            //        else dto.PlanType = SelectablePlanType.Year;
            //    }
            //    dto.PlanId = dailyPlanEnts.Id;
            //    dto.PlanTypeStr = EnumHelper.GetDescription(dto.PlanType);
            //    dto.Id = dailyPlanEnts.Id;
            //    dto.Number = plan != null ? plan.Number : 0;
            //    dto.EquipName = plan.DeviceName;
            //    dto.Content = plan.RepairContent;
            //    dto.PlanDate = dailyPlanEnts.PlanDate;
            //    var samePlanDateYearMonthPlan = allDailyPlans.Where(p => p.PlanId == dailyPlanEnts.PlanId && p.PlanDate == dailyPlanEnts.PlanDate && p.PlanType == dailyPlanEnts.PlanType);
            //    dto.Count = samePlanDateYearMonthPlan.Sum(s => s.Count);
            //    dto.UnFinishCount = dto.Count - reduceCount;
            //    dto.Unit = plan.Unit;
            //    dto.PlanRepairDetailId = plan.RepairDetailsId;
            //    var repair_ifds = allRepairDetails.Where(s => s.RepairDetailId == dto.PlanRepairDetailId).ToList();
            //    if (repair_ifds != null && repair_ifds.Count > 0)
            //    {
            //        var ifdIds = repair_ifds.Select(s => s.IFDCodeId);
            //        dto.IFDCodes = allIFDCode.Where(s => ifdIds.Contains(s.Id)).Select(s => s.Code).ToList();
            //    }
            //    res.Add(dto);
            //}
            #endregion

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //var yearMonthPlans = yearMonthPlansRepository.Where(s => s.Id != null).ToList();
            var dailyPlanEnts = _dailyPlansRepository.Where(s => s.RepairTagId == RepairTagId && input.Ids.Contains(s.Id)).ToList();
            //满足要求的日计划的详细信息
            var detialRe = _planDetailsRepository.Where(s => s.RepairTagId == RepairTagId && input.Ids.Contains(s.DailyPlanId)).ToList();
            //详细信息天窗信息
            var skylights = _skylightPlansRepository.Where(s => s.RepairTagId == RepairTagId && detialRe.Select(d => d.SkylightPlanId).Contains(s.Id)).ToList();
            //需减去的数量
            Dictionary<Guid, decimal> data = new Dictionary<Guid, decimal>();
            var alterEnts = _alterRecordRepository.Where(s => s.RepairTagId == RepairTagId && s.State != YearMonthPlanState.审核驳回);
            var alterRecords = _dailyPlan_AlterRepository.Where(s => s.RepairTagId == RepairTagId && alterEnts.Select(s => s.Id).Contains(s.AlterRecordId));
            foreach (var item in detialRe)
            {
                //已完成天窗计划数量
                var skylight = skylights.FirstOrDefault(s => s.Id == item.SkylightPlanId);
                if (skylight != null)
                {
                    decimal count = skylight.PlanState == Enums.PlanState.Complete ? item.WorkCount : item.PlanCount;
                    if (data.Keys.Contains(item.DailyPlanId))
                    {
                        data[item.DailyPlanId] += count;
                    }
                    else
                    {
                        data.Add(item.DailyPlanId, count);
                    }
                }
            }
            foreach (var item in alterRecords)
            {
                //变更了的计划的数量
                decimal alterCount = item.AlterCount;
                if (data.Keys.Contains(item.DailyId))
                {
                    data[item.DailyId] += alterCount;
                }
                else
                {
                    data.Add(item.DailyId, alterCount);
                }
            }
            List<DailyPlanSelectableDto> DailyPlans = new List<DailyPlanSelectableDto>();
            var allRepairIFD = _repairDetailsIFDRepository.Where(s => s.Id != null);
            var allIFD = _componentCategoryRepository.Where(s => s.Id != null && s.Code != null);
            var tempDailyPlanEnts = dailyPlanEnts.Distinct(new MonthYearCompare());
            var tempYearMonthPlanIds = tempDailyPlanEnts.Select(s => s.PlanId);
            var yearMonthPlans = _yearMonthPlansRepository.Where(s => s.RepairTagId == RepairTagId && tempYearMonthPlanIds.Contains(s.Id)).ToList();
            var dailyPlanWithYearPlan = from a in tempDailyPlanEnts
                                        join b in yearMonthPlans on a.PlanId equals b.Id
                                        select new
                                        {
                                            DailyPlan = a,
                                            YearMonthPlan = b
                                        };
            foreach (var t in dailyPlanWithYearPlan)
            {
                var item = t.DailyPlan;
                var plan = t.YearMonthPlan;
                DailyPlanSelectableDto dto = new DailyPlanSelectableDto();
                //var plan = yearMonthPlans.FirstOrDefault(p => p.Id == item.PlanId);
                if (plan.PlanType == 2)
                {
                    dto.PlanType = SelectablePlanType.Month;
                }
                else if (plan.PlanType == 1 || plan.PlanType == 3)
                {
                    var times = plan.Times.Trim();
                    if (times == "2") dto.PlanType = SelectablePlanType.HalfYaer;
                    else if (times == "4") dto.PlanType = SelectablePlanType.QuarterYear;
                    else dto.PlanType = SelectablePlanType.Year;
                }
                dto.PlanId = item.Id;
                dto.PlanTypeStr = EnumHelper.GetDescription(dto.PlanType);
                dto.Id = item.Id;
                if (plan != null)
                {
                    string newNum = "";
                    var nums = plan.Number.Split("-");
                    foreach (var i in nums)
                    {
                        newNum += int.Parse(i).ToString().PadLeft(3, '0') + "-";
                    }
                    newNum = newNum.TrimEnd('-');
                    dto.Number = newNum;
                    dto.EquipName = plan.DeviceName;
                    dto.Content = plan.RepairContent;
                    dto.Unit = plan.Unit;
                    dto.PlanRepairDetailId = plan.RepairDetailsId;
                }
                dto.PlanDate = item.PlanDate;
                var samePlanDateYearMonthPlan = dailyPlanEnts.Where(p => p.PlanId == item.PlanId && p.PlanDate == item.PlanDate && p.PlanType == item.PlanType);
                dto.Count = samePlanDateYearMonthPlan.Sum(s => s.Count);
                if (data.Where(s => s.Key == dto.PlanId) != null && data.Where(s => s.Key == dto.PlanId).Count() > 0)
                    dto.UnFinishCount = dto.Count - data[dto.PlanId];
                else dto.UnFinishCount = dto.Count;
                //if (dto.UnFinishCount <= 0) continue;
                DailyPlans.Add(dto);
            }

            res = DailyPlans.OrderBy(s => s.PlanType).ThenBy(s => s.Number.Replace("-", "")).ThenBy(s => s.PlanDate).ToList();
            foreach (var item in res)
            {
                var repair_ifds = allRepairIFD.Where(s => s.RepairItemId == item.PlanRepairDetailId);//.ToList();
                if (repair_ifds != null && repair_ifds.Count() > 0)
                {
                    var ifdIds = repair_ifds.Select(s => s.ComponentCategoryId);
                    item.IFDCodes = allIFD.Where(s => ifdIds.Contains(s.Id)).Select(s => s.Code).ToList();
                }
                var nums = item.Number.Split('-');
                string newNum = "";
                foreach (var num in nums)
                {
                    newNum += int.Parse(num).ToString() + "-";
                }
                item.Number = newNum.TrimEnd('-');
            }

            return res;
            //});
        }

        //天窗类型转换
        private string[] GetConvertEnumStr(PlanType? type)
        {
            if (type != null)
            {
                switch (type)
                {
                    case PlanType.ComprehensiveSkylight:
                        return new string[] { SkyligetType.综合天窗.ToString() };
                    case PlanType.VerticalSkylight:
                        return new string[] { SkyligetType.垂直天窗.ToString() };
                    case PlanType.SkylightOutside:
                        return new string[] { SkyligetType.天窗点外.ToString() };
                    case PlanType.Other:
                        return new string[] { SkyligetType.其他.ToString(), SkyligetType.各网管.ToString() };
                }
            }
            return new string[] { };
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(CrPlanPermissions.PlanChange.Export)]
        [Produces("application/octet-stream")]
        public async Task<Stream> Export(AlterRecordExportInputDto input)
        {
            Stream st = null;
            byte[] sbuf;
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("id不正确");

            await Task.Run(async () =>
            {
                var ent = _alterRecordRepository.FirstOrDefault(s => s.Id == input.Id);
                if (ent == null) throw new UserFriendlyException("对象不存在");
                AlterRecordDetailDto dto = ObjectMapper.Map<AlterRecord, AlterRecordDetailDto>(ent);
                dto = (await SetDetailInfo(new List<AlterRecordDetailDto>() { dto }, ent))[0];

                sbuf = ExporAlterRecord(dto, null, input.SignatureName);
                //File.WriteAllBytes(@"h:/计划变更导出表.xls", sbuf);
                st = new MemoryStream(sbuf);
            });
            return st;
        }

        [Authorize(CrPlanPermissions.PlanChange.Create)]
        public async Task<AlterRecordDto> Create(AlterRecordCreateDto input)
        {
                if (string.IsNullOrEmpty(input.Reason.Trim())) throw new UserFriendlyException("变更原因不能为空");
            if (input.PlanTime == null) throw new UserFriendlyException("原计划时间不能为空");
            if (input.AlterTime == null) throw new UserFriendlyException("申请变更时间不能为空");
            if (!Enum.IsDefined(typeof(SelectablePlanType), input.AlterType)) throw new UserFriendlyException("变更类型有误");
            if (input.DailyPlanAlters?.Count == 0) throw new UserFriendlyException("变更计划内容不能为空");
            AlterRecordDto res = new AlterRecordDto();
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                AlterRecord ent = new AlterRecord(Guid.NewGuid());
                ent.AlterTime = input.AlterTime;
                ent.AlterType = input.AlterType;
                ent.PlanTime = input.PlanTime;
                ent.Reason = input.Reason;
                ent.State = YearMonthPlanState.未提交;
                ent.RepairTagId = RepairTagId;
                //if (CurrentUser == null || CurrentUser.Id == null || CurrentUser.Id == Guid.Empty)
                //{
                //    throw new Exception("用户未登录");
                //}
                //var orgIds = org_UserRepository.Where(s => s.UserId == CurrentUser.Id);
                //if (orgIds != null && orgIds.Count() > 0)
                //{
                //    ent.OrganizationId = orgIds.First().OrganizationId;
                //}
                //else
                //{
                //    throw new Exception("用户未关联组织机构");
                //}
                ent.OrganizationId = input.OrganizationId;
                ent.CreateTime = DateTime.Now;
                //相同组织机构下的变更集合
                var sameOrgEnts = _alterRecordRepository.Where(s => s.RepairTagId == RepairTagId && s.OrganizationId == ent.OrganizationId && s.AlterType == input.AlterType).ToList();
                if (sameOrgEnts != null && sameOrgEnts.Count > 0)
                {
                    var savedLastEnt = sameOrgEnts.OrderBy(s => s.Number).Last();
                    ent.Number = savedLastEnt != null ? savedLastEnt.Number + 1 : 1;
                }
                else
                {
                    ent.Number = 1;
                }
                await _alterRecordRepository.InsertAsync(ent);
                foreach (var item in input.DailyPlanAlters)
                {
                    DailyPlanAlter temp = new DailyPlanAlter(Guid.NewGuid());
                    temp.AlterCount = item.AlterCount;
                    temp.AlterRecordId = ent.Id;
                    temp.DailyId = item.DailyId;
                    temp.PlanCount = item.PlanCount;
                    temp.Remark = "变更计划" + "(";//item.Remark;
                    temp.RepairTagId = RepairTagId;
                    temp.AlterTime = item.AlterDateTime;
                    await _dailyPlan_AlterRepository.InsertAsync(temp);
                }
                res = ObjectMapper.Map<AlterRecord, AlterRecordDto>(ent);
                return res;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [Authorize(CrPlanPermissions.PlanChange.Update)]
        public async Task<AlterRecordDto> Update(AlterRecordUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("id不正确");
            if (string.IsNullOrEmpty(input.Reason.Trim())) throw new UserFriendlyException("变更原因不能为空");
            if (input.PlanTime == null) throw new UserFriendlyException("原计划时间不能为空");
            if (input.AlterTime == null) throw new UserFriendlyException("申请变更时间不能为空");
            if (!Enum.IsDefined(typeof(SelectablePlanType), input.AlterType)) throw new UserFriendlyException("变更类型有误");
            if (input.DailyPlanAlters?.Count == 0) throw new UserFriendlyException("变更计划内容不能为空");
            AlterRecordDto res = new AlterRecordDto();
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var oldEnt = _alterRecordRepository.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == input.Id);
                if (oldEnt == null) throw new Exception("对象不存在");
                oldEnt.AlterTime = input.AlterTime;
                oldEnt.AlterType = input.AlterType;
                //if (CurrentUser == null || CurrentUser.Id == null || CurrentUser.Id == Guid.Empty)
                //{
                //    throw new Exception("用户未登录");
                //}
                //var orgIds = org_UserRepository.Where(s => s.UserId == CurrentUser.Id);
                //if (orgIds != null && orgIds.Count() > 0)
                //{
                //    oldEnt.OrganizationId = orgIds.First().OrganizationId;
                //}
                //else
                //{
                //    throw new Exception("用户未关联组织机构");
                //}
                oldEnt.OrganizationId = input.OrganizationId;
                oldEnt.PlanTime = input.PlanTime;
                oldEnt.Reason = input.Reason;
                oldEnt.UpdateTime = DateTime.Now;
                oldEnt.RepairTagId = RepairTagId;
                await _alterRecordRepository.UpdateAsync(oldEnt);

                await _dailyPlan_AlterRepository.DeleteAsync(s => s.RepairTagId == RepairTagId && s.AlterRecordId == oldEnt.Id);
                foreach (var item in input.DailyPlanAlters)
                {
                    DailyPlanAlter temp = new DailyPlanAlter(Guid.NewGuid()); ;
                    temp.AlterCount = item.AlterCount;
                    temp.AlterRecordId = oldEnt.Id;
                    temp.DailyId = item.DailyId;
                    temp.PlanCount = item.PlanCount;
                    temp.Remark = item.Remark;
                    temp.RepairTagId = RepairTagId;
                    temp.AlterTime = item.AlterDateTime;
                    await _dailyPlan_AlterRepository.InsertAsync(temp);
                }
                res = ObjectMapper.Map<AlterRecord, AlterRecordDto>(oldEnt);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return res;
        }

        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SubmitForExam(CommonGuidGetDto input)
        {
            var details = await Get(input);
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            string organizationName = "";
            if (input.OrganizationId != null && input.OrganizationId != Guid.Empty)
            {
                organizationName = _organiztionRepository.FirstOrDefault(x => x.Id == input.OrganizationId)?.Name;
            }
            if (details.State != YearMonthPlanState.未提交 && details.State != YearMonthPlanState.审核驳回)
            {
                throw new UserFriendlyException("审核状态不正确");
            }

            //var dataTable = JsonToDataTable()

            var key = "PlanChange";


            var userId = CurrentUser.Id.GetValueOrDefault();
            // 构造数据
            var value = new JObject();
            var rows = new JArray();
            int index = 0;
            foreach (var item in details.DailyPlanAlters.OrderBy(x => x.Number))
            {
                index++;
                var row = new JObject();
                row["序号"] = index;
                row["设备名称"] = item.EquipName ?? "";
                row["工作内容"] = item.Content ?? "";
                row["日期"] = item.PlanDate.ToString("yyyy-MM-dd") ?? "";
                row["计划数量"] = item.PlanCount;
                row["当前未完成数量"] = item.AlterCount;
                rows.Add(row);
            }
            value["input_reason"] = details.Reason;
            value["input_forePlanTime"] = details.PlanTime.ToString("yyyy-MM");
            value["input_changePlanTime"] = details.AlterTime.ToString("yyyy-MM");
            value["input_changeType"] = EnumHelper.GetDescription(details.AlterType);

            var tableName = organizationName + details.PlanTime.ToString("yyyy-MM") + "-计划变更";

            var fileJObject = await _crPlanManager.JsonToDataTable(rows, tableName, false);

            var fileArray = new JArray { (fileJObject) };

            value["uploadFile_PlanChange"] = fileArray;

            var workflow = await _bpmManager.CreateWorkflowByWorkflowTemplateKey(key, value.ToString(), userId);

            Guid workFlowId = workflow != null ? workflow.Id : Guid.Empty;

            if (workFlowId == Guid.Empty) throw new UserFriendlyException("审核工作流提交失败");
            var ent = _alterRecordRepository.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == details.Id);
            ent.AR_Key = workFlowId;
            ent.State = YearMonthPlanState.审核中;
            ent.RepairTagId = RepairTagId;
            await _alterRecordRepository.UpdateAsync(ent);
            return true;
        }

        /// <summary>
        /// 变更计划审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> ApprovePlanAlter(Guid id, WorkflowState state)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("变更计划对应审批不正确");
            //var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            var ent = _alterRecordRepository.FirstOrDefault(s => s.AR_Key == id);
            var dailys = _dailyPlansRepository.Where(s => s.Id != null);
            if (ent == null) throw new UserFriendlyException("该变更计划不存在");
            switch (state)
            {
                case WorkflowState.Finished:
                    ent.State = YearMonthPlanState.审核通过;
                    break;
                case WorkflowState.Stopped:
                    ent.State = YearMonthPlanState.审核驳回;
                    break;
            }
            await _alterRecordRepository.UpdateAsync(ent);
            if (state == WorkflowState.Finished)
            {
                var alters = _dailyPlan_AlterRepository.Where(s => s.AlterRecordId == ent.Id);

                var dailyIds = dailys.Select(s => s.PlanId).ToList();

                var tempData = (from a in alters
                                join d in dailys on a.DailyId equals d.Id
                                join y in _yearMonthPlansRepository.Where(s => dailyIds.Contains(s.Id)) on d.PlanId equals y.Id
                                select new
                                {
                                    Alter = a,
                                    Daily = d,
                                    YearMonthPlan = y,
                                }).ToList();

                var groupedData = from a in tempData
                                  group a by a.YearMonthPlan.RepairDetailsId into res
                                  select new
                                  {
                                      RepairDetailId = res.Key,
                                      AlterDatas = res.ToList(),
                                  };

                foreach (var item in groupedData)
                {
                    DailyPlan daily = new DailyPlan(Guid.NewGuid());
                    daily.Count = item.AlterDatas.Sum(s => s.Alter.AlterCount);
                    daily.PlanDate = item.AlterDatas.FirstOrDefault().Alter.AlterTime;
                    var tempDailyPlan = item.AlterDatas.First().Daily;
                    daily.PlanId = tempDailyPlan != null ? tempDailyPlan.PlanId : Guid.Empty;
                    daily.PlanType = tempDailyPlan != null ? tempDailyPlan.PlanType : -1;
                    daily.State = 1;
                    daily.RepairTagId = tempDailyPlan.RepairTagId;

                    await _dailyPlansRepository.InsertAsync(daily);
                }

                //foreach (var t in tempData)
                //{
                //    var item = t.Alter;
                //    var tempDailyPlan = t.Daily;
                //    DailyPlan daily = new DailyPlan(Guid.NewGuid());
                //    daily.Count = item.AlterCount;
                //    daily.PlanDate = ent.AlterTime;
                //    //var tempDailyPlan = dailys.FirstOrDefault(d => d.Id == item.DailyId);
                //    daily.PlanId = tempDailyPlan != null ? tempDailyPlan.PlanId : Guid.Empty;
                //    daily.PlanType = tempDailyPlan != null ? tempDailyPlan.PlanType : -1;
                //    daily.State = 1;
                //    daily.RepairTagId = tempDailyPlan.RepairTagId;
                //    await _dailyPlansRepository.InsertAsync(daily);
                //}
            }
            return true;
        }

        [Authorize(CrPlanPermissions.PlanChange.Delete)]
        public async Task<bool> Delete(CommonGuidGetDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("id不正确");
            try
            {
                var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
                var ent = _alterRecordRepository.FirstOrDefault(s => s.RepairTagId == RepairTagId && s.Id == input.Id);
                if (ent == null) throw new UserFriendlyException("对象不存在");

                await _dailyPlan_AlterRepository.DeleteAsync(s => s.RepairTagId == RepairTagId && s.AlterRecordId == input.Id);
                await _alterRecordRepository.DeleteAsync(input.Id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return true;
        }

        #region 私有方法

        private async Task<List<AlterRecordDetailDto>> SetDetailInfo(List<AlterRecordDetailDto> target, AlterRecord alterRecord = null)
        {
            var allOrg = await _orgRepository.Where(s => s.Id != null);
            var allRelateEnt = _dailyPlan_AlterRepository.Where(s => s.Id != null);
            //var allRelate = ObjectMapper.Map<List<DailyPlanAlter>, List<DailyPlanAlterDto>>(allRelateEnt);
            var allDailyPlanEnts = _dailyPlansRepository.Where(s => s.Id != null);
            //var allDailyPlans = ObjectMapper.Map<List<DailyPlan>, List<DailyPlanDto>>(allDailyPlanEnts);
            var allYearMonthEnts = _yearMonthPlansRepository.Where(s => s.Id != null);
            //var allYearMonths = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(allYearMonthEnts);
            foreach (var item in target)
            {
                var org = allOrg.FirstOrDefault(s => s.Id == item.OrganizationId);
                item.Organization = org != null ? ObjectMapper.Map<Organization, OrganizationDto>(org) : null;
                var relate = allRelateEnt.Where(s => s.AlterRecordId == item.Id).ToList();

                List<DailyPlanAlterDetailDto> temp = new List<DailyPlanAlterDetailDto>();
                foreach (var relateItem in relate)
                {
                    DailyPlanAlterDetailDto dto = new DailyPlanAlterDetailDto();
                    var dailyPlan = allDailyPlanEnts.FirstOrDefault(s => s.Id == relateItem.DailyId);
                    if (dailyPlan != null)
                    {
                        var planId = dailyPlan?.PlanId;
                        var plan = allYearMonthEnts.FirstOrDefault(s => s.Id == planId);
                        dto.Content = plan?.RepairContent;
                        dto.PlanCount = relateItem.PlanCount;
                        dto.AlterCount = relateItem.AlterCount;
                        dto.EquipName = plan?.DeviceName;
                        dto.Id = relateItem.DailyId;
                        dto.AlterDateTime =
                            relateItem.AlterTime == new DateTime(0) ? alterRecord.AlterTime : relateItem.AlterTime;
                        if (plan != null)
                        {
                            var nums = plan.Number.Split('-');
                            string newNum = "";
                            foreach (var num in nums)
                            {
                                newNum += int.Parse(num).ToString() + "-";
                            }
                            dto.Number = newNum.TrimEnd('-');
                        }
                        //dto.Number = plan != null ? plan.Number : "-1";
                        dto.PlanDate = dailyPlan.PlanDate;
                        dto.PlanId = dailyPlan.Id;
                        dto.PlanType = dailyPlan.PlanType == 4 ? SelectablePlanType.Month : SelectablePlanType.Year;
                        dto.PlanTypeStr = EnumHelper.GetDescription(dto.PlanType);
                        dto.Unit = plan.Unit;
                    }
                    temp.Add(dto);
                }
                item.DailyPlanAlters = temp;
                item.AlterTypeStr = EnumHelper.GetDescription(item.AlterType);
                item.StateStr = Enum.Parse(typeof(YearMonthPlanState), item.State.ToString()).ToString();
                item.FullNumber = item.AlterTypeStr + ' ' + item.Number.ToString().PadLeft(2, '0');
            }
            return target;
        }

        private List<AlterRecordSimpleDto> SetSimpleInfo(List<AlterRecordSimpleDto> target)
        {
            foreach (var item in target)
            {
                item.AlterTypeStr = EnumHelper.GetDescription(item.AlterType);
                item.StateStr = Enum.Parse(typeof(YearMonthPlanState), item.State.ToString()).ToString();
                item.FullNumber = item.AlterTypeStr + ' ' + item.Number.ToString().PadLeft(2, '0');
                item.OrganizationName = _organiztionRepository.FirstOrDefault(x => x.Id == item.OrganizationId)?.Name;
            }
            return target;
        }

        /// <summary>
        /// 计划变更导出
        /// </summary>
        /// <param name="dto">计划变更实体</param>
        /// <param name="seal">盖章审批</param>
        /// <param name="signatureName">"签章名称"</param>
        /// <returns></returns>
        private byte[] ExporAlterRecord(AlterRecordDetailDto dto, object seal, string signatureName)
        {
            if (dto == null || dto.DailyPlanAlters == null || dto.DailyPlanAlters.Count == 0)
            {
                return new byte[0];
            }

            int listCount = dto.DailyPlanAlters.Count;
            int loopCount = Math.Max(listCount, 10) + 6;

            IWorkbook workBook = CreateAlterListTemplete(listCount);
            ISheet sheet = workBook.GetSheetAt(0);

            var lastRow = sheet.LastRowNum;
            #region 查询人员信息
            var organizationName = dto.Organization?.Name;
            if (null == dto.AR_Key) throw new UserFriendlyException("未找到相关工作流程");
            var workflowData = workflowDataRepository.OrderBy(x => x.CreationTime).LastOrDefault(); ;
            sheet.GetRow(lastRow - 2).GetCell(0).SetCellValue("申请车间（盖章）:" + organizationName);
            sheet.GetRow(lastRow - 2).GetCell(2).SetCellValue("审核人：" + organizationName);
            sheet.GetRow(lastRow - 2).GetCell(4).SetCellValue(organizationName);
            sheet.GetRow(lastRow - 2).GetCell(6).SetCellValue(dto.CreateTime.ToString("yyyy年MM月dd日 HH:mm:ss"));
            sheet.GetRow(lastRow - 3).GetCell(2).SetCellValue("技术科主管（签章）" + workflowData.CreationTime.ToString("yyyy年MM月dd日"));
            sheet.GetRow(lastRow - 4).GetCell(2).SetCellValue(string.IsNullOrWhiteSpace(workflowData.Comments) ? "无" : workflowData.Comments);
            #endregion

            //string strAlterNo = string.Format("{0}-{1}", planAlter.StrType, planAlter.AlterNo.ToString().PadLeft(2, '0'));
            sheet.GetRow(1).CreateCell(6).SetCellValue(dto.FullNumber);
            sheet.GetRow(2).GetCell(2).SetCellValue(dto.Reason);
            sheet.GetRow(3).GetCell(2).SetCellValue(dto.PlanTime.ToString("yyyy年MM月"));
            sheet.GetRow(3).GetCell(5).SetCellValue(dto.AlterTime.ToString("yyyy年MM月"));
            sheet.GetRow(4).GetCell(0).SetCellValue("申请变更生产任务内容：" + dto.AlterTypeStr);
            //workBook.CreateCellStyle().CloneStyleFrom()
            int rowNum = 6;
            foreach (var item in dto.DailyPlanAlters)
            {
                IRow curRow = sheet.GetRow(rowNum);
                curRow.GetCell(0).SetCellValue(rowNum - 5);
                curRow.GetCell(1).SetCellValue(item.EquipName);
                curRow.GetCell(2).SetCellValue(item.Content);
                curRow.GetCell(3).SetCellValue(item.Unit);
                curRow.GetCell(4).SetCellValue((double)item.PlanCount);
                curRow.GetCell(5).SetCellValue((double)item.AlterCount);
                //curRow.GetCell(6).SetCellValue(item.Remark);         //暂时无备注
                rowNum++;
            }
            if (seal != null)
            {
                //sheet.GetRow(loopCount).GetCell(2).SetCellValue(seal.OpinionDuan);
                //sheet.GetRow(loopCount + 1).GetCell(2).SetCellValue(string.Format("技术科主管（签章）{0}   {1} ", seal.ApproveUserDuan, seal.ApproveDataDuan));
                //sheet.GetRow(loopCount + 2).GetCell(2).SetCellValue(string.Format("{0}   审核人：{1}", seal.CheJian.Value, seal.ApproveUserCJ));
                //sheet.GetRow(loopCount + 2).GetCell(4).SetCellValue(seal.ApproveUserCJ);
                //sheet.GetRow(loopCount + 2).GetCell(6).SetCellValue(seal.ApproveDataCJ);

                //if (seal.ApproveResultCJ == CommonLibrary.ApproveState.同意)
                //{
                //    string sealUrl = GetSealPath(seal.CheJian.Value);
                //    //添加车间的公章
                //    AddImage(workBook, sheet, sealUrl, 1, loopCount + 1, 1, loopCount + 1);
                //}

                //if (seal.ApproveResultDuan == CommonLibrary.ApproveState.同意)
                //{
                //    string duanlUrl = GetSealPath(seal.Duan.Value);
                //    //添加段的公章
                //    AddImage(workBook, sheet, duanlUrl, 3, loopCount, 3, loopCount);
                //}
            }

            #region 插入签章
            var path = Path.Combine(Directory.GetCurrentDirectory(), path2: $"signature\\{signatureName}");

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            int pictureIdx = workBook.AddPicture(bytes, PictureType.PNG);

            IDrawing patriarch = sheet.CreateDrawingPatriarch();

            IClientAnchor anchor = new HSSFClientAnchor(0, 0, 250, 167, 4, lastRow - 5, 5, lastRow - 1);

            var pict = patriarch.CreatePicture(anchor, pictureIdx);// (XSSFPicture)

            pict.Resize();

            #endregion

            MemoryStream bookStream = new MemoryStream();
            workBook.Write(bookStream);
            workBook.Close();
            return bookStream.ToArray();
        }

        /// <summary>
        /// 创建变更单表格结构
        /// </summary>
        /// <param name="listCount"></param>
        /// <param name="isNewVersion">新版本后缀名xlsx 默认版本未xls</param>
        /// <returns></returns>
        private static IWorkbook CreateAlterListTemplete(int listCount, bool isNewVersion = false)
        {
            IWorkbook workBook;
            if (isNewVersion)
            {
                workBook = new XSSFWorkbook();
            }
            else
            {
                workBook = new HSSFWorkbook();
            }
            ISheet sheet = workBook.CreateSheet("变更单");
            sheet.SetColumnWidth(0, 1600);
            sheet.SetColumnWidth(1, 4000);
            sheet.SetColumnWidth(2, 8000);
            sheet.SetColumnWidth(3, 2400);
            sheet.SetColumnWidth(4, 2400);
            sheet.SetColumnWidth(5, 3200);
            sheet.SetColumnWidth(6, 4000);

            IFont titleFont = workBook.CreateFont();
            titleFont.FontHeightInPoints = 18;//设置字体高度
            titleFont.FontName = "宋体";//设置字体
            titleFont.IsBold = true;

            IFont tbTitleFont = workBook.CreateFont();
            tbTitleFont.FontHeightInPoints = 11;
            tbTitleFont.FontName = "宋体";
            tbTitleFont.IsBold = false;

            IFont contentFont = workBook.CreateFont();
            contentFont.FontHeightInPoints = 11;
            contentFont.FontName = "宋体";
            contentFont.IsBold = false;

            ICellStyle stTitleStyle = workBook.CreateCellStyle();
            stTitleStyle.IsLocked = true;
            stTitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            stTitleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            stTitleStyle.SetFont(titleFont);

            ICellStyle borderCell = workBook.CreateCellStyle();
            borderCell.BorderBottom = BorderStyle.Thin;//设置单元格低边框为细线
            borderCell.BorderLeft = BorderStyle.Thin;
            borderCell.BorderRight = BorderStyle.Thin;
            borderCell.BorderTop = BorderStyle.Thin;
            borderCell.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            borderCell.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            ICellStyle tbTelStyle = workBook.CreateCellStyle();
            tbTelStyle.IsLocked = true;
            tbTelStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            tbTelStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            tbTelStyle.SetFont(tbTitleFont);
            borderCell.SetFont(contentFont);

            int loopCount = Math.Max(listCount, 10) + 6;
            //因为i从2开始计数，所以要边界是loopCount + 3，
            for (int i = 2; i < loopCount + 3; i++)
            {
                IRow row = sheet.CreateRow(i);
                row.Height = 20 * 20;
                for (int j = 0; j < 7; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle = borderCell;
                }
            }
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 1));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 6));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 1));
            //sheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 1));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 3, 4));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 5, 6));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 6));
            sheet.AddMergedRegion(new CellRangeAddress(loopCount, loopCount + 1, 0, 1));
            sheet.AddMergedRegion(new CellRangeAddress(loopCount, loopCount, 2, 6));
            sheet.AddMergedRegion(new CellRangeAddress(loopCount + 1, loopCount + 1, 2, 6));
            sheet.AddMergedRegion(new CellRangeAddress(loopCount + 2, loopCount + 2, 0, 1));
            sheet.AddMergedRegion(new CellRangeAddress(loopCount + 4, loopCount + 4, 0, 6));

            IRow rowTitle = sheet.CreateRow(0);
            rowTitle.Height = 30 * 20;
            ICell titelCell = rowTitle.CreateCell(0);
            titelCell.SetCellValue("生产任务变更申请审批单");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
            titelCell.CellStyle = stTitleStyle;

            IRow rowCJ = sheet.CreateRow(1);
            rowCJ.Height = 20 * 20;
            ICell cellCJ = rowCJ.CreateCell(5);
            cellCJ.SetCellValue("编号：");
            cellCJ.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            cellCJ.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            cellCJ.CellStyle.SetFont(tbTitleFont);
            IRow rowReason = sheet.GetRow(2);
            rowReason.Height = 50 * 20;
            rowReason.GetCell(0).SetCellValue("变更原因");
            rowReason.GetCell(0).CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IRow rowDate = sheet.GetRow(3);
            rowDate.Height = 30 * 20;
            rowDate.GetCell(0).SetCellValue("原计划时间");
            rowDate.GetCell(0).CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            rowDate.GetCell(3).SetCellValue("申请变更时间");
            rowDate.GetCell(3).CellStyle = tbTelStyle;
            IRow rowTl = sheet.GetRow(4);
            rowTl.Height = 20 * 20;
            ICell cellTl = rowTl.GetCell(0);
            cellTl.SetCellValue("申请变更生产任务内容：");
            cellTl.CellStyle.SetFont(tbTitleFont);
            cellTl.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            cellTl.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            IRow tbTl = sheet.GetRow(5);
            tbTl.Height = 30 * 20;
            tbTl.GetCell(0).SetCellValue("序号");
            tbTl.GetCell(1).SetCellValue("设备名称/项目名称");
            tbTl.GetCell(2).SetCellValue("工作内容");
            tbTl.GetCell(3).SetCellValue("单位");
            tbTl.GetCell(4).SetCellValue("计划数量");
            tbTl.GetCell(5).SetCellValue("变更数量");
            tbTl.GetCell(6).SetCellValue("备注");

            IRow tbDuanAp = sheet.GetRow(loopCount);
            tbDuanAp.Height = 80 * 20;
            tbDuanAp.GetCell(0).SetCellValue("技术科审批意见");
            ICell opCell = tbDuanAp.GetCell(2);
            opCell.CellStyle = workBook.CreateCellStyle();
            opCell.CellStyle.Alignment = HorizontalAlignment.Center;
            opCell.CellStyle.VerticalAlignment = VerticalAlignment.Center;

            IRow tbDuanUser = sheet.GetRow(loopCount + 1);
            ICell duanSignCell = tbDuanUser.GetCell(2);
            duanSignCell.SetCellValue("技术科主管（签章）           年    月   日 ");
            duanSignCell.CellStyle = workBook.CreateCellStyle();
            duanSignCell.CellStyle.Alignment = HorizontalAlignment.Right;
            duanSignCell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
            duanSignCell.CellStyle.BorderTop = BorderStyle.Thin;
            duanSignCell.CellStyle.BorderBottom = BorderStyle.Thin;

            IRow approveRow = sheet.CreateRow(loopCount + 2);
            approveRow.Height = 20 * 20;
            ICell icj = approveRow.CreateCell(0);
            icj.SetCellValue("申请车间（盖章）：");
            icj.CellStyle = tbTelStyle;
            ICell apUser = approveRow.CreateCell(2);
            apUser.SetCellValue("审核人：");
            apUser.CellStyle = tbTelStyle;
            ICell user = approveRow.CreateCell(3);
            user.SetCellValue("申请人：");
            user.CellStyle = tbTelStyle;
            approveRow.CreateCell(4);
            ICell date = approveRow.CreateCell(5);
            date.SetCellValue("申请日期：");
            date.CellStyle = tbTelStyle;
            approveRow.CreateCell(6);

            IRow notic = sheet.CreateRow(loopCount + 4);
            notic.Height = 100 * 20;
            ICell notCtn = notic.CreateCell(0);
            StringBuilder sbNote = new StringBuilder("注：\n");
            sbNote.Append("1.本表适用于管内各车间（中心）年月表、重点整治等生产任务变更申请。\n");
            sbNote.Append("2.上述表“序号”、“设备名称 / 项目名称”、“工作内容”、“单位”、“计划数量”列中的内容，要求与原计划内容填写一致。\n");
            sbNote.Append("3.如果变更数量，将变更后的数量填写在：“变更后数量”列中。\n");
            sbNote.Append("4.请将变更的站点信息列在“备注”列中。\n");
            sbNote.Append("5.审核人为车间负责人。");
            ICellStyle notesStyle = workBook.CreateCellStyle();
            notesStyle.WrapText = true;
            notCtn.SetCellValue(sbNote.ToString());
            notCtn.CellStyle = notesStyle;
            notCtn.CellStyle.SetFont(contentFont);
            return workBook;
        }

        #endregion
    }
    public class MonthYearCompare : IEqualityComparer<DailyPlan>
    {
        public bool Equals(DailyPlan x, DailyPlan y)
        {
            if (x == null || y == null)
                return false;
            if (x.PlanId == y.PlanId && x.PlanDate == x.PlanDate && x.PlanType == y.PlanType)
                return true;
            else
                return false;
        }

        public int GetHashCode(DailyPlan obj)
        {
            if (obj == null)
                return 0;
            else
                return obj.PlanId.GetHashCode() ^ obj.PlanDate.GetHashCode() ^ obj.PlanType.GetHashCode();
        }
    }
}
