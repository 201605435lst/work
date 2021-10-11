using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Common.Dtos
{
    public class AreaSearchDto : PagedAndSortedResultRequestDto
    {
        public string? KeyWord { get; set; }

        public int? ParentId { get; set; }
    }
}
