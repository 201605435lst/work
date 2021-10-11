using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;


namespace SnAbp.Exam.Dtos
{
    public class ExamPaperDto: AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 分类
        /// </summary>
        public Guid? CategoryId { get; set; }
        public ExamCategoryDto Category { get; set; }
        public virtual ExamCategorySimpleDto Categoty { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public Guid? ExamPaperTemplateId { get; set; }
        public ExamPaperTemplateSimpleDto ExamPaperTemplate { get; set; }

        /// <summary>
        /// 组题方式
        /// </summary>
        public GroupQuestionType GroupQuestionType { get; set; }

        /// <summary>
        /// 题目总数
        /// </summary>
        public int QuestionTotalNumber { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// 考试时长
        /// </summary>
        public int ExaminationDuration { get; set; }


        /// <summary>
        /// 试卷和题库的关联关系
        /// </summary>
        public List<ExamPaperRltQuestionDto> ExamPaperRltQuestions { get; set; } = new List<ExamPaperRltQuestionDto>();

    }
}
