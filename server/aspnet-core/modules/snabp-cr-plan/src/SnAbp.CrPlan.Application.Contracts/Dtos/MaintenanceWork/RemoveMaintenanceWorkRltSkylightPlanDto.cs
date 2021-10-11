using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos.MaintenanceWork
{
    public class RemoveMaintenanceWorkRltSkylightPlanDto
    {
        //维修计划对应工作流Id
        public Guid WorkflowId { get; set; }
        //垂直天窗id
        public Guid SkylightPlanId { get; set; }
        //退回原因
        public string Opinion { get; set; }
    }
}
