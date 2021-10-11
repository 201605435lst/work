using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Schedule.Entities
{
    public class ScheduleRltSchedule : AuditedEntity<Guid>
    {
        public ScheduleRltSchedule(Guid id) => Id = id;

        public Schedule Schedule { get; set; }
        public virtual Guid ScheduleId { get; set; }

        /// <summary>
        /// 前置计划
        /// </summary>
        public virtual Guid FrontScheduleId { get; set; }
    }
}
