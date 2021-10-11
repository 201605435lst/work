using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class GetPieStatisticalDto
    {
        public float FinshedTotal { get; set; }
        public float UnFinshedTotal { get; set; }
        public float ChangeTotal { get; set; }

        public List<GetHistogramStatisticDto> HistogramInfos { get; set; } = new List<GetHistogramStatisticDto>();
    }
}
