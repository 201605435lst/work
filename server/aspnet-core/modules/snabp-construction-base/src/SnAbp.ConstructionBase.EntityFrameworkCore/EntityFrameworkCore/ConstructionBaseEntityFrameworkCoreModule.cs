using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.ConstructionBase.Entities;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.ConstructionBase.EntityFrameworkCore
{
	[DependsOn(
		typeof(ConstructionBaseDomainModule),
		typeof(AbpEntityFrameworkCoreModule)
	)]
	public class ConstructionBaseEntityFrameworkCoreModule : AbpModule
	{
		// 模块配置服务 
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			context.Services.AddAbpDbContext<ConstructionBaseDbContext>(options =>
			{
				/* Add custom repositories here. Example:
				 * options.AddRepository<Question, EfCoreQuestionRepository>();
				 */
				// 添加默认仓储



				// 配置 工序规范维护 关联 
				options.Entity<Standard>(x => x.DefaultWithDetailsFunc = q =>
					q.Include(standard => standard.Profession)
				);
				// 配置 施工区段 关联 
				options.Entity<Section>(x => x.DefaultWithDetailsFunc = q => q
					.Include(m => m.Children)
					.Include(m => m.Parent)
				);

				options.AddDefaultRepositories<IConstructionBaseDbContext>(true);

				// 配置 设备台班 关联 
				options.Entity<EquipmentTeam>(x => x.DefaultWithDetailsFunc = q =>
					q.Include(x => x.Type)
				);
				// 配置 工序 关联 
				options.Entity<Procedure>(x => x.DefaultWithDetailsFunc = q => q
					.Include(x => x.Type)
					.Include(x => x.ProcedureWorkers).ThenInclude(x => x.Worker)
					.Include(x => x.ProcedureEquipmentTeams).ThenInclude(x => x.EquipmentTeam)
					.Include(x => x.ProcedureMaterials).ThenInclude(x => x.ConstructionBaseMaterial)
					.Include(x => x.ProcedureRtlFiles).ThenInclude(x=>x.File)
				);

				options.Entity<ProcedureWorker>(x => x.DefaultWithDetailsFunc = q => q .Include(x => x.Worker) );
				options.Entity<ProcedureMaterial>(x => x.DefaultWithDetailsFunc = q => q .Include(x => x.ConstructionBaseMaterial) );
				options.Entity<ProcedureEquipmentTeam>(x => x.DefaultWithDetailsFunc = q => q .Include(x => x.EquipmentTeam) );
				options.Entity<ProcedureRltFile>(x => x.DefaultWithDetailsFunc = q => q .Include(x => x.File) );
				
				
				// 配置 分布分项 关联 
				options.Entity<SubItem>(x => x.DefaultWithDetailsFunc = q => q
					.Include(x => x.Creator)
					.Include(x => x.SubItemContent)
				);
				options.Entity<SubItemContent>(x => x.DefaultWithDetailsFunc = q => q
					.Include(x => x.SubItem)
					.Include(x => x.Children)
					.Include(x => x.Parent)
					.Include(x => x.SubItemContentRltProcedures).ThenInclude(x => x.Procedure)
				);
				options.Entity<SubItemContentRltProcedure>(x=>x.DefaultWithDetailsFunc=q=>q
					.Include(x=>x.Procedure)
					.Include(x=>x.ProcedureWorkers).ThenInclude(x=>x.Worker)
					.Include(x=>x.ProcedureMaterials).ThenInclude(x=>x.ConstructionBaseMaterial)
					.Include(x=>x.ProcedureEquipmentTeams).ThenInclude(x=>x.EquipmentTeam)
					.Include(x=>x.ProcedureRtlFiles).ThenInclude(x=>x.File)
				);
			});
		}
	}
}
