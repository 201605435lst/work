using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace SnAbp.FileApprove
{
    [DependsOn(
        typeof(FileApproveDomainModule),
        typeof(FileApproveApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class FileApproveApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<FileApproveApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<FileApproveApplicationModule>();
            });
        }
    }
}
