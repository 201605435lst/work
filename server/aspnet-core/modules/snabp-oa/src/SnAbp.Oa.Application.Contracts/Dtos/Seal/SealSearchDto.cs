using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos.Seal
{
    public class SealSearchDto : PagedAndSortedResultRequestDto
    {
        public string Keywords { get; set; }

        public bool Personal { get; set; }
    }
}
