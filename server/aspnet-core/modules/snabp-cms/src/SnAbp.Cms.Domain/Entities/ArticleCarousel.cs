using SnAbp.File.Entities;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Cms.Entities
{
    /// <summary>
    /// 文章 轮播图
    /// </summary>
    public class ArticleCarousel : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual File.Entities.File File { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        protected ArticleCarousel() { }
        public ArticleCarousel(Guid id)
        {
            Id = id;
        }
    }
}
