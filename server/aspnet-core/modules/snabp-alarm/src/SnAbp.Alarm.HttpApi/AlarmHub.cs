using Microsoft.AspNetCore.SignalR;
using SnAbp.Message.Hubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace SnAbp.Alarm
{
    public class AlarmHub : AbpHub, IBaseHub
    {

        public Task Register(string topic)
        {
            Clients.Client(Context.ConnectionId).SendAsync("Alarms", AlarmServerProvider.alarmSimples);
            return Task.FromResult(0);
        }


        public async Task GetAlarms(string data)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Alarms", AlarmServerProvider.alarmSimples);
        }
    }
}
