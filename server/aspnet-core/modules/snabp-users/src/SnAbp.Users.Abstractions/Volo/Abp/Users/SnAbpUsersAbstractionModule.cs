using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace SnAbp.Users
{
    //TODO: Consider to (somehow) move this to the framework to the same assemblily of ICurrentUser!

    [DependsOn(
        typeof(AbpMultiTenancyModule),
        typeof(AbpEventBusModule)
        )]
    public class SnAbpUsersAbstractionModule : AbpModule
    {

    }
}
