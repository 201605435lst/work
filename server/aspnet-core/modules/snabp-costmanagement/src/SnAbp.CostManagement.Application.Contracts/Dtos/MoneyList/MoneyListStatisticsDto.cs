using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CostManagement.Dtos
{
    public class MoneyListStatisticsDto
    {
        /// <summary>
        /// 组织机构名字
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public List<string> Dates { get; set; } = new List<string>();
        /// <summary>
        /// 应收
        /// </summary>
        public List<Decimal> Receivables { get; set; } = new List<decimal>();
        /// <summary>
        /// 已收
        /// </summary>
        public List<Decimal> Receiveds { get; set; } = new List<decimal>();
        /// <summary>
        /// 应付
        /// </summary>
        public List<Decimal> Dues { get; set; } = new List<decimal>();
        /// <summary>
        /// 已付
        /// </summary>
        public List<Decimal> Paids { get; set; } = new List<decimal>();
    }
}
