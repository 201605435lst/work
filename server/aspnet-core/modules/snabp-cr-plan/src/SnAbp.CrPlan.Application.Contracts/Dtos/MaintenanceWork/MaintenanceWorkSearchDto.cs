
using SnAbp.Bpm;
using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    public class MaintenanceWorkSearchDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 发起单位
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 工作流状态 审批状态
        /// </summary>
        public WorkflowState WorkflowState { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public String PlanTime { get; set; }

        public string RepairTagKey { get; set; }
    }
}
