using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Services;
using Volo.Abp.Modularity;

namespace SnAbp.Message
{
    [DependsOn(
        typeof(MessageDomainSharedModule)
    )]
    public class MessageDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<MessageManager>();
        }
    }
}