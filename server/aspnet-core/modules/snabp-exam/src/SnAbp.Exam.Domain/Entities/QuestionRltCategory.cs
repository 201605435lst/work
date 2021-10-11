using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    public class QuestionRltCategory : AuditedEntity<Guid>
    {
        protected QuestionRltCategory() { }
        public QuestionRltCategory(Guid id) { Id = id; }

        /// <summary>
        /// 考题ID
        /// </summary>
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
