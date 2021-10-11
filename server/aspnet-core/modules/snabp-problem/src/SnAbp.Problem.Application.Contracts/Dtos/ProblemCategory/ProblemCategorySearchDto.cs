using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Problem.Dtos
{
    public class ProblemCategorySearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否全部查询
        /// </summary>
        public bool IsAll { get; set; }
    }
}
