using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos.Equipment_Org
{
    public class EquipmentRltOrganizationCreateDto : EntityDto<Guid>
    {
        public Guid OrganizationId { get; set; }
    }
}
