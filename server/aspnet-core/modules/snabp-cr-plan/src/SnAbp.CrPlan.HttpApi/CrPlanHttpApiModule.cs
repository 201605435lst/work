using Localization.Resources.AbpUi;
using SnAbp.CrPlan.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.CrPlan
{
    [DependsOn(
        typeof(CrPlanApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class CrPlanHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(CrPlanHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IServiceCollection service = context.Services;
            service.AddScoped<IAbpServiceConvention, SnAbpServiceConvention>();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<CrPlanResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
