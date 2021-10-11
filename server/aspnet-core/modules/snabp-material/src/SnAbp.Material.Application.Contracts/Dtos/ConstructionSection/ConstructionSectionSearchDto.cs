using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class ConstructionSectionSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 施工区段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否全查
        /// </summary>
        public  bool  IsAll { get; set; }
    }
}
