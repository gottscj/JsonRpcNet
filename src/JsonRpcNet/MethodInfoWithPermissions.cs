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
	}
}