using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    public class ExamPaper : AuditedEntity<Guid>
    {
        protected ExamPaper() { }
        public ExamPaper(Guid id) { Id = id; }

        /// <summary>
        /// 分类
        /// </summary>
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public Guid? ExamPaperTemplateId { get; set; }
        public virtual ExamPaperTemplate ExamPaperTemplate { get; set; }

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

        [InverseProperty("ExamPaper")]
        /// <summary>
        /// 试卷和题库的关联关系
        /// </summary>
        public List<ExamPaperRltQuestion> ExamPaperRltQuestions { get; set; }
    }
}
