using Microsoft.EntityFrameworkCore;

using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.Technology.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Technology.EntityFrameworkCore
{
    [ConnectionStringName(TechnologyDbProperties.ConnectionStringName)]
    public class TechnologyDbContext : AbpDbContext<TechnologyDbContext>, ITechnologyDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public TechnologyDbContext(DbContextOptions<TechnologyDbContext> options)
            : base(options)
        {

        }
      
        public DbSet<Disclose> Disclose { get; set; }

        public DbSet<ConstructInterface> ConstructInterface { get; set; }

        public DbSet<ConstructInterfaceInfo> ConstructInterfaceInfo { get; set; }

        public DbSet<ConstructInterfaceInfoRltMarkFile> ConstructInterfaceInfoRltMarkFile { get; set; }

        // 材料维护 相关
        public DbSet<Material> Material { get; set; }
        public DbSet<MaterialPlan> MaterialPlan { get; set; }
        public DbSet<MaterialPlanFlowInfo> MaterialPlanFlowInfo { get; set; }
        public DbSet<MaterialPlanRltMaterial> MaterialPlanRltMaterial { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureTechnology();
            builder.ConfigureIdentity();
            builder.ConfigureResource();
        }
    }
}