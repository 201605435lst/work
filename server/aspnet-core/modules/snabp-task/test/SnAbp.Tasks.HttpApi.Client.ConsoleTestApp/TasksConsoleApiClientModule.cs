using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Tasks
{
    [DependsOn(
        typeof(TasksHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class TasksConsoleApiClientModule : AbpModule
    {
        
    }
}
