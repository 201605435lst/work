using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单关联人员
    /// </summary>
    public class DispatchRltWorker : Entity<Guid>
    {
        public DispatchRltWorker(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public virtual Guid WorkerId { get; set; }
        public virtual IdentityUser Worker { get; set; }
    }
}
