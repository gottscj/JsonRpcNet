using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JsonRpcNet
{
    public abstract class WebSocketConnection : IWebSocketConnection
    {
        private IWebSocket _webSocket;
        private static readonly IDictionary<string, IWebSocket> Sockets = new Dictionary<string, IWebSocket>();

        private CancellationToken _cancellation = default(CancellationToken);
        async Task IWebSocketConnection.HandleMessagesAsync(IWebSocket socket, CancellationToken cancellationToken)
        {
            _webSocket = socket;
            Sockets[socket.Id] = socket;
            _cancellation = cancellationToken;
            await OnConnected();
            while (_webSocket.WebSocketState == JsonRpcWebSocketState.Open)
            {
                var (type, buffer) = await _webSocket.ReceiveAsync(cancellationToken);
                string message = null;
                if (buffer.Array == null)
                {
                    throw new InvalidOperationException("Received empty data buffer from underlying socket");
                }
                if (type != MessageType.Binary)
                {
                    message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
                }
                
                switch (type)
                {
                    case MessageType.Text:
                        await OnMessage(message);
                        break;
                    case MessageType.Binary:
                        await OnBinaryMessage(buffer);
                        break;
                    case MessageType.Close:
                        Sockets.Remove(_webSocket.Id);
                        await OnDisconnected(CloseStatusCode.Normal, message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected virtual Task OnConnected()
        {
            return Task.CompletedTask;
        }
                                           
        protected virtual Task OnDisconnected(CloseStatusCode statusCode, string reason)
        {
            return Task.CompletedTask;
        }

        protected Task CloseAsync(CloseStatusCode statusCode, string reason)
        {
            return _webSocket.CloseAsync((int)statusCode, reason, _cancellation);
        }
        
        protected async Task SendAsync(string message)
        {
            if(_webSocket.WebSocketState != JsonRpcWebSocketState.Open)
                return;

            await _webSocket.SendAsync(message, _cancellation).ConfigureAwait(false);
        }

        protected async Task BroadcastAsync(string message)
        {
            var tasks = new List<Task>();
            foreach (var jsonRpcWebSocket in Sockets.Where(kvp => kvp.Key != _webSocket.Id).Select(kvp => kvp.Value))
            {
                tasks.Add(jsonRpcWebSocket.SendAsync(message, _cancellation));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        protected IPAddress GetUserEndpointIpAddress()
        {
            // UserEndPoint can be disposed if e.g. the user closes the connection prematurely
            try
            {
                return _webSocket.UserEndPoint.Address;
            }
            catch
            {
                return null;
            }
        }
        protected abstract Task OnMessage(string message);
        protected abstract Task OnBinaryMessage(ArraySegment<byte> buffer);
    }
}