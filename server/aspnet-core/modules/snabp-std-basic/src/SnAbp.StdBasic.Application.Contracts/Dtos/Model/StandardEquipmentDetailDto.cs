using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 标准设备
    /// </summary>
    public class StandardEquipmentDetailDto : EntityDto<Guid>
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
        public string? CSRGCode { get; set; }

        /// <summary>
        /// 使用寿命
        /// </summary>
        public float? ServiceLife { get; set; }


        /// <summary>
        /// 使用寿命单位
        /// </summary>
        public ServiceLifeUnit? ServiceLifeUnit { get; set; }

        /// <summary>
        /// 构件分类id
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public virtual ComponentCategoryDto ComponentCategory { get; set; }

        /// <summary>
        /// 产品分类id
        /// </summary>
        public Guid? ProductCategoryId { get; set; }
        public virtual ProductCategoryDto ProductCategory { get; set; }

        /// <summary>
        /// 厂家id
        /// </summary>
        public Guid? ManufacturerId { get; set; }
        public virtual ManufacturerDto Manufacturer { get; set; }

        /// <summary>
        /// 标准设备关联端子
        /// </summary>
        public virtual List<StandardEquipmentTerminalDto> Terminals { get; set; }
    }
}
