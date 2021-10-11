using SnAbp.CrPlan.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.CrPlan.Repositories
{
    public interface ICrPlanStatistialRepository : ITransientDependency
    {
        /// <summary>
        /// 智能报表年表统计图
        /// </summary>
        /// <param name="id">组织机构ID</param>
        /// <returns></returns>
        Dictionary<string, decimal> GetYearStatisticalBySql(Guid id);

        /// <summary>
        /// 智能报表月表统计图
        /// </summary>
        /// <param name="orgId">组织机构ID</param>
        /// <param name="planType">2：月计划 3：年表月度计划</param>
        /// <param name="planTime">计划时间</param>
        /// <returns></returns>
        Dictionary<string,List<decimal>> GetMonthStatistical(Guid orgId, int planType, DateTime planTime);
    }
}
