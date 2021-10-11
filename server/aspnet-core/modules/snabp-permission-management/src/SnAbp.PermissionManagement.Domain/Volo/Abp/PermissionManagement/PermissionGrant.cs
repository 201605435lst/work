using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SnAbp.PermissionManagement
{
    //TODO: To aggregate root?
    public class PermissionGrant : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        public virtual string ProviderName { get; protected set; }

        // [CanBeNull]
        [CanBeNull] public virtual string ProviderKey { get; protected internal set; }

        /// <summary>
        /// 提供者id，Easten新增，用于对原生授权模式进行改造。
        /// </summary>
        [CanBeNull]
        public virtual Guid ProviderGuid { get; protected internal set; }

        protected PermissionGrant()
        {

        }

        public PermissionGrant(
            Guid id,
            [NotNull] string name,
            [NotNull] string providerName ,
            [CanBeNull] string providerKey,
            [CanBeNull] Guid providerGuid,
            Guid? tenantId = null)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
            ProviderKey = providerKey;
            TenantId = tenantId;
            ProviderGuid = providerGuid;
        }

        public PermissionGrant(
            Guid id,
            [NotNull] string name,
            [NotNull] string providerName,
            [CanBeNull] string providerKey,
            Guid? tenantId = null)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
            ProviderKey = providerKey;
            TenantId = tenantId;
        }
    }
}
