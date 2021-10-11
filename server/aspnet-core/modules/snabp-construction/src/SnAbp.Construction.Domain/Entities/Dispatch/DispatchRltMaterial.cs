using SnAbp.MultiProject.MultiProject;
using SnAbp.Technology.Entities;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单关联材料
    /// </summary>
    public class DispatchRltMaterial : Entity<Guid>
    {
        public DispatchRltMaterial(Guid id) => Id = id;
        public void SetId(Guid id) => Id = id;
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal Count { get; set; }
    }
}
