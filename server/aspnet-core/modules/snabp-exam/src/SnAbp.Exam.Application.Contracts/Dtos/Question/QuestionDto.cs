using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SnAbp.Exam.Enums;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class QuestionDto : EntityDto<Guid>
    {

        /// <summary>
        /// 类型
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// 难度系数
        /// </summary>
        public float DifficultyCoefficient { get; set; }

        /// <summary>
        /// 题目
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 答案：
        /// 单选题：存 id；
        /// 多选题：存 id1,id2,...逗号隔开；
        /// 填空题：存 空1,空2,...逗号隔开；
        /// 判断题：存 0（错）、1（对）；
        /// 解答题：存 文本
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public string Analysis { get; set; }

        /// <summary>
        /// 考题与分类关联表
        /// </summary>
        public List<QuestionRltCategorySimpleDto> QuestionRltCategories { get; set; } = new List<QuestionRltCategorySimpleDto>();

        /// <summary>
        /// 考题与知识点关联列表
        /// </summary>
        public List<QuestionRltKnowledgePointSimpleDto> QuestionRltKnowledgePoints { get; set; } = new List<QuestionRltKnowledgePointSimpleDto>();

        /// <summary>
        /// 答案选项
        /// </summary>
        public List<AnswerOptionSimpleDto> AnswerOptions { get; set; } = new List<AnswerOptionSimpleDto>();
    }
}
