
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Entities
{
    public class FlowTemplateNode : Entity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected FlowTemplateNode() { }
        public FlowTemplateNode(Guid id) { Id = id; }

        public virtual Guid FlowTemplateId { get; set; }
        public virtual FlowTemplate FlowTemplate { get; set; }

        public string Label { get; set; }
        public List<float> Size { get; set; }
        public string Type { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool Active { get; set; } = false;
        public string Code { get; set; }
        public virtual List<FlowNodeFormItemPermisstion> FormItemPermisstions { get; set; }
        public virtual List<FlowTemplateNodeRltMember> Members { get; set; }
    }
}