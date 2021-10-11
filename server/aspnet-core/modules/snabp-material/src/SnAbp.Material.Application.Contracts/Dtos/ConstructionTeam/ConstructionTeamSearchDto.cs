using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
  public  class ConstructionTeamSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 施工点
        /// </summary>
        public virtual Guid ConstructionSectionId { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords { get; set; }
    }
}
