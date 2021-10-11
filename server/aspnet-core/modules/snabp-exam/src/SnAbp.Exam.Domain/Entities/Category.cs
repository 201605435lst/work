
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    /// <summary>
    /// 题库分类
    /// </summary>
    public class Category : AuditedEntity<Guid>, IGuidKeyTree<Category>
    {
        protected Category() { }
        public Category(Guid id) { Id = id; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 分类描述
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 添加外键ParentId
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual List<Category> Children { get; set; }

        /// <summary>
        /// 分类排序
        /// </summary>
        public int Order { get; set; }
    }
}
