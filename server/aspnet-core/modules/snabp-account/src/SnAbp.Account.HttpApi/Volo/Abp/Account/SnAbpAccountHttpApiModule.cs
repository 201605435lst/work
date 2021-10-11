using Localization.Resources.AbpUi;
using SnAbp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc;
using SnAbp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.Account
{
    [DependsOn(
        typeof(SnAbpAccountApplicationContractsModule),
        typeof(SnAbpIdentityHttpApiModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class SnAbpAccountHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpAccountHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IServiceCollection service = context.Services;
            service.AddScoped<IAbpServiceConvention, SnAbpServiceConvention>();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AccountResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}