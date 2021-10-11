using SnAbp.Exam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class QuestionSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public QuestionType? QuestionType { get; set; }

        /// <summary>
        /// 分类id集合
        /// </summary>
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 题目
        /// </summary>
        public string Title { get; set; }

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
