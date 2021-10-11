using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Schedule.Dtos
{
    public class EduceApprovalDto
    {
        public List<Guid> ApprovalIds { get; set; }

        public bool IsExcel { get; set; }
    }
}
