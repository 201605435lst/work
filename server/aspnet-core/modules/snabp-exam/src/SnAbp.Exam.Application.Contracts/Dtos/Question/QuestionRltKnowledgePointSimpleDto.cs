using SnAbp.Exam.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
   public class QuestionRltKnowledgePointSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 考题
        /// </summary>
        public virtual Guid QuestionId { get; set; }

        /// <summary>
        /// 知识点
        /// </summary>
        public virtual Guid KnowledgePointId { get; set; }
        public KnowledgePoint KnowledgePoint { get; set; }

        

    }
}
