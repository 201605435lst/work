using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class PlanCoverInfoDto
    {
        public string CreateName { get; set; }
        public string CreateTime { get; set; }
        public string Auditor { get; set; }
        public string AuditorTime { get; set; }
        public string ApproveName { get; set; }
        public string ApproveTime { get; set; }
        public string PlanCoverName { get; set; }
    }
}
