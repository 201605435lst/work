using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class QuestionRltKnowledgePointCreateDto : EntityDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 考题
        /// </summary>
        //public Guid QuestionId { get; set; }

        /// <summary>
        /// 知识点
        /// </summary>
        //public Guid KnowledgePointId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        //public int Order { get; set; }
    }
}
