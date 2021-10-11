using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProductCategoryRltMVDPropertyCreateDto : PagedAndSortedResultRequestDto
    {
        public Guid ProductCategoryId { get; set; }

        public List<ProductCategoryRltMVDPropertyDto> list { get; set; }

        public bool IsAll { get; set; }
    }
}
