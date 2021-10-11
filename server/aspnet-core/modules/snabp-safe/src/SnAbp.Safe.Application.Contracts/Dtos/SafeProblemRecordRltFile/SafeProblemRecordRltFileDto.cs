using SnAbp.Safe.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemRecordRltFileDto : EntityDto<Guid> 
    {
        public Guid SafeProblemRecordId { get; set; }
        public virtual SafeProblemRecord SafeProblemRecord { get; set; }
        public Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
    }
}
