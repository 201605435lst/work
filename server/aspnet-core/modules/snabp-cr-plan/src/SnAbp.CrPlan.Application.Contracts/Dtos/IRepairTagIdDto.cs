using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public interface IRepairTagIdDto
    {
        /// <summary>
        /// 类型Id 数据字典中的维修项标签外键
        /// </summary>
        public Guid? RepairTagId { get; set; }
    }
}
