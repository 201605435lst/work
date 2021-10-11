using Microsoft.Extensions.DependencyInjection;

using SnAbp.Message.Bpm.Services;

using Volo.Abp.Modularity;

namespace SnAbp.Message.Bpm
{
    [DependsOn(
        typeof(BpmDomainSharedModule)
        )]
    public class BpmDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
