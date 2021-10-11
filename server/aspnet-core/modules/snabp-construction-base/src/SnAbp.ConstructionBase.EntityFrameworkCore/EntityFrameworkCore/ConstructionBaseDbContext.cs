using Microsoft.EntityFrameworkCore;
using SnAbp.ConstructionBase.Entities;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Project.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.ConstructionBase.EntityFrameworkCore
{
	[ConnectionStringName(ConstructionBaseDbProperties.ConnectionStringName)]
	public class ConstructionBaseDbContext : AbpDbContext<ConstructionBaseDbContext>, IConstructionBaseDbContext
	{
		/* Add DbSet for each Aggregate Root here. Example:
		 * public DbSet<Question> Questions { get; set; }
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
                            
                            

		public ConstructionBaseDbContext(DbContextOptions<ConstructionBaseDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ConfigureConstructionBase();
			builder.ConfigureFile();
			builder.ConfigureProject();
			//  加这个 , builder.ConfigureIdentity(); 不然会报  下面的错
			// System.InvalidOperationException:“The property 'IdentityUser.ExtraProperties'
			// could not be mapped, because it is of type 'Dictionary<string, object>'
			// which is not a supported primitive type or a valid entity type.
			// Either explicitly map this property, or ignore it using the '[NotMapped]'
			// attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.”	
			builder.ConfigureIdentity();
		}
	}
}
