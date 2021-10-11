using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ComponentCategoryRltMVDPropertySearchDto : PagedAndSortedResultRequestDto
    {
        public bool IsAll { get; set; }

        public Guid ComponentCategoryId { get; set; }

        public List<Guid?> Ids { get; set; }
    }
}
