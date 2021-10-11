using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Entities
{
    public class DossierRltFile : FullAuditedEntity<Guid>
    {
        public DossierRltFile(Guid id) => Id = id;

        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public virtual Dossier Dossier { get; set; }
        public virtual Guid? DossierId { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
