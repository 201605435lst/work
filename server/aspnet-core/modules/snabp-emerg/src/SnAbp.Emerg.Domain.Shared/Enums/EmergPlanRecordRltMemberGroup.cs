using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Emerg.Enums
{
    public enum EmergPlanRecordRltMemberGroup
    {
        [Description("由我发起")]
        Launched = 1,

        [Description("待我处理")]
        Waiting = 2,

        [Description("我已处理")]
        Processed = 3,

        [Description("抄送给我")]
        Cc = 4,
    }
}
