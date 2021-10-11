using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Schedule.Entities
{
    public class DiaryRltFile : Entity<Guid>
    {
        public DiaryRltFile(Guid id) => Id = id;
        public File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public Diary Diary { get; set; }
        public virtual Guid? DiaryId { get; set; }
        //施工过程视频、班前讲话视频、讲话图片
        public DiaryRltFileType Type { get; set; }
    }
}
