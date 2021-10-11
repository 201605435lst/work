using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class DiaryRltFileDto : EntityDto<Guid>
    {
        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public virtual Diary Diary { get; set; }
        public virtual Guid? DiaryId { get; set; }
        //施工过程视频、班前讲话视频、讲话图片
        public DiaryRltFileType Type { get; set; }
    }
}
