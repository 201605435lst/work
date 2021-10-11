using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 维护单位
        /// </summary>
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 设备分类
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }


        /// <summary>
        /// 安装地点    线路/站点/机房
        /// </summary>
        public Guid? InstallationSiteId { get; set; }

        /// <summary>
        /// 关键字 设备名称 设备编码
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 父级 Id
        /// </summary>
        public Guid? ParentId { get; set; }


        public bool IsAll { get; set; } = false;

        /// <summary>
        /// 是否查询待入库设备
        /// </summary>
        public bool IsWaitingStorage { get; set; }
        
    }
}
