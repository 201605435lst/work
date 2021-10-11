using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Exam
{
    [DependsOn(
        typeof(ExamApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ExamHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Exam";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ExamApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
