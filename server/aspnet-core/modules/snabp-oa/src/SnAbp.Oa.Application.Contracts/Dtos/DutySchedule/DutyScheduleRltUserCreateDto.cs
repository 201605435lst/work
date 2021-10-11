using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Oa.Dtos
{
    public class DutyScheduleRltUserCreateDto
    {
        public virtual Guid UserId { get; set; }
      
        public virtual Guid DutyScheduleId { get; set; }
    }
}
