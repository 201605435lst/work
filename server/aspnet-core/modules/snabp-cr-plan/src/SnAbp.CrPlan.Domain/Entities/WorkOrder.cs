using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 派工单实体
    /// </summary>
    public class WorkOrder : AuditedEntity<Guid>, IRepairTag
    {
        /// <summary>
        /// 派工单名称
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 天窗计划ID
        /// </summary>
        public Guid SkylightPlanId { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime StartPlanTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime EndPlanTime { get; set; }

        /// <summary>
        /// 影响范围
        /// </summary>
        [StringLength(1000)]
        public string InfluenceScope { get; set; }

        /// <summary>
        /// 通信工具检查实验情况
        /// </summary>
        public string ToolSituation { get; set; }

        /// <summary>
        /// 注意事项
        /// </summary>
        public string Announcements { get; set; }

        /// <summary>
        /// 派工时间
        /// </summary>
        public DateTime DispatchingTime { get; set; }

        /// <summary>
        /// 派工人员
        /// </summary>
        public Guid SendWorkersId { get; set; }

        /// <summary>
        /// 实际作业起始时间
        /// </summary>
        public DateTime StartRealityTime { get; set; }

        /// <summary>
        /// 实际作业终止时间
        /// </summary>
        public DateTime EndRealityTime { get; set; }

        /// <summary>
        /// 命令票号
        /// </summary>
        [StringLength(50)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 完成情况反馈
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// 派工单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 工单状态 
        /// 未完成 = 0,已完成 = 1,已验收 = 2，撤销=3
        /// </summary>
        public OrderState OrderState { get; set; }
        
        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public WorkOrder() { }
        public WorkOrder(Guid id) { Id = id; }
    }
}
