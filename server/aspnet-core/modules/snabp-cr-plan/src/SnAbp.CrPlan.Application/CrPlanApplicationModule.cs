using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp;
using SnAbp.Bpm;
using SnAbp.Bpm.Services;
using IoFile = System.IO.File;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace SnAbp.CrPlan
{
    [DependsOn(
        typeof(CrPlanDomainModule),
        typeof(CrPlanApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(BpmDomainModule),
        //typeof(AbpBackgroundJobsQuartzModule),
        typeof(AbpBackgroundWorkersQuartzModule)
        )]
    public class CrPlanApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CrPlanApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CrPlanApplicationModule>();
                options.Configurators.Add(ctx => {
                });
            });

            //Configure<AbpBackgroundWorkersQuartzModule>(options =>
            //{
            //    options.
            //});

        }

        //public override void PreConfigureServices(ServiceConfigurationContext context)
        //{
        //    var configuration = context.Services.GetConfiguration();

        //    PreConfigure<AbpQuartzOptions>(options =>
        //    {
        //        options.Configurator = configure =>
        //        {
        //            configure.UsePersistentStore(storeOptions =>
        //            {
        //                storeOptions.UseProperties = true;
        //                //storeOptions.UseJsonSerializer();
        //                //storeOptions.UseSqlServer(configuration.GetConnectionString("Quartz"));
        //                //storeOptions.UseClustering(c =>
        //                //{
        //                //    c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
        //                //    c.CheckinInterval = TimeSpan.FromSeconds(10);
        //                //});
        //                //storeOptions.Properties.co
        //            });
        //        };
        //    });
        //}



        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPostApplicationInitialization(context);

            //var bpmManager = context.ServiceProvider.GetService<BpmManager>();

            //var workflowTemplateRoot = "workflow_templates/";
            //var pathCrPlan = workflowTemplateRoot + "cr-plan/";

            //var key = "YearPlanExam";
            //var jsonString = IoFile.ReadAllText(pathCrPlan + key + ".json");
            //_=bpmManager.RegisterWorkflowTemplate(key, jsonString).Result;

            //key = "MonthPlanExam";
            //jsonString = IoFile.ReadAllText(pathCrPlan + key + ".json");
            //_=bpmManager.RegisterWorkflowTemplate(key, jsonString).Result;

            ////����¶ȼƻ�
            //key = "MonthOfYearPlanExam";
            //jsonString = IoFile.ReadAllText(pathCrPlan + key + ".json");
            //_=bpmManager.RegisterWorkflowTemplate(key, jsonString).Result;

            ////������
            //key = "YearChangeExam";
            //jsonString = IoFile.ReadAllText(pathCrPlan + key + ".json");
            //_=bpmManager.RegisterWorkflowTemplate(key, jsonString).Result;

            ////�±����
            //key = "MonthChangeExam";
            //jsonString = IoFile.ReadAllText(pathCrPlan + key + ".json");
            //_=bpmManager.RegisterWorkflowTemplate(key, jsonString).Result;

            ////�ƻ����
            //key = "PlanChange";
            //jsonString = IoFile.ReadAllText(pathCrPlan + key + ".json");
            //_=bpmManager.RegisterWorkflowTemplate(key, jsonString).Result;

            //////ά����-1
            ////key = "MaintenanceWork";
            ////jsonString = IoFile.ReadAllText(pathCrPlan + key + ".json");
            ////_=bpmManager.RegisterWorkflowTemplate(key, jsonString).Result;
        }
    }
}
