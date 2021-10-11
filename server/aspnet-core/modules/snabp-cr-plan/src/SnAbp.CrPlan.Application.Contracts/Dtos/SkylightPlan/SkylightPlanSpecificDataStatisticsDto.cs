using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class SkylightPlanSpecificDataStatisticsDto 
    {
        public decimal OrderCancelCount { get; set; }
        public decimal NaturalCancelCount { get; set; }
        public decimal OtherCancelCount { get; set; }
        public decimal ProcessingCount { get; set; }
        public decimal CompleteCount { get; set; }
    }
}
