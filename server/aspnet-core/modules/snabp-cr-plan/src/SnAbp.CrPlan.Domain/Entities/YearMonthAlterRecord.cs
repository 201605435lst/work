using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CrPlan.Entities
{
    public class YearMonthAlterRecord : AuditedEntity<Guid>
    {
        ///// <summary>
        ///// 变更计划
        ///// </summary>
        //public Guid PlanAlterId { get; set; }
        //public YearMonthPlanAlter PlanAlter { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public int State { get; set; }
        
        /// <summary>
        /// 计划类型
        /// </summary>
        public int PlanType { get; set; }

        public Guid? ARKey { get; set; }

        public YearMonthAlterRecord() { }

        public YearMonthAlterRecord(Guid id)
        {
            Id = id;
        }

        public Guid OrganizationId { get; set; }


        public string ChangeReason { get; set; }
    }
}
