using SnAbp.MultiProject.MultiProject;
using SnAbp.Tasks;
using SnAbp.Tasks.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Tasks.Entities
{
    public class TaskRltFile : FullAuditedEntity<Guid>
    {
        public TaskRltFile(Guid id) => Id = id;

        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public Task Task { get; set; }
        public virtual Guid? TaskId { get; set; }

        public FileType FileType { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
