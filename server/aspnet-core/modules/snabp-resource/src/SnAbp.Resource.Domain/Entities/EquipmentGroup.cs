using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Resource.Entities
{
    public class EquipmentGroup : Entity<Guid>, IGuidKeyTree<EquipmentGroup>
    {

        [MaxLength(50)]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public EquipmentGroup Parent { get; set; }
        public List<EquipmentGroup> Children { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        protected EquipmentGroup() { }
        public EquipmentGroup(Guid id) { Id = id; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
    }
}
