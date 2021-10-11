using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Identity;

namespace SnAbp.CrPlan.Entities
{
    public interface IRepairTag
    {
        /// <summary>
        /// 类型 数据字典中的维修项标签外键
        /// </summary>
        public Guid? RepairTagId { get; set; }

        public DataDictionary RepairTag { get; set; }
    }
}
