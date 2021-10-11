using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.PlanTodo
{
    /// <summary>
    /// 单位待办
    /// </summary>
    public class PlanTodoDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 级别
        /// </summary>
        public RepairLevel Level { get; set; }
        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime WorkTime { get; set; }
        /// <summary>
        /// 计划时长
        /// </summary>
        public int TimeLength { get; set; }
        /// <summary>
        /// 作业机房
        /// </summary>
        public Guid WorkSiteId { get; set; }
        /// <summary>
        /// 作业机房名称
        /// </summary>
        public string WorkSiteName { get; set; }
        /// <summary>
        /// 位置（里程）
        /// </summary>
        public string WorkArea { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Guid? RepairTagId { get; set; }

        public DataDictionaryDto RepairTag { get; set; }
    }
}
