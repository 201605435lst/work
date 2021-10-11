using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Message.Bpm
{
    [DependsOn(
        typeof(BpmDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class BpmApplicationContractsModule : AbpModule
    {

    }
}
