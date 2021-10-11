using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using SnAbp.Common.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class StoreHouse : FullAuditedEntity<Guid>, IGuidKeyTree<StoreHouse>
    {
        protected StoreHouse() { }
        public StoreHouse(Guid id) { Id = id; }

        /// <summary>
        /// 项目id
        /// </summary>
         public Guid? ProjectTagId { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
        public StoreHouse Parent { get; set; }
        public List<StoreHouse> Children { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域地址
        /// </summary>
        public int? AreaId { get; set; }
        public Area Area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 经纬度坐标 Json {longitude:2.5，latitude:3.6}
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
