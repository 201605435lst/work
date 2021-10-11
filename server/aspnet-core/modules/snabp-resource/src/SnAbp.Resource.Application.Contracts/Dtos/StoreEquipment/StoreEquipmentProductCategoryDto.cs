using System;
using System.Collections.Generic;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentProductCategoryDto : EntityDto<Guid>, IGuidKeyTree<StoreEquipmentProductCategoryDto>
    {
        /// <summary>
        /// 上级产品分类id
        /// </summary>
        public Guid? ParentId { get; set; }
        public StoreEquipmentProductCategoryDto Parent { get; set; }
        public List<StoreEquipmentProductCategoryDto> Children { get; set; } = new List<StoreEquipmentProductCategoryDto>();
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
