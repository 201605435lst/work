using System;
using System.Text.Json;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SnAbp.Identity
{
    /// <summary>
    /// Represents membership of a User to an OU.
    /// </summary>
    public class OrganizationRltRole : FullAuditedEntity<Guid>, IMultiTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// Id of the Role.
        /// </summary>
        public virtual Guid RoleId { get; protected set; }

        /// <summary>
        /// Id of the <see cref="Organization"/>.
        /// </summary>
        public virtual Guid OrganizationId { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationRltRole"/> class.
        /// </summary>
        protected OrganizationRltRole()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationRltRole"/> class.
        /// </summary>
        /// <param name="tenantId">TenantId</param>
        /// <param name="roleId">Id of the User.</param>
        /// <param name="organizationId">Id of the <see cref="Organization"/>.</param>
        public OrganizationRltRole(Guid roleId, Guid organizationId, Guid? tenantId = null)
        {
            RoleId = roleId;
            OrganizationId = organizationId;
            TenantId = tenantId;
        }

        //public override object[] GetKeys()
        //{
        //    return new object[] { OrganizationId, RoleId };
        //}
    }
}