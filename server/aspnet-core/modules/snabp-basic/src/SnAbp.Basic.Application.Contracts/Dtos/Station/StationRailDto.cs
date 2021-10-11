using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    /// <summary>
    /// 线路-站点
    /// </summary>
    public class StationRailDto
    {
        //线路id  或者线站关联关系id
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid StaId { get; set; }
        /// <summary>
        /// 类型0线路  -1 上行 -2下行 -3上下行 11单线站点 1上行站点  2下行站点 3上下行站点
        /// </summary>
        public int Type { get; set; }
        public List<StationRailDto> Children { get; set; } = new List<StationRailDto>();
    }
}
