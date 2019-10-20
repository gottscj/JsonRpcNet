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
        [JsonRpcMethod("SendMessage", Description = "Sends a message to the chat")]
        public void SendMessage(string message)
        {
            _ = BroadcastAsync(message);
        }

        [JsonRpcMethod("SendMessageEcho", Description = "Sends a message to the chat and get and echo back")]
        public string EchoMessage(string message)
        {
            _ = BroadcastAsync(message);
            return message;
        }

        protected override Task OnBinaryMessage(ArraySegment<byte> buffer)
        {
            throw new NotImplementedException();
        }
    }
}
