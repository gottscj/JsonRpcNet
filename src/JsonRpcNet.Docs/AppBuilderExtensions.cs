using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace JsonRpcNet.Docs
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseJsonRpcApi(this IApplicationBuilder app, string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new ManifestEmbeddedFileProvider(typeof(AppBuilderExtensions).Assembly, "resources"),
                RequestPath = path,
                EnableDirectoryBrowsing = true
            });

            return app;
        }
    }
}