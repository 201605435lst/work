using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    public class Station_RailwayInputDto
    {
        /// <summary>
        /// 站点Guid
        /// </summary>
        public Guid StaId { get; set; }

        /// <summary>
        /// 经过顺序
        /// </summary>
        //public int PassOrder { get; set; }

        /// <summary>
        /// 公里标
        /// </summary>
        public int KMMark { get; set; }

        /// <summary>
        /// 线别
        /// </summary>
        public RelateRailwayType RailwayType { get; set; }
    }
}
