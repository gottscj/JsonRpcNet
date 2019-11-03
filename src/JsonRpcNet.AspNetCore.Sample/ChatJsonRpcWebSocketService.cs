using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;
using NJsonSchema;
using NJsonSchema.Annotations;

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

        [JsonRpcMethod("GetUsers", Description = "Gets users in the chat")]
        [return:JsonSchema(JsonObjectType.Array, Name = "users", ArrayItem = typeof(User))]
        public Collection<User> GetUsers()
        {
            return new Collection<User>
            {
                new User
                {
                    Name = "John Wick",
                    Id = "1",
                    UserType = UserType.Admin
                },
                new User
                {
                    Name = "Hella joof",
                    Id = "2",
                    UserType = UserType.NonAdmin
                }
            };
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
