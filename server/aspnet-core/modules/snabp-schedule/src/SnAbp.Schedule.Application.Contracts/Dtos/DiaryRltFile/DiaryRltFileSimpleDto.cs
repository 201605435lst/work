using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class DiaryRltFileSimpleDto : EntityDto<Guid>
    {
        public virtual Guid? FileId { get; set; }

        public virtual Guid? DiaryId { get; set; }
        //施工过程视频、班前讲话视频、讲话图片
        public DiaryRltFileType Type { get; set; }
    }
}
