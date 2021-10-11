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
    /// 表单模板
    /// </summary>
    public class FormTemplate : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected FormTemplate() { }

        public FormTemplate(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 工作流 Id
        /// </summary>
        public Guid WorkflowTemplateId { get; set; }

        /// <summary>
        /// 工作流模板
        /// </summary>
        public virtual WorkflowTemplate WorkflowTemplate { get; set; }

        /// <summary>
        /// 表单项
        /// </summary>
        public virtual string FormItems { get; set; }

        /// <summary>
        /// 表单配置
        /// </summary>
        public virtual string Config { get; set; }



        /// <summary>
        /// 流程模板列表
        /// </summary>
        public List<FlowTemplate> FlowTemplates { get; set; }


        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
