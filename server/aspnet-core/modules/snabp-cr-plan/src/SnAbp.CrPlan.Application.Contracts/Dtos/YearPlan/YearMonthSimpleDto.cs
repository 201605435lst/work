using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class YearMonthSimpleDto
    {
        public int Total { get; set; }

        public List<Guid> OrganizationIds { get; set; }
    }
}
