using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProjectItemRltComponentCategoryCreateDto : PagedAndSortedResultRequestDto
    {
        public  Guid ProjectItemId { get; set; }
        public  List<Guid> ComponentCategoryIdList { get; set; }

        public bool IsAll { get; set; }
    }
}
