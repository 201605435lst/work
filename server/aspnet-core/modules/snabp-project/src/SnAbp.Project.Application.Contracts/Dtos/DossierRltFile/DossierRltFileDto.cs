using SnAbp.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Dtos
{
    public class DossierRltFileDto : EntityDto<Guid>
    {

        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public virtual Dossier Dossier { get; set; }
        public virtual Guid? DossierId { get; set; }
    }
}
