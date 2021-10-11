using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 联锁设备型号（铁路联锁设备控制类型）
    /// </summary>
    public class EquipmentControlType : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        public Guid? ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [MaxLength(50)]
        [Description("类型")]
        public string TypeGroup { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        protected EquipmentControlType() { }
        public EquipmentControlType(Guid id)
        {
            Id = id;
        }
    }
}
