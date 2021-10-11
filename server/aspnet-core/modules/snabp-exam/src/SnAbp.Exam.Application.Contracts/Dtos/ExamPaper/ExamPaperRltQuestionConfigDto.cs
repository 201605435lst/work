using SnAbp.Exam.Dtos;
using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperRltQuestionConfigDto : AuditedEntityDto<Guid>
    {

        /// <summary>
        /// 类型
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// 难度系数
        /// </summary>
        public float DifficultyCoefficient { get; set; }

        /// <summary>
        /// 题目
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 考题与分类关联表
        /// </summary>
        public List<ExamPaperQuestionRltCategorySimpleDto> QuestionRltCategories { get; set; } = new List<ExamPaperQuestionRltCategorySimpleDto>();

        /// <summary>
        /// 考题与知识点关联列表
        /// </summary>
        public List<ExamPaperQuestionRltKnowledgePointSimpleDto> QuestionRltKnowledgePoints { get; set; } = new List<ExamPaperQuestionRltKnowledgePointSimpleDto>();
        /// <summary>
        /// 试卷和题库的关联关系
        /// </summary>
        public List<ExamPaperRltQuestionDto> ExamPaperRltQuestions { get; set; } = new List<ExamPaperRltQuestionDto>();
    }
}
