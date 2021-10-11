using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 标准设备端子（安装位置，信号提示灯，开关等）
    /// </summary>
    public class StandardEquipmentTerminalDto : EntityDto<Guid>
    {
        /// <summary>
        /// 标准设备
        /// </summary>
        public Guid ModelId { get; set; }
        public StandardEquipmentDto Model { get; set; }

        /// <summary>
        /// 产品分类（改连接件标准设备的产品分类的子分类）
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }

        /// <summary>
        /// 连接件名称
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }
    }
}