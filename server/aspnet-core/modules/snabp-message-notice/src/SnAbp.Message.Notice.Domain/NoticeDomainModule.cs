using Volo.Abp.Modularity;

namespace SnAbp.Message.Notice
{
    [DependsOn(
        typeof(NoticeDomainSharedModule)
        )]
    public class NoticeDomainModule : AbpModule
    {

    }
}
