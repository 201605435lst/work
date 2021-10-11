using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dto.CategoryRltArticle
{
    public class CategoryRltArticleSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 栏目id集合
        /// </summary>
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enable { get; set; }

        /// <summary>
        /// 是否查询全部
        /// </summary>
        public bool IsAll { get; set; }
    }
}
