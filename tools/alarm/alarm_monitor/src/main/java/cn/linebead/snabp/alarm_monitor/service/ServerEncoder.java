package cn.linebead.snabp.alarm_monitor.service;

import cn.linebead.snabp.alarm_monitor.service.dto.AlarmMessage;
import cn.linebead.snabp.alarm_monitor.service.dto.AlarmResult;
import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.serializer.SerializerFeature;

import javax.websocket.EncodeException;
import javax.websocket.Encoder;
import javax.websocket.EndpointConfig;

public class ServerEncoder implements Encoder.Text<AlarmResult> {
    @Override
    public String encode(AlarmResult object) throws EncodeException {
        try {
            return JSON.toJSONString(object, SerializerFeature.DisableCircularReferenceDetect);

        } catch (Exception e) {
            return null;
        }
    }

    @Override
    public void init(EndpointConfig endpointConfig) {

    }

    @Override
    public void destroy() {

    }
}
