using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 模型信息交换模板属性关联表
    /// </summary>
    public class ComponentCategoryRltMVDProperty : Entity<Guid>
    {
        /// <summary>
        /// 模型Id
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public virtual ComponentCategory ComponentCategory { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [MaxLength(50)]
        [Description("值")]
        public string Value { get; set; }

        /// <summary>
        /// 模板属性Id
        /// </summary>
        public Guid? MVDPropertyId { get; set; }
        public virtual MVDProperty MVDProperty { get; set; }

        protected ComponentCategoryRltMVDProperty() { }
        public ComponentCategoryRltMVDProperty(Guid id)
        {
            Id = id;
        }
    }
}
