using SnAbp.CrPlan.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SnAbp.CrPlan.Repositories
{
    public interface ICrPlanDaliyPlanRepository : Volo.Abp.Domain.Repositories.IRepository<DailyPlan, Guid>, ITransientDependency
    {
        Task<bool> OperDaliyPlan(List<DailyPlan>addList, List<DailyPlan> updateList, List<DailyPlan>delList);
    }
}
