using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Bpm.Dtos
{
    public class GetTotalSimpleDto
    {
        /// <summary>
        /// 统计图表数据（我发起的）
        /// </summary>
        public int InitialTotal { get; set; }
        /// <summary>
        /// 统计图表数据（带我审批）
        /// </summary>
        public int WaitingTotal { get; set; }

        /// 统计图表数据（我已审批）
        public int ApprovedTotal { get; set; }
        /// <summary>
        /// 统计图表数据（抄送我的）
        /// </summary>
        public int CcTotal { get; set; }
    }
}
