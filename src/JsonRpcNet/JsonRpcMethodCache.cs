using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;

namespace JsonRpcNet
{
	internal class JsonRpcMethodCache
	{
		private readonly Dictionary<Type, Dictionary<string, MethodInfoWithPermissions>> _typeMethodCache = new Dictionary<Type, Dictionary<string, MethodInfoWithPermissions>>();

		public Dictionary<string, MethodInfoWithPermissions> Get(JsonRpcWebSocketService service)
		{
			var type = service.GetType();
			if (!_typeMethodCache.TryGetValue(type, out var methodCache))
			{
				methodCache = type.GetMethods()
					.Where(m => m.GetCustomAttributes(typeof(JsonRpcMethodAttribute), false).Length > 0)
					.ToDictionary(m => m.GetCustomAttribute<JsonRpcMethodAttribute>().Name.ToLowerInvariant(), m =>
					{
						var authorizeAttribute = m.GetCustomAttribute<AuthorizeAttribute>();
						if (authorizeAttribute == null)
						{
							return new MethodInfoWithPermissions(new FastMethodInfo(m), null, null);
						}

						return new MethodInfoWithPermissions(new FastMethodInfo(m), authorizeAttribute.Roles, authorizeAttribute.Users);
					});
				_typeMethodCache.Add(type, methodCache);
			}
			return methodCache;
		}
	}
}