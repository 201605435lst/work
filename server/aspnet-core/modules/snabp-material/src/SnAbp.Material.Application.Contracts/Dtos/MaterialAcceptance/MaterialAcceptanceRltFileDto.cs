using SnAbp.File.Dtos;
using SnAbp.Material.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Entities
{
    public class MaterialAcceptanceRltFileDto : EntityDto<Guid>
    {
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptanceDto MaterialAcceptance { get; set; }

        public virtual Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }
    }
}
