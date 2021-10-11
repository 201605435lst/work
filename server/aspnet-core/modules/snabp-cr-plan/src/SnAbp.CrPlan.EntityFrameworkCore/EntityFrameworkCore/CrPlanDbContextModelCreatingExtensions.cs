using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Settings;
using SnAbp.CrPlan.Entities;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Settings;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    public static class CrPlanDbContextModelCreatingExtensions
    {
        public static void ConfigureCrPlan(
            this ModelBuilder builder,
            Action<CrPlanModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CrPlanModelBuilderConfigurationOptions(
                CrPlanDbProperties.DbTablePrefix,
                CrPlanDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<DailyPlan>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(DailyPlan), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<YearMonthPlan>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(YearMonthPlan), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<YearMonthPlanAlter>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(YearMonthPlanAlter), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<YearMonthAlterRecord>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(YearMonthAlterRecord), options.Schema);
                b.ConfigureByConvention();
            });

            //天窗计划
            builder.Entity<SkylightPlan>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(SkylightPlan), options.Schema);
                b.ConfigureByConvention();
            });

            //计划详细信息
            builder.Entity<PlanDetail>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(PlanDetail), options.Schema);
                b.ConfigureByConvention();
            });

            //设备测试项
            builder.Entity<EquipmentTestResult>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EquipmentTestResult), options.Schema);
                b.ConfigureByConvention();
            });

            //关联设备
            builder.Entity<PlanRelateEquipment>(b =>
         {
             b.ToTable(options.TablePrefix + nameof(PlanRelateEquipment), options.Schema);
             b.ConfigureByConvention();
         });

            //设备检修人员
            builder.Entity<RepairUser>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RepairUser), options.Schema);
                b.ConfigureByConvention();
            });
            //派工单
            builder.Entity<WorkOrder>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkOrder), options.Schema);
                b.ConfigureByConvention();
            });

            /// 派工单测试项添加附件测试项
            builder.Entity<WorkOrderTestAdditional>(b => {
                b.ToTable(options.TablePrefix + nameof(WorkOrderTestAdditional), options.Schema);
                b.ConfigureByConvention();
            });


            //作业人员
            builder.Entity<Worker>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Worker), options.Schema);
                b.ConfigureByConvention();
            });

            //作业单位
            builder.Entity<WorkOrganization>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkOrganization), options.Schema)
                    .HasIndex(x=> new { x.OrganizationId, x.RepairTagId });

                b.ConfigureByConvention();
            });
            builder.Entity<YearMonthPlanTestItem>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(YearMonthPlanTestItem), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<AlterRecord>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(AlterRecord), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<DailyPlanAlter>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(DailyPlanAlter), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });

            builder.Entity<Equipment>(b =>
            {
                b.ToTable(ResourceSettings.DbTablePrefix + nameof(Equipment), ResourceSettings.DbSchema);
            });

            builder.Entity<Organization>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(Organization), SystemSettings.DbSchema);
                b.ConfigureByConvention();
            });
            builder.Entity<Railway>(b =>
            {
                b.ToTable(BasicSettings.DbTablePrefix + nameof(Railway), BasicSettings.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + "Users", SnAbpIdentityDbProperties.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<MaintenanceWork>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MaintenanceWork), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<MaintenanceWorkRltSkylightPlan>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MaintenanceWorkRltSkylightPlan), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<MaintenanceWorkRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MaintenanceWorkRltFile), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<WorkTicket>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkTicket), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<WorkTicketRltCooperationUnit>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkTicketRltCooperationUnit), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<SkylightPlanRltWorkTicket>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(SkylightPlanRltWorkTicket), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<SkylightPlanRltInstallationSite>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(SkylightPlanRltInstallationSite), options.Schema);
                b.ConfigureByConvention();
            });

            //图表统计饼状图
            builder.Entity<StatisticsPieWorker>(b => {
                b.ToTable(options.TablePrefix + nameof(StatisticsPieWorker), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<StatisticsEquipmentWorker>(b => {
                b.ToTable(options.TablePrefix + nameof(StatisticsEquipmentWorker), options.Schema);
                b.ConfigureByConvention();
            });
        }
    }
}