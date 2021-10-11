using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 出库关联物资
    /// </summary>
    public class OutRecordRltMaterial : Entity<Guid>
    {
        /// <summary>
        /// 出库记录
        /// </summary>
        public virtual Guid OutRecordId { get; set; }
        public virtual OutRecord OutRecord { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual Technology.Entities.Material Material { get; set; }

        /// <summary>
        /// 关联库存
        /// </summary>
        public virtual Guid InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public virtual decimal Count { get; set; }

        /// <summary>
        /// 采购价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        protected OutRecordRltMaterial() { }
        public OutRecordRltMaterial(Guid id)
        {
            Id = id;
        }
    }
}
