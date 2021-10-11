package cn.linebead.snabp.alarm_monitor.service;

import cn.linebead.snabp.alarm_monitor.service.dto.AlarmMessage;
import cn.linebead.snabp.alarm_monitor.service.dto.AlarmResult;
import cn.linebead.snabp.alarm_monitor.service.dto.RealAlarmInfo;
import cn.linebead.snabp.alarm_monitor.service.dto.RealAlarmVaryInfo;
import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;
import com.alibaba.fastjson.util.TypeUtils;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Component;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.IOException;
import java.util.*;
import java.util.concurrent.atomic.AtomicInteger;

@Slf4j
@Component
@ServerEndpoint(value = "/", encoders = {ServerEncoder.class})
@ResponseBody()
public class OneWebSocket {

    public static AtomicInteger onlineCount = new AtomicInteger(0);


    @OnOpen
    public void onOpen(Session session) {

        int count = onlineCount.incrementAndGet();
        log.info("有新连接加入：{}，当前在线人数为：{}", session.getId(), count);


        schedule(session);
    }


    @OnClose
    public void onClose(Session session) {
        onlineCount.decrementAndGet(); // 在线数减1
        log.info("有一连接关闭：{}，当前在线人数为：{}", session.getId(), onlineCount.get());
    }

    @OnMessage
    public void onMessage(String message, Session session) {
        Object o = JSON.toJSON(message);
        log.info(o.toString());
        log.info("服务端收到客户端[{}]的消息:{}", session.getId(), message);
//        this.sendMessage("Hello, " + message, session);
    }

    @OnError
    public void onError(Session session, Throwable error) {
        log.error("发生错误");
        error.printStackTrace();
    }

    private void sendMessage(String message, Session toSession) {
        try {


            log.info("服务端给客户端[{}]发送消息{}", toSession.getId(), message);
            toSession.getBasicRemote().sendText(message);
        } catch (Exception e) {
            log.error("服务端发送消息给客户端失败：{}", e);
        }
    }

    private void schedule(Session session) {

        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                String id = session.getId();
                log.info("++++++ 开始全局广播 ++++++");
                log.info("客户端 id：" + id);

                try {


                    ArrayList<RealAlarmVaryInfo> realAlarmVaryInfos = new ArrayList<>();

                    realAlarmVaryInfos.add(Mock.GetRealAlarmVaryInfo(
                            900231100001L,
                            "4097",
                            1,
                            "1407385618531287",
                            1608521903000L,
                            "故障名称: CPU占有率超上限 , 故障原因:CPU占有率超上限"
                    ));
                    realAlarmVaryInfos.add(Mock.GetRealAlarmVaryInfo(
                            900230400002L,
                            "4098",
                            2,
                            "1407385618531288",
                            1608521083000L,
                            "故障名称: 内存占有率超上限 , 故障原因:内存占有率超上限"
                    ));
                    realAlarmVaryInfos.add(Mock.GetRealAlarmVaryInfo(
                            900230400004L,
                            "4099",
                            3,
                            "1407385618531289",
                            1608521083000L,
                            "故障名称: 硬盘占有率超上限 , 故障原因:硬盘占有率超上限"
                    ));


                    AlarmResult result = AlarmResult.builder()
                            .type("alarm")
                            .build();

                    ArrayList<AlarmMessage> data = new ArrayList<>();
                    AlarmMessage alarmMessage = AlarmMessage.builder()
                            .type("alarm")
                            .MessageType(1)
                            .RealAlarmVaryInfo(realAlarmVaryInfos)
                            .build();

                    data.add(alarmMessage);
                    result.setData(data);
                    String s1 = JSONObject.toJSONString(result);
                    String s = JSON.toJSON(result).toString();

                    log.info(s);
                    session.getBasicRemote().sendText(s);

                } catch (IOException e) {
                    e.printStackTrace();
                }

                log.info("------ 完成全局广播 ------");
            }
        };

        Timer timer = new Timer();

        long delay = 5 * 000;
        long intevalPeriod = 5 * 1000;
        timer.scheduleAtFixedRate(task, delay, intevalPeriod);

    }
}
