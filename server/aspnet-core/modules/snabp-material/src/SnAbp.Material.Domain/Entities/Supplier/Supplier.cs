using SnAbp.Material.Enums;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 供应商实体
    /// </summary>
    public class Supplier : Entity<Guid>
    {
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public SupplierType Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public SupplierLevel Level { get; set; }

        /// <summary>
        /// 性质
        /// </summary>
        public SupplierProperty Property { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [MaxLength(50)]
        public string Principal { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(50)]
        public string Telephone { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        [MaxLength(50)]
        public string LegalPerson { get; set; }

        /// <summary>
        /// 纳税人识别号
        ///全称 Taxpayer Identification Number
        /// </summary>
        [MaxLength(100)]
        public string TIN { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        [MaxLength(500)]
        public string BusinessScope { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [MaxLength(200)]
        public string OpeningBank { get; set; }

        /// <summary>
        /// 开户行账户
        /// </summary>
        [MaxLength(100)]
        public string BankAccount { get; set; }

        /// <summary>
        /// 开户单位
        /// </summary>
        [MaxLength(200)]
        public string AccountOpeningUnit { get; set; }

        /// <summary>
        /// 注册资本
        /// </summary>
        [MaxLength(100)]
        public string RegisteredAssets { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        public string Remark { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<SupplierRltAccessory> SupplierRltAccessories { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public List<SupplierRltContacts> SupplierRltContacts { get; set; }


        protected Supplier() { }
        public Supplier(Guid id)
        {
            Id = id;
        }


    }
}
