using Volo.Abp.Modularity;

namespace SnAbp.Project
{
    [DependsOn(
        typeof(ProjectDomainSharedModule)
        )]
    public class ProjectDomainModule : AbpModule
    {

    }
}
