using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    public class StationCascaderDto
    {
        public Guid? organizationId { get; set; }
        public Guid? railwayId { get; set; }
        /// <summary>
        /// 是否展示上下行
        /// </summary>
        public bool IsShowUpAndDown { get; set; }

        /// <summary>
        /// 是否只展示站点信息
        /// </summary>
        public bool isShowStation { get; set; }
    }
}