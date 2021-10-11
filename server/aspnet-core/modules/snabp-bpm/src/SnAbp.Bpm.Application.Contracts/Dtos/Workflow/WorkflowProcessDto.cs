using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowProcessDto : EntityDto<Guid>
    {
        /// <summary>
        /// 表单值，终止流程的时候可以没有表单值
        /// </summary>
        public string FormValues { get; set; }


        [Required]
        /// <summary>
        /// 起始节点 Id
        /// </summary>
        public Guid SourceNodeId { get; set; }


        [Required]
        /// <summary>
        /// 目标节点 Id
        /// </summary>
        public Guid TargetNodeId { get; set; }


        [Required]
        /// <summary>
        /// 处理结果
        /// </summary>
        public WorkflowStepState StepState { get; set; }


        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comments { get; set; }
    }
}