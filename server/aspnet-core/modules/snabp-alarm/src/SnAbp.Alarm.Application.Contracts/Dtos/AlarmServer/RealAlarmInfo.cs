using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Alarm.Dtos
{
    public class RealAlarmInfo
    {
        /// <summary>
        /// 告警方式，
        /// 0：非法，
        /// 1：空，不进行处理，
        /// 2:事件，
        /// 3：告警，
        /// 4：告警恢复
        /// 如果仅展示告警，需要筛选nMode==3
        /// </summary>
        public int nMode;
        public long lParentDeviceId;

        /// <summary>
        /// 是否已确认，0：未确认，1：已确认
        /// </summary>
        public int bIsAck;

        /// <summary>
        /// 设备id
        /// </summary>
        public long lDeviceId;

        /// <summary>
        /// 故障码
        /// </summary>
        public string strFaultInfo;

        /// <summary>
        /// 车站id
        /// </summary>
        public long nStationId;

        /// <summary>
        /// nSubsystemId
        /// </summary>
        public long nSubsystemId;

        /// <summary>
        /// 告警级别
        /// </summary>
        public int nLevel;

        /// <summary>
        /// 告警唯一标识
        /// </summary>
        public string lId;

        /// <summary>
        /// 告警发生时间戳
        /// </summary>
        public long tTime;

        /// <summary>
        /// 告警描述
        /// </summary>
        public string strDescription;
    }
}
