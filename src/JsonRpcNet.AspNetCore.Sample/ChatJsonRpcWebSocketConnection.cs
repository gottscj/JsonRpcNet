using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonRpcNet.AspNetCore.Sample
{
    [JsonRpcRoutePrefix("chat")]
    public class ChatJsonRpcWebSocketConnection : JsonRpcWebSocketConnection
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
