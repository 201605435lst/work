using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class AnswerOptionCreateDto : EntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 问题ID
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
