using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JsonRpcNet
{
    public abstract class WebSocketHandler : IWebSocketConnectionHandler
    {
        private IJsonRpcWebSocket _jsonRpcWebSocket;
        private static readonly IDictionary<string, IJsonRpcWebSocket> sockets = new Dictionary<string, IJsonRpcWebSocket>();

        Task IWebSocketConnectionHandler.InitializeConnectionAsync(IJsonRpcWebSocket socket)
        {
            _jsonRpcWebSocket = socket;
            sockets[socket.Id] = socket;
            _jsonRpcWebSocket.OnMessageReceived += JsonRpcWebSocketOnOnMessage;
            _jsonRpcWebSocket.OnConnectionClosed += JsonRpcWebSocketOnOnConnectionClosed;
            return OnConnected();
        }

        private Task JsonRpcWebSocketOnOnMessage(MessageType messageType, string message)
        {
            return OnMessage(messageType, message);

        }

        private Task JsonRpcWebSocketOnOnConnectionClosed(int code, string reason)
        {
            sockets.Remove(_jsonRpcWebSocket.Id);
            return OnDisconnected((CloseStatusCode)code, reason);
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
            return _jsonRpcWebSocket.CloseAsync((int)statusCode, reason);
        }
        
        protected async Task SendMessageAsync(string message)
        {
            if(_jsonRpcWebSocket.WebSocketState != JsonRpcWebSocketState.Open)
                return;

            await _jsonRpcWebSocket.SendAsync(message).ConfigureAwait(false);
        }

        protected async Task BroadcastAsync(string message)
        {
            var tasks = new List<Task>();
            foreach (var jsonRpcWebSocket in sockets.Where(kvp => kvp.Key != _jsonRpcWebSocket.Id))
            {
                tasks.Add(SendMessageAsync(message));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        protected IPAddress GetUserEndpointIpAddress()
        {
            // UserEndPoint can be disposed if e.g. the user closes the connection prematurely
            try
            {
                return _jsonRpcWebSocket.UserEndPoint.Address;
            }
            catch
            {
                return null;
            }
        }
        protected abstract Task OnMessage(MessageType messageType, string message);
    }
}