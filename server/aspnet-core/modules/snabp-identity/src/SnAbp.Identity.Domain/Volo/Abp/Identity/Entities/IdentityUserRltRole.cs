using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SnAbp.Identity
{
    /// <summary>
    /// Represents the link between a user and a role.
    /// </summary>
    public class IdentityUserRltRole : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual Guid RoleId { get; protected set; }

        protected IdentityUserRltRole()
        {
            
        }

        protected internal IdentityUserRltRole(Guid userId, Guid roleId, Guid? tenantId)
        {
            UserId = userId;
            RoleId = roleId;
            TenantId = tenantId;
        }

        public IdentityUserRltRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, RoleId };
        }
    }
}