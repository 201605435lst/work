using SnAbp.CrPlan.Dto.Worker;
using SnAbp.CrPlan.Dto.WorkOrganization;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工单实体（天窗计划到设备测试项）
    /// 获取数据使用
    /// </summary>
    public class WorkOrderDetailDto : EntityDto<Guid>, IRepairTagDto
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
        public string OrderNo { get; set; }

        /// <summary>
        /// 完成情况反馈
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// 作业组长
        /// </summary>
        public WorkerDto WorkLeader { get; set; }

        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// 作业内容类型
        /// </summary>
        public WorkContentType WorkContentType { get; set; }
        

        /// <summary>
        /// 作业成员
        /// </summary>
        public List<WorkerDto> WorkMemberList { get; set; }

        /// <summary>
        /// 现场防护员
        /// </summary>
        public List<WorkerDto> FieldGuardList { get; set; }

        /// <summary>
        /// 驻站联络员
        /// </summary>
        public List<WorkerDto> StationLiaisonOfficerList { get; set; }

        /// <summary>
        /// 工单状态 
        /// 未完成 = 0,已完成 = 1,已验收 = 2，撤销=3
        /// </summary>
        public OrderState OrderState { get; set; }

        /// <summary>
        /// 检修单位
        /// </summary>
        public WorkOrganizationDto MaintenanceUnit { get; set; }

        /// <summary>
        /// 通信单位
        /// </summary>
        public WorkOrganizationDto CommunicationUnit { get; set; }

        /// <summary>
        /// 天窗计划
        /// </summary>
        public Guid SkylightPlanId { get; set; }

        /// <summary>
        /// 派工单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 天窗计划内容
        /// 派工单所使用的新结构Dto
        /// </summary>
        public List<JobContentDetailDto> PlanDetailList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
