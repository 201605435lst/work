using SnAbp.Identity;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Schedule.Entities
{
    public class ApprovalRltMember : FullAuditedEntity<Guid>
    {
        public ApprovalRltMember(Guid id) => Id = id;
        //审批id   
        public Approval Approval { get; set; }
        public virtual Guid? ApprovalId { get; set; }
        //成员id
        public IdentityUser Member { get; set; }
        public virtual Guid? MemberId { get; set; }
        //责任类型,是指派人还是抄送人
        public PersonType Type { get; set; }
    }
}
