using SnAbp.Basic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class RailwayStationDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string Name { get; set; }

        public Guid StationId { get; set; }

        public string StationName { get; set; }

        public int KMMark { get; set; }

        public Guid? SectionStartStationId { get; set; }

        public Guid? SectionEndStationId { get; set; }

        public int Type { get; set; }
    }
}
