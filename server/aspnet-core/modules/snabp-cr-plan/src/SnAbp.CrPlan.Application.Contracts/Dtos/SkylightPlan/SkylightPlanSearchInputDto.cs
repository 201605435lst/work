using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    public class SkylightPlanSearchInputDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 作业单位
        /// </summary>
        public Guid? WorkUnit { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 车站
        /// </summary>
        public Guid? Station { get; set; }
        /// <summary>
        /// 所属线路
        /// </summary>
        public Guid? RailwayId { get; set; }
        /// <summary>
        /// 机房
        /// </summary>
        public Guid? WorkSite { get; set; }
        /// <summary>
        /// 作业内容、里程
        /// </summary>
        public string ContentMileage { get; set; }

        /// <summary>
        /// 维修等级
        /// </summary>
        public RepairLevel? RepaireLevel { get; set; }

        /// <summary>
        /// 天窗状态
        /// </summary>
        public PlanState State { get; set; }

        public PlanType PlanType { get; set; }
        public string RepairTagKey { get; set; }

        public bool IsAll { get; set; }

        public bool? IsOnRoad { get; set; }

        public bool IsSearchData { get; set; }
    }
}
