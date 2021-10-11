using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public interface IRepairTagKeyDto
    {
        /// <summary>
        /// 类型  数据字典中的维修项标签的唯一标识
        /// </summary>
        public string RepairTagKey { get; set; }
    }
}
