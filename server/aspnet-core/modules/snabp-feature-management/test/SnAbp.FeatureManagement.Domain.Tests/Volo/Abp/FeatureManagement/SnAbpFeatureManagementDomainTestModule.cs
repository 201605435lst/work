using SnAbp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.FeatureManagement
{
    [DependsOn(
        typeof(SnAbpFeatureManagementEntityFrameworkCoreTestModule)
        )]
    public class SnAbpFeatureManagementDomainTestModule : AbpModule
    {
        
    }
}
