using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperSearchDto:PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 分类
        /// </summary>
        public Guid CategoryId { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

    }
}
