using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using SnAbp.Message.Notice;

namespace SnAbp.Report
{
    [DependsOn(
        typeof(ReportDomainModule),
        typeof(ReportApplicationContractsModule),
        typeof(SnAbpMessageNoticeModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class ReportApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<ReportApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ReportApplicationModule>();
            });
        }
    }
}
