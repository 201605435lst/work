using SnAbp.Safe.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemRltCcUserDto : EntityDto<Guid> 
    {
        public Guid SafeProblemId { get; set; }
        public SafeProblem SafeProblem { get; set; }
        public Guid CcUserId { get; set; }
        public virtual Identity.IdentityUser CcUser { get; set; }
    }
}
