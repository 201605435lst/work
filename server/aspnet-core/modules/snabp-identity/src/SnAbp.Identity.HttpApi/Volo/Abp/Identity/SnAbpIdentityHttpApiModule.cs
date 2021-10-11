using Volo.Abp.AspNetCore.Mvc;
using SnAbp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.Identity
{
    [DependsOn(typeof(AbpIdentityApplicationContractsModule), typeof(AbpAspNetCoreMvcModule))]
    public class SnAbpIdentityHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpIdentityHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IServiceCollection service = context.Services;
            service.AddScoped<IAbpServiceConvention, SnAbpServiceConvention>();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<IdentityResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
            //Configure<AbpLocalizationOptions>(options =>
            //{
            //    options.Resources
            //        .Get<IdentityResource>()
            //        .AddBaseTypes(
            //            typeof(AbpUiResource)
            //        );
            //});
        }
    }
}