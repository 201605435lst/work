using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    /// <summary>
    /// 流程模板
    /// </summary>
    public class FlowTemplate : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected FlowTemplate() { }

        public FlowTemplate(Guid id)
        {
            Id = id;
        }


        /// <summary>
        /// 表单模板 Id 该字段考虑到后期的扩展可以为空，为空时，没有表单数据，只有流程
        /// </summary>
        public Guid? FormTemplateId { get; set; }


        /// <summary>
        /// 表单模板
        /// </summary>
        public virtual FormTemplate FormTemplate { get; set; }


        /// <summary>
        /// 流程节点
        /// </summary>
        public virtual List<FlowTemplateNode> Nodes { get; set; }


        /// <summary>
        /// 流程关系
        /// </summary>
        public virtual List<FlowTemplateStep> Steps { get; set; }


        /// <summary>
        /// 工作流实例
        /// </summary>
        public virtual List<Workflow> Workflows { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
