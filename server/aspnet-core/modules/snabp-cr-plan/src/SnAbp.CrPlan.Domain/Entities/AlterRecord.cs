using SnAbp.CrPlan.Enumer;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 生产任务变更记录表
    /// </summary>
    public class AlterRecord : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 变更原因
        /// </summary>
        [MaxLength(500)]
        public string Reason { get; set; }

        /// <summary>
        /// 原计划时间
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 申请变更时间
        /// </summary>
        public DateTime AlterTime { get; set; }

        /// <summary>
        /// 申请车间 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public YearMonthPlanState State { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public SelectablePlanType AlterType { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 审批批号
        /// </summary>
        public Guid? AR_Key { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        protected AlterRecord() { }
        public AlterRecord(Guid id)
        {
            Id = id;
        }
    }
}
