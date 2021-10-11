using SnAbp.Identity;
using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
   public class DiaryRltBuilderDto : EntityDto<Guid>
    {
        public virtual Diary Diary { get; set; }
        public virtual Guid? DiaryId { get; set; }

        public virtual IdentityUser Builder { get; set; }
        public virtual Guid? BuilderId { get; set; }
        //施工过程视频、班前讲话视频、讲话图片
        public DiaryRltBuilderType Type { get; set; }
    }
}
