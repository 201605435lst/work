using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CrPlan.EntityFrameworkCore.EFCoreRepositories
{
    public class CrPlanStatisticalRepository : ICrPlanStatistialRepository, ITransientDependency
    {
        private readonly IDbContextProvider<CrPlanDbContext> _dbContext;
        public CrPlanStatisticalRepository(IDbContextProvider<CrPlanDbContext> crPlanDbContext)
        {
            _dbContext = crPlanDbContext;
        }

        /// <summary>
        /// 智能报表年表统计图
        /// </summary>
        /// <param name="id">组织机构ID</param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetYearStatisticalBySql(Guid id)
        {
            //var parameters = new SqlParameter("@orgid", id.ToString());
            string sql = @"SELECT sum(dy.""Count"") as PCount,sum(pd.""WorkCount"") as WCount,dy.""Name"" FROM ""SnCrPlan_PlanDetail"" as pd RIGHT JOIN"
            + "(SELECT d.\"Id\", d.\"Count\", yc.\"Name\" FROM \"SnCrPlan_DailyPlan\" as d RIGHT JOIN"
            + "(SELECT y.\"Id\", o.\"Name\" FROM \"SnCrPlan_YearMonthPlan\" as y RIGHT JOIN"
            + "(SELECT \"Id\", \"Name\" FROM \"SnSystem_Organization\" WHERE substr(\"Code\", 1, 4) = (SELECT substr(\"Code\", 1, 4) FROM \"SnSystem_Organization\" "
            + "WHERE \"Id\" = '" + id.ToString() + "') AND \"length\"(\"Code\") = 8) as o ON (y.\"ResponsibleUnit\" = o.\"Id\" AND y.\"PlanType\" = 1 AND y.\"Year\" = " + DateTime.Now.Year + ")) as yc ON d.\"PlanId\" = yc.\"Id\") as dy "
            + "ON pd.\"DailyPlanId\" = dy.\"Id\" GROUP BY dy.\"Name\"; ";
            var result = _dbContext.GetDbContext().Database.SqlQuery<YearStatisticalDto>(sql);
            if (result == null || result.Count == 0) return null;
            Dictionary<string, decimal> list = new Dictionary<string, decimal>();
            result.ForEach(x =>
            {
                if (x.PCount <= 0 || x.WCount <= 0)
                    list.Add(x.Name, 0);
                else
                    list.Add(x.Name, Math.Round(x.WCount / x.PCount, 3));
            });
            return list;
        }

        /// <summary>
        /// 智能报表月表统计图
        /// </summary>
        /// <param name="orgId">组织机构ID</param>
        /// <param name="planType">2：月计划 3：年表月度计划</param>
        /// <param name="planTime">计划时间</param>
        /// <returns></returns>
        public Dictionary<string, List<decimal>> GetMonthStatistical(Guid orgId, int planType, DateTime planTime)
        {
            string sql = @"select a.pcount,a.wcount,b.changecount,b.""Name"" "
            +"from(SELECT sum(dy.\"Count\") as PCount, sum(pd.\"WorkCount\") as WCount, dy.\"Name\" FROM \"SnCrPlan_PlanDetail\" as pd RIGHT JOIN "
            +"(SELECT d.\"Id\", d.\"Count\", yc.\"Name\" FROM \"SnCrPlan_DailyPlan\" as d RIGHT JOIN "
            +"(SELECT y.\"Id\", o.\"Name\" FROM \"SnCrPlan_YearMonthPlan\" as y RIGHT JOIN "
            +"(SELECT \"Id\", \"Name\" FROM \"SnSystem_Organization\" WHERE substr(\"Code\", 1, 4) = (SELECT substr(\"Code\", 1, 4) FROM \"SnSystem_Organization\" WHERE \"Id\" = '"+ orgId.ToString() +"') "
            +"AND length(\"Code\") = 8) as o ON (y.\"ResponsibleUnit\" = o.\"Id\" AND y.\"PlanType\" = "+planType+" AND y.\"Year\" = "+planTime.Year+" AND y.\"Month\" = "+planTime.Month+")) as yc ON d.\"PlanId\" = yc.\"Id\") as dy ON pd.\"DailyPlanId\" = dy.\"Id\" GROUP BY dy.\"Name\")  a FULL JOIN("
            +"SELECT sum(ar.\"PlanCount\" - ar.\"AlterCount\") as ChangeCount, dy.\"Name\" FROM \"SnCrPlan_DailyPlanAlter\" as ar RIGHT JOIN "
            +"(SELECT d.\"Id\", d.\"Count\", yc.\"Name\" FROM \"SnCrPlan_DailyPlan\" as d RIGHT JOIN "
            +"(SELECT y.\"Id\", o.\"Name\" FROM \"SnCrPlan_YearMonthPlan\" as y RIGHT JOIN "
            +"(SELECT \"Id\", \"Name\" FROM \"SnSystem_Organization\" WHERE substr(\"Code\", 1, 4) = (SELECT substr(\"Code\", 1, 4) FROM \"SnSystem_Organization\" WHERE \"Id\" = '" + orgId.ToString() + "') AND length(\"Code\") = 8) as o ON (y.\"ResponsibleUnit\" = o.\"Id\" AND "
            +"y.\"PlanType\" = " + planType + " AND y.\"Year\" = " + planTime.Year + " AND y.\"Month\" = " + planTime.Month + ")) as yc ON d.\"PlanId\" = yc.\"Id\") as dy ON ar.\"DailyId\" = dy.\"Id\" GROUP BY dy.\"Name\""
            +") b on a.\"Name\" = b.\"Name\"";
            var result = _dbContext.GetDbContext().Database.SqlQuery<MonthStatistiscalDto>(sql);
            if (result == null || result.Count == 0) return null;
            Dictionary<string, List<decimal>> list = new Dictionary<string, List<decimal>>();
            result.ForEach(x =>
            {
                list.Add(x.Name, new List<decimal>() { x.PCount, x.WCount, x.ChangeCount < 0 ? 0 : x.ChangeCount });
            });
            return list;
        }

    }
    public class YearStatisticalDto
    {
        public string Name { get; set; }
        public decimal PCount { get; set; }
        public decimal WCount { get; set; }
    }

    public class MonthStatistiscalDto
    {
        public string Name { get; set; }
        public decimal PCount { get; set; }
        public decimal WCount { get; set; }
        public decimal ChangeCount { get; set; }
    }
}
