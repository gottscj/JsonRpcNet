using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace JsonRpcNet.WebSocketSharp
{
    public class WebSocketSharpWebSocket : WebSocketBehavior, IWebSocket
    {
        private readonly CancellationToken _cancellationToken;
        private readonly AsyncQueue<(MessageType messageType,ArraySegment<byte> data)> _queue;
        
        public WebSocketSharpWebSocket(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _queue = new AsyncQueue<(MessageType messageType, ArraySegment<byte> data)>();
        }
        
        public string Id => ID;
        public IPEndPoint UserEndPoint => Context.UserEndPoint;

        public JsonRpcWebSocketState WebSocketState
        {
            get
            {
                switch (Context.WebSocket.ReadyState)
                {
                    case global::WebSocketSharp.WebSocketState.Connecting:
                        return JsonRpcWebSocketState.Connecting;
                    case global::WebSocketSharp.WebSocketState.Open:
                        return JsonRpcWebSocketState.Open;
                    case global::WebSocketSharp.WebSocketState.Closing:
                        return JsonRpcWebSocketState.Closing;
                    case global::WebSocketSharp.WebSocketState.Closed:
                        return JsonRpcWebSocketState.Closed;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Task<(MessageType messageType, ArraySegment<byte> data)> ReceiveAsync(CancellationToken cancellation)
        {
            return _queue.DequeueAsync(cancellation);
        }

        public Task SendAsync(string message, CancellationToken cancellation)
        {
            base.Send(message);
            return Task.CompletedTask;
        }

        public Task CloseAsync(int code, string reason, CancellationToken cancellation)
        {
            base.Context.WebSocket.Close((ushort)code, reason);
            return Task.CompletedTask;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                _queue.Enqueue((MessageType.Binary, new ArraySegment<byte>(e.RawData)));
                return;
            }
            
            if( e.IsText)
            {
                _queue.Enqueue((MessageType.Text, new ArraySegment<byte>(e.RawData)));
                return;
            }

            if (e.IsPing)
            {
                // TODO handle
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _queue.Enqueue((MessageType.Close, new ArraySegment<byte>(Encoding.UTF8.GetBytes(e.Reason))));
        }
    }
}