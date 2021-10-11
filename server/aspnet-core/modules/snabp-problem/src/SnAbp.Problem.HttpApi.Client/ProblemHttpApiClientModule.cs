using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Problem
{
    [DependsOn(
        typeof(ProblemApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ProblemHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Problem";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ProblemApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
