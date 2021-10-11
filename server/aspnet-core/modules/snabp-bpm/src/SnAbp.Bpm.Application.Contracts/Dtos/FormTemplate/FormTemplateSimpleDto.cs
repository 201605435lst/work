using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class FormTemplateSimpleDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 工作流 Id
        /// </summary>
        public Guid WorkflowTemplateId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 流程模板列表
        /// </summary>
        public List<FlowTemplateSimpleDto> FlowTemplates { get; set; }
    }
}
