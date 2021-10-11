using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 其他作业实体
    /// </summary>
    public class OtherHomeworkDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanTime { get; set; }

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
        public string WorkUnit { get; set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public OrderState OrderState { get; set; }

        public Guid? RepairTagId { get; set; }

        public DataDictionaryDto RepairTag { get; set; }

        public string OrderNo { get; set; }

        public DateTime CreationTime { get; set; }

        public OtherHomeworkDto() { }

        public OtherHomeworkDto(Guid id) { Id = id; }
    }
}
