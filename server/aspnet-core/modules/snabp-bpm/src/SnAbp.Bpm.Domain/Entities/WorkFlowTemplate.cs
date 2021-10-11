
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using JetBrains.Annotations;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    /// <summary>
    /// 工作流模板
    /// </summary>
    public class WorkflowTemplate : FullAuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected WorkflowTemplate() { }

        public WorkflowTemplate(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 是否静态 静态的工作流是程序创建的，不能删除
        /// </summary>
        public bool IsStatic { get; set; } = false;

        [Required]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; } = false;

        /// <summary>
        /// 审批回调
        /// </summary>
        public string WebHookUrl { get; set; }

        public virtual Guid? WorkflowTemplateGroupId { get; set; }
        [CanBeNull] public virtual WorkflowTemplateGroup WorkflowTemplateGroup { get; set; }

        public virtual WorkflowTemplateType Type { get; set; }
        /// <summary>
        /// 表单模板列表
        /// </summary>
        public virtual List<FormTemplate> FormTemplates { get; set; }

        /// <summary>
        /// 发布权限成员
        /// </summary>
        public virtual List<WorkflowTemplateRltMember> Members { get; set; } = new List<WorkflowTemplateRltMember>();
    }
}
