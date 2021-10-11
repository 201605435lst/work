using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProjectItemRltProcessTemplateSeachDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        ///// <summary>
        ///// 类型0（单项工程）1（工程工项）
        ///// </summary>
        //public int Type { get; set; }
        public bool IsAll { get; set; }
    }
}
