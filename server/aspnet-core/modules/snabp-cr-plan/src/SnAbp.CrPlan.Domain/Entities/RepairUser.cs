using SnAbp.CrPlan.Enums;
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
    /// 天窗关联设备检修验收人员实体
    /// </summary>
    public class RepairUser : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 关联派工单Id
        /// </summary>
        public Guid? WorkerOrderId { get; set; }

        /// <summary>
        /// 关联设备表ID
        /// </summary>
        public Guid? PlanRelateEquipmentId { get; set; }

        /// <summary>
        /// 作业人员
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 类型
        /// 0-检修，1-验收
        /// </summary>
        public Duty Duty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }
        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public RepairUser() { }
        public RepairUser(Guid id) { Id = id; }

    }
}
