using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Entities
{
    public class ProjectRltFile : FullAuditedEntity<Guid>
    {
        public ProjectRltFile(Guid id) => Id = id;

        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public Project Project { get; set; }
        public virtual Guid? ProjectId { get; set; }
    }
}
