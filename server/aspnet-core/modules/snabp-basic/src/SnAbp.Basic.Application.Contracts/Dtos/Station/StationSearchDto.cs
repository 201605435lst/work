using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class StationSearchDto: PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }
        public Guid RepairTeamId { get; set; }
        public Guid BelongRaId { get; set; }

        public bool IsAll { get; set; }

        public StationSearchDto()
        {
            RepairTeamId = Guid.Empty;
            BelongRaId = Guid.Empty;
        }
    }
}
