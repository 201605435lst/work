using SnAbp.File.Entities;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Cms.Entities
{
    /// <summary>
    /// 文章 附件
    /// </summary>
    public class ArticleAccessory : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public virtual Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        protected ArticleAccessory() { }
        public ArticleAccessory(Guid id)
        {
            Id = id;
        }
    }
}
