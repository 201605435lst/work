using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
  public  class SafeProblemRltFileSimpleDto : EntityDto<Guid>
    {
        public virtual Guid SafeProblemId { get; set; }
        public virtual Guid FileId { get; set; }
    }
}
