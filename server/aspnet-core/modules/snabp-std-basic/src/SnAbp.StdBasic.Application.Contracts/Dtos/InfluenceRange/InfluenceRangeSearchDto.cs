using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class InfluenceRangeSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 维修级别
        /// </summary>
        public List<SkylightPlanRepairLevel>? RepairLevel { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Keyword { get; set; }

        public string RepairTagKey { get; set; }

        public bool IsAll { get; set; }
    }
}
