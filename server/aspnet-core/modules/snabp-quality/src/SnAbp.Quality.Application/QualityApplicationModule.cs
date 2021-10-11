using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using SnAbp.Message.Notice;

namespace SnAbp.Quality
{
    [DependsOn(
        typeof(QualityDomainModule),
        typeof(QualityApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(SnAbpMessageNoticeModule),
        typeof(AbpAutoMapperModule)
        )]
    public class QualityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<QualityApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<QualityApplicationModule>(validate: false);
            });
        }
    }
}
