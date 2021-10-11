
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class DutyScheduleRltUserDto : EntityDto<Guid>
    {
        public virtual Guid UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Guid DutyScheduleId { get; set; }
        public virtual DutyScheduleDto DutySchedule { get; set; }
    }
}
