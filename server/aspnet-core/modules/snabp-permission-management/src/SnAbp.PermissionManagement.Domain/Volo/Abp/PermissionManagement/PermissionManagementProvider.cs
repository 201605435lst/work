using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace SnAbp.PermissionManagement
{
    public abstract class PermissionManagementProvider : IPermissionManagementProvider
    {
        public abstract string Name { get; }

        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IGuidGenerator GuidGenerator { get; }

        protected ICurrentTenant CurrentTenant { get; }

        protected PermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            PermissionGrantRepository = permissionGrantRepository;
            GuidGenerator = guidGenerator;
            CurrentTenant = currentTenant;
        }

        #region 原有方法

        public virtual async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
        {
            if (providerName != Name)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            return new PermissionValueProviderGrantInfo(
                await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null,
                providerKey
            );
        }

        public virtual Task SetAsync(string name, string providerKey, bool isGranted)
        {
            return isGranted
                ? GrantAsync(name, providerKey)
                : RevokeAsync(name, providerKey);
        }

        protected virtual async Task GrantAsync(string name, string providerKey)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (permissionGrant != null)
            {
                return;
            }

            await PermissionGrantRepository.InsertAsync(
                new PermissionGrant(
                    GuidGenerator.Create(),
                    name,
                    Name,
                    providerKey,
                    CurrentTenant.Id
                )
            );
        }

        protected virtual async Task RevokeAsync(string name, string providerKey)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (permissionGrant == null)
            {
                return;
            }

            await PermissionGrantRepository.DeleteAsync(permissionGrant);
        }
        #endregion

        public virtual async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, Guid providerGuid)
        {
            if (providerName != Name)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            return new PermissionValueProviderGrantInfo(
                await PermissionGrantRepository.FindAsync(name, providerName, providerGuid) != null,
                providerGuid
            );
        }
      
        public virtual Task SetAsync(string name, Guid providerGuid, bool isGranted)
        {
            return isGranted
                ? GrantAsync(name, providerGuid)
                : RevokeAsync(name, providerGuid);
        }

        protected virtual async Task GrantAsync(string name, Guid providerGuid)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerGuid);
            if (permissionGrant != null)
            {
                return;
            }

            await PermissionGrantRepository.InsertAsync(
                new PermissionGrant(
                    GuidGenerator.Create(),
                    name,
                    Name,
                    string.Empty,
                    providerGuid,
                    CurrentTenant.Id
                )
            );
        }

        protected virtual async Task RevokeAsync(string name, Guid providerGuid)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerGuid);
            if (permissionGrant == null)
            {
                return;
            }

            await PermissionGrantRepository.DeleteAsync(permissionGrant);
        }

    }
}