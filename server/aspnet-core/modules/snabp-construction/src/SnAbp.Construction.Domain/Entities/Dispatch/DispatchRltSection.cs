using SnAbp.ConstructionBase.Entities;
using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单关联施工区段
    /// </summary>
    public class DispatchRltSection : Entity<Guid>
    {
        public DispatchRltSection(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }

        /// <summary>
        /// 施工区段
        /// </summary>
        public virtual Guid SectionId { get; set; }
        public virtual Section Section { get; set; }
    }
}
