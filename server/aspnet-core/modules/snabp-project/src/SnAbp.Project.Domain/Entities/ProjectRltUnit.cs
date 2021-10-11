using SnAbp.Project.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Entities
{
    /// <summary>
    /// 项目关联建设单位
    /// </summary>
    public class ProjectRltUnit : FullAuditedEntity<Guid>
    {
        public ProjectRltUnit(Guid id) => Id = id; 
        public Project Project { get; set; }

        public Unit Unit { get; set; }
        public virtual Guid? ProjectId { get; set; }
        public virtual Guid? UnitId { get; set; }
    }
}
