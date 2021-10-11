using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
   public class RepairItemTagMigratioDto
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public string RepairTagKey { get; set; }

        /// <summary>
        /// 目标数据类型
        /// </summary>
        public Guid TargetTagId { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public List<Guid> RepairGroupIds { get; set; }

    }
}
