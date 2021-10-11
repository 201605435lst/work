using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Identity
{
    [DependsOn(
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class SnAbpIdentityHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpIdentityApplicationContractsModule).Assembly,
                IdentityRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}