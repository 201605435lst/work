using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 栏目id集合
        /// </summary>
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 栏目标识
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 发布开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 发布结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否全部显示
        /// </summary>
        public bool IsAll { get; set; }
    }
}
