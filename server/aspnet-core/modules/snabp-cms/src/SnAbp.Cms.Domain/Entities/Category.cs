using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;
using SnAbp.File.Entities;
using SnAbp.Utils.TreeHelper;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Cms.Entities
{
    /// <summary>
    /// 栏目
    /// </summary>
    public class Category : AuditedEntity<Guid>, IGuidKeyTree<Category>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected Category() { }
        public Category(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [MaxLength(30)]
        public string Code { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        [MaxLength(200)]
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 缩略图文件id
        /// </summary>
        public virtual Guid? ThumbId { get; set; }

        /// <summary>
        /// 缩略图文件
        /// </summary>
        public virtual File.Entities.File Thumb { get; set; }

        /// <summary>
        /// 使用启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Category Parent { get; set; }

        public virtual List<Category> Children { get; set; }
    }
}
