using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dto.CategoryRltArticle
{
    public class CategoryRltArticleUpdateOrderDto : EntityDto<Guid>
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
