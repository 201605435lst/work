
using SnAbp.Message.MessageDefine;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Message.Core;
using Volo.Abp.Domain.Services;

namespace SnAbp.Message.Bpm
{
    public class MessageBpmProvider : IMessageBpmProvider
    {
        IHttpClientProvider ClientProvider { get; }
        public MessageBpmProvider(IHttpClientProvider clientProvider) => ClientProvider = clientProvider;

        public async Task PushAsync(byte[] data)
        {
            await ClientProvider.PostAsync<BpmMessage>(data);
        }

        public Task FileNotice(byte[] data)
        {
            throw new NotImplementedException();
        }

        public Task FilesNotice(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
