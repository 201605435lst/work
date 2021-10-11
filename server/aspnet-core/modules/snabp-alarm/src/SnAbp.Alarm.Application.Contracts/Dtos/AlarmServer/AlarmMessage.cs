using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Alarm.Dtos
{
    public class AlarmMessage
    {
        public string type;
        public int MessageType;
        public List<RealAlarmVaryInfo> RealAlarmVaryInfo;
    }
}
