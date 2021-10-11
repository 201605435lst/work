using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Project.Dtos
{
    public class DossierRltFileCreateDto : EntityDto<Guid>
    {
        public virtual Guid? FileId { get; set; }

        public virtual Guid? DossierId { get; set; }
    }
}
