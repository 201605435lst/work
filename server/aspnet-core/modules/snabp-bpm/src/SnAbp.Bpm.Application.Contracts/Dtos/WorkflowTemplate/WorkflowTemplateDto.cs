
using System;
using System.Collections.Generic;
using SnAbp.Identity;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowTemplateDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 是否静态 静态的工作流是程序创建的，不能删除
        /// </summary>
        public bool IsStatic { get; set; } = false;

        /// <summary>
        /// 表单列表
        /// </summary>
        public List<FormTemplateSimpleDto> FormTemplates { get; set; }

        /// <summary>
        /// 发布权限成员
        /// </summary>
        public virtual List<Member> Members { get; set; }
    }
}