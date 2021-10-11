using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProductCategoryRltQuotaCreateDto : PagedAndSortedResultRequestDto
    {
        public Guid ProductionCategoryId { get; set; }
        public List<Guid> QuotaIdList { get; set; }
        public bool IsAll { get; set; }
    }
}
