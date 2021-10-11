using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    public class Question : AuditedEntity<Guid>
    {
        protected Question() { }
        public Question(Guid id) { Id = id; }

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
        [MaxLength(300)]
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

        [InverseProperty("Question")]
        /// <summary>
        /// 考题与分类关联列表
        /// </summary>
        public List<QuestionRltCategory> QuestionRltCategories { get; set; }

        [InverseProperty("Question")]
        /// <summary>
        /// 考题与知识点关联列表
        /// </summary>
        public List<QuestionRltKnowledgePoint> QuestionRltKnowledgePoints { get; set; }

        /// <summary>
        /// 答案选项
        /// </summary>
        public List<AnswerOption> AnswerOptions { get; set; }
    }
}
