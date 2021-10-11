using System;

namespace SnAbp.Identity
{
    public class IdentityRoleCreateDto : IdentityRoleCreateOrUpdateDtoBase
    {
        public Guid? OrganizationId { get; set; }
    }
}