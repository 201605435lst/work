using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class SkylightPalnDataStatisticDto
    {
        public VerticalSkylight VerticalSkylightCount { get; set; } = new VerticalSkylight();

        public SkylightOutside SkylightOutsideCount { get; set; } = new SkylightOutside();
    }

    public class VerticalSkylight
    {
        public decimal LeveIRepairInSkylightFinshedCount { get; set; }
        public decimal LeveIRepairInSkylightUnFinshedCount { get; set; }
        public decimal LeveIIRepairInSkylightFinshedCount { get; set; }
        public decimal LeveIIRepairInSkylightUnFinshedCount { get; set; }

        public List<CompletionRateRltMonth> VerticalSkylightCompletionRate { get; set; } = new List<CompletionRateRltMonth>();
    }
    public class SkylightOutside
    {
        public decimal FinshedCount { get; set; }
        public decimal UnFinshedCount { get; set; }
        public List<CompletionRateRltMonth> SkylightOutsideCompletionRate { get; set; } = new List<CompletionRateRltMonth>();   

    }

    public class CompletionRateRltMonth
    {
        public int Month { get; set; }

        public decimal CompletionRate { get; set; }
    }
}
