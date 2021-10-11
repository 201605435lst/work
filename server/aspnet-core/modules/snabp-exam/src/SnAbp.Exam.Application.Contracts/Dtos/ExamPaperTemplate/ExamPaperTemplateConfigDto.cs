using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperTemplateConfigDto :EntityDto<Guid>
    {
        /// <summary>
        /// 考卷模板
        /// </summary>
        public virtual Guid ExamPaperTemplateId { get; set; }

        public virtual ExamPaperTemplateDto ExamPaperTemplate { get; set; }

        /// <summary>
        /// 题目类型
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// 难度系数
        /// </summary>
        public float DifficultyDegree { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

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
