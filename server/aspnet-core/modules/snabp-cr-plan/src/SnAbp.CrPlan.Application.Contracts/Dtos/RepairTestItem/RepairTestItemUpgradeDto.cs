using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class RepairTestItemUpgradeDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 更新目标年份
        /// </summary>
        public int Year { get; set; }
        public string RepairTagKey { get; set; }

        /// <summary>
        /// 更新的维修项的id
        /// </summary>
        //public List<Guid> RepairDetailIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 是否更新所有维修项
        /// </summary>
        //public bool IsUpgradeAll { get; set; }
    }
}
