using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 构件分类表
    /// </summary>
    public class ComponentCategory : FullAuditedEntity<Guid>, IGuidKeyTree<ComponentCategory>, ICodeTree<ComponentCategory>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 上级构件分类id
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual ComponentCategory Parent { get; set; }
        public virtual List<ComponentCategory> Children { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        [Description("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 层级名称
        /// </summary>
        [MaxLength(50)]
        [Description("层级名称")]
        public string LevelName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [MaxLength(30)]
        [Description("单位")]
        public string Unit { get; set; }

        /// <summary>
        /// 扩展编码
        /// </summary>
        [MaxLength(50)]
        [Description("扩展编码")]
        public string ExtendCode { get; set; }

        /// <summary>
        /// 扩展名称
        /// </summary>
        [MaxLength(50)]
        [Description("扩展名称")]
        public string ExtendName { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public virtual List<ComponentCategoryRltProductCategory> ComponentCategoryRltProductCategories { get; set; }


        /// <summary>
        /// 构件分类与MVD
        /// </summary>
        public virtual List<ComponentCategoryRltMVDProperty> ComponentCategoryRltMVDProperties { get; set; }

        protected ComponentCategory() { }
        public ComponentCategory(Guid id)
        {
            Id = id;
        }
    }
}
