using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Alarm.Enums
{
    public enum AlarmState
    {
        // 激活中
        Actived = 1,
        // 已确认
        Confirmed,
        // 已消除
        Cleared
    }
}
