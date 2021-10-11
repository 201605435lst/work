using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Project
{
    [DependsOn(
        typeof(ProjectApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ProjectHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Project";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ProjectApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
