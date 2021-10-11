using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CrPlan.Entities
{
    public class WorkTicketRltCooperationUnit: AuditedEntity<Guid>
    {
        public WorkTicketRltCooperationUnit(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 工作票
        /// </summary>
        public WorkTicket WorkTicket { get; set; }
        public Guid WorkTicketId { get; set; }

        /// <summary>
        /// 配合车间
        /// </summary>
        public Guid CooperateWorkShopId { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 主体车间
        /// </summary>
        public Guid MainWorkShopId { get; set; }

        /// <summary>
        /// 配合车间完成情况
        /// </summary>
        public string Completion { get; set; }

        /// <summary>
        /// 配合内容
        /// </summary>
        public string CooperateContent { get; set; }

        /// <summary>
        /// 配合车间实际结束时间
        /// </summary>
        public DateTime CooperateRealFinishTime { get; set; }

        /// <summary>
        /// 配合车间实际开始时间
        /// </summary>
        public DateTime CooperateRealStartTime { get; set; }

        public WorkTicketRltCooperationUnitState State { get; set; }
    }
}
