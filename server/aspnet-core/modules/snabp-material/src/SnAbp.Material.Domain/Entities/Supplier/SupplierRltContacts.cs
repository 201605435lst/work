using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 联系人
    /// </summary>
    public class SupplierRltContacts : Entity<Guid>
    {
        /// <summary>
        /// 供应商id
        /// </summary>
        public virtual Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [MaxLength(50)]
        public string Telephone { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [MaxLength(50)]
        public string LandlinePhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [MaxLength(50)]
        public string QQ { get; set; }

        protected SupplierRltContacts() { }
        public SupplierRltContacts(Guid id)
        {
            Id = id;
        }
    }
}
