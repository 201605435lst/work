using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Identity.AspNetCore;
using SnAbp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Account.Web
{
    [DependsOn(
        typeof(SnAbpAccountWebModule),
        typeof(SnAbpIdentityServerDomainModule)
        )]
    public class SnAbpAccountWebIdentityServerModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<SnAbpIdentityAspNetCoreOptions>(options =>
            {
                options.ConfigureAuthentication = false;
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpAccountWebIdentityServerModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpAccountWebIdentityServerModule>("SnAbp.Account.Web");
            });

            //TODO: Try to reuse from AbpIdentityAspNetCoreModule
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
