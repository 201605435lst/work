using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dto.CategoryRltArticle
{
    public class CategoryRltArticleUpdateEnableDto : EntityDto<Guid>
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
