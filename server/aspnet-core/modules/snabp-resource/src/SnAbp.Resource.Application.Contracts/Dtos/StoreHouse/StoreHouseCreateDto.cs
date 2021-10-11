using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreHouseCreateDto
    {
        /// <summary>
        /// 父级//
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域地址
        /// </summary>
        public int? AreaId { get; set; }
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
        public int Order { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid OrganizationId { get; set; }

    }
}
