using SnAbp.Exam.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperTemplateSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public virtual Guid? CategoryId { get; set; }
    }
}
