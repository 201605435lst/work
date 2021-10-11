using SnAbp.Resource.Entities;
using SnAbp.Safe.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
   public class SafeProblemRltEquipmentDto : EntityDto<Guid>
    {
        public Guid SafeProblemId { get; set; }
        public SafeProblem SafeProblem { get; set; }
        public Guid EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
    }
}
