using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JsonRpcNet.Docs
{
    public static class AppBuilderExtensions
    {
        public static void AddJsonRpcNetDocs(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(JsonRpcNetDocsConfigureOptions));
        }
        public static IApplicationBuilder UseJsonRpcApi(this IApplicationBuilder app, string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            
            app.UseMiddleware(typeof(JsonRpcApiMiddleware), path);

            return app;
        }
    }
}