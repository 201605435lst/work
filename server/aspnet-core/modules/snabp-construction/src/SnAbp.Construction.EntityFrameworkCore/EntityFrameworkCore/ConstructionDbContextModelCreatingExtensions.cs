using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.Entities;
using SnAbp.Construction.Entities;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Construction.Plans;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Construction.EntityFrameworkCore
{
    public static class ConstructionDbContextModelCreatingExtensions
    {
        public static void ConfigureConstruction( this ModelBuilder builder,
        Action<ConstructionModelBuilderConfigurationOptions> optionsAction =
        null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ConstructionModelBuilderConfigurationOptions(
            ConstructionDbProperties.DbTablePrefix,
            ConstructionDbProperties.DbSchema);

            optionsAction?.Invoke(options);

			builder.Entity<PlanMaterial>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(PlanMaterial), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<PlanMaterialRltEquipment>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(PlanMaterialRltEquipment), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<PlanContentRltFile>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(PlanContentRltFile), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<PlanContent>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(PlanContent), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
				b.HasOne(p => p.Plan)
					.WithOne(d => d.PlanContent)
					.HasForeignKey<PlanContent>(p => p.PlanId)
					.HasConstraintName("FK_Plan_PlanContent"); // 因为 外键约束名太长 放不下导致重名,这里改一下约束名,不然无法迁移 
			});


			builder.Entity<Plan>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(Plan), options.Schema);
				b.ConfigureByConvention();
				// b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<PlanContentRltMaterial>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(PlanContentRltMaterial), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<PlanContentRltAntecedent>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(PlanContentRltAntecedent), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
				b.HasOne(x => x.PlanContent).WithMany().HasForeignKey(x => x.PlanContentId).HasConstraintName("FK_Main");
				b.HasOne(x => x.FrontPlanContent).WithMany().HasForeignKey(x => x.FrontPlanContentId).HasConstraintName("FK_Rlt");
			});


			builder.Entity<PlanRltWorkflowInfo>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(PlanRltWorkflowInfo), options.Schema);
				b.ConfigureByConvention();
				// b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<MasterPlanRltContentRltAntecedent>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(MasterPlanRltContentRltAntecedent), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});


			builder.Entity<MasterPlanContent>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(MasterPlanContent), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
				b.HasOne(p => p.MasterPlan)
					.WithOne(d => d.MasterPlanContent)
					.HasForeignKey<MasterPlanContent>(p => p.MasterPlanId)
					.HasConstraintName("FK_MasterPlan_MasterPlanContent"); // 因为 外键约束名太长 放不下导致重名,这里改一下约束名,不然无法迁移 
			});

            

			builder.Entity<MasterPlanRltWorkflowInfo>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(MasterPlanRltWorkflowInfo), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			});



			builder.Entity<MasterPlan>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(MasterPlan), options.Schema);
				b.ConfigureByConvention();
			});
			builder.Entity<MasterPlanRltContentRltAntecedent>(b =>
			{
				b.ToTable(options.TablePrefix + nameof(MasterPlanRltContentRltAntecedent), options.Schema);
				b.ConfigureByConvention();
				b.HasOne(x => x.MasterPlanContent).WithMany().HasForeignKey(x => x.MasterPlanContentId).HasConstraintName("FK_Main");
				b.HasOne(x => x.FrontMasterPlanContent).WithMany().HasForeignKey(x => x.FrontMasterPlanContentId).HasConstraintName("FK_Rlt");
			});

			builder.Entity<DispatchTemplate>(b =>{b.ToTable(options.TablePrefix + nameof(DispatchTemplate), options.Schema);b.ConfigureByConvention();b.ConfigureFullAudited();});
			builder.Entity<Dispatch>(b => {b.ToTable(options.TablePrefix + nameof(Dispatch), options.Schema);b.ConfigureByConvention();});
			builder.Entity<DispatchRltFile>(b => {b.ToTable(options.TablePrefix + nameof(DispatchRltFile), options.Schema);b.ConfigureByConvention();});
			builder.Entity<DispatchRltMaterial>(b => {b.ToTable(options.TablePrefix + nameof(DispatchRltMaterial), options.Schema);b.ConfigureByConvention();});
			builder.Entity<DispatchRltPlanContent>(b => {b.ToTable(options.TablePrefix + nameof(DispatchRltPlanContent), options.Schema);b.ConfigureByConvention();});
			builder.Entity<DispatchRltWorkFlow>(b => {b.ToTable(options.TablePrefix + nameof(DispatchRltWorkFlow), options.Schema);b.ConfigureByConvention();});
			builder.Entity<DispatchRltSection>(b => {b.ToTable(options.TablePrefix + nameof(DispatchRltSection), options.Schema);b.ConfigureByConvention();});
			builder.Entity<DispatchRltStandard>(b => {b.ToTable(options.TablePrefix + nameof(DispatchRltStandard), options.Schema);b.ConfigureByConvention();});
			builder.Entity<DispatchRltWorker>(b => {b.ToTable(options.TablePrefix + nameof(DispatchRltWorker), options.Schema);b.ConfigureByConvention();});

			builder.Entity<Daily>(b => { b.ToTable(options.TablePrefix + nameof(Daily), options.Schema); b.ConfigureByConvention(); });
			builder.Entity<DailyRltFile>(b => { b.ToTable(options.TablePrefix + nameof(DailyRltFile), options.Schema); b.ConfigureByConvention(); });
			builder.Entity<DailyRltQuality>(b => { b.ToTable(options.TablePrefix + nameof(DailyRltQuality), options.Schema); b.ConfigureByConvention(); });
			builder.Entity<DailyRltSafe>(b => { b.ToTable(options.TablePrefix + nameof(DailyRltSafe), options.Schema); b.ConfigureByConvention(); });
			builder.Entity<DailyTemplate>(b => { b.ToTable(options.TablePrefix + nameof(DailyTemplate), options.Schema); b.ConfigureByConvention(); b.ConfigureFullAudited(); });
			builder.Entity<UnplannedTask>(b => { b.ToTable(options.TablePrefix + nameof(UnplannedTask), options.Schema); b.ConfigureByConvention(); });
			builder.Entity<DailyRltPlanMaterial>(b => { b.ToTable(options.TablePrefix + nameof(DailyRltPlanMaterial), options.Schema); b.ConfigureByConvention(); });
			builder.Entity<DailyFlowInfo>(b => { b.ToTable(options.TablePrefix + nameof(DailyFlowInfo), options.Schema); b.ConfigureByConvention(); });

		}
	}
}
