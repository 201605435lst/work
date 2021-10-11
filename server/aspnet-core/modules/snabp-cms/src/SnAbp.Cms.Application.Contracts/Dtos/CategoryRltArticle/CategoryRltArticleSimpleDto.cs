using SnAbp.Cms.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dto.CategoryRltArticle
{
    public class CategoryRltArticleSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        public Guid CategoryId { get; set; }
        public string CategoryTitle { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        public Guid ArticleId { get; set; }
        public string ArticleTitle { get; set; }

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
