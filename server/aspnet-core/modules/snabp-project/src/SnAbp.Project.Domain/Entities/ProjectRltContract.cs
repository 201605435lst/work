using SnAbp.Oa.Entities;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Entities
{
   public class ProjectRltContract : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 项目关联合同
        /// </summary>
        public ProjectRltContract(Guid id) => Id = id;
        public Contract Contract { get; set; }
        public virtual Guid? ContractId { get; set; }
        public virtual Guid? ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
