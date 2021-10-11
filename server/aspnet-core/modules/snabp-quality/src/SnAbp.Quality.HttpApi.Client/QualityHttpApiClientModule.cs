using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Quality
{
    [DependsOn(
        typeof(QualityApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class QualityHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Quality";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(QualityApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
