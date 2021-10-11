using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 构件
        /// </summary>
        public List<Guid> ComponentCategoryIds { get; set; }

        /// <summary>
        /// 预案等级
        /// </summary>
        public Guid LevelId { get; set; }
    }
}
