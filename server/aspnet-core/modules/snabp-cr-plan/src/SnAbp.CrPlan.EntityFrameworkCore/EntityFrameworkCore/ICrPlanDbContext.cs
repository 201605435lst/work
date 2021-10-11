using Microsoft.EntityFrameworkCore;
using SnAbp.CrPlan.Entities;
//using SnAbp.CrPlan.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    [ConnectionStringName(CrPlanDbProperties.ConnectionStringName)]
    public interface ICrPlanDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        //DbSet<Organization> Organization { get; set; }

        DbSet<SkylightPlan> SkylightPlans { get; set; }

        DbSet<PlanDetail> PlanDetails { get; set; }

        /// <summary>
        /// �豸��������ʵ��
        /// </summary>
        DbSet<EquipmentTestResult> EquipmentTestResult { get; set; }

        /// <summary>
        /// �����豸
        /// </summary>
        DbSet<PlanRelateEquipment> PlanRelateEquipment { get; set; }
        /// <summary>
        /// �����豸���ޡ�������Ա
        /// </summary>
        DbSet<RepairUser> RepairUser { get; set; }

        /// <summary>
        /// �ɹ���
        /// </summary>
        DbSet<WorkOrder> WorkOrder { get; set; }

        /// <summary>
        /// 派工作业测试项添加附加项
        /// </summary>
        DbSet<WorkOrderTestAdditional> WorkOrderTestAdditional { get; set; }

        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        DbSet<Worker> Worker { get; set; }

        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        DbSet<WorkOrganization> WorkOrganization { get; set; }
        DbSet<DailyPlan> DailyPlan { get; set; }
        DbSet<YearMonthPlan> YearMonthPlan { get; set; }
        DbSet<YearMonthPlanAlter> YearMonthPlanAlter { get; set; }
        DbSet<YearMonthAlterRecord> YearMonthAlterRecord { get; set; }
        DbSet<YearMonthPlanTestItem> YearMonthPlanTestItem { get; set; }
        DbSet<DailyPlanAlter> DailyPlanAlters { get; set; }
        DbSet<AlterRecord> AlterRecords { get; set; }
        DbSet<MaintenanceWork> MaintenanceWorks { get; set; }
        DbSet<MaintenanceWorkRltSkylightPlan> MaintenanceWorkRltSkylightPlans { get; set; }
        DbSet<MaintenanceWorkRltFile> MaintenanceWorkRltFile { get; set; }
        DbSet<WorkTicket> WorkTickets { get; set; }
        DbSet<WorkTicketRltCooperationUnit> WorkTicketRltCooperationUnit { get; set; }
        DbSet<SkylightPlanRltWorkTicket> SkylightPlanRltWorkTickets { get; set; }
        DbSet<SkylightPlanRltInstallationSite> SkylightPlanRltInstallationSites { get; set; }
        DbSet<StatisticsPieWorker> StatisticsPieWorker { get; set; }

    }
}