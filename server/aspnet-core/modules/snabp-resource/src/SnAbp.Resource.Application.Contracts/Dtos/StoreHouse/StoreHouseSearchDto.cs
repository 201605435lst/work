using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreHouseSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字：名称或者地址
        /// </summary>
        public string KeyWords { get; set; }

        public bool? Enabled { get; set; }
        /// <summary>
        /// 区域地址
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 是否全部加载
        /// </summary>
        public bool IsAll { get; set; }
        /// <summary>
        /// 获取兄弟及父级元素的Id集合
        /// </summary>
        public List<Guid>? Ids { get; set; }
        /// <summary>
        /// 查询仓库的状态
        /// </summary>
        public bool Status { get; set; }

    }
}
