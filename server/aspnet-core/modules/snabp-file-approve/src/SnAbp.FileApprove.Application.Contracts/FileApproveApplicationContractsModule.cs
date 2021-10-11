using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.FileApprove
{
    [DependsOn(
        typeof(FileApproveDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class FileApproveApplicationContractsModule : AbpModule
    {

    }
}
