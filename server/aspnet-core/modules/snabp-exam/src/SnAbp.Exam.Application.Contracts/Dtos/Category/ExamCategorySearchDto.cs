using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamCategorySearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 根据分类名称查询
        /// </summary>
        public string Name { get; set; }

    }
}
