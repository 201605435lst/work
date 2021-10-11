using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace  SnAbp.Material.Entities
{
    public class MaterialAcceptanceRltPurchase : Entity<Guid>
    {
        /// <summary>
        /// 关联验收单
        /// </summary>
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptance MaterialAcceptance { get; set; }

        /// <summary>
        /// 采购清单
        /// </summary>
        public virtual Guid PurchaseListId { get; set; }
        public virtual PurchaseList PurchaseList { get; set; }

        protected MaterialAcceptanceRltPurchase() { }
        public MaterialAcceptanceRltPurchase(Guid id)
        {
            Id = id;
        }
    }
}
