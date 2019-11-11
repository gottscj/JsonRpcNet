using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using JsonRpcNet.Attributes;
using JsonRpcNet.Docs;
using WebSocketSharp;
using WebSocketSharp.Net;
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

		public static void UseJsonRpcApi(this HttpServer server, JsonRpcInfo jsonRpcInfo)
		{
			server.OnGet += (s, e) =>
			{
				var referer = e.Request.UrlReferrer?.AbsolutePath ?? "";
				var requestPath = referer + e.Request.Url.AbsolutePath;
				if (requestPath.StartsWith("//"))
				{
					requestPath = requestPath.Substring(1);
				}
				var file = JsonRpcFileReader.GetFile(requestPath, jsonRpcInfo);
				if (!file.Exist)
				{
					return;
				}
				e.Response.ContentType = MimeTypeProvider.Get(file.Extension);
				e.Response.ContentEncoding = Encoding.UTF8;
				
				e.Response.StatusCode = 200;
				
				e.Response.WriteContent(file.Buffer);
			};
		}
	}
}
