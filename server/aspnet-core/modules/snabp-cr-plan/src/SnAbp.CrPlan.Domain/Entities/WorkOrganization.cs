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
    /// 作业单位实体
    /// </summary>
    public class WorkOrganization : Entity<Guid>, IRepairTag
    {
        /// <summary>
        /// 派工单ID
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// 作业单位
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 类型
        /// 0-检修，1-验收
        /// </summary>
        public Duty Duty { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }



        public WorkOrganization() { }
        public WorkOrganization(Guid id) { Id = id; }
    }
}
