using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Report
{
    [DependsOn(
        typeof(ReportApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ReportHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Report";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ReportApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
