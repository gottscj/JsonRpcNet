using System;
using System.Net;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace JsonRpcNet.WebSocketSharp
{
    public class JsonRpcWebSocketSharpJsonRpcWebSocket : WebSocketBehavior, IJsonRpcWebSocket
    {
        public JsonRpcWebSocketSharpJsonRpcWebSocket()
        {
        }
        
        public event MessageReceived OnMessageReceived;
        public event ConnectionClosed OnConnectionClosed;

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

        public Task SendAsync(string message)
        {
            base.Send(message);
            return Task.CompletedTask;
        }

        public Task CloseAsync(int code, string reason)
        {
            base.Context.WebSocket.Close((ushort)code, reason);
            return Task.CompletedTask;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                // TODO handle binary
                //OnMessageReceived?.Invoke(MessageType.Binary, e.RawData);
                return;
            }
            
            if( e.IsText)
            {
                OnMessageReceived?.Invoke(MessageType.Text, e.Data);
                return;
            }

            if (e.IsPing)
            {
                // TODO handle
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            OnConnectionClosed?.Invoke(e.Code, e.Reason);
        }
    }
}