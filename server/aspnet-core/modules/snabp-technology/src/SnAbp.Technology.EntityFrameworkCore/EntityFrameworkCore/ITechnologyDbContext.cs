using Microsoft.EntityFrameworkCore;
using SnAbp.Technology.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Technology.EntityFrameworkCore
{
    [ConnectionStringName(TechnologyDbProperties.ConnectionStringName)]
    public interface ITechnologyDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<Disclose> Disclose { get; }
        DbSet<ConstructInterface> ConstructInterface { get; }
        DbSet<ConstructInterfaceInfo> ConstructInterfaceInfo { get; }
        DbSet<ConstructInterfaceInfoRltMarkFile> ConstructInterfaceInfoRltMarkFile { get; }
        DbSet<Material> Material { get; }
        DbSet<MaterialPlan> MaterialPlan { get; }
        DbSet<MaterialPlanFlowInfo> MaterialPlanFlowInfo { get; }
        DbSet<MaterialPlanRltMaterial> MaterialPlanRltMaterial { get; }


    }
}