using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 出库关联物资
    /// </summary>
    public class OutRecordRltMaterialDto : EntityDto<Guid>
    {
        /// <summary>
        /// 出库记录
        /// </summary>
        public virtual Guid OutRecordId { get; set; }
        public virtual OutRecordDto OutRecord { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual Technology.Dtos.MaterialDto Material { get; set; }

        /// <summary>
        /// 关联库存
        /// </summary>
        public virtual Guid InventoryId { get; set; }
        public virtual InventoryDto Inventory { get; set; }

        /// <summary>
        /// 入库数量
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
        public virtual SupplierDto Supplier { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        protected OutRecordRltMaterialDto() { }
        public OutRecordRltMaterialDto(Guid id)
        {
            Id = id;
        }
    }
}
