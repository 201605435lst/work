using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Resource.Entities
{
    public class EquipmentRltOrganization : Entity<Guid>
    {
        public Guid EquipmentId { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public EquipmentRltOrganization(Guid id)
        {
            Id = id;
        }
    }
}
