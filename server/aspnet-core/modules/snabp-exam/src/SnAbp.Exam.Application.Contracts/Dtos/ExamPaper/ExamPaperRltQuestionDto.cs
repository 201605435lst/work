using SnAbp.Exam.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperRltQuestionDto
    {
        /// <summary>
        /// 考题Id
        /// </summary>
        public Guid QuestionId { get; set; }
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