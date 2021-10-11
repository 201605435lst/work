using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Dtos
{
   public class ProjectRltMemberDto
    {
        public IdentityUserDto Manager { get; set; }
        public Guid ManagerId { get; set; }
    }
}
