using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace SnAbp.Message
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(MessageDomainModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpAspNetCoreSignalRModule))]
    public class MessageHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}