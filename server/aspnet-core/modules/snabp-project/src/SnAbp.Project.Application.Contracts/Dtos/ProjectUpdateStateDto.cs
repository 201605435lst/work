using SnAbp.Project.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Project.Dtos
{
    public class ProjectUpdateStateDto :FullAuditedEntityDto<Guid>
    {
        public ProjectState State { get; set; }
    }
}
