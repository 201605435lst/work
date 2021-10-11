using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Tasks
{
    [DependsOn(
        typeof(TasksDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class TasksApplicationContractsModule : AbpModule
    {

    }
}
