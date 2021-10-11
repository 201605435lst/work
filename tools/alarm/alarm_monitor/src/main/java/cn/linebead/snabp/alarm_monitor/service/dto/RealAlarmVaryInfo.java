package cn.linebead.snabp.alarm_monitor.service.dto;


import com.alibaba.fastjson.annotation.JSONField;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;

@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class RealAlarmVaryInfo  implements Serializable {
    @JSONField(name = "nVaryType")
    public Integer nVaryType;

    @JSONField(name = "RealAlarmInfo")
    public RealAlarmInfo RealAlarmInfo;
}
