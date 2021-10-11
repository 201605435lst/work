using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Alarm.Enums
{
    public enum AlarmLevel
    {
        // 紧急告警    
        Emergency = 1,
        // 重要告警    
        Important,
        // 一般告警    
        Normal,
        // 预警告警    
        PreAlarm,
    }
}
