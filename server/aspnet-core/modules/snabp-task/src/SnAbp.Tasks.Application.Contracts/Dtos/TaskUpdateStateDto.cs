using SnAbp.Tasks.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Tasks.Dtos
{
    public class TaskUpdateStateDto : FullAuditedEntityDto<Guid>
    {
        public StateType State { get; set; }
    }
}
