using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;

namespace JsonRpcNet.AspNetCore.Sample
{
    [JsonRpcService("chat")]
    public class ChatJsonRpcWebSocketService : JsonRpcWebSocketService
    {
        [JsonRpcMethod("sendMessage")]
        public void SendMessage(string message)
        {
            BroadcastAsync(message);
        }

        protected override Task OnBinaryMessage(ArraySegment<byte> buffer)
        {
            throw new NotImplementedException();
        }
    }
}
