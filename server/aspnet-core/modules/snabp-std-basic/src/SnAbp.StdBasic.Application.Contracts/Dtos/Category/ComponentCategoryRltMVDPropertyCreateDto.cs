using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ComponentCategoryRltMVDPropertyCreateDto : PagedAndSortedResultRequestDto
    {
        public Guid ComponentCategoryId { get; set; }

        public List<ComponentCategoryRltMVDPropertyDto> list { get; set; }

        public bool IsAll { get; set; }
    }
}
