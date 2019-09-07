using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JsonRpcNet.AspNetCore
{
    public static class JsonRpcApplicationBuilder
    {
        public static IApplicationBuilder AddJsonRpcHandler<TJsonRpcWebSocketHandler>(this IApplicationBuilder app)
            where TJsonRpcWebSocketHandler : JsonRpcWebSocketHandler
        {
            var routeprefixAttr = typeof(TJsonRpcWebSocketHandler).GetCustomAttribute<JsonRpcRoutePrefixAttribute>();
            var handler = app.ApplicationServices.GetRequiredService<TJsonRpcWebSocketHandler>();
            return app.Map(routeprefixAttr.RoutePrefix, a => a.UseMiddleware<JsonRpcWebSocketMiddleware>(handler));
        }
        
        public static IServiceCollection AddWebSocketHandlers(this IServiceCollection services)
        {
            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if(type.GetTypeInfo().BaseType == typeof(JsonRpcWebSocketHandler))
                {
                    services.AddTransient(type);
                }
            }

            return services;
        }
    }
    
}