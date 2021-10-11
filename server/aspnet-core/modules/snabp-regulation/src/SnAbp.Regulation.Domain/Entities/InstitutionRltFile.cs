using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Regulation.Entities
{
    public  class InstitutionRltFile : Entity<Guid>
    {        
        public virtual Guid InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public virtual Guid? FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        public override object[] GetKeys() => new object[] { this.InstitutionId,this.FileId };
    }
}
