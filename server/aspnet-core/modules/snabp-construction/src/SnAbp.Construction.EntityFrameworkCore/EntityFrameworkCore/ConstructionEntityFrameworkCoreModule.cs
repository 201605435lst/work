using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Construction.Entities;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Construction.MasterPlans.IRepositories;
using SnAbp.Construction.Plans;
using SnAbp.Construction.Repositories;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Construction.EntityFrameworkCore
{
    [DependsOn(
        typeof(ConstructionDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ConstructionEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ConstructionDbContext>(options =>
            {
                /* Add custom repositories here. Example: 
		         * options.AddRepository<Question, EfCoreQuestionRepository>();
		         */
                options.Services.AddTransient<IMasterPlanContentRepository, MasterPlanContentRepository>();



                options.AddDefaultRepositories<IConstructionDbContext>(true);
                // 配置 总体计划 关联 
                options.Entity<MasterPlan>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(e => e.Charger)
                    .Include(e => e.Workflow)
                );
                // 配置 总体计划详情 关联 
                options.Entity<MasterPlanContent>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(e => e.SubItemContent)
                    .Include(e => e.MasterPlan)
                    .Include(e => e.Antecedents).ThenInclude(a => a.MasterPlanContent)
                    .Include(e => e.Parent)
                    .Include(e => e.Children)
                );

                // 配置 施工计划 关联 
                options.Entity<Plan>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(e => e.MasterPlan)
                    .Include(e => e.Charger)
                    .Include(e => e.Workflow)
                    .Include(e => e.PlanRltWorkflowInfos)
                );
                // 配置 施工计划详情 关联 
                options.Entity<PlanContent>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(e => e.SubItemContent)
                    .Include(e => e.Antecedents).ThenInclude(a => a.PlanContent)
                    .Include(e => e.Files).ThenInclude(a => a.File)
                    .Include(e => e.Parent)
                    .Include(e => e.Children)
                    .Include(e => e.PlanMaterials).ThenInclude(i => i.PlanMaterialRltEquipments).ThenInclude(d => d.Equipment).ThenInclude(a => a.Group)
                );

                // 配置 施工计划工程量 关联 
                options.Entity<PlanMaterial>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(planMaterial => planMaterial.PlanMaterialRltEquipments).ThenInclude(e => e.Equipment).ThenInclude(e => e.ComponentCategory)
                    .Include(planMaterial => planMaterial.PlanMaterialRltEquipments).ThenInclude(e => e.Equipment).ThenInclude(e => e.ProductCategory)
                    .Include(planMaterial => planMaterial.PlanMaterialRltEquipments).ThenInclude(e => e.Equipment).ThenInclude(e => e.Group)
                );


                // 配置 施工计划工程量 关联 设备  关联 
                options.Entity<PlanMaterialRltEquipment>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(planMaterialRltEquipment => planMaterialRltEquipment.Equipment)
                    .Include(planMaterialRltEquipment => planMaterialRltEquipment.PlanMaterial)

                );


                // 派工管理
                options.Entity<Dispatch>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Contractor)
                    .Include(x => x.Creator)
                    .Include(x => x.DispatchTemplate)
                    .Include(x => x.DispatchRltFiles).ThenInclude(y => y.File)

                );
                // 施工日志
                options.Entity<Daily>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Dispatch).ThenInclude(x => x.DispatchRltPlanContents).ThenInclude(y => y.PlanContent).ThenInclude(s => s.PlanMaterials).ThenInclude(m => m.PlanMaterialRltEquipments).ThenInclude(n => n.Equipment).ThenInclude(d => d.ComponentCategory)
                    .Include(x => x.Dispatch).ThenInclude(x => x.DispatchRltPlanContents).ThenInclude(y => y.PlanContent).ThenInclude(s => s.PlanMaterials).ThenInclude(m => m.PlanMaterialRltEquipments).ThenInclude(n => n.Equipment).ThenInclude(d => d.ProductCategory)
                    .Include(x => x.Informant)
                    .Include(x => x.UnplannedTask)
                    .Include(x => x.DailyTemplate)
                    .Include(x => x.DailyRltSafe).ThenInclude(y => y.SafeProblem).ThenInclude(z => z.Checker)
                    .Include(x => x.DailyRltSafe).ThenInclude(y => y.SafeProblem).ThenInclude(z => z.Type)
                    .Include(x => x.DailyRltQuality).ThenInclude(y => y.QualityProblem).ThenInclude(z => z.Checker)
                    .Include(x => x.DailyRltFiles).ThenInclude(y => y.File)
                    .Include(x => x.DailyRltPlan).ThenInclude(y => y.PlanMaterial)
                );

                // 配置 派工单模板 关联计划内容 
                options.Entity<DispatchRltPlanContent>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(y => y.PlanContent).ThenInclude(s => s.PlanMaterials).ThenInclude(m => m.PlanMaterialRltEquipments).ThenInclude(n => n.Equipment).ThenInclude(d => d.ComponentCategory)
                    .Include(y => y.PlanContent).ThenInclude(s => s.PlanMaterials).ThenInclude(m => m.PlanMaterialRltEquipments).ThenInclude(n => n.Equipment).ThenInclude(d => d.ProductCategory)

                );
                // 配置 派工单模板 关联材料 
                options.Entity<DispatchRltMaterial>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(y => y.Material)
                );
                // 配置 派工单模板 关联工序指引
                options.Entity<DispatchRltStandard>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(y => y.Standard)
                );

                // 配置 派工单模板 关联施工人员
                options.Entity<DispatchRltWorker>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(y => y.Worker)
                );

                // 配置 派工单模板 关联施工人员
                options.Entity<DispatchRltSection>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(y => y.Section)
                );

            });
        }
    }
}
