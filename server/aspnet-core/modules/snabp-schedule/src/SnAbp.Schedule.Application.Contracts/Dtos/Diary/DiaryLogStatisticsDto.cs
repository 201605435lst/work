using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Schedule.Dtos
{
  public  class DiaryLogStatisticsDto
    {
        public DateTime DateTime { get; set; }
        public List<DiaryLogDayStatisticsDto> DiaryLogDayStatisticsDto { get; set; } = new List<DiaryLogDayStatisticsDto>();
        public int SumDay { get; set; }
        public int SumLog { get; set; }

    }
}
