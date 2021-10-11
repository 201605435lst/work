using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    /// <summary>
    /// 答案选项
    /// </summary>
    public class AnswerOption : AuditedEntity<Guid>
    {
        protected AnswerOption() { }
        public AnswerOption(Guid id) { Id = id; }

        /// <summary>
        /// 题目ID
        /// </summary>
        [Required]
        public virtual Guid? QuestionId { get; set; }
        public virtual Question Question { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


    }
}
