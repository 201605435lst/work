using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.CrPlan
{
    [DependsOn(
        typeof(CrPlanApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class CrPlanHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "CrPlan";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CrPlanApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
