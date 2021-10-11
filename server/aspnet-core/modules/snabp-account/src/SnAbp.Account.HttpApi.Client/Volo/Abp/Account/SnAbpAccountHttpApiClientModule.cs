using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Account
{
    [DependsOn(
        typeof(SnAbpAccountApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class SnAbpAccountHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(SnAbpAccountApplicationContractsModule).Assembly, 
                AccountRemoteServiceConsts.RemoteServiceName);
        }
    }
}