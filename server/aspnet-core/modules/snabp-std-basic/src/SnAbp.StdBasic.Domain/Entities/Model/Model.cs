using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 模型（有产品分类编码和厂家的属于标准设备）
    /// </summary>
    public class Model : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 编码（标准设备编码）
        /// </summary>
        [MaxLength(100)]
        [Description("编码")]
        public string? Code { get; set; }

        /// <summary>
        /// 铁总编码（标准设备编码）
        /// </summary>
        [MaxLength(100)]
        [Description("编码")]
        public string? CSRGCode { get; set; }

        /// <summary>
        /// 构件分类id
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public virtual ComponentCategory ComponentCategory { get; set; }

        /// <summary>
        /// 产品分类id
        /// </summary>
        public Guid? ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// 厂家id
        /// </summary>
        public Guid? ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// 使用寿命
        /// </summary>
        public float? ServiceLife { get; set; }


        /// <summary>
        /// 使用寿命单位
        /// </summary>
        public ServiceLifeUnit? ServiceLifeUnit { get; set; }


        [InverseProperty("Model")]
        /// <summary>
        /// 模型嵌套关系
        /// </summary>
        public virtual List<ModelRltModel> ModelRltModels { get; set; }

        /// <summary>
        /// 模型文件
        /// </summary>
        public virtual List<ModelFile> ModelFiles { get; set; }

        /// <summary>
        /// 信息模板属性关联
        /// </summary>
        public virtual List<ModelRltMVDProperty> ModelRltMVDProperties { get; set; }

        /// <summary>
        /// 信息图块关联
        /// </summary>
        public virtual List<ModelRltBlock> ModelRltBlocks { get; set; }


        /// <summary>
        /// 标准设备端子
        /// </summary>
        public virtual List<ModelTerminal> Terminals { get; set; }


        protected Model() { }
        public Model(Guid id)
        {
            Id = id;
        }
    }

}
