using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class FinsCooperationWorkDto
    {
        public Guid CooperationWorkId { get; set; }

        /// <summary>
        /// 配合车间实际结束时间
        /// </summary>
        public DateTime CooperateRealFinishTime { get; set; }
        /// <summary>
        /// 配合车间实际开始时间
        /// </summary>
        public DateTime CooperateRealStartTime { get; set; }

        /// <summary>
        /// 配合车间完成情况
        /// </summary>
        public string Completion { get; set; }
    }
}
