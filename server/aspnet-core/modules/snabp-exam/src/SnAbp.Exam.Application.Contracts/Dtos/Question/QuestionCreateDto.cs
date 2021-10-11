using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class QuestionCreateDto
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


        public List<QuestionRltCategoryCreateDto> QuestionRltCategories { get; set; } = new List<QuestionRltCategoryCreateDto>();

        /// <summary>
        /// 考题知识点关联列表
        /// </summary>
        public List<QuestionRltKnowledgePointCreateDto> QuestionRltKnowledgePoints { get; set; } = new List<QuestionRltKnowledgePointCreateDto>();

        /// <summary>
        /// 答案选项
        /// </summary>
        public List<AnswerOptionCreateDto> AnswerOptions { get; set; } = new List<AnswerOptionCreateDto>();
    }
}
