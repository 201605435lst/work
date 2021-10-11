using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
    public class GetMaxNumberDto
    {
        /// <summary>
        /// 是否是月表
        /// </summary>
        public bool? IsMonth { get; set; }

        /// <summary>
        /// 分组（设备名称） Id
        /// </summary>
        public Guid GroupId { get; set; }


        public string RepairTagKey { get; set; }
    }
}
