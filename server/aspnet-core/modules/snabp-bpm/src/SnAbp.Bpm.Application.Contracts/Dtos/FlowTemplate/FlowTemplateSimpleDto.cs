using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class FlowTemplateSimpleDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 表单模板 Id
        /// </summary>
        public Guid FormTemplateId { get; set; }


        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
