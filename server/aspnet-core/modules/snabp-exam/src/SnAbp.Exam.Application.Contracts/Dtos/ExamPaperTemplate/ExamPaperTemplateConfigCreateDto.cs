using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperTemplateConfigCreateDto :EntityDto<Guid>
    {
        /// <summary>
        /// 考卷模板
        /// </summary>
        public Guid ExamPaperTemplateId { get; set; }
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
