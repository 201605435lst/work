using Microsoft.EntityFrameworkCore;
using SnAbp.Construction.Entities;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Construction.Plans;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Construction.EntityFrameworkCore
{
    [ConnectionStringName(ConstructionDbProperties.ConnectionStringName)]
    public interface IConstructionDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

		public DbSet<MasterPlan> MasterPlans { get; set; }

		public DbSet<MasterPlanRltWorkflowInfo> MasterPlanRltWorkflowInfos { get; set; }

		public DbSet<MasterPlanContent> MasterPlanContents { get; set; }

		public DbSet<MasterPlanRltContentRltAntecedent> MasterPlanRltContentRltAntecedents { get; set; }

		public DbSet<PlanRltWorkflowInfo> PlanRltWorkflowInfos { get; set; }

		public DbSet<PlanContentRltAntecedent> PlanContentRltAntecedents { get; set; }

		public DbSet<PlanContentRltMaterial> PlanContentRltMaterials { get; set; }

		public DbSet<Plan> Plans { get; set; }

		public DbSet<PlanContent> PlanContents { get; set; }

		public DbSet<PlanContentRltFile> PlanContentRltFiles { get; set; }

		public DbSet<PlanMaterialRltEquipment> PlanMaterialRltEquipments { get; set; }

		public DbSet<PlanMaterial> PlanMaterials { get; set; }
                      
                
        //�ɹ���ģ��
        public DbSet<DispatchTemplate> DispatchTemplates { get; set; }

        //�ɹ���
        public DbSet<Dispatch> Dispatchs { get; set; }
        public DbSet<DispatchRltFile> DispatchRltFiles { get; set; }
        public DbSet<DispatchRltMaterial> DispatchRltMaterials { get; set; }
        public DbSet<DispatchRltPlanContent> DispatchRltPlanContents { get; set; }
        public DbSet<DispatchRltSection> DispatchRltSections { get; set; }
        public DbSet<DispatchRltStandard> DispatchRltStandards { get; set; }
        public DbSet<DispatchRltWorker> DispatchRltWorkers { get; set; }
        public DbSet<DispatchRltWorkFlow> DispatchRltWorkFlows { get; set; }



        public DbSet<Daily> Daily { get; set; }
        public DbSet<DailyRltFile> DailyRltFile { get; set; }
        public DbSet<DailyRltQuality> DailyRltQuality { get; set; }
        public DbSet<DailyRltSafe> DailyRltSafe { get; set; }

        public DbSet<DailyTemplate> DailyTemplate { get; set; }
        public DbSet<UnplannedTask> UnplannedTask { get; set; }
        public DbSet<DailyRltPlanMaterial> DailyRltPlanMaterial { get; set; }
        public DbSet<DailyFlowInfo> DailyFlowInfo { get; set; }

    }

}
