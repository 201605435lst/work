using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.FileApprove
{
    [DependsOn(
        typeof(FileApproveApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class FileApproveHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "FileApprove";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(FileApproveApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
