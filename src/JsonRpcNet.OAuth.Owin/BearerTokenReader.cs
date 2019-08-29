using System;
using System.Security.Principal;
using JsonRpcNet.Authentication;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace JsonRpcNet.OAuth.Owin
{
	public class BearerTokenReader : ITokenReader
	{
		private readonly OAuthAuthorizationServerOptions _authorizationServerOptions;

		public BearerTokenReader(OAuthAuthorizationServerOptions authorizationServerOptions)
		{
			_authorizationServerOptions = authorizationServerOptions;
		}

		public IIdentity GetIdentity(string token)
		{
			var ticket = _authorizationServerOptions.AccessTokenFormat.Unprotect(token);
			return IsTicketValid(ticket) ? ticket.Identity : null;
		}

		private static bool IsTicketValid(AuthenticationTicket ticket)
		{
			if (ticket == null)
			{
				return false;
			}

			if (!ticket.Properties.ExpiresUtc.HasValue)
			{
				return true;
			}

			return (ticket.Properties.ExpiresUtc.Value.DateTime - DateTime.UtcNow).TotalSeconds > 0;
		}
	}
}