using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 厂家
    /// </summary>
    public class Manufacturer : FullAuditedEntity<Guid>, IGuidKeyTree<Manufacturer>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [MaxLength(50)]
        [Description("简称")]
        public string ShortName { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [MaxLength(500)]
        [Description("简介")]
        public string Introduction { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [MaxLength(50)]
        [Description("类型")]
        public string Type { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        [Description("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 铁路编码
        /// </summary>
        [MaxLength(50)]
        [Description("CSRG编码")]
        public string CSRGCode { get; set; }

        /// <summary>
        /// 上级厂家
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual Manufacturer Parent { get; set; }
        public virtual List<Manufacturer> Children { get; set; } = new List<Manufacturer>();

        /// <summary>
        /// 负责人
        /// </summary>
        [MaxLength(50)]
        [Description("负责人")]
        public string Principal { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(50)]
        [Description("联系电话")]
        public string Telephone { get; set; }

        /// <summary>
        /// 厂家地址
        /// </summary>
        [MaxLength(500)]
        [Description("厂家地址")]
        public string Address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public virtual List<EquipmentControlType> EquipmentControlTypes { get; set; } = new List<EquipmentControlType>();

        protected Manufacturer() { }
        public Manufacturer(Guid id)
        {
            Id = id;
        }
    }
}
