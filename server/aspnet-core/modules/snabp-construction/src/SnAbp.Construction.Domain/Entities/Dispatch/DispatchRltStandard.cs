using SnAbp.ConstructionBase.Entities;
using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单关联工序指引
    /// </summary>
    public class DispatchRltStandard : Entity<Guid>
    {
        public DispatchRltStandard(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }

        /// <summary>
        /// 工序指引
        /// </summary>
        public virtual Guid StandardId { get; set; }
        public virtual Standard Standard { get; set; }
    }
}
