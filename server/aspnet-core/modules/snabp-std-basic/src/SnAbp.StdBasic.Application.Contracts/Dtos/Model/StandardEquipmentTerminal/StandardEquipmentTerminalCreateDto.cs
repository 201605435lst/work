using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{

    public class StandardEquipmentTerminalCreateDto
    {
        /// <summary>
        /// 标准设备
        /// </summary>
        public Guid ModelId { get; set; }

        /// <summary>
        /// 产品分类（改连接件标准设备的产品分类的子分类）
        /// </summary>
        public Guid ProductCategoryId { get; set; }

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