using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Institution
{
    public class InstitutionSearchDto : PagedAndSortedResultRequestDto
    {
        public string KeyWords { get; set; }
        public string ColumnKey { get; set; }
        public string Order { get; set; }
    }
}
