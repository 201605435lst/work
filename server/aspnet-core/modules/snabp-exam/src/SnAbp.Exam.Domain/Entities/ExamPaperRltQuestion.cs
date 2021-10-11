using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Exam.Entities
{
    public class ExamPaperRltQuestion : Entity<Guid>
    {
        protected ExamPaperRltQuestion() { }
        public ExamPaperRltQuestion(Guid id) { Id = id; }

        /// <summary>
        /// 试卷 id
        /// </summary>
        public Guid ExamPaperId { get; set; }
        public virtual ExamPaper ExamPaper { get; set; }
        /// <summary>
        /// 题库 Id
        /// </summary>
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
