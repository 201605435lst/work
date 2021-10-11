using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class AlterRecordGetListDto : IRepairTagKeyDto
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
        public string RepairTagKey { get; set; }
    }
}
