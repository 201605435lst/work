using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Schedule.Entities
{
    public class ScheduleFlowTemplate : Entity<Guid>
    {
        public ScheduleFlowTemplate(Guid id) => Id = id;
        public Guid WorkflowTemplateId { get; set; }
    }
}
