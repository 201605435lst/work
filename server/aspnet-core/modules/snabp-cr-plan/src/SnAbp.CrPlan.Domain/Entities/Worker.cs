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
    /// 作业人员实体
    /// </summary>
    public class Worker : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 派工单ID
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// 作业人员
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 人员职责
        /// 0-作业成员，1-作业组长，2-现场防护员，3-驻站联络员
        /// </summary>
        public UserDuty Duty { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public Worker() { }
        public Worker(Guid id) { Id = id; }
    }
}
