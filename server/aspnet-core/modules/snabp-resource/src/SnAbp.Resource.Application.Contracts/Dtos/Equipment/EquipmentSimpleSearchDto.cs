using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentSimpleSearchDto: PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 安装地点    线路/站点/机房
        /// </summary>
        public List<Guid>? InstallationSiteIds { get; set; }

        public Guid OrgId { get; set; }

        /// <summary>
        /// 关键字 设备名称
        /// </summary>
        public string Keyword { get; set; }

        public List<string> IFDCodes { get; set; } = new List<string>();
    }
}
