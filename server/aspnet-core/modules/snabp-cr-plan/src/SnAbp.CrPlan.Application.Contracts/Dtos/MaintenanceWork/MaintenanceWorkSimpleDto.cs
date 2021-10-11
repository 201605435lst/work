using SnAbp.Bpm;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    public class MaintenanceWorkSimpleDto : CreationAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 提交车间机构
        /// </summary>
        public Guid OrganizationId { get; set; }
        public OrganizationDto Organization { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 第一阶段审批流程id
        /// </summary>
        public Guid? ARKey { get; set; }

        /// <summary>
        /// 第二阶段审批流程id
        /// </summary>
        public Guid? SecondARKey { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public string LanuchPerson { get; set; }

        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime LaunchTime { get; set; }

        /// <summary>
        /// 第一阶段工作流状态
        /// </summary>
        public WorkflowState WorkflowState { get; set; }

        /// <summary>
        /// 第二阶段工作流状态
        /// </summary>
        public WorkflowState SecondWorkflowState { get; set; }

        public Guid? RepairTagId { get; set; }

        /// <summary>
        /// 维修等级
        /// </summary>
        public RepairLevel RepairLevel { get; set; }
    }
}
