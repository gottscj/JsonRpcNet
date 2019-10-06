using System;
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
				if (e.Request.Url.AbsolutePath.Equals(path))
				{
					using (var stream = typeof(JsonRpcDoc).Assembly.GetManifestResourceStream( typeof(JsonRpcDoc).Namespace + ".resources.index.html"))
					{
						if (stream == null)
						{
							return;
						}
						e.Response.ContentType = "text/html";
						e.Response.StatusCode = 200;
						var bytes = ReadToEnd(stream);
						e.Response.WriteContent(bytes);
					}
				}
			};
		}
		
		private static byte[] ReadToEnd(Stream stream)
		{
			long originalPosition = 0;

			if(stream.CanSeek)
			{
				originalPosition = stream.Position;
				stream.Position = 0;
			}

			try
			{
				byte[] readBuffer = new byte[4096];

				int totalBytesRead = 0;
				int bytesRead;

				while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
				{
					totalBytesRead += bytesRead;

					if (totalBytesRead == readBuffer.Length)
					{
						int nextByte = stream.ReadByte();
						if (nextByte != -1)
						{
							byte[] temp = new byte[readBuffer.Length * 2];
							Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
							Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
							readBuffer = temp;
							totalBytesRead++;
						}
					}
				}

				byte[] buffer = readBuffer;
				if (readBuffer.Length != totalBytesRead)
				{
					buffer = new byte[totalBytesRead];
					Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
				}
				return buffer;
			}
			finally
			{
				if(stream.CanSeek)
				{
					stream.Position = originalPosition; 
				}
			}
		}

	}
}
