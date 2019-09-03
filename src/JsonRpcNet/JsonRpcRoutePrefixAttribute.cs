using System;

namespace JsonRpcNet
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class JsonRpcRoutePrefixAttribute : Attribute
	{
		public string RoutePrefix { get; }
		public JsonRpcRoutePrefixAttribute(string routePrefix)
		{
            if (!routePrefix.StartsWith("/"))
            {
                routePrefix = "/" + routePrefix;
            }

			RoutePrefix = routePrefix;
		}
	}
}