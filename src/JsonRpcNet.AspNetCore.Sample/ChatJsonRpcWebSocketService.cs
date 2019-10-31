using System;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;

namespace JsonRpcNet.AspNetCore.Sample
{
    public enum UserType
    {
        Admin,
        NonAdmin
    }
    public class AddUserRequest
    {
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
    }
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
           BroadcastAsync(message).GetAwaiter().GetResult();
        }

        [JsonRpcMethod("SendMessageEcho", Description = "Sends a message to the chat and get and echo back")]
        public string EchoMessage(string message)
        {
            BroadcastAsync(message).GetAwaiter().GetResult();
            return message;
        }
        
        [JsonRpcMethod("AddUser", Description = "Add a user to the chat")]
        public void AddUser(AddUserRequest request)
        {
            BroadcastAsync($"User {request.Name} joined").GetAwaiter().GetResult();
            UserAdded?.Invoke(this, new UserAddedEventArgs{UserName = "Hello world"});
        }

        protected override Task OnBinaryMessage(ArraySegment<byte> buffer)
        {
            throw new NotImplementedException();
        }

        protected override Task OnConnected()
        {
            Console.WriteLine($"{GetType().Name} started...");
            return base.OnConnected();
        }
    }
}
