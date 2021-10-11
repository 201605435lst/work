using SnAbp.CostManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.CostManagement
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(CostManagementEntityFrameworkCoreTestModule)
        )]
    public class CostManagementDomainTestModule : AbpModule
    {
        
    }
}
