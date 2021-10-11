using SnAbp.CrPlan.Dto.Worker;
using SnAbp.CrPlan.Dto.WorkOrganization;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工单实体
    /// 添加派工单使用（无天窗内部数据操作）
    /// </summary>
    public class WorkOrderCreateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 派工单名称
        /// </summary>
        public string Name { get; set; }

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
        /// 派工人员ID
        /// </summary>
        public Guid SendWorkersId { get; set; }

        /// <summary>
        /// 发起人员id
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 工单状态 
        /// 未完成 = 0,已完成 = 1,已验收 = 2，撤销=3
        /// </summary>
        public OrderState OrderState { get; set; }

        /// <summary>
        /// 检修单位
        /// </summary>
        public Guid MaintenanceUnitId { get; set; }

        /// <summary>
        /// 通信单位
        /// </summary>
        public Guid CommunicationUnitId { get; set; }

        /// <summary>
        /// 天窗计划ID
        /// </summary>
        public Guid SkylightPlanId { get; set; }

        public string RepairTagKey { get ; set ; }

        /// <summary>
        /// 是否为车间派工
        /// </summary>
        public bool WorkShop { get; set; }
    }
}
