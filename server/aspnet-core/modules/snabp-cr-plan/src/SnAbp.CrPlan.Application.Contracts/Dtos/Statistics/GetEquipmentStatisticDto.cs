using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class GetEquipmentStatisticDto
    {
        public string GroupName { get; set; }
        public float Finshed { get; set; }
        public float UnFinshed { get; set; }
        public float Changed{ get; set; }
        public Guid? OrgizationId { get; set; }

    }
}
