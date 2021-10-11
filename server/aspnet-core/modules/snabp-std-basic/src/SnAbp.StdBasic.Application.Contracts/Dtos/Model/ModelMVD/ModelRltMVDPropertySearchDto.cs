using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model
{
    public class ModelRltMVDPropertySearchDto : PagedAndSortedResultRequestDto
    {
        public bool IsAll { get; set; }

        public Guid ModelId { get; set; }

        public List<Guid?> Ids { get; set; }
    }
}
