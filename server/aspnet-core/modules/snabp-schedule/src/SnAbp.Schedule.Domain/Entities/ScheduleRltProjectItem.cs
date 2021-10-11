using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Schedule.Entities
{
    public class ScheduleRltProjectItem : Entity<Guid>
    {
        public ScheduleRltProjectItem(Guid id) => Id = id;
        public virtual Guid ProjectItemId { get; set; }
        public virtual Guid ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        public override object[] GetKeys() => new object[] { ProjectItemId, ScheduleId };
    }
}
