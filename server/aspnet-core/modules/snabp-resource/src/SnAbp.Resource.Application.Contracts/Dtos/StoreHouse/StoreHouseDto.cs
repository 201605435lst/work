using SnAbp.Common.Dtos;
using SnAbp.Common.Entities;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
   public class StoreHouseDto : EntityDto<Guid>, IGuidKeyTree<StoreHouseDto>
    {
        /// <summary>
        /// 父级
        /// </summary>
        public StoreHouseDto Parent { get; set; }
        public Guid? ParentId { get; set; }
        public List<StoreHouseDto> Children { get; set; } = new List<StoreHouseDto>();
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域地址
        /// </summary>
        public int? AreaId { get; set; }
        public AreaDto Area { get; set; }
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
        /// 排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
