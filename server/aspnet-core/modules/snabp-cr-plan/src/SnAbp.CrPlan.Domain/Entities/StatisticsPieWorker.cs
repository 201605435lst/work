using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CrPlan.Entities
{
    public class StatisticsPieWorker : AuditedEntity<Guid>
    {
        public StatisticsPieWorker() { }
        public StatisticsPieWorker(Guid id) => Id = id;

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 已完成
        /// </summary>
        public float Finshed { get; set; }

        /// <summary>
        /// 未完成
        /// </summary>
        public float UnFinshed { get; set; }

        /// <summary>
        /// 已变更
        /// </summary>
        public float Changed { get; set; }

        /// <summary>
        /// 车间名称 
        /// </summary>
        public string OrgizationName { get; set; }

        public Guid? OrgizationId { get; set; }
    }
}
