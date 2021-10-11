using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class YearMonthPlanInputSearchDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 1年表  2月表
        /// </summary>
        public int Type { get; set; }

        public string RepairTagKey { get ; set ; }

        public bool IsLoginFree { get; set; }
    }
}
