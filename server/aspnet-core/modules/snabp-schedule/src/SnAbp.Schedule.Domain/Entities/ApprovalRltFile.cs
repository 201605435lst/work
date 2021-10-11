using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Schedule.Entities
{
    public class ApprovalRltFile : Entity<Guid>
    {
        public ApprovalRltFile(Guid id) => Id = id;
        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public Approval Approval { get; set; }
        public virtual Guid? ApprovalId { get; set; }
    }
}
