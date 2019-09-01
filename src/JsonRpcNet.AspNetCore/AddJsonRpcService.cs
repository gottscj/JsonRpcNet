using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JsonRpcNet.AspNetCore
{
    public static class JsonRpcApplicationBuilder
    {
        public static IApplicationBuilder AddJsonRpcHandler(this IApplicationBuilder app, PathString path,
            JsonRpcWebSocketHandler handler)
        {
            return app.Map(path, a => a.UseMiddleware<JsonRpcWebSocketMiddleware>(handler));
        }
        
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
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