using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Cms.Entities
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        [MaxLength(100)]
        public string Summary { get; set; }

        /// <summary>
        /// 缩略图文件id
        /// </summary>
        public virtual Guid? ThumbId { get; set; }

        /// <summary>
        /// 缩略图文件
        /// </summary>
        public virtual File.Entities.File Thumb { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [MaxLength(50)]
        public string Author { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 栏目
        /// </summary>
        public List<CategoryRltArticle> Categories { get; set; }

        /// <summary>
        /// 文章附件
        /// </summary>
        public List<ArticleAccessory> Accessories { get; set; }

        /// <summary>
        /// 文章轮播图
        /// </summary>
        public List<ArticleCarousel> Carousels { get; set; }


        protected Article() { }
        public Article(Guid id)
        {
            Id = id;
        }
    }
}
