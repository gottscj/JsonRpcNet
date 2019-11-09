using System;
using System.Threading.Tasks;

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

		public Task<object> InvokeAsync(object instance, object[] parameters, string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				return MethodInfo.InvokeAsync(instance, parameters);
			}
			
			if (Roles.Contains(role))
			{
				return MethodInfo.InvokeAsync(instance, parameters);
			}

			return null;
		}
	}
}