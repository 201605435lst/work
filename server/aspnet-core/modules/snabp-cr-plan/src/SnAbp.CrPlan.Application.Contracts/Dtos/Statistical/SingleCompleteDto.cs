using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.Statistical
{
    public class SingleCompleteDto : IRepairTagDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }


        /// <summary>
        /// 已完成数量
        /// </summary>
        public decimal FinishCount { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
