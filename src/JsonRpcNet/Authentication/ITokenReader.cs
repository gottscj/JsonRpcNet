using System.Collections.Specialized;
using System.Security.Principal;

namespace JsonRpcNet.Authentication
{
	public interface ITokenReader
	{
		IIdentity GetIdentity(string token);
	}
}