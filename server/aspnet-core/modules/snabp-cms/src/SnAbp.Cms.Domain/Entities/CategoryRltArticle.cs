using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Cms.Entities
{
    /// <summary>
    /// 栏目-文章关联表
    /// </summary>
    public class CategoryRltArticle : Entity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 栏目id
        /// </summary>
        public virtual Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        public virtual Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        protected CategoryRltArticle() { }
        public CategoryRltArticle(Guid id)
        {
            Id = id;
        }
    }
}
