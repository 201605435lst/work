using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    /// <summary>
    /// 考题与知识点关联表
    /// </summary>
    public class QuestionRltKnowledgePoint : AuditedEntity<Guid>
    {
        protected QuestionRltKnowledgePoint() { }
        public QuestionRltKnowledgePoint(Guid id) { Id = id; }

        /// <summary>
        /// 考题
        /// </summary>
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }

        /// <summary>
        /// 知识点
        /// </summary>
        public Guid KnowledgePointId { get; set; }
        public virtual KnowledgePoint KnowledgePoint { get; set; }
    }
}
