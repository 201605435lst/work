using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using SnAbp.Identity;
using Volo.Abp.MultiTenancy;
using SnAbp.PermissionManagement;

namespace SnAbp.PermissionManagement.Identity
{
    public class RolePermissionManagementProvider : PermissionManagementProvider
    {
        public override string Name => RolePermissionValueProvider.ProviderName;

        protected IUserRoleFinder UserRoleFinder { get; }

        public RolePermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant, 
            IUserRoleFinder userRoleFinder)
            : base(
                permissionGrantRepository,
                guidGenerator,
                currentTenant)
        {
            UserRoleFinder = userRoleFinder;
        }

        public override async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, Guid providerGuid)
        {
            if (providerName == Name)
            {
                return new PermissionValueProviderGrantInfo(
                    await PermissionGrantRepository.FindAsync(name, providerName, providerGuid) != null,
                    providerGuid
                );
            }

            if (providerName == UserPermissionValueProvider.ProviderName)
            {
                var userId = providerGuid;
                var roleNames = await UserRoleFinder.GetRolesAsync(userId);

                foreach (var roleName in roleNames)
                {
                    var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, Guid.Parse(roleName));
                    if (permissionGrant != null)
                    {
                        return new PermissionValueProviderGrantInfo(true, Guid.Parse(roleName));
                    }
                }
            }

            return PermissionValueProviderGrantInfo.NonGranted;
        }
    }
}