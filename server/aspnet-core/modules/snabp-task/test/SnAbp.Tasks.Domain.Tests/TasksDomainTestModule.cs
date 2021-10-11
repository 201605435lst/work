using SnAbp.Tasks.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Tasks
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(TasksEntityFrameworkCoreTestModule)
        )]
    public class TasksDomainTestModule : AbpModule
    {
        
    }
}
