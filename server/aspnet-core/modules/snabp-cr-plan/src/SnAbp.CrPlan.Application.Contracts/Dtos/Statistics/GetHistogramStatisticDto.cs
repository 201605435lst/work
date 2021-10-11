using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class GetHistogramStatisticDto
    {
        public string  OrganizationName { get; set; }

        public float FinshedTotal { get; set; }
        public float UnFinshedTotal { get; set; }
        public float ChangeTotal { get; set; }

        public Guid? OrgId { get; set; }
    }
}
