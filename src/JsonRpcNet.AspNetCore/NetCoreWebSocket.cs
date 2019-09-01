using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JsonRpcNet.AspNetCore
{
    public class NetCoreWebSocket : IJsonRpcWebSocket
    {
        private readonly WebSocket _webSocket;

        public NetCoreWebSocket(WebSocket webSocket)
        {
            _webSocket = webSocket;
            Id = Guid.NewGuid().ToString();
        }
        public event MessageReceived OnMessageReceived;
        public event ConnectionClosed OnConnectionClosed;
        
        public string Id { get; }
        public IPEndPoint UserEndPoint => null;

        public JsonRpcWebSocketState WebSocketState
        {
            get
            {
                switch (_webSocket.State)
                {
                    case System.Net.WebSockets.WebSocketState.Aborted:
                        return JsonRpcWebSocketState.Closed;
                    case System.Net.WebSockets.WebSocketState.Closed:
                        return JsonRpcWebSocketState.Closed;
                    case System.Net.WebSockets.WebSocketState.CloseReceived:
                        return JsonRpcWebSocketState.Closing;
                    case System.Net.WebSockets.WebSocketState.CloseSent:
                        return JsonRpcWebSocketState.Closing;
                    case System.Net.WebSockets.WebSocketState.Connecting:
                        return JsonRpcWebSocketState.Connecting;
                    case System.Net.WebSockets.WebSocketState.None:
                        return JsonRpcWebSocketState.Closed;
                    case System.Net.WebSockets.WebSocketState.Open:
                        return JsonRpcWebSocketState.Open;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Task SendAsync(string message)
        {
            return _webSocket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.UTF8.GetBytes(message),
                    offset: 0, 
                    count: message.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }

        public Task CloseAsync(int code, string reason)
        {
            return _webSocket.CloseAsync((WebSocketCloseStatus)code, reason, CancellationToken.None);
        }
        
        public async Task HandleMessages()
        {
            var buffer = new ArraySegment<byte>(new byte[1024 * 4]);

            while(_webSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(buffer,
                    cancellationToken: CancellationToken.None).ConfigureAwait(false);

                if (result.MessageType == WebSocketMessageType.Text && buffer.Array != null)
                {
                    var request = Encoding.UTF8.GetString(buffer.Array, 
                    buffer.Offset, 
                    buffer.Count);
                    OnMessageReceived?.Invoke(MessageType.Text, request);
                    return;
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    OnConnectionClosed?.Invoke(
                        result.CloseStatus.HasValue ? (int) result.CloseStatus.Value : (int) CloseStatusCode.NoStatus,
                        result.CloseStatusDescription);
                }

            }
        }
    }
}