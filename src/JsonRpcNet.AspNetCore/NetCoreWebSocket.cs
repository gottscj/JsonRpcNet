using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JsonRpcNet.AspNetCore
{
    public class NetCoreWebSocket : IWebSocket
    {
        private readonly WebSocket _webSocket;
        private readonly AsyncQueue<(MessageType messageType, ArraySegment<byte> data)> _queue =
            new AsyncQueue<(MessageType messageType, ArraySegment<byte> data)>();
        
        public NetCoreWebSocket(WebSocket webSocket)
        {
            _webSocket = webSocket;
            Id = Guid.NewGuid().ToString();
        }
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

        public Task SendAsync(string message, CancellationToken cancellation)
        {
            return _webSocket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.UTF8.GetBytes(message),
                    offset: 0, 
                    count: message.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }

        public Task CloseAsync(int code, string reason, CancellationToken cancellation)
        {
            return _webSocket.CloseAsync((WebSocketCloseStatus)code, reason, cancellation);
        }

        public Task<(MessageType messageType, ArraySegment<byte> data)> ReceiveAsync(CancellationToken cancellation)
        {
            return _queue.DequeueAsync(cancellation);
        }

        
        
        public async void BeginProcessMessages(CancellationToken cancellationToken)
        {
            while(_webSocket.State == System.Net.WebSockets.WebSocketState.Open || !cancellationToken.IsCancellationRequested)
            {
                var (buffer, type, closeStatusDescription) = await ReceiveMessageAsync(cancellationToken).ConfigureAwait(false);

                if (type == WebSocketMessageType.Text && buffer != null)
                {
                    _queue.Enqueue(((MessageType)type, buffer));
                    return;
                }

                if (type == WebSocketMessageType.Close)
                {
                    _queue.Enqueue(((MessageType)type, new ArraySegment<byte>(Encoding.UTF8.GetBytes(closeStatusDescription))));
                }

            }
        }

        private async
            Task<(ArraySegment<byte> buffer, WebSocketMessageType type, string
                closeStatusDescription)> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            const int bufferSize = 4096;
            var buffer = new byte[bufferSize];
            var offset = 0;
            var free = buffer.Length;
            WebSocketMessageType type;
            string closeStatusDescription;
            while (true)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, free),
                    cancellationToken).ConfigureAwait(false);
                offset += result.Count;
                free -= result.Count;
                if (result.EndOfMessage)
                {
                    type = result.MessageType;
                    closeStatusDescription = result.CloseStatusDescription;
                    break;
                }

                if (free == 0)
                {
                    // No free space
                    // Resize the outgoing buffer
                    var newSize = buffer.Length + bufferSize;

                    var newBuffer = new byte[newSize];
                    Array.Copy(buffer, 0, newBuffer, 0, offset);
                    buffer = newBuffer;
                    free = buffer.Length - offset;
                }
            }
            
            return (new ArraySegment<byte>(buffer, 0, offset), type, closeStatusDescription);
        }
    }
}