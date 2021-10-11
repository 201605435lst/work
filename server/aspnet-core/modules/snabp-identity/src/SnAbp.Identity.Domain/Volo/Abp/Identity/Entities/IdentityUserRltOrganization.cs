using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SnAbp.Identity
{
    /// <summary>
    /// Represents membership of a User to an OU.
    /// </summary>
    public class IdentityUserRltOrganization : CreationAuditedEntity<Guid>, IMultiTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// Id of the related <see cref="Organization"/>.
        /// </summary>
        public virtual Guid OrganizationId { get; protected set; }

        public virtual Organization Organization { get; set; }
        protected IdentityUserRltOrganization()
        {

        }

        public IdentityUserRltOrganization(Guid userId, Guid organizationId, Guid? tenantId = null)
        {
            UserId = userId;
            OrganizationId = organizationId;
            TenantId = tenantId;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, OrganizationId };
        }
    }
}