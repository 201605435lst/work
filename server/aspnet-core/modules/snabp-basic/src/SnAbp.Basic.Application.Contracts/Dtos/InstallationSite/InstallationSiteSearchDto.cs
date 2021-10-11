using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class InstallationSiteSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 线别
        /// </summary>
        public Guid? RailwayId { get; set; }

        /// <summary>
        /// 站区Id
        /// </summary>
        public Guid? StationId { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// 机房类型
        /// </summary>
        public Guid? TypeId { get; set; }

        /// <summary>
        /// 类别(数据字典) 中继站、变电所等
        /// </summary>
        public  Guid? CategoryId { get; set; }

        public InstallationSiteLocationType LocationType { get; set; }

        /// <summary>
        /// 关键字 站区、机房名称、编码
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否获取全部
        /// </summary>
        public bool IsAll { get; set; }
    }
}
