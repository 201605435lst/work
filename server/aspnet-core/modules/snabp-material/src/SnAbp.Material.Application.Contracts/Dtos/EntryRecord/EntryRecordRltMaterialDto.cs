using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 入库关联物资
    /// </summary>
    public class EntryRecordRltMaterialDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联库存
        /// </summary>
        public virtual Guid InventoryId { get; set; }
        public virtual InventoryDto Inventory { get; set; }

        /// <summary>
        /// 入库记录
        /// </summary>
        public virtual Guid EntryRecordId { get; set; }
        public virtual EntryRecordDto EntryRecord { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual Technology.Dtos.MaterialDto Material { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public virtual decimal Count { get; set; }

        /// <summary>
        /// 采购价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 到货日期
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual Guid? SupplierId { get; set; }
        public virtual SupplierDto Supplier { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        protected EntryRecordRltMaterialDto() { }
        public EntryRecordRltMaterialDto(Guid id)
        {
            Id = id;
        }
    }
}
