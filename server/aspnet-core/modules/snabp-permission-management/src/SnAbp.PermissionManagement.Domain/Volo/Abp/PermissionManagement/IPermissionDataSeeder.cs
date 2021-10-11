using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnAbp.PermissionManagement
{
    public interface IPermissionDataSeeder
    {
        //Task SeedAsync(
        //    string providerName,
        //    string providerKey,
        //    IEnumerable<string> grantedPermissions,
        //    Guid? tenantId = null
        //);

        Task SeedAsync(
            string providerName,
            Guid providerGuid,
            IEnumerable<string> grantedPermissions,
            Guid? tenantId = null
        );
    }
}
