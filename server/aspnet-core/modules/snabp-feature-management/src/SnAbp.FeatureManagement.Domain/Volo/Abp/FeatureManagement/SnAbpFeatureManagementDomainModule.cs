using Volo.Abp.Caching;
using SnAbp.FeatureManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.FeatureManagement
{
    [DependsOn(
        typeof(SnAbpFeatureManagementDomainSharedModule),
        typeof(AbpFeaturesModule),
        typeof(AbpCachingModule)
        )]
    public class SnAbpFeatureManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<FeatureManagementOptions>(options =>
            {
                options.Providers.Add<DefaultValueFeatureManagementProvider>();
                options.Providers.Add<EditionFeatureManagementProvider>();

                //TODO: Should be moved to the Tenant Management module
                options.Providers.Add<TenantFeatureManagementProvider>();
                options.ProviderPolicies[TenantFeatureValueProvider.ProviderName] = "AbpTenantManagement.Tenants.ManageFeatures";
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("AbpFeatureManagement", typeof(AbpFeatureManagementResource));
            });
        }
    }
}
