using Volo.Abp.Modularity;

namespace SnAbp.FeatureManagement
{
    [DependsOn(
        typeof(SnAbpFeatureManagementApplicationModule),
        typeof(SnAbpFeatureManagementDomainTestModule)
        )]
    public class FeatureManagementApplicationTestModule : AbpModule
    {

    }
}
