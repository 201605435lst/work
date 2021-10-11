using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.EntityFrameworkCore;
using SnAbp.CrPlan.Entities;
//using SnAbp.CrPlan.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    [ConnectionStringName(CrPlanDbProperties.ConnectionStringName)]
    public class CrPlanDbContext : AbpDbContext<CrPlanDbContext>, ICrPlanDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        //public DbSet<Organization> Organization { get; set; }
        /// <summary>
        /// 天窗计划
        /// </summary>
        public DbSet<SkylightPlan> SkylightPlans { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PlanDetail> PlanDetails { get; set; }

        /// <summary>
        /// 设备测试项
        /// </summary>
        public DbSet<EquipmentTestResult> EquipmentTestResult { get; set; }

        /// <summary>
        /// 关联设备
        /// </summary>
        public DbSet<PlanRelateEquipment> PlanRelateEquipment { get; set; }

        /// <summary>
        /// 检修人员
        /// </summary>
        public DbSet<RepairUser> RepairUser { get; set; }

        /// <summary>
        /// 派工单
        /// </summary>
        public DbSet<WorkOrder> WorkOrder { get; set; }

        /// <summary>
        /// 派工单测试项添加附件测试项
        /// </summary>
        public DbSet<WorkOrderTestAdditional> WorkOrderTestAdditional { get; set; }

        /// <summary>
        /// 作业人员
        /// </summary>
        public DbSet<Worker> Worker { get; set; }

        /// <summary>
        /// 作业单位
        /// </summary>
        public DbSet<WorkOrganization> WorkOrganization { get; set; }


        public DbSet<DailyPlan> DailyPlan { get; set; }
        public DbSet<YearMonthPlanAlter> YearMonthPlanAlter { get; set; }

        public DbSet<YearMonthAlterRecord> YearMonthAlterRecord { get; set; }

        public DbSet<YearMonthPlan> YearMonthPlan { get; set; }
        public DbSet<YearMonthPlanTestItem> YearMonthPlanTestItem { get; set; }
        public DbSet<DailyPlanAlter> DailyPlanAlters { get; set; }
        public DbSet<AlterRecord> AlterRecords { get; set; }
        public DbSet<MaintenanceWork> MaintenanceWorks { get; set; }
        public DbSet<MaintenanceWorkRltSkylightPlan> MaintenanceWorkRltSkylightPlans { get; set; }
        public DbSet<MaintenanceWorkRltFile> MaintenanceWorkRltFile { get; set; }

        public DbSet<WorkTicket> WorkTickets { get; set; }

        public DbSet<WorkTicketRltCooperationUnit> WorkTicketRltCooperationUnit{ get;set;}

        public DbSet<SkylightPlanRltWorkTicket> SkylightPlanRltWorkTickets { get; set; }
        public DbSet<SkylightPlanRltInstallationSite> SkylightPlanRltInstallationSites { get; set; }

        public DbSet<StatisticsPieWorker> StatisticsPieWorker { get; set; }
        public DbSet<StatisticsEquipmentWorker> StatisticsEquipmentWorker { get; set; }

        public CrPlanDbContext(DbContextOptions<CrPlanDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCrPlan();
            builder.ConfigureBasic();
        }
    }
}