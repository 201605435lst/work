using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentRltOrganizationDto : EntityDto<Guid>
    {
        public Guid EquipmentId { get; set; }
        public Guid? OrganizationId { get; set; }
        public OrganizationDto? Organization { get; set; }

    }
}
