using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SnAbp.Identity
{
    public class IdentityUserDto : ExtensibleFullAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
    {
        public Guid? TenantId { get; set; }

        public List<Guid> ProjectIds { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }
        public virtual Guid? PositionId { get; set; }
        public virtual DataDictionary Position { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public string ConcurrencyStamp { get; set; }

        public List<IdentityUserRltOrganization> Organizations { get; set; }

        public List<IdentityRoleDto> Roles { get; set; }

        public bool IsChangePassword { get; set; }
    }
}