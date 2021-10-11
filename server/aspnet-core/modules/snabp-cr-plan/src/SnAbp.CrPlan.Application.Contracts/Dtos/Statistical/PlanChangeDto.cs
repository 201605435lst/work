using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 计划变更统计实体
    /// </summary>
    public class PlanChangeDto : IRepairTagDto
    {
        /// <summary>
        /// 变更数量
        /// </summary>
        public decimal ChangeCount { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public YearMonthPlanState ApprovalStatus { get; set; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public string ChangeTime { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string ChangeReason { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
