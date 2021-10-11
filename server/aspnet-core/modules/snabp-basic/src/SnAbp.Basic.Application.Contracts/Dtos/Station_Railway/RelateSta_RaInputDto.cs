using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    public class RelateSta_RaInputDto
    {
        /// <summary>
        /// 线路id
        /// </summary>
        public Guid RaliwayId { get; set; }

        /// <summary>
        /// 站点线路关联信息集合 上行 或 单线
        /// </summary>
        public List<Station_RailwayInputDto> RelateInfos { get; set; }

        /// <summary>
        /// 站点线路关联信息集合 下行
        /// </summary>
        public List<Station_RailwayInputDto> DownLinkRelateInfos { get; set; }
    }
}
