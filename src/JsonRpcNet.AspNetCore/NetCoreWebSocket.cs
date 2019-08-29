using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace JsonRpcNet.AspNetCore
{
    public class NetCoreWebSocket : IJsonRpcWebSocket
    {
        private readonly WebSocket _webSocket;

        public NetCoreWebSocket(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }
        public event MessageReceived OnMessageReceived;
        public event ConnectionClosed OnConnectionClosed;
        public string Id { get; }
        public IPEndPoint UserEndPoint { get; }
        public JsonRpcWebSocketState WebSocketState { get; }
        public Task SendAsync(string message)
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(int code, string reason)
        {
            throw new NotImplementedException();
        }
    }
}