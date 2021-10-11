using Volo.Abp.Modularity;

namespace SnAbp.FileApprove
{
    [DependsOn(
        typeof(FileApproveDomainSharedModule)
        )]
    public class FileApproveDomainModule : AbpModule
    {

    }
}
