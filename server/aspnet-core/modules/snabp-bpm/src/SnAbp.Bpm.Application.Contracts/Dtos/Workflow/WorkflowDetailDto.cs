using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowDetailDto : WorkflowSimpleDto
    {
        /// <summary>
        /// 表单项
        /// </summary>
        public string FormItems { get; set; }


        /// <summary>
        /// 表单配置
        /// </summary>
        public string FormConfig { get; set; }


        /// <summary>
        /// 表单值
        /// </summary>
        public string FormValue { get; set; }


        /// <summary>
        /// 流程节点
        /// </summary>
        public virtual List<FlowTemplateNodeDto> FlowNodes { get; set; }


        /// <summary>
        /// 流程关系
        /// </summary>
        public virtual List<FlowTemplateStepDto> FlowSteps { get; set; }


        /// <summary>
        /// 当前激活的流程
        /// </summary>
        public List<FlowTemplateStep> ActivedSteps { get; set; }


        /// <summary>
        /// 当前激活的流程
        /// </summary>
        public FlowTemplateStep CurrentUserActivedStep { get; set; }
    }
}