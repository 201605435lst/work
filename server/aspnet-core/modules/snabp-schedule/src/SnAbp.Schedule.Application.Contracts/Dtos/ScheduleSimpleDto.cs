using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class ScheduleSimpleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
