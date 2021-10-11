using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperRltQuestionSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 试题类型
        /// </summary>
        public QuestionType Type { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public virtual Guid KnowledgePointId { get; set; }
        /// <summary>
        /// 开始的难度系数
        /// </summary>
        public float StartDifficultyCoefficient { get; set; }
        /// <summary>
        /// 结束的难度系数
        /// </summary>
        public float EndDifficultyCoefficient { get; set; }
    }
}
