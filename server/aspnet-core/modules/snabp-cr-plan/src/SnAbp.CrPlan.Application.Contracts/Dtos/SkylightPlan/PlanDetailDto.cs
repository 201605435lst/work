using SnAbp.Basic.Dtos;
using SnAbp.CrPlan.Dto.AlterRecord;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.Identity;
using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    public class PlanDetailDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 天窗计划ID
        /// </summary>
        public Guid SkylightPlanId { get; set; }

        /// <summary>
        /// 日计划ID
        /// </summary>
        public Guid DailyPlanId { get; set; }

        /// <summary>
        /// 日计划详情
        /// </summary>
        public DailyPlanSelectableDto DailyPlan { get; set; }

        /// <summary>
        /// 影响范围
        /// </summary>
        public Guid? InfluenceRangeId { get; set; }
        public InfluenceRangeDto InfluenceRange { get; set; }

        /// <summary>
        /// 关联设备列表
        /// </summary>
        public List<PlanRelateEquipmentDto> RelateEquipments { get; set; } = new List<PlanRelateEquipmentDto>();
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }
        /// <summary>
        /// 作业数量
        /// </summary>
        public decimal WorkCount { get; set; }
        /// <summary>
        /// IFD列表
        /// </summary>
        public List<Guid> IFDCodeList { get; set; }

        /// <summary>
        /// 作业机房ID（派工用）
        /// </summary>
        public List<Guid>? WorkSiteIds { get; set; }
        /// <summary>
        /// 作业机房名称（派工用）
        /// </summary>
        public string WorkSiteName { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
