using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class CategorySearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enable { get; set; }

        /// <summary>
        /// 是否全部查询
        /// </summary>
        public bool IsAll { get; set; }
    }
}
