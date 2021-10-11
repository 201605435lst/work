using SnAbp.StdBasic.Entities;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Resource.Dtos
{
   public class StoreEquipmentSimpleDto: AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 库存编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// 构件分类
        ///// </summary>
        //public Guid ComponentCategoryId { get; set; }
        //public ComponentCategory ComponentCategory { get; set; }


        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public Guid? StoreHouseId { get; set; }




    }
}
