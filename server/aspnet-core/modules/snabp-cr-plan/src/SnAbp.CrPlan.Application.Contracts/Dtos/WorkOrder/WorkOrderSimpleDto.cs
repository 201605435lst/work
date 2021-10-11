using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工作业、已派工、已完成模块使用派工单实体
    /// 获取数据使用
    /// </summary>
    public class WorkOrderSimpleDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 计划日期(天窗计划)
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 作业时间(派工单)
        /// </summary>
        public string WorkTime { get; set; }

        /// <summary>
        /// 作业时长(天窗计划)
        /// </summary>
        public int TimeLength { get; set; }

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
        /// 维修等级(天窗计划) 级别RepairLevel枚举值，多选后逗号隔开
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 作业单位（派工单）
        /// </summary>
        public string WorkUintString { get; set; }

        /// <summary>
        /// 作业组长(派工单)
        /// </summary>
        public string WorkLeader { get; set; }

        /// <summary>
        /// 作业成员(派工单)
        /// </summary>
        public string WorkMemberString { get; set; }

        /// <summary>
        /// 计划类型(天窗计划)
        /// </summary>
        public PlanType PlanType { get; set; }

        /// <summary>
        /// 工单状态 (派工单)
        /// 未完成 = 0,已完成 = 1,已验收 = 2，撤销=3
        /// </summary>
        public OrderState OrderState { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        //派工取消原因
        public string CancelReason { get; set; }

        public WorkOrderSimpleDto() { }
        public WorkOrderSimpleDto(Guid id) { Id = id; }

    }
}
