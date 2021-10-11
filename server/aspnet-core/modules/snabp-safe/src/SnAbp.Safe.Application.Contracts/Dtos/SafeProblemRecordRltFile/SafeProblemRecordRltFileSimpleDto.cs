using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemRecordRltFileSimpleDto : EntityDto<Guid> 
    {
        public Guid SafeProblemRecordId { get; set; }
        public Guid FileId { get; set; }
    }
}
