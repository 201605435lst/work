using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    public class KnowledgePointRltCategory : AuditedEntity<Guid>
    {
        protected KnowledgePointRltCategory() { }
        public KnowledgePointRltCategory(Guid id) {Id = id; }

        /// <summary>
        /// 知识点 Id
        /// </summary>
        public Guid KnowledgePointId { get; set; }
        public virtual KnowledgePoint KnowledgePoint { get; set; }

        /// <summary>
        /// 分类 Id
        /// </summary>
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
