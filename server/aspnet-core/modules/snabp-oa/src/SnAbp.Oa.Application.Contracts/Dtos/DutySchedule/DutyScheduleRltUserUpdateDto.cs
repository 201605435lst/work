using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class DutyScheduleRltUserUpdateDto : EntityDto<Guid>
    {
        public virtual Guid UserId { get; set; }

        public virtual Guid DutyScheduleId { get; set; }
    }
}
