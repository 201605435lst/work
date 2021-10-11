using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class WorkOrderTestAdditionalDto //: AuditedEntityDto<Guid>
    {
        public Guid WorkOrderId { get; set; }

        public string Number { get; set; }

        public string TestConctent { get; set; }
    }
}
