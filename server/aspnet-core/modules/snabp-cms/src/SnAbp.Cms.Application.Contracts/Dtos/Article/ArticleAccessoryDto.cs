using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleAccessoryDto : EntityDto<Guid>
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public Guid ArticleId { get; set; }
        public ArticleDto Article { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public Guid FileId { get; set; }
    }
}
