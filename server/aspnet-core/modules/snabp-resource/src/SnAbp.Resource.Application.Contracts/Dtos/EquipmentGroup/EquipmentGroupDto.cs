
using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentGroupDto : EntityDto<Guid>, IGuidKeyTree<EquipmentGroupDto>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public EquipmentGroupDto Parent { get; set; }
        public List<EquipmentGroupDto> Children { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
