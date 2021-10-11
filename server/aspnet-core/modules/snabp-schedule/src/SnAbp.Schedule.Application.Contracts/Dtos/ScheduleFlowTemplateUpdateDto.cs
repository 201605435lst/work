using SnAbp.Bpm;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class ScheduleFlowTemplateUpdateDto : EntityDto<Guid>
    {
        public virtual Guid WorkflowTemplateId { get; set; }
    }
}
