using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class RailwaySearchDto : PagedAndSortedResultRequestDto
    {
        public string RailwayName { get; set; }
        public string StationName { get; set; }

        /// <summary>
        /// 0 单线 1复线
        /// </summary>
        public RailwayType? Type { get; set; }

        /// <summary>
        /// 所属单位id
        /// </summary>
        public Guid? BelongOrgId { get; set; }

        public bool IsAll { get; set; }

        public RailwaySearchDto()
        { }
    }
}
