using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace JsonRpcNet.AspNetCore
{
    public class JsonRpcWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketConnectionHandler _webSocketHandler;

        public JsonRpcWebSocketMiddleware(RequestDelegate next, IWebSocketConnectionHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }
            
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var jsonRpcWebSocket = new NetCoreWebSocket(socket);
            await _webSocketHandler.InitializeConnectionAsync(jsonRpcWebSocket);

            await jsonRpcWebSocket.HandleMessages();

            await _next.Invoke(context);
        }
    }
}