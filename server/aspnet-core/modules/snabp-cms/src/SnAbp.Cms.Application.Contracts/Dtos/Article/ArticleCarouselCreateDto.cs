using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleCarouselCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
