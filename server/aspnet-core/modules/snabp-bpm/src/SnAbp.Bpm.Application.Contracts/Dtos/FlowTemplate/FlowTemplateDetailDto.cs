using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class FlowTemplateDetailDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 表单模板 Id
        /// </summary>
        public Guid FormTemplateId { get; set; }

        /// <summary>
        /// 流程节点
        /// </summary>
        public virtual List<FlowTemplateNodeDto> Nodes { get; set; }


        /// <summary>
        /// 流程关系
        /// </summary>
        public virtual List<FlowTemplateStepDto> Steps { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
