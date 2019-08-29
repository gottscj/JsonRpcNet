using System.Collections.Generic;

namespace JsonRpcNet.WebSocketSharp
{
	public interface IWebSocketContext<T>
	{
		IList<T> Connections(IList<string> connections);
	};
}