using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Project.Dtos
{
    public class ProjectSearchDto : PagedAndSortedResultRequestDto
    {
        public string KeyWords { get; set; }
        public bool IsAll { get; set; }
    }
}
