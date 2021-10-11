using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemLibrarySearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public QualityProblemType? Type { get; set; }
        /// <summary>
        /// 问题等级
        /// </summary>
        public QualityProblemLevel? Level { get; set; }

        /// <summary>
        /// 是否全部显示
        /// </summary>
        public bool IsAll { get; set; }

    }
}
