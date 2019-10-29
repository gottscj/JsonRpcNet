using System;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;

namespace JsonRpcNet.AspNetCore.Sample
{
    public class UserAddedEventArgs : EventArgs
    {
        public string UserName { get; set; }
    }
    [JsonRpcService("chat", Description = "Chat hub", Name = "ChatService")]
    public class ChatJsonRpcWebSocketService : JsonRpcWebSocketService
    {
        public event EventHandler<UserAddedEventArgs> UserAdded;
        
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

        protected override Task OnConnected()
        {
            UserAdded?.Invoke(this, new UserAddedEventArgs{UserName = "Hello world"});
            return base.OnConnected();
        }
    }
}
