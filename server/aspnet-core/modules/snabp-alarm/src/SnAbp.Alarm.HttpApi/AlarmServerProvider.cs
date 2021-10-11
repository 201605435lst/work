using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnAbp.Alarm.Dtos;
using SnAbp.Alarm.IRepositories;
using SnAbp.Alarm.IServices;
using SnAbp.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnAbp.Alarm
{
    public class AlarmServerProvider : MessageServiceProvider
    {
        private ClientWebSocket _webSocket = new ClientWebSocket();
        private CancellationToken _cancellation = new CancellationToken();
        private IHubContext<AlarmHub> MessageContext { get; }
        private IConfiguration _configuration;
        private Timer timer = null;
        private IAlarmAlarmAppService _alarmAppService;

        public static List<AlarmSimple> alarmSimples;

        public AlarmServerProvider(
            IHubContext<AlarmHub> messageContext,
            IConfiguration configuration,
            IEfCoreAlarmRepository coreAlarmRepository,
            IAlarmAlarmAppService alarmAppService
            )
        {
            MessageContext = messageContext;
            _configuration = configuration;
            _alarmAppService = alarmAppService;
        }

        public void Start()
        {
            //Connect();
            timer = new Timer(ConnectLoop, "Check Connect Status", 5000, 5000);
        }


        public async Task Connect()
        {
            // 清空消息
            alarmSimples = new List<AlarmSimple>();
            _ = setAlarmsMessageAsync(new List<RealAlarmVaryInfo>());

            try
            {
                //建立连接
                //System.Diagnostics.Debug.WriteLine("******************* 建立连接 \n");

                var url = _configuration.GetValue<string>("AlarmModule:WebSocketUrl");
                var station = _configuration.GetValue<string>("AlarmModule:Subscribe.Station");
                var system = _configuration.GetValue<string>("AlarmModule:Subscribe.System");
                _webSocket = new ClientWebSocket();
                await _webSocket.ConnectAsync(new Uri(url), _cancellation);
                //System.Diagnostics.Debug.WriteLine("******************* 建立连接成功 \n");

                // 订阅告警信息
                //System.Diagnostics.Debug.WriteLine("******************* 发送订阅消息\n");
                var subScribeMsg = new JObject()
                 {
                     new JProperty("type","alarm"),
                     new JProperty("action","subscribe"),
                     new JProperty("station", new string []{ station}),
                     new JProperty("system", Array.ConvertAll<string, int>(system.Split("_"), delegate(string s) { return int.Parse(s); })),
                 };
                var str = subScribeMsg.ToString();
                var subScribeMsgBuffer = Encoding.UTF8.GetBytes(str);
                await _webSocket.SendAsync(
                   new ArraySegment<byte>(subScribeMsgBuffer, 0, subScribeMsgBuffer.Length),
                   WebSocketMessageType.Text,
                   true,
                   CancellationToken.None
               );

                await OnMessage();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        // 连接定时器，断线后尝试重新连接
        void ConnectLoop(object state)
         {
            //System.Diagnostics.Debug.WriteLine("******************* 定时器检查连接状态\n");
            if (_webSocket.State == WebSocketState.Closed ||
                _webSocket.State == WebSocketState.CloseReceived ||
                _webSocket.State == WebSocketState.None)
            {
                //System.Diagnostics.Debug.WriteLine("******************* 已断线，尝试重新建立连接\n");
                _ = Connect();
            }
        }


        // 收到消息
        private async Task OnMessage()
        {
            //var buffer = new byte[1024 * 8 * 102400];

            //// 接收消息
            //while (_webSocket.State != WebSocketState.Closed)
            //{
            //    try
            //    {
            //       var result =  await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //        var messageString = Encoding.Default.GetString(buffer);
            //        if (!string.IsNullOrEmpty(messageString))
            //        {
            //            System.Diagnostics.Debug.WriteLine("******************* 收到消息\n" + messageString + "\n\n");
            //            //System.Diagnostics.Debug.WriteLine("\n");
            //            //System.Diagnostics.Debug.WriteLine("\n");
            //            var message = JsonConvert.DeserializeObject<AlarmResult>(messageString) ?? new AlarmResult();
            //            AlarmMessage _alarmMessages = JsonConvert.DeserializeObject<AlarmMessage>(message.data ?? "");

            //            _ = setAlarmsMessageAsync(_alarmMessages != null ? _alarmMessages.RealAlarmVaryInfo : new List<RealAlarmVaryInfo>());
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        //System.Diagnostics.Debug.WriteLine(e);
            //    }
            //}



            //// 接收消息
            //while (_webSocket.State != WebSocketState.Closed)
            //{
            //    try
            //    {
            //        var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //        var messageString = Encoding.Default.GetString(buffer);
            //        if (!string.IsNullOrEmpty(messageString))
            //        {
            //            System.Diagnostics.Debug.WriteLine("******************* 收到消息\n" + messageString + "\n\n");
            //            //System.Diagnostics.Debug.WriteLine("\n");
            //            //System.Diagnostics.Debug.WriteLine("\n");
            //            var message = JsonConvert.DeserializeObject<AlarmResult>(messageString) ?? new AlarmResult();
            //            AlarmMessage _alarmMessages = JsonConvert.DeserializeObject<AlarmMessage>(message.data ?? "");

            //            _ = setAlarmsMessageAsync(_alarmMessages != null ? _alarmMessages.RealAlarmVaryInfo : new List<RealAlarmVaryInfo>());
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        //System.Diagnostics.Debug.WriteLine(e);
            //    }
            //}



            int bufferSize = 1000;
            var buffer = new byte[bufferSize];
            var offset = 0;
            var free = buffer.Length;
            while (true)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, free), CancellationToken.None);
                offset += result.Count;
                free -= result.Count;
                if (result.EndOfMessage) break;
                if (free == 0)
                {
                    var newSize = buffer.Length + bufferSize;
                    var newBuffer = new byte[newSize];
                    Array.Copy(buffer, 0, newBuffer, 0, offset);
                    buffer = newBuffer;
                    free = buffer.Length - offset;
                }
            }

            var messageString = Encoding.Default.GetString(buffer);
            if (!string.IsNullOrEmpty(messageString))
            {
                //System.Diagnostics.Debug.WriteLine("******************* 收到消息\n" + messageString + "\n\n");
                //System.Diagnostics.Debug.WriteLine("\n");
                //System.Diagnostics.Debug.WriteLine("\n");
                var alarmResult = JsonConvert.DeserializeObject<AlarmResult>(messageString);

                if (alarmResult != null)
                {
                    List<RealAlarmVaryInfo> realAlarmVaryInfos = new List<RealAlarmVaryInfo>();
                    alarmResult.data.ForEach(item =>
                    {
                        var alarmMessage = JsonConvert.DeserializeObject<AlarmMessage>(item);
                        if (alarmMessage != null)
                        {
                            realAlarmVaryInfos.AddRange(alarmMessage.RealAlarmVaryInfo);
                        }
                    });

                    _ = setAlarmsMessageAsync(realAlarmVaryInfos);
                }
                
            }
        }

        private async Task setAlarmsMessageAsync(List<RealAlarmVaryInfo> realAlarmVaryInfos)
        {
            //System.Diagnostics.Debug.WriteLine("******************* 开始处理消息\n");

            realAlarmVaryInfos.OrderByDescending(x => x.RealAlarmInfo.tTime).Skip(0).Take(20);

            alarmSimples = await _alarmAppService.GetAlarmEquipmentBindIdsByIds(realAlarmVaryInfos);
            //System.Diagnostics.Debug.WriteLine("******************* 广播消息\n" + alarmSimples);
            //System.Diagnostics.Debug.WriteLine("******************* end *******************\n\n" + alarmSimples);
            _ = MessageContext.Clients.All.SendAsync("Alarms", alarmSimples);
        }

        public override Task Receive(byte[] data)
        {
            return default;
        }
    }
}
