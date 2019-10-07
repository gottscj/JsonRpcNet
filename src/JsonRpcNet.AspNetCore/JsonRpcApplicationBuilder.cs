using System;
using System.Diagnostics;
using System.Reflection;
using JsonRpcNet.Attributes;
using JsonRpcNet.Docs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace JsonRpcNet.AspNetCore
{
    public static class JsonRpcApplicationBuilder
    {
        public static IApplicationBuilder AddJsonRpcService<TJsonRpcWebSocketService>(this IApplicationBuilder app)
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
        
        public static IApplicationBuilder UseJsonRpcApi(this IApplicationBuilder app, string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.Value.StartsWith(path))
                {
                    await next.Invoke();
                    return;
                }
                
                try
                {
                    var reader = new EmbeddedFileReader(context.Request.Path, path);
                    var bytes = reader.GetEmbeddedFile();
                    context.Response.ContentType = "text/html";
                    context.Response.StatusCode = 200;
                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    context.Response.StatusCode = 404; // not found
                }
            });

            return app;
        }
    }
    
}