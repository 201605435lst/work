using Volo.Abp.Modularity;

namespace SnAbp.Tasks
{
    [DependsOn(
        typeof(TasksApplicationModule),
        typeof(TasksDomainTestModule)
        )]
    public class TasksApplicationTestModule : AbpModule
    {

    }
}
