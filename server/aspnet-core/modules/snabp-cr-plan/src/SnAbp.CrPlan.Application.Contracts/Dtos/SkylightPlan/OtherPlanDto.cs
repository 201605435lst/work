using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    /// <summary>
    /// 
    /// </summary>
    public class OtherPlanDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime WorkTime { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        public string WorkUnitName { get; set; }
        /// <summary>
        /// 作业工区
        /// </summary>
        public string WorkAreaName { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        public PlanState PlanState { get; set; }

        //是否变更
        public bool IsChange { get; set; }

        //变更后新计划的工作时间
        public string ChangTime { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Opinion { get; set; }
        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        public OtherPlanDto() { }
        public OtherPlanDto(Guid id) { Id = id; }
    }
}
