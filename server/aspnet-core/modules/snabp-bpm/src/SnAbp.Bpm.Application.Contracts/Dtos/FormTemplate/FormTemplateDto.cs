using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class FormTemplateDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 工作流 Id
        /// </summary>
        public Guid WorkflowTemplateId { get; set; }

        /// <summary>
        /// 表单项
        /// </summary>
        public virtual string FormItems { get; set; }

        /// <summary>
        /// 表单配置
        /// </summary>
        public virtual string Config { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
