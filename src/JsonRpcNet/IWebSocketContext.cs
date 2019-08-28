using System.Collections.Generic;

namespace JsonRpcNet
{
	public interface IWebSocketContext<T>
	{
		IList<T> Connections(IList<string> connections);
	};
}