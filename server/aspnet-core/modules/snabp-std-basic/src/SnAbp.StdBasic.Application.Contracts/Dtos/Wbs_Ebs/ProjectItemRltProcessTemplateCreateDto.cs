using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProjectItemRltProcessTemplateCreateDto : PagedAndSortedResultRequestDto
    {
        public List<Guid> ProjectItemIdList { get; set; }
        public Guid ProcessTemplateId { get; set; }

        ///// <summary>
        ///// 类型0（单项工程）1（工程工项）
        ///// </summary>
        //public int Type { get; set; }

        public bool IsAll { get; set; }
    }
}
