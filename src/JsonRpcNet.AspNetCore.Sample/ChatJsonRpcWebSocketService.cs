using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;

namespace JsonRpcNet.AspNetCore.Sample
{
    [JsonRpcService("chat", Description = "Chat hub", Name = "ChatService")]
    public class ChatJsonRpcWebSocketService : JsonRpcWebSocketService
    {
        [JsonRpcMethod("sendMessage", Description = "Sends a message to the chat")]
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
