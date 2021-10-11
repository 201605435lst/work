
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    public class KnowledgePoint : AuditedEntity<Guid>,IGuidKeyTree<KnowledgePoint>
    {
        protected KnowledgePoint() { }
        public KnowledgePoint(Guid id) { Id = id; }

        /// <summary>
        /// 知识点名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual KnowledgePoint Parent { get; set; }
        public virtual List<KnowledgePoint> Children { get; set; }


       [InverseProperty("KnowledgePoint")]
        /// <summary>
        /// 分类关联
        /// </summary>
        public List<KnowledgePointRltCategory> KnowledgePointRltCategories { get; set; }
    }
}
