using SnAbp.Identity;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Entities
{
    /// <summary>
    /// 项目关联成员
    /// </summary>
    public class ProjectRltMember : FullAuditedEntity<Guid>
    {
        public ProjectRltMember(Guid id) => Id = id;
        //项目名称id   
        public Project Project { get; set; }
        //成员id
        public IdentityUser Manager { get; set; }
        public virtual Guid? ManagerId { get; set; }
        public virtual Guid? ProjectId { get; set; }
    }
}
