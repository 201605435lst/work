using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 计划变更关联表
    /// </summary>
    public class DailyPlanAlter : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 变更表id
        /// </summary>
        public Guid AlterRecordId { get; set; }

        /// <summary>
        /// 日计划id
        /// </summary>
        public Guid DailyId { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 变更后数量
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal AlterCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public DateTime AlterTime { get; set; }

        protected DailyPlanAlter() { }
        public DailyPlanAlter(Guid id)
        {
            Id = id;
        }
    }
}
