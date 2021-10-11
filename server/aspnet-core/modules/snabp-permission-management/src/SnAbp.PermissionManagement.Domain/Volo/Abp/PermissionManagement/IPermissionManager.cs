using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace SnAbp.PermissionManagement
{
    //TODO: Write extension methods for simple IsGranted check

    public interface IPermissionManager
    {
      //  Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, string providerKey);
        Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, Guid providerGuid);

       // Task<List<PermissionWithGrantedProviders>> GetAllAsync([NotNull] string providerName, [NotNull] string providerKey);
        Task<List<PermissionWithGrantedProviders>> GetAllAsync([NotNull] string providerName, [NotNull] Guid providerGuid);

        Task<bool> IsGranted([NotNull] string providerName, [NotNull] Guid providerGuid);

        //  Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted);
        Task SetAsync(string permissionName, string providerName, Guid providerGuid, bool isGranted);

        //  Task<PermissionGrant> UpdateProviderKeyAsync(PermissionGrant permissionGrant, string providerKey);
        Task<PermissionGrant> UpdateProviderGuidAsync(PermissionGrant permissionGrant, Guid providerGuid);
    }
}