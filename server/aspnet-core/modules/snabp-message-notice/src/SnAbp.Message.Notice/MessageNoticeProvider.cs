
using SnAbp.Message.MessageDefine;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Message.Core;
using Volo.Abp.Domain.Services;

namespace SnAbp.Message.Notice
{
    public class MessageNoticeProvider : IMessageNoticeProvider
    {
        IHttpClientProvider ClientProvider { get; }
        public MessageNoticeProvider(IHttpClientProvider clientProvider) => ClientProvider = clientProvider;
        public async Task PushAsync(byte[] data)=>await ClientProvider.PostAsync<NoticeMessage>(data);

        public async Task FileNotice(object data)
        {
            await ClientProvider.PostAsync<string>("/api/app/filetag/file",data:data.GetBytes());
        }
    }
}
