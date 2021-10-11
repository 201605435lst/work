using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Schedule.Entities
{
    public class ScheduleFlowInfo : SingleFlowRltEntity
    {
        public ScheduleFlowInfo(Guid id) => Id = id;
        public virtual Guid ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
