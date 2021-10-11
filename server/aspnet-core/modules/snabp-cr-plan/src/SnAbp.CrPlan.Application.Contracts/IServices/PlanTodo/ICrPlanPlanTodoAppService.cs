using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Dto.PlanTodo;
using SnAbp.CrPlan.Dto.WorkOrder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using SnAbp.CrPlan.Dtos;

namespace SnAbp.CrPlan.IServices.UnitTask
{
    public interface ICrPlanPlanTodoAppService : IApplicationService
    {
        ///// <summary>
        ///// 获取单位待办列表
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task<PagedResultDto<PlanTodoDto>> GetList(SkylightSearchInputDto input);
        /// <summary>
        /// 根据天窗计划组织派工单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkOrderDto> GetWorkOrderByPlanId(CommonGuidGetDto input);
    }
}
