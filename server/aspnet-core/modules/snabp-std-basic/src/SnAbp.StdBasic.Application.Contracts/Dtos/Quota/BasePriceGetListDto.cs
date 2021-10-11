using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class BasePriceGetListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 电算代号Id
        /// </summary>
        public Guid ComputerCodeId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Keyword { get; set; }

        public bool IsAll { get; set; }
    }
}
