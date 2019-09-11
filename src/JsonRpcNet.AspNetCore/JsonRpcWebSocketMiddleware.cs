using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace JsonRpcNet.AspNetCore
{
    public class JsonRpcWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketConnection _webSocketHandler;

        public JsonRpcWebSocketMiddleware(RequestDelegate next, IWebSocketConnection webSocketHandler)
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

            //context.Request.Path
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var netCoreWebsocket = new NetCoreWebSocket(socket);
            netCoreWebsocket.BeginProcessMessages(context.RequestAborted);
            await _webSocketHandler.HandleMessagesAsync(netCoreWebsocket, context.RequestAborted);

            await _next.Invoke(context);
        }
    }
}