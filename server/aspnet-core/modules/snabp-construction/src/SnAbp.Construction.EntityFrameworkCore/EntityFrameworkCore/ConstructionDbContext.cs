using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Construction.Plans;
using SnAbp.ConstructionBase.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Project.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;
using SnAbp.Construction.Entities;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.Technology.EntityFrameworkCore;
using SnAbp.Safe.EntityFrameworkCore;
using SnAbp.Quality.EntityFrameworkCore;

namespace SnAbp.Construction.EntityFrameworkCore
{
    [ConnectionStringName(ConstructionDbProperties.ConnectionStringName)]
    public class ConstructionDbContext : AbpDbContext<ConstructionDbContext>, IConstructionDbContext
    {

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
                            
        public DbSet<DispatchTemplate> DispatchTemplates { get; set; }

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
        public ConstructionDbContext(DbContextOptions<ConstructionDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureConstruction();
            builder.ConfigureConstructionBase();
			builder.ConfigureFile();
			builder.ConfigureProject();
			builder.ConfigureIdentity();
            builder.ConfigureSafe();
            builder.ConfigureQuality();
            builder.ConfigureBpm();
            builder.ConfigureResource();
            builder.ConfigureTechnology();
        }

    }
}
