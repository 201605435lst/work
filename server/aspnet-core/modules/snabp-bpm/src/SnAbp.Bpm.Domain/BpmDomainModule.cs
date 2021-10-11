using SnAbp.Message.Bpm;
using Volo.Abp.Modularity;

namespace SnAbp.Bpm
{
    [DependsOn(
        typeof(BpmDomainSharedModule)
        )]
    public class BpmDomainModule : AbpModule
    {

    }
}
