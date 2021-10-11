using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CrPlan.EntityFrameworkCore.EFCoreRepositories
{
    public class EFCrPlanDaliyPlanRepository:EfCoreRepository<CrPlanDbContext, DailyPlan, Guid>, ICrPlanDaliyPlanRepository, ITransientDependency
    {
        public EFCrPlanDaliyPlanRepository(IDbContextProvider<CrPlanDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<bool> OperDaliyPlan(List<DailyPlan> addList, List<DailyPlan> updateList, List<DailyPlan> delList)
        {
            await Task.Run(() =>
            {
                if (delList?.Count > 0)
                {
                    DbContext.RemoveRange(delList);
                }
                if (updateList?.Count > 0)
                {
                    DbContext.UpdateRange(updateList);
                }
                if (addList?.Count > 0)
                {
                    DbContext.AddRange(addList);
                }
            });
            return true;
        }
    }
}
