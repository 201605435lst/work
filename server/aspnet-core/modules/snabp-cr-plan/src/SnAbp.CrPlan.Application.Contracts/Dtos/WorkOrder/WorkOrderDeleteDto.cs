using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    public class WorkOrderDeleteDto : CommonGuidGetDto
    {
        public bool IsOtherPlan { get; set; }
    }
}
