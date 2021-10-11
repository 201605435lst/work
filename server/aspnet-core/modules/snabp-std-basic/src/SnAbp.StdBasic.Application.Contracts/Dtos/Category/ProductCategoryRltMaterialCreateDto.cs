using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProductCategoryRltMaterialCreateDto : PagedAndSortedResultRequestDto
    {
        public Guid ProductCategoryId { get; set; }

        public List<Guid> ComputerCodeIdList { get; set; }
        public bool IsAll { get; set; }
    }
}
