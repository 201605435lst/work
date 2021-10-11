using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 垂直天窗与工作票的关联
    /// </summary>
    public class SkylightPlanRltWorkTicket : Entity<Guid>
    {
        /// <summary>
        /// 垂直天窗id
        /// </summary>
        public Guid SkylightPlanId { get; set; }
        public virtual SkylightPlan SkylightPlan { get; set; }

        /// <summary>
        /// 工作票id
        /// </summary>
        public Guid WorkTicketId { get; set; }
        public virtual WorkTicket WorkTicket { get; set; }

        public SkylightPlanRltWorkTicket(Guid id)
        {
            Id = id;
        }

    }
}
