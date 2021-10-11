using Volo.Abp.Modularity;

namespace SnAbp.Tasks
{
    [DependsOn(
        typeof(TasksDomainSharedModule)
        )]
    public class TasksDomainModule : AbpModule
    {

    }
}
