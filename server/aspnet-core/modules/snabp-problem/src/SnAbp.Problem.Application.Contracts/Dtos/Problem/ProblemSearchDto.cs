using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Problem.Dtos
{
    public class ProblemSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 问题分类id集合
        /// </summary>
        public List<Guid> ProblemCategoryIds { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 是否全部查询
        /// </summary>
        public bool IsAll { get; set; }
    }
}
