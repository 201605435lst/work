using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Notice;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

//using SnAbp.File.Services;

namespace SnAbp.File
{
    [DependsOn(
        typeof(SnAbpMessageNoticeModule),
        typeof(FileDomainModule),
        typeof(FileApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class FileApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<FileApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<FileApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });
        }
    }
}