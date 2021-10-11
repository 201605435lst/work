using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class InstallationSiteSearchSimpleDto: PagedAndSortedResultRequestDto
    {
        public Guid OrgId { get; set; }
    }
}
