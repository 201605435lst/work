using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class SearchListByScopeInput
    {
        /// <summary>
        /// 查询范围 1@Name@OrganizationId.2@Name@RailwayId.3@Name@StationId.4@Name@InstallationId
        /// </summary>
        public string ScopeCode { get; set; } 


        /// <summary>
        /// 父级 id
        /// </summary>
        public Guid? ParentId { get; set; }


        /// <summary>
        /// 初始选中的设备  Group@Name
        /// </summary>
        public List<string>? InitialGroupAndNames { get; set; }


        /// <summary>
        /// 类别
        /// </summary>
        public GetListByScopeType? Type { get; set; }
    }
}
