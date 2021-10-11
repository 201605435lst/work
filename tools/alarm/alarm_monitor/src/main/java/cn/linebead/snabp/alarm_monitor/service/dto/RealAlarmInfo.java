package cn.linebead.snabp.alarm_monitor.service.dto;

import com.alibaba.fastjson.annotation.JSONField;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;
import java.math.BigInteger;
import java.sql.Time;

@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class RealAlarmInfo implements Serializable {
    @JSONField(name = "nMode")
    public Integer nMode;

    @JSONField(name = "lParentDeviceId")
    public long lParentDeviceId;

    @JSONField(name = "bIsAck")
    public Integer bIsAck;

    @JSONField(name = "lDeviceId")
    public long lDeviceId;

    @JSONField(name = "strFaultInfo")
    public String strFaultInfo;

    @JSONField(name = "nStationId")
    public long nStationId;

    @JSONField(name = "nSubsystemId")
    public long nSubsystemId;

    @JSONField(name = "nLevel")
    public Integer nLevel;

    @JSONField(name = "lId")
    public String lId;

    @JSONField(name = "tTime")
    public long tTime;

    @JSONField(name = "strDescription")
    public String strDescription;
}
