using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 产品分类表
    /// </summary>
    public class ProductCategory : FullAuditedEntity<Guid>, IGuidKeyTree<ProductCategory>, ICodeTree<ProductCategory>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 上级产品分类id
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual ProductCategory Parent { get; set; }
        public virtual List<ProductCategory> Children { get; set; }

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

        /// <summary>
        /// 带有上级的全名称
        /// </summary>
        [NotMapped]
        public string FullName { get; set; }


        /// <summary>
        /// 端子图
        /// </summary>
        public string TerminalSymbol { get; set; }


        /// <summary>
        /// 构件分类与产品分类关联关系
        /// </summary>
        public virtual List<ComponentCategoryRltProductCategory> ComponentCategoryRltProductCategories { get; set; }

        /// <summary>
        /// 产品分类与MVD
        /// </summary>
        public virtual List<ProductCategoryRltMVDProperty> ProductCategoryRltMVDProperties { get; set; }

        protected ProductCategory() { }
        public ProductCategory(Guid id)
        {
            Id = id;
        }
    }
}