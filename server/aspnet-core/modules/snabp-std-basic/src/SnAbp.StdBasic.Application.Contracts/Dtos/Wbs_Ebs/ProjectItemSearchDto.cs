using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
   public class ProjectItemSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Keyword { get; set; }


        public bool IsAll { get; set; }

        /// <summary>
        /// 关联工程工项时传入的Ids
        /// </summary>
        public List<Guid>? Ids { get; set; }
    }
}
