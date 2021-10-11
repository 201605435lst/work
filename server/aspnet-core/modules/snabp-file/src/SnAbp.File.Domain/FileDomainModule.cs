using Microsoft.Extensions.DependencyInjection;
using SnAbp.File.OssSdk;
using Volo.Abp.Modularity;

namespace SnAbp.File
{
    [DependsOn(
        typeof(FileDomainSharedModule)
    )]
    public class FileDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var service = context.Services;
            service.AddTransient<IOssRepository, OssRepository>();
            //base.ConfigureServices(context);
        }
    }
}