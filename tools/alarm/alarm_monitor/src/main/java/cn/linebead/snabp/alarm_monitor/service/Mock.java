package cn.linebead.snabp.alarm_monitor.service;

import cn.linebead.snabp.alarm_monitor.service.dto.RealAlarmInfo;
import cn.linebead.snabp.alarm_monitor.service.dto.RealAlarmVaryInfo;

import java.util.List;

public class Mock {

    /**
     * GetRealAlarmVaryInfo
     * @param lDeviceId
     * @param strFaultInfo
     * @param nLevel
     * @param lId
     * @param tTime
     * @param strDescription
     * @return
     */
    public static final RealAlarmVaryInfo GetRealAlarmVaryInfo(long lDeviceId, String strFaultInfo, int nLevel, String lId, long tTime, String strDescription) {

        RealAlarmInfo realAlarmInfo = RealAlarmInfo.builder()
                .nMode(3)
                .lParentDeviceId(900350500005L)
                .bIsAck(0)
                .lDeviceId(lDeviceId)
                .strFaultInfo(strFaultInfo)
                .nStationId(35)
                .nSubsystemId(5)
                .nLevel(nLevel)
                .lId(lId)
                .tTime(tTime)
                .strDescription(strDescription)
                .build();

        RealAlarmVaryInfo realAlarmVaryInfo = RealAlarmVaryInfo.builder()
                .nVaryType(0)
                .RealAlarmInfo(realAlarmInfo)
                .build();


        return realAlarmVaryInfo;
    }
}
