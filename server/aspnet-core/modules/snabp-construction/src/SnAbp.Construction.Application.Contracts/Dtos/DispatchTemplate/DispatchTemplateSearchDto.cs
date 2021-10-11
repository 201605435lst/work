
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单模板 SearchDto (搜索Dto) 
    /// </summary>
    public class DispatchTemplateSearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
    {

        /// <summary>
        /// 模糊搜索 
        /// </summary>
        public string SearchKey { get; set; }
        /// <summary>
        /// 是否全部
        /// </summary>
        public bool IsAll { get; set; }

    }
}
