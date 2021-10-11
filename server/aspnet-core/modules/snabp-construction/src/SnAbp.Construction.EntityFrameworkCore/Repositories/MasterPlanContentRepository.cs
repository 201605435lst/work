using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnAbp.Construction.EntityFrameworkCore;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Construction.MasterPlans.IRepositories;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Construction.Repositories
{
	public class MasterPlanContentRepository  : EfCoreRepository<ConstructionDbContext, MasterPlanContent, Guid>, IMasterPlanContentRepository
	{
		public MasterPlanContentRepository(IDbContextProvider<ConstructionDbContext> dbContextProvider) : base(dbContextProvider)
		{
		}

		/// <summary>
		/// 批量添加
		/// </summary>
		/// <param name="masterPlanContents"></param>
		public async Task AddRangeAsync(List<MasterPlanContent> masterPlanContents)
		{
			await DbSet.AddRangeAsync(masterPlanContents);
			await DbContext.SaveChangesAsync();
		}

		/// <summary>
		/// 批量修改 
		/// </summary>
		/// <param name="masterPlanContents"></param>
		public async Task UpdateRangeAsync(List<MasterPlanContent> masterPlanContents)
		{
			DbSet.UpdateRange(masterPlanContents);
			await DbContext.SaveChangesAsync();
		}

		public async Task<bool> DeleteRangeAsync(List<MasterPlanContent> masterPlanContents)
		{
			DbContext.Set<MasterPlanContent>().RemoveRange(masterPlanContents);
			await DbContext.SaveChangesAsync();
			return true;
		}
	}
}