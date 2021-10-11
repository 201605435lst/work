using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CrPlan.Entities
{
    public class WorkOrderTestAdditional : AuditedEntity<Guid>
    {
        /// <summary>
        /// 派工作业id
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// 测试项序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 测试项附加内容
        /// </summary>
        public string TestConctent { get; set; }

        public WorkOrderTestAdditional()
        {
        }

        public WorkOrderTestAdditional(Guid id)
        {
            Id = id;
        }
    }
}
