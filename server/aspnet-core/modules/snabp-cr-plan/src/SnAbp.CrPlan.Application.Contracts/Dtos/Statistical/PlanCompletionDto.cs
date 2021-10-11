using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 计划实体
    /// </summary>
    public class PlanCompletionDto : IRepairTagDto
    {
        /// <summary>
        /// 计划日期
        /// </summary>
        public string PlanTime { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 是否上月变更
        /// </summary>
        public bool IsLastMonthChange { get; set; }

        /// <summary>
        /// 计划明细
        /// </summary>
        public List<SkylightPlanDailyCompletionDto> SkylightPlanDailyCompletionList { get; set; }

        /// <summary>
        /// 计划合计
        /// </summary>
        public PlanDailyTotalDto PlanDailyTotal { get; set; }

        /// <summary>
        /// 变更明细
        /// </summary>
        public List<PlanChangeDto> PlanChangeList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
