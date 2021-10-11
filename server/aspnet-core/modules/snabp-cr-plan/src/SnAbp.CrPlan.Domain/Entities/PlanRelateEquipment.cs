using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 天窗关联设备实体
    /// </summary>
    public class PlanRelateEquipment : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 计划编号
        /// </summary>
        public Guid PlanDetailId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public Guid? EquipmentId { get; set; }

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

        /// <summary>
        /// 是否完成
        /// 0：未做 / 1：合格  /  2：不合格
        /// </summary>
        public AcceptanceResults IsComplete { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public PlanRelateEquipment() { }
        public PlanRelateEquipment(Guid id) { Id = id; }


    }
}
