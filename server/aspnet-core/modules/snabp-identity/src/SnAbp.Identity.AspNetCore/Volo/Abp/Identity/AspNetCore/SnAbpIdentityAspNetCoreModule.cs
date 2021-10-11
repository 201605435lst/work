using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace SnAbp.Identity.AspNetCore
{
    [DependsOn(
        typeof(SnAbpIdentityDomainModule)
        )]
    public class SnAbpIdentityAspNetCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IdentityBuilder>(builder =>
            {
                builder.AddDefaultTokenProviders();
                builder.AddSignInManager();
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //(TODO: Extract an extension method like IdentityBuilder.AddAbpSecurityStampValidator())
            context.Services.AddScoped<SnAbpSecurityStampValidator>();
            context.Services.AddScoped(typeof(SecurityStampValidator<IdentityUser>), provider => provider.GetService(typeof(SnAbpSecurityStampValidator)));
            context.Services.AddScoped(typeof(ISecurityStampValidator), provider => provider.GetService(typeof(SnAbpSecurityStampValidator)));

            var options = context.Services.ExecutePreConfiguredActions(new SnAbpIdentityAspNetCoreOptions());

            if (options.ConfigureAuthentication)
            {
                context.Services
                    .AddAuthentication(o =>
                    {
                        o.DefaultScheme = IdentityConstants.ApplicationScheme;
                        o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                    })
                    .AddIdentityCookies();
            }
        }
    }
}