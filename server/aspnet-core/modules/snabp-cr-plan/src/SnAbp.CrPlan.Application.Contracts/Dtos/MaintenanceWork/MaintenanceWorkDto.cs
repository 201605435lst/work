using SnAbp.Bpm;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    public class MaintenanceWorkDto : EntityDto<Guid>
    {
        /// <summary>
        /// 提交车间机构
        /// </summary>
        public Guid OrganizationId { get; set; }
        public OrganizationDto Organization { get; set; }
        /// <summary>
        /// 维修类型
        /// </summary>
        public PlanType MaintenanceType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 一阶段审批流程id
        /// </summary>
        public Guid? ARKey { get; set; }

        /// <summary>
        /// 第二阶段审批流程id
        /// </summary>
        public Guid? SecondARKey { get; set; }

        /// <summary>
        /// 一阶段工作流状态
        /// </summary>
        public WorkflowState WorkflowState { get; set; }

        /// <summary>
        /// 维修等级
        /// </summary>
        public RepairLevel RepaireLevel { get; set; }
        /// 第二阶段工作流状态
        /// </summary>
        public WorkflowState SecondWorkflowState { get; set; }

        /// <summary>
        /// 关联关系
        /// </summary>
        public List<MaintenanceWorkRltSkylightPlanDto> MaintenanceWorkRltSkylightPlans { get; set; } = new List<MaintenanceWorkRltSkylightPlanDto>();

        /// <summary>
        /// 关联文件
        /// </summary>
        public List<MaintenanceWorkRltFileDto> MaintenanceWorkRltFiles { get; set; } = new List<MaintenanceWorkRltFileDto>();

        public Guid? RepairTagId { get; set; }
    }
}