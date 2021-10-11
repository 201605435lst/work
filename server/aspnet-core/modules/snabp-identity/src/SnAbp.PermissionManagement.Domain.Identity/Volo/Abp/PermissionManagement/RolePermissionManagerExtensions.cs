using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SnAbp.PermissionManagement;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement
{
    public static class RolePermissionManagerExtensions
    {
        public static Task<PermissionWithGrantedProviders> GetForRoleAsync([NotNull] this IPermissionManager permissionManager, Guid roleId, string permissionName)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.GetAsync(permissionName, RolePermissionValueProvider.ProviderName, roleId);
        }

        public static Task<List<PermissionWithGrantedProviders>> GetAllForRoleAsync([NotNull] this IPermissionManager permissionManager, Guid roleId)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.GetAllAsync(RolePermissionValueProvider.ProviderName, roleId);
        }

        public static Task SetForRoleAsync([NotNull] this IPermissionManager permissionManager, Guid roleId, [NotNull] string permissionName, bool isGranted)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.SetAsync(permissionName, RolePermissionValueProvider.ProviderName, roleId, isGranted);
        }
    }
}