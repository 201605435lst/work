using Volo.Abp.Modularity;

namespace SnAbp.Message.IOT
{
    [DependsOn(
        typeof(IOTDomainSharedModule)
        )]
    public class IOTDomainModule : AbpModule
    {

    }
}
