using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentSearchSimpleDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 库存编码
        /// </summary>
        public List<string> Codes { get; set; }
    }
}
