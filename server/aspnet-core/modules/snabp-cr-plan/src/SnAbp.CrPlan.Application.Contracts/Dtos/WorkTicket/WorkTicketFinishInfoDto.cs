using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class WorkTicketFinishInfoDto
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public int FinishCount { get; set; }
    }
}
