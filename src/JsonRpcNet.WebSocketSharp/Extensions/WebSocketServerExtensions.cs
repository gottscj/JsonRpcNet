using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;
using JsonRpcNet.Docs;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace JsonRpcNet.WebSocketSharp.Extensions
{
	public static class WebSocketServerExtensions
	{
		public static void AddJsonRpcService<T>(this HttpServer server, CancellationToken cancellationToken, Func<T> initializer) where T : JsonRpcWebSocketService
		{
			var serviceType = typeof(T);
			var routePrefix = serviceType.GetCustomAttribute<JsonRpcServiceAttribute>()?.Path;
			if (string.IsNullOrWhiteSpace(routePrefix))
			{
				throw new InvalidOperationException($"Service {serviceType.FullName} does not have a {nameof(JsonRpcServiceAttribute)} or a route prefix is not defined");
			}

			server.AddWebSocketService(routePrefix, () => new WebSocketSharpWebSocket(cancellationToken, initializer));
		}
		public static void AddJsonRpcService<T>(this HttpServer server, Func<T> initializer) where T : JsonRpcWebSocketService
		{
			AddJsonRpcService(server, CancellationToken.None, initializer);
		}

		public static void UseJsonRpcApi(this HttpServer server, string path)
		{
			if (!path.StartsWith("/"))
			{
				path = "/" + path;
			}
			server.OnGet += (s, e) =>
			{

				if (e.Request.Url.AbsolutePath.StartsWith(path)) return;
				
				try
				{
					var bytes = EmbeddedFileReader.GetEmbeddedFile(e.Request.Url.AbsolutePath, path);
					e.Response.ContentType = "text/html";
					e.Response.StatusCode = 200;
					e.Response.WriteContent(bytes);
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.ToString());
					e.Response.StatusCode = 404; // not found
				}
				
			};
		}
		
		

	}
}
