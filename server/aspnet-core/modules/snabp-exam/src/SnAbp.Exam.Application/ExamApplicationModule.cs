using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
//using SnAbp.Exam.Services;

namespace SnAbp.Exam
{
    [DependsOn(
        typeof(ExamDomainModule),
        typeof(ExamApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class ExamApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<ExamApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ExamApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });
            
        }
    }
}
