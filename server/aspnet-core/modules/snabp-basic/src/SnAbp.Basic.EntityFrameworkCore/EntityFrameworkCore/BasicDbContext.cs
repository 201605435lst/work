using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.Entities;
//using SnAbp.Basic.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Basic.EntityFrameworkCore
{
    [ConnectionStringName(BasicDbProperties.ConnectionStringName)]
    public class BasicDbContext : AbpDbContext<BasicDbContext>, IBasicDbContext
    {
        public DbSet<Railway> Railway { get; set; }
        public DbSet<RailwayRltOrganization> RailwayRltOrganization { get; set; }
        public DbSet<StationRltRailway> StationRltRailway { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<StationRltOrganization> StationRltOrganization { get; set; }
        public DbSet<InstallationSite> InstallationSite { get; set; }

        public BasicDbContext(DbContextOptions<BasicDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBasic();
        }
    }
}