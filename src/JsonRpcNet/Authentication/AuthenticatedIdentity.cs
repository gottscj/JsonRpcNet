using System.Security.Principal;

namespace JsonRpcNet.Authentication
{
	public class AuthenticatedIdentity
	{
		public IIdentity Identity { get; }
		public AuthenticatedIdentity(IIdentity identity)
		{
			Identity = identity;
		}
	}
}