using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Schedule.Dtos
{
    public class ScheduleFlowTemplateDto : Entity<Guid>
    {
        public virtual Guid WorkflowTemplateId { get; set; }
    }
}
