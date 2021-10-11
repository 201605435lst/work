package cn.linebead.snabp.alarm_monitor.service.dto;

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
public class AlarmResult implements Serializable
{
    public String type;

    public List<AlarmMessage> data;
}
