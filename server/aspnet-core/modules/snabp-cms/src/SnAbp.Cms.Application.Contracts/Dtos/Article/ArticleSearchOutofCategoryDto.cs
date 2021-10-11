using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleSearchOutofCategoryDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 是否全部显示
        /// </summary>
        public bool IsAll { get; set; }
    }
}
