using SnAbp.Bpm;
using SnAbp.CrPlan.Dtos;
using System;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class ApprovalPlanAlterInputDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 变更计划id
        /// </summary>
        //public Guid AlterRecordId { get; set; }

        /// <summary>
        /// 审批流程Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WorkflowState State { get; set; }
        public string RepairTagKey { get ; set ; }
    }
}
