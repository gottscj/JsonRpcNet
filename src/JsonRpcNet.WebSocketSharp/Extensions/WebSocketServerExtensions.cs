using System;
using System.Reflection;
using System.Threading;
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

		public static void UseJsonRpcApi(this HttpServer server, JsonRpcInfoDoc jsonRpcInfo)
		{
			server.OnGet += (s, e) =>
			{
				var referer = e.Request.UrlReferrer?.AbsolutePath ?? "";
				var requestPath = referer + e.Request.Url.AbsolutePath;
				var file = JsonRpcFileReader.GetFile(requestPath, jsonRpcInfo);
				if (!file.Exist)
				{
					return;
				}
				e.Response.ContentType = MimeTypeProvider.Get(file.Extension);
				e.Response.StatusCode = 200;
				e.Response.WriteContent(file.Buffer);
			};
		}
		
		

	}
}
