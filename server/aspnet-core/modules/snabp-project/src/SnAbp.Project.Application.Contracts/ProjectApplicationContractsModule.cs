using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Project
{
    [DependsOn(
        typeof(ProjectDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class ProjectApplicationContractsModule : AbpModule
    {

    }
}
