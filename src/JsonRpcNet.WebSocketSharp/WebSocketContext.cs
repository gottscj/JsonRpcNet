using System.Collections.Generic;
using System.Linq;
using WebSocketSharp.Server;

namespace JsonRpcNet.WebSocketSharp
{
	public class WebSocketContext<T> : IWebSocketContext<T> where T : class
	{
		private readonly WebSocketServiceHost _webSocketServiceHost;

		public WebSocketContext(WebSocketServiceManager webSocketServiceManager)
		{
			_webSocketServiceHost = webSocketServiceManager.Hosts
				.Single(h => h.Type.GetInterface(typeof(T).Name) != null);
		}

		public IList<T> Connections(IList<string> connections)
		{
			return _webSocketServiceHost.Sessions.Sessions
				.Where(s => connections.Contains(s.ID))
				.OfType<T>()
				.ToList();
		}
	}
}