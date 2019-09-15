using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonRpcNet.Docs
{
    public static class JsonRpcNetDocsExtensions
    {
        public static void AddJsonRpcNetDocs(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(JsonRpcNetDocsConfigureOptions));
        }
    }
}
