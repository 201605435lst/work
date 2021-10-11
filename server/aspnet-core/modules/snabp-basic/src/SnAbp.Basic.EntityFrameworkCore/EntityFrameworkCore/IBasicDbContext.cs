using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Basic.EntityFrameworkCore
{
    [ConnectionStringName(BasicDbProperties.ConnectionStringName)]
    public interface IBasicDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<Railway> Railway { get; set; }
        DbSet<RailwayRltOrganization> RailwayRltOrganization { get; set; }
        DbSet<StationRltRailway> StationRltRailway { get; set; }
        DbSet<Station> Station { get; set; }
        DbSet<StationRltOrganization> StationRltOrganization { get; set; }
        DbSet<InstallationSite> InstallationSite { get; set; }
    }
}