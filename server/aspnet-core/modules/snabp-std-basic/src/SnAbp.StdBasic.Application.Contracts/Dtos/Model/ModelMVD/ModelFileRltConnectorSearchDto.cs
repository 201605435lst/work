using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
    public class ModelFileRltConnectorSearchDto : PagedAndSortedResultRequestDto
    {
        public bool IsAll { get; set; }

        public Guid ModelFileId { get; set; }
    }
}
