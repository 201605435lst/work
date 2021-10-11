using SnAbp.Basic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class RailwayDetailDto : RailwaySimpleDto
    {
        public List<RailwayOrgDto> RailwayRltOrganizations { get; set; } = new List<RailwayOrgDto>();
    }
}
