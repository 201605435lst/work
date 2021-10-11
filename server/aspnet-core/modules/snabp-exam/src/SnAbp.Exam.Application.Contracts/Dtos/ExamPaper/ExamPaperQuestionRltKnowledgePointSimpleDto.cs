using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperQuestionRltKnowledgePointSimpleDto
    {
        /// <summary>
        /// 考题
        /// </summary>
        public virtual Guid QuestionId { get; set; }

        /// <summary>
        /// 知识点
        /// </summary>
        public virtual Guid KnowledgePointId { get; set; }
    }
}
