using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos.DiaryRltBuilder
{
    public class DiaryRltBuilderSimpleDto : EntityDto<Guid>
    {
        public virtual Guid? DiaryId { get; set; }

        public virtual Guid? BuilderId { get; set; }
      
        public DiaryRltBuilderType Type { get; set; }
    }
}
