using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 年月表日计划
    /// </summary>
    public class DailyPlan : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 年月表计划主键
        /// </summary>
        public Guid PlanId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PlanDate { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Count { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public int PlanType { get; set; }

        /// <summary>
        /// 状态    1为变更通过后 添加进日计划的数据
        /// </summary>
        public int State { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        protected DailyPlan() { }
        public DailyPlan(Guid id)
        {
            Id = id;
        }
    }
}
