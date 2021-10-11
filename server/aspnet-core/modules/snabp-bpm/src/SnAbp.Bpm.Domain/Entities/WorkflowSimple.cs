using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    [NotMapped]
    /// <summary>
    /// 工作流实例表
    /// </summary>
    public class WorkflowSimple : Workflow
    {
        protected WorkflowSimple() { }

        public WorkflowSimple(Guid id)
        {
            Id = id;
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 简报
        /// </summary>
        public List<WorkflowInfo> Infos { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public WorkflowState State { get; set; }


        /// <summary>
        /// 表单版本
        /// </summary>
        public int FormTemplateVersion { get; set; }


        /// <summary>
        /// 流程版本
        /// </summary>
        public int FlowTemplateVersion { get; set; }

        public bool IsStatic { get; set; }
    }
}