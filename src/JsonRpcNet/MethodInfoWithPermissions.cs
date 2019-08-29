using System;

namespace JsonRpcNet
{
	internal class MethodInfoWithPermissions
	{
		public FastMethodInfo MethodInfo { get; }
		public string Roles { get; set; }
		
		public string Users { get; set; }

		public MethodInfoWithPermissions(FastMethodInfo methodInfo, string roles, string users)
		{
			MethodInfo = methodInfo;
			Roles = roles;
			Users = users;
		}

		public object Invoke(object instance, object[] parameters, string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				return MethodInfo.Invoke(instance, parameters);
			}
			
			if (Roles.Contains(role))
			{
				return MethodInfo.Invoke(instance, parameters);
			}

			return null;
		}
	}
}