using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 天窗计划完成合计实体
    /// </summary>
    public class PlanDailyTotalDto : IRepairTagDto
    {
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }


        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishCount { get; set; }


        /// <summary>
        /// 未完成数量
        /// </summary>
        public decimal UnFinishedCount { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
