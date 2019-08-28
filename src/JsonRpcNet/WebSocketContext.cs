using System.Collections.Generic;

namespace JsonRpcNet
{
	public class WebSocketContext<T> : IWebSocketContext<IClientWebSocketRpcApi<T>>
	{
		private readonly WebSocketServiceHost _webSocketServiceHost;

		public WebSocketContext(WebSocketServiceManager webSocketServiceManager)
		{
			_webSocketServiceHost = webSocketServiceManager.Hosts
				.Single(h => h.Type.GetInterface(typeof(IClientWebSocketRpcApi<T>).Name) != null);
		}

		public IList<IClientWebSocketRpcApi<T>> Connections(IList<string> connections)
		{
			return _webSocketServiceHost.Sessions.Sessions
				.Where(s => connections.Contains(s.ID))
				.OfType<IClientWebSocketRpcApi<T>>()
				.ToList();
		}
	}
}