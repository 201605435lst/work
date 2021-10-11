using Localization.Resources.AbpUi;
using SnAbp.Bpm.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.Bpm
{
    [DependsOn(
        typeof(BpmApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class BpmHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(BpmHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IServiceCollection service = context.Services;
            service.AddScoped<IAbpServiceConvention, SnAbpServiceConvention>();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BpmResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
