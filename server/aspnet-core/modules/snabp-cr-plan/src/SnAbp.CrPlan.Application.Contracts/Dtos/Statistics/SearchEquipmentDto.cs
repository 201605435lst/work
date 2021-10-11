using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class SearchEquipmentDto
    {
        public Guid OrgId { get; set; }

        public DateTime Time { get; set; }

        public int Type { get; set; }
    }
}
