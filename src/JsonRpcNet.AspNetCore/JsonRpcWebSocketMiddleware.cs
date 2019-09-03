using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace JsonRpcNet.AspNetCore
{
    public class JsonRpcWebSocketMiddleware : IMiddleware
    {
        private readonly IWebSocketConnectionHandler _webSocketHandler;

        public JsonRpcWebSocketMiddleware(IWebSocketConnectionHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }
            //context.Request.Path
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var jsonRpcWebSocket = new NetCoreWebSocket(socket);
            await _webSocketHandler.InitializeConnectionAsync(jsonRpcWebSocket);

            await jsonRpcWebSocket.HandleMessages();

            await next.Invoke(context);
        }
    }
}