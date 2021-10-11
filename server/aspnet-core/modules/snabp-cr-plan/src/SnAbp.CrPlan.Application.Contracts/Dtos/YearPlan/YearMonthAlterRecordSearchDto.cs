using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class YearMonthAlterRecordSearchDto : PagedAndSortedResultRequestDto
    {
        public Guid OrganizationId { get; set; }

        public int State { get; set; }

        public int PlanType { get; set; }

    }
}
