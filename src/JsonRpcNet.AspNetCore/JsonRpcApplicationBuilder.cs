using System;
using System.Reflection;
using JsonRpcNet.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JsonRpcNet.AspNetCore
{
    public static class JsonRpcApplicationBuilder
    {
        public static IApplicationBuilder AddJsonRpcHandler<TJsonRpcWebSocketService>(this IApplicationBuilder app)
            where TJsonRpcWebSocketService : JsonRpcWebSocketService
        {
            var rpcServiceAttribute = typeof(TJsonRpcWebSocketService).GetCustomAttribute<JsonRpcServiceAttribute>();
            var path = rpcServiceAttribute?.Path ?? "/" + typeof(TJsonRpcWebSocketService).Name;
            Func<IWebSocketConnection> factory = () => app.ApplicationServices.GetRequiredService<TJsonRpcWebSocketService>();
            
            return app.Map(path, a => a.UseMiddleware<JsonRpcWebSocketMiddleware>(factory));
        }

        public static IServiceCollection AddWebSocketHandlers(this IServiceCollection services)
        {
            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if(type.GetTypeInfo().BaseType == typeof(JsonRpcWebSocketService))
                {
                    services.AddTransient(type);
                }
            }

            return services;
        }
    }
    
}