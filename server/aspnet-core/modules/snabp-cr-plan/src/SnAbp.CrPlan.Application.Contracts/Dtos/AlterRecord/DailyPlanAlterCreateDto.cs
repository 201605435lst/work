using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class DailyPlanAlterCreateDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 日计划id
        /// </summary>
        public Guid DailyId { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 变更后数量
        /// </summary>
        public decimal AlterCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string RepairTagKey { get ; set ; }

        public DateTime AlterDateTime { get; set; }
    }
}
