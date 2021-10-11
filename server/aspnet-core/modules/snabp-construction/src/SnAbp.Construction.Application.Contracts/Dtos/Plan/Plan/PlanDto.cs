using System;
using System.Collections.Generic;
using SnAbp.Bpm.Dtos;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlan;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanRltWorkflowInfo;
using SnAbp.Construction.Dtos.Plan.PlanRltWorkflowInfo;
using SnAbp.Construction.Enums;
using SnAbp.Identity;
using SnAbp.Project.Dtos;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.Plan.Plan
{

	/// <summary>
	/// 施工计划 dto 
	/// </summary>
	public class PlanDto : EntityDto<Guid>
	{

		/// <summary>
		/// 负责人Id 
		/// </summary>
		public Guid? ProjectId {get;set;}
		/// <summary>
		/// 负责人
		/// </summary>
		public ProjectDto Project {get;set;}
		/// <summary>
		/// 总体计划Id 
		/// </summary>
		public Guid? MasterPlanId {get;set;}
		/// <summary>
		/// 总体计划
		/// </summary>
		public MasterPlanDto MasterPlan {get;set;}
		/// <summary>
		/// 计划名称
		/// </summary>
		public string Name {get;set;}
		/// <summary>
		/// 计划描述
		/// </summary>
		public string Content {get;set;}
		/// <summary>
		/// 计划开始时间
		/// </summary>
		public DateTime PlanStartTime {get;set;}
		/// <summary>
		/// 计划结束时间
		/// </summary>
		public DateTime PlanEndTime {get;set;}
		/// <summary>
		/// 计划工期
		/// </summary>
		public double Period {get;set;}
		/// <summary>
		/// 负责人
		/// </summary>
		public IdentityUserDto Charger {get;set;}
		/// <summary>
		/// 负责人Id 
		/// </summary>
		public Guid? ChargerId {get;set;}
		/// <summary>
		/// 审批流程状态
		/// </summary>
		public ConstructionPlanState State {get;set;}
		/// <summary>
		/// 整体进度
		/// </summary>
		public double AllProgress {get;set;}
		
		
        /// <summary>
        /// 工作流id
        /// </summary>
        public  Guid? WorkflowId { get; set; }
        /// <summary>
        /// 工作流模板id
        /// </summary>
        public  Guid? WorkflowTemplateId { get; set; }
        
        /// <summary>
        /// 工作流 Dto
        /// </summary>
        public  WorkflowDto Workflow { get; set; }
        /// <summary>
        /// 审批状态 
        /// </summary>
        public  string WorkflowState { get; set; }
		
		/// <summary>
		/// 审批记录
		/// </summary>
		public List<PlanRltWorkflowInfoDto> PlanRltWorkflowInfos { get; set; } = new List<PlanRltWorkflowInfoDto>();
	}
}