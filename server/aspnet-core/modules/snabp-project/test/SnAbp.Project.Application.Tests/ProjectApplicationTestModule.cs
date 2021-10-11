using Volo.Abp.Modularity;

namespace SnAbp.Project
{
    [DependsOn(
        typeof(ProjectApplicationModule),
        typeof(ProjectDomainTestModule)
        )]
    public class ProjectApplicationTestModule : AbpModule
    {

    }
}
