using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowSearchInputDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 用户工作流分组
        /// </summary>
        public UserWorkflowGroup Group { get; set; } = UserWorkflowGroup.All;


        /// <summary>
        /// 工作流状态
        /// </summary>
        public WorkflowState State { get; set; } = WorkflowState.All;


        /// <summary>
        /// 工作流名称
        /// </summary>
        public string Name { get; set; }

        //西安通信段新增需求：添加提报时间查询

        /// <summary>
        /// 提报开始时间
        /// </summary>
        public DateTime SubmitStartTime { get; set; }

        /// <summary>
        /// 提报结束时间
        /// </summary>
        public DateTime SubmitEndTime { get; set; }

        /// <summary>
        /// 是否查询全部
        /// </summary>
        public bool IsAll { get; set; }

        //是否根据计划时间进行查询
        public bool IsSelecyByPlanTime { get; set; }

        //计划时间
        public DateTime? PlanTime { get; set; }
    }
}