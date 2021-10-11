using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentGroupGetListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 获取兄弟元素的Id集合
        /// </summary>
        public List<Guid> ? Ids { get; set; }
        /// <summary>
        /// 是否分页加载
        /// </summary>
        public bool IsAll { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
