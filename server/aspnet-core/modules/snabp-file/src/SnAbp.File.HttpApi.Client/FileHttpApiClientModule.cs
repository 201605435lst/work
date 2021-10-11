using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.File
{
    [DependsOn(
        typeof(FileApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class File2HttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "File2";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(FileApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}