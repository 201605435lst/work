using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 供应商附件
    /// </summary>
    public class SupplierRltAccessory : Entity<Guid>
    {
        /// <summary>
        /// 供应商id
        /// </summary>
        public virtual Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        protected SupplierRltAccessory() { }
        public SupplierRltAccessory(Guid id)
        {
            Id = id;
        }
    }
}
