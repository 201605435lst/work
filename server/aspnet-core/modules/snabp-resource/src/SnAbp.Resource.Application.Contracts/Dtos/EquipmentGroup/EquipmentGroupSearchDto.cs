using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
   public  class EquipmentGroupSearchDto :PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
