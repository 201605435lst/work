using Microsoft.EntityFrameworkCore;
using SnAbp.ConstructionBase.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.ConstructionBase.EntityFrameworkCore
{
	[ConnectionStringName(ConstructionBaseDbProperties.ConnectionStringName)]
	public interface IConstructionBaseDbContext : IEfCoreDbContext
                
	{
		/* Add DbSet for each Aggregate Root here. Example:
		 * DbSet<Question> Questions { get; }
		 */
		public DbSet<Worker> Workers { get; set; }
		public DbSet<EquipmentTeam> EquipmentTeams { get; set; }
		public DbSet<ConstructionBaseMaterial> Materials { get; set; }
		public DbSet<Procedure> Procedures { get; set; }
		public DbSet<ProcedureWorker> ProcedureWorkers { get; set; }
		public DbSet<ProcedureEquipmentTeam> ProcedureEquipmentTeams { get; set; }
		public DbSet<ProcedureMaterial> ProcedureMaterials { get; set; }
		public DbSet<ProcedureRltFile> ProcedureRltFiles { get; set; }
		public DbSet<SubItem> SubItems { get; set; }
		public DbSet<SubItemContent> SubItemContents { get; set; }
		public DbSet<SubItemContentRltProcedure> SubItemContentRltProcedures { get; set; }
		public DbSet<RltProcedureRltFile> RltProcedureRltFiles { get; set; }
		public DbSet<RltProcedureRltMaterial> ProcedureRltMaterials { get; set; }
		public DbSet<RltProcedureRltEquipmentTeam> RltProcedureRltEquipmentTeams { get; set; }
		public DbSet<RltProcedureRltWorker> RltProcedureRltWorkers { get; set; }

		public DbSet<Standard> Standards { get; set; }

		public DbSet<Section> Sections { get; set; }
                
                
	}
}
