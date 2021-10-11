using Microsoft.AspNetCore.Authorization;
using SnAbp.Basic.Entities;
using SnAbp.CrPlan.Authorization;
using SnAbp.CrPlan.Dto.PlanTodo;
using SnAbp.CrPlan.Dto.WorkOrder;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Enums;
using SnAbp.CrPlan.IServices.SkylightPlan;
using SnAbp.CrPlan.IServices.UnitTask;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.CrPlan.Services
{
    [Authorize]
    public class CrPlanPlanTodoAppService : CrPlanAppService, ICrPlanPlanTodoAppService
    {
        private readonly IRepository<SkylightPlan, Guid> _skylightPlanRepository;    //天窗计划
        private readonly IRepository<InstallationSite, Guid> _installationSiteRepository;    //机房
        private readonly ICrPlanSkylightPlanAppService _crPlanSkylightPlanService;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;

        public CrPlanPlanTodoAppService(IRepository<SkylightPlan, Guid> skylightPlanRepo,
            IRepository<InstallationSite, Guid> installationSiteRepo,
            ICrPlanSkylightPlanAppService crPlanSkylightPlan,
            IRepository<DataDictionary, Guid> dataDictionaries
            )
        {
            _skylightPlanRepository = skylightPlanRepo;
            _installationSiteRepository = installationSiteRepo;
            _crPlanSkylightPlanService = crPlanSkylightPlan;
            _dataDictionaries = dataDictionaries;
        }

        ///// <summary>
        ///// 获取单位待办列表
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<PagedResultDto<PlanTodoDto>> GetList(SkylightSearchInputDto input)
        //{
        //    PagedResultDto<PlanTodoDto> result = new PagedResultDto<PlanTodoDto>();
        //    if (input == null) return null;
        //    try
        //    {
        //        var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
        //        await Task.Run(() =>
        //        {
        //            var allList = _skylightPlanRepository.Where(x => x.RepairTagId == RepairTagId &&
        //            x.PlanState == PlanState.Publish && (input.PlanType == PlanType.All || x.PlanType == input.PlanType) &&
        //            (input.StartPlanTime == DateTime.MinValue || x.WorkTime >= input.StartPlanTime) &&
        //            (input.EndPlanTime == DateTime.MinValue || x.WorkTime <= input.EndPlanTime) &&
        //            (input.WorkUnitId == Guid.Empty || x.WorkUnit == input.WorkUnitId) &&
        //            (input.InstallationSiteId == Guid.Empty || x.WorkSite == input.InstallationSiteId) &&
        //            (string.IsNullOrEmpty(input.OtherConditions) || (!string.IsNullOrEmpty(x.WorkContent) && x.WorkContent.Contains(input.OtherConditions)) ||
        //            (!string.IsNullOrEmpty(x.WorkArea) && x.WorkArea.Contains(input.OtherConditions)))).OrderBy(m => m.WorkTime).ToList();

        //            if (allList?.Count > 0)
        //            {
        //                result.TotalCount = allList.Count;
        //                var dtos = ObjectMapper.Map<List<SkylightPlan>, List<PlanTodoDto>>(allList);
        //                var resultItems = dtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
        //                //工点、机房名称赋值
        //                foreach (var resItem in resultItems)
        //                {
        //                    var resEnt = allList.Find(x => x.Id == resItem.Id);
        //                    if (resEnt != null)
        //                    {
        //                        resItem.WorkSiteId = resEnt.WorkSite;
        //                        var installSite = _installationSiteRepository.Where(x => x.Id == resItem.WorkSiteId).FirstOrDefault();
        //                        resItem.WorkSiteName = installSite?.Name;
        //                    }
        //                }
        //                result.Items = resultItems;
        //            }
        //        });
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}

        /// <summary>
        /// 根据天窗计划组织派工单详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(CrPlanPermissions.WorkOrder.Create)]
        public async Task<WorkOrderDto> GetWorkOrderByPlanId(CommonGuidGetDto input)
        {
            try
            {
                var skylightPlan = await _crPlanSkylightPlanService.Get(new CommonGuidGetDto { Id = input.Id, RepairTagKey = input.RepairTagKey });
                if (skylightPlan == null) return null;
                WorkOrderDto result = new WorkOrderDto();
                result.Id = Guid.NewGuid();
                result.InfluenceScope = skylightPlan.Incidence;
                result.PlanDetailList = skylightPlan.PlanDetails;
                result.SkylightPlanId = skylightPlan.Id;
                //result.StartPlanTime = DateTime.Parse(skylightPlan.WorkTime.ToString("yyyy-MM-dd"));
                //result.EndPlanTime = DateTime.Parse(skylightPlan.WorkTime.ToString("yyyy-MM-dd"));
                result.StartPlanTime = skylightPlan.WorkTime;
                result.EndPlanTime = skylightPlan.WorkTime.AddMinutes(skylightPlan.TimeLength);
                if (skylightPlan.WorkContentType == WorkContentType.MonthYearPlan)
                {
                    result.WorkContentType = skylightPlan.WorkContentType;
                    result.PlanDetailList.ForEach(x =>
                    {
                        x.WorkSiteIds = skylightPlan.WorkSiteIds;
                        x.WorkSiteName = skylightPlan.WorkSiteName;
                    });
                }
                if (skylightPlan.WorkContentType == WorkContentType.OtherPlan)
                {
                    result.WorkContentType = skylightPlan.WorkContentType;
                    result.WorkContent = skylightPlan.WorkContent;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

    }
}
