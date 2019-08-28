using System;
using System.Linq;
using System.Security.Claims;

namespace JsonRpcNet.Authentication
{
	public class BearerTokenReader: ITokenReader
	{
		private readonly OAuthAuthorizationServerOptions _authorizationServerOptions;

		public BearerTokenReader(OAuthAuthorizationServerOptions authorizationServerOptions)
		{
			_authorizationServerOptions = authorizationServerOptions;
		}

		public AuthenticatedIdentity Read(string token)
		{
			var identity = GetIdentity(token);

			if (identity?.IsAuthenticated != true ||
			    !PermissionSet.TryParse(identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value, out var permissionSet))
			{
				return null;
			}

			return new AuthenticatedIdentity(identity, permissionSet);
		}

		private ClaimsIdentity GetIdentity(string token)
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