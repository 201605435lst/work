using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Emerg.Entities
{
    public class EmergPlanRltFile : Entity<Guid>
    {
        protected EmergPlanRltFile() { }
        public EmergPlanRltFile(Guid id) { Id = id; }

        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        /// <summary>
        /// 预案id
        /// </summary>
        public virtual Guid EmergPlanId { get; set; }
        public virtual EmergPlan EmergPlan { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
