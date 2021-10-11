using Volo.Abp.Modularity;

namespace SnAbp.FileApprove
{
    [DependsOn(
        typeof(FileApproveApplicationModule),
        typeof(FileApproveDomainTestModule)
        )]
    public class FileApproveApplicationTestModule : AbpModule
    {

    }
}
