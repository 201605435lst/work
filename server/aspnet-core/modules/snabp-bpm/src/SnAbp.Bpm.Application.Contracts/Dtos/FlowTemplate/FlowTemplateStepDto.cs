using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class FlowTemplateStepDto : EntityDto<Guid>
    {
        public string Type { get; set; }
        public Guid Source { get; set; }
        public Guid Target { get; set; }
        public int SourceAnchor { get; set; }
        public int TargetAnchor { get; set; }
        public bool Active { get; set; } = false;


        /// <summary>
        /// 流程状态
        /// </summary>
        public WorkflowStepState? State { get; set; }


        /// <summary>
        /// 审批意见
        /// </summary>
        public string Comments { get; set; }
    }
}
