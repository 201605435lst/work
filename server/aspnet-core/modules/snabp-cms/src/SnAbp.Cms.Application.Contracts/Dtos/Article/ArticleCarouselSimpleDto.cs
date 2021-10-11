using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleCarouselSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public virtual Guid ArticleId { get; set; }
        public virtual ArticleSimpleDto Article { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual FileSimpleDto File { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}