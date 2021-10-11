using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public interface IRepairTagDto : IRepairTagIdDto
    {
        /// <summary>
        /// 类型 数据字典中的维修项标签外键
        /// </summary>
        public DataDictionaryDto RepairTag { get; set; }
    }
}
