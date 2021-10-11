using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class InfluenceRangeUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 维修级别
        /// </summary>
        public SkylightPlanRepairLevel RepairLevel { get; set; }

        /// <summary>
        /// 影响范围内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 标签Key
        /// </summary>
        public string RepairTagKey { get; set; }
    }
}
