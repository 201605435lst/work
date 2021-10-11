using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowTemplateUpdateFlowTemplateInputDto : EntityDto<Guid>
    {
        [Required]
        /// <summary>
        /// 表单模板 Id
        /// </summary>
        public Guid FormTemplateId { get; set; }

        [Required]
        /// <summary>
        /// 流程模板 Nodes 数据
        /// </summary>
        public List<FlowTemplateNodeDto> FlowNodes { get; set; }

        [Required]
        /// <summary>
        /// 流程模板 Edges 数据
        /// </summary>
        public List<FlowTemplateStepDto> FlowSteps { get; set; }
    }
}