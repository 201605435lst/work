using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
   public class QuotaGetListDto : PagedAndSortedResultRequestDto
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string Keyword { get; set; }

        public bool IsAll { get; set; }
    }
}
