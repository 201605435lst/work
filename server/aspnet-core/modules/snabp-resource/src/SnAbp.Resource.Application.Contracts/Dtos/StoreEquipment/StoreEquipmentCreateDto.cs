using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentCreateDto 
    {
        /// <summary>
        /// 库存编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 构件分类
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
        public ComponentCategory ComponentCategory { get; set; }


        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        public Guid ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public StoreEquipmentState State { get; set; } = StoreEquipmentState.UnActived;

        /// <summary>
        /// 仓库
        /// </summary>
        public Guid? StoreHouseId { get; set; }
        public StoreHouseDto StoreHouse { get; set; }
    }
}
