using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Cms.Dtos
{
    public class CategoryRltArticleCreateDto
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
