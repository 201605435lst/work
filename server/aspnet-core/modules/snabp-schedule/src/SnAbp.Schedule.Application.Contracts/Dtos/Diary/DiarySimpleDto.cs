using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Schedule.Dtos
{
  public  class DiarySimpleDto
    {
        public virtual Guid? ApprovalId { get; set; }
        public virtual Guid? DiaryId { get; set; }
    }
}
