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
    /// 工作流实例详情
    /// </summary>
    public class WorkflowDetail : WorkflowSimple
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        public WorkflowDetail() { }

        public WorkflowDetail(Guid id)
        {
            Id = id;
        }

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
        public virtual List<FlowTemplateNode> FlowNodes { get; set; }


        /// <summary>
        /// 流程关系
        /// </summary>
        public virtual List<FlowTemplateStep> FlowSteps { get; set; }


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
