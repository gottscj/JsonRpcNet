using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace JsonRpcNet
{
    public interface IWebSocket
    {
        string Id { get; }
        
        IPEndPoint UserEndPoint { get; }
        
        JsonRpcWebSocketState WebSocketState { get; }

        Task SendAsync(string message);

        Task CloseAsync(int code, string reason);

        Task<(MessageType messageType, ArraySegment<byte> data)> ReceiveAsync();
    }
}