using SnAbp.Bpm;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class AlterRecordPlanAlterDto //: IRepairTagKeyDto
    {
        public Guid Id { get; set; }
        public WorkflowState State { get; set; }
       // public string RepairTagKey { get; set; }
    }
}
