using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    public class MaterialOfBillRltMaterial: AuditedEntity<Guid>
    {
        public MaterialOfBillRltMaterial(Guid id) => Id = id;

        public virtual MaterialOfBill MaterialOfBill { get; set; }
        public virtual Guid MaterialOfBillId { get; set; }

        public Inventory Inventory { get; set; }
        public virtual Guid? InventoryId { get; set; }

        /// <summary>
        /// 领料量
        /// </summary>
        public virtual decimal count { get; set; }
    }
}
