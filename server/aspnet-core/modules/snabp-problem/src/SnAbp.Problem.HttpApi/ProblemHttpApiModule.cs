using Localization.Resources.AbpUi;
using SnAbp.Problem.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.Problem
{
    [DependsOn(
        typeof(ProblemApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ProblemHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ProblemHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IServiceCollection service = context.Services;
            service.AddScoped<IAbpServiceConvention, SnAbpServiceConvention>();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ProblemResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
