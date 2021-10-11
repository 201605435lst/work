using Volo.Abp.Modularity;

namespace SnAbp.Message.Email
{
    [DependsOn(
        typeof(EmailDomainSharedModule)
        )]
    public class EmailDomainModule : AbpModule
    {

    }
}
