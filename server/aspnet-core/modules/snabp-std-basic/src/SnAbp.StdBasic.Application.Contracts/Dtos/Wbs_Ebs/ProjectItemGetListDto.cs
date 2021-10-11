using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
   public class ProjectItemGetListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称\简称
        /// </summary>
        public string Keyword { get; set; }


        public bool IsAll { get; set; } = false;
    }
}
