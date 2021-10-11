using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.Construction.MasterPlans.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.MasterPlans.IRepositories
{
	public interface IMasterPlanContentRepository : IRepository<MasterPlanContent, Guid>
	{
		/// <summary>
		/// 批量添加
		/// </summary>
		/// <param name="masterPlanContents"></param>
		/// <returns></returns>
		public Task AddRangeAsync(List<MasterPlanContent> masterPlanContents);
		/// <summary>
		/// 批量修改
		/// </summary>
		/// <param name="masterPlanContents"></param>
		/// <returns></returns>
		public Task UpdateRangeAsync(List<MasterPlanContent> masterPlanContents);

		/// <summary>
		/// 批量删除
		/// </summary>
		/// <param name="masterPlanContents"></param>
		/// <returns></returns>
		public Task<bool> DeleteRangeAsync(List<MasterPlanContent> masterPlanContents);
	}
}