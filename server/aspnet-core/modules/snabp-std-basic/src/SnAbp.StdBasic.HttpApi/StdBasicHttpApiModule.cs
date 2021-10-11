using Localization.Resources.AbpUi;
using SnAbp.StdBasic.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.StdBasic
{
    [DependsOn(
        typeof(StdBasicApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class StdBasicHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(StdBasicHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IServiceCollection service = context.Services;
            service.AddScoped<IAbpServiceConvention, SnAbpServiceConvention>();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<StdBasicResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
