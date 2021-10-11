using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    /// <summary>
    /// 天窗计划实体，单位待办使用
    /// 获取实体数据
    /// </summary>
    public class SkylightPlanSearchDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 计划日期(天窗计划)
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 作业时长
        /// </summary>
        public int TimeLength { get; set; }
        /// <summary>
        /// 所属线路
        /// </summary>
        public Guid? RailwayId { get; set; }
        /// <summary>
        /// 作业机房(天窗计划)
        /// </summary>
        public string WorkSite { get; set; }

        /// <summary>
        /// 位置（里程）(天窗计划)
        /// </summary>
        public string WorkArea { get; set; }

        /// <summary>
        /// 作业内容(天窗计划)
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// 维修等级(天窗计划)
        /// </summary>
        public RepairLevel Level { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType { get; set; }
        public string RepairTagKey { get ; set ; }
    }
}
