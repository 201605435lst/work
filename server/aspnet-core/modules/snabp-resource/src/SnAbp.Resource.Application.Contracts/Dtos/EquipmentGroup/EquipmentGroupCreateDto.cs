using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentGroupCreateDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public Guid? ParentId { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}
