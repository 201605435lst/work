package cn.linebead.snabp.alarm_monitor.service.dto;

import com.alibaba.fastjson.annotation.JSONField;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;
import java.util.List;


@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class AlarmMessage implements Serializable {
    @JSONField( name = "type")
    public String type;

    @JSONField( name = "MessageType")
    public Integer MessageType;

    @JSONField(name = "RealAlarmVaryInfo")
    public List<RealAlarmVaryInfo> RealAlarmVaryInfo;
}
