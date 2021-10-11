using SnAbp.MultiProject.MultiProject;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Entities
{
    public class FlowTemplateStep : Entity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected FlowTemplateStep() { }
        public FlowTemplateStep(Guid id) { Id = id; }

        public virtual Guid FlowTemplateId { get; set; }
        public virtual FlowTemplate FlowTemplate { get; set; }
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