using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
  public  class SafeProblemRltEquipmentSimpleDto : EntityDto<Guid>
    {
        public Guid SafeProblemId { get; set; }
        public Guid EquipmentId { get; set; }
    }
}
