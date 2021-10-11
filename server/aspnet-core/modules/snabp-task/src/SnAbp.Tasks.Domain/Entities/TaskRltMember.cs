using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Tasks;
using SnAbp.Tasks.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Tasks.Entities
{
    public class TaskRltMember : FullAuditedEntity<Guid>
    {
        public TaskRltMember(Guid id) => Id = id;
        //项目名称id   
        public Task Task { get; set; }
        public virtual Guid? TaskId { get; set; }
        //成员id
        public IdentityUser Member { get; set; }
        public virtual Guid? MemberId { get; set; }
        //责任类型,是指派人还是抄送人
        public ResponsibleType Responsible { get; set; }

        public Guid? ProjectId { get;}
    }
}
