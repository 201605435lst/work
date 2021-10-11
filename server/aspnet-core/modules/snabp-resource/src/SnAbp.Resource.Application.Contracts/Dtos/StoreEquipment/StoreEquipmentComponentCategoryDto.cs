using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentComponentCategoryDto : EntityDto<Guid>, IGuidKeyTree<StoreEquipmentComponentCategoryDto>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级构件分类id
        /// </summary>
        public Guid? ParentId { get; set; }
        public StoreEquipmentComponentCategoryDto Parent { get; set; }
        public List<StoreEquipmentComponentCategoryDto> Children { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
