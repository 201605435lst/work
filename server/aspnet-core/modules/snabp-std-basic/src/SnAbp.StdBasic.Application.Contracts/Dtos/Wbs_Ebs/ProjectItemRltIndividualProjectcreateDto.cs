using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProjectItemRltIndividualProjectCreateDto : PagedAndSortedResultRequestDto
    {
        public List<Guid> ProjectItemIdList { get; set; }
        public Guid IndividualProjectId { get; set; }
        public bool IsAll { get; set; }
    }
}
