using SnAbp.Basic.Dtos;
using SnAbp.Basic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class Station_RailwayDto : EntityDto<Guid>
    {
        /// <summary>
        /// 线路Guid
        /// </summary>
        public Guid RailwayId { get; set; }
        public RailwayDto Railway { get; set; }

        /// <summary>
        /// 站点Guid
        /// </summary>
        public Guid StationId { get; set; }
        public StationDto Station { get; set; }

        /// <summary>
        /// 经过顺序
        /// </summary>
        public int PassOrder { get; set; }
    }
}
