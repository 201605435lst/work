using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowDto : AuditedEntity<Guid>
    {
        /// <summary>
        /// 流程模板 Id 该字段在首次发起的时候为空，
        /// </summary>
        public virtual Guid? FlowTemplateId { get; set; }

        /// <summary>
        /// 流程模板
        /// </summary>
        public virtual FlowTemplate FlowTemplate { get; set; }


        /// <summary>
        /// 工作流实例数据
        /// </summary>
        public virtual List<WorkflowData> WorkflowDatas { get; set; }


        public WorkflowState State { get; set; }
    }
}
