using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 设备属性
    /// </summary>
    public class EquipmentProperty : FullAuditedEntity<Guid>
    {
        protected EquipmentProperty() { }
        public EquipmentProperty(Guid id) { Id = id; }

        /// <summary>
        /// 所属设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 关联 MVD 属性分类
        /// </summary>
        public Guid? MVDCategoryId { get; set; }
        public virtual MVDCategory MVDCategory { get; set; }


        /// <summary>
        /// 关联 MVD 属性
        /// </summary>
        public Guid? MVDPropertyId { get; set; }
        public virtual MVDProperty MVDProperty { get; set; }
    }
}
