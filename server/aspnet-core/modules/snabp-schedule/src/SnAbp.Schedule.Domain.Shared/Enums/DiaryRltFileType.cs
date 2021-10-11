using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Schedule.Enums
{
   public enum DiaryRltFileType
    {
        [Description("班前讲话视频")]
        TalkMedias=1,
        [Description("讲话图片")]
        Pictures =2,
        [Description("施工过程视频")]
        ProcessMedias = 3,
    }
}
