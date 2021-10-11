using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperCreateDto:EntityDto<Guid>
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public Guid? CategoryId { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板id
        /// </summary>
        public Guid? ExamPaperTemplateId { get; set; }

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
        public List<ExamPaperRltQuestionDto> ExamPaperRltQuestions { get; set; }

    }
}
