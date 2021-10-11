using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class CommonGuidGetDto : IRepairTagKeyDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string RepairTagKey { get; set; }
        public PlanType? PlanType { get; set; }
        public Boolean IsUpdate { get; set; }
    }
}
