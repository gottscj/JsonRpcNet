using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace JsonRpcNet.Docs
{
    public static class JsonRpcNetDocsExtensions
    {
        public static void AddJsonRpcNetDocs(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(JsonRpcNetDocsConfigureOptions));
            services.Configure<JsonRpcNetDocsOptions>(options => {
                options.Enabled = true;
            });
        }

        public static void AddJsonRpcNetDocs(this IServiceCollection services, string route)
        {
            services.AddJsonRpcNetDocs();

            if (!route.StartsWith("/"))
            {
                route = "/" + route;
            }

            services.Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AddAreaPageRoute("Docs", "/index", route);
            });
        }
    }
}
