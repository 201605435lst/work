using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperQuestionRltCategorySimpleDto :EntityDto<Guid>
    {
        /// <summary>
        /// 考题
        /// </summary>
        public virtual Guid QuestionId { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual Guid CategoryId { get; set; }

    }
}
