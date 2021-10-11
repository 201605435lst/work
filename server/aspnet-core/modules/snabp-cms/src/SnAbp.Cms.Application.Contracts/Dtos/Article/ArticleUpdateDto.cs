using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 缩略图文件id
        /// </summary>
        public Guid? ThumbId { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 所属栏目id
        /// </summary>
        public List<ArticleCategoryCreateDto> Categories { get; set; } = new List<ArticleCategoryCreateDto>();

        /// <summary>
        /// 附件
        /// </summary>
        public List<ArticleAccessoryCreateDto> Accessories { get; set; } = new List<ArticleAccessoryCreateDto>();

        /// <summary>
        /// 轮播图
        /// </summary>
        public List<ArticleCarouselCreateDto> Carousels { get; set; } = new List<ArticleCarouselCreateDto>();
    }
}
