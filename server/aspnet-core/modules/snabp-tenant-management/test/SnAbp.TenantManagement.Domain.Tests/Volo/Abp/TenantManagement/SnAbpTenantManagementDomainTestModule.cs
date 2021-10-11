using Volo.Abp.Modularity;
using SnAbp.TenantManagement.EntityFrameworkCore;

namespace SnAbp.TenantManagement
{
    [DependsOn(
        typeof(SnAbpTenantManagementEntityFrameworkCoreTestModule),
        typeof(SnAbpTenantManagementTestBaseModule))]
    public class SnAbpTenantManagementDomainTestModule : AbpModule
    {

    }
}
