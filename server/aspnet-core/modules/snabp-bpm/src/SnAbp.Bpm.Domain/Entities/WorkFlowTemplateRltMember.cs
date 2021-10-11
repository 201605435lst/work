
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Entities
{
    public class WorkflowTemplateRltMember : Entity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected WorkflowTemplateRltMember() { }
        public WorkflowTemplateRltMember(Guid id) { Id = id; }

        public virtual Guid WorkflowTemplateId { get; set; }

        public virtual WorkflowTemplate WorkflowTemplate { get; set; }

        public Guid MemberId { get; set; }
        public MemberType Type { get; set; }
        public string Name { get; set; }
    }
}
