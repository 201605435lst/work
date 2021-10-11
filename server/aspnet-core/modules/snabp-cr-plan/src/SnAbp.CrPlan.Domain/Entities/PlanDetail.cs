using SnAbp.Identity;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 计划详细信息
    /// </summary>
    public class PlanDetail : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 天窗计划ID
        /// </summary>
        public Guid SkylightPlanId { get; set; }
        /// <summary>
        /// 日计划ID
        /// </summary>
        public Guid DailyPlanId { get; set; }

        /// <summary>
        /// 影响范围
        /// </summary>
        public Guid? InfluenceRangeId { get; set; }
        public virtual InfluenceRange InfluenceRange { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal PlanCount { get; set; }
        /// <summary>
        /// 作业数量
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal WorkCount { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public PlanDetail(Guid id)
        {
            Id = id;
        }
    }
}
