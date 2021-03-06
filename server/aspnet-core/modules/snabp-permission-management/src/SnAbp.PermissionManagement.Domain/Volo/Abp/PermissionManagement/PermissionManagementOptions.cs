using System.Collections.Generic;
using Volo.Abp.Collections;

namespace SnAbp.PermissionManagement
{
    public class PermissionManagementOptions
    {
        //TODO: rename to Providers
        public ITypeList<IPermissionManagementProvider> ManagementProviders { get; }

        public Dictionary<string, string> ProviderPolicies { get; }

        public PermissionManagementOptions()
        {
            ManagementProviders = new TypeList<IPermissionManagementProvider>();
            ProviderPolicies = new Dictionary<string, string>();
        }
    }
}
