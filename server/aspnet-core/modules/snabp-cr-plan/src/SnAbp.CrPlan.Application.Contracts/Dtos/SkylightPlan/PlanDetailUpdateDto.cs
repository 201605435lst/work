using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    public class PlanDetailUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
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
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }
        /// <summary>
        /// 影响范围
        /// </summary>
        public Guid? InfluenceRangeId { get; set; }
        /// <summary>
        /// 作业数量
        /// </summary>
        public decimal WorkCount { get; set; }
        /// <summary>
        /// 关联设备列表
        /// </summary>
        public List<PlanRelateEquipmentUpdateDto> RelateEquipments { get; set; }

        public string RepairTagKey { get ; set ; }
    }
}
