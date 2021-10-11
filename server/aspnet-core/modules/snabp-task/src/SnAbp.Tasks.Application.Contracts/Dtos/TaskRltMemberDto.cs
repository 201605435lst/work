using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Tasks.Dtos
{
    public class TaskRltMemberDto
    {
        public IdentityUserDto Manager { get; set; }
        public Guid ManagerId { get; set; }
    }
}
