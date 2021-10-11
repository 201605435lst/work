using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    public class OtherPlanSearchInputDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 作业单位
        /// </summary>
        public Guid? WorkUnitId { get; set; }
        /// <summary>
        /// 作业工区
        /// </summary>
        public Guid? WorkAreaId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }

        public string RepairTagKey { get; set; }

        public PlanState PlanState { get; set; }
    }
}
