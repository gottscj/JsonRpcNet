using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace JsonRpcNet.Docs
{
    public class JsonRpcApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _path;

        public JsonRpcApiMiddleware(RequestDelegate next, string path)
        {
            _next = next;
            _path = path;
        }

        public async Task Invoke(HttpContext context, IOptions<StaticFileOptions> options)
        {
            if (HttpMethods.IsGet(context.Request.Method) &&
                (context.Request.Path.Equals(_path, StringComparison.Ordinal) ||
                 context.Request.Path.Equals(_path + "index") ||
                 context.Request.Path.Equals(_path + "/index.html")))
            {
                var contents = options.Value.FileProvider.GetDirectoryContents("/");

                var file = contents.FirstOrDefault(f => f.Name.Equals("index.html", StringComparison.Ordinal));
                if (file == null)
                {
                    throw new Exception("'index.html' not found");
                }

                using (var reader = new StreamReader(file.CreateReadStream()))
                {
                    var text = reader.ReadToEnd();
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(text);
                    return;
                }
            }
            
            await _next(context);
        }
    }
}