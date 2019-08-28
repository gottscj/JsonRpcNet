using System;

namespace JsonRpcNet
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute
    {
        public string Roles { get; set; }
		
        public string Users { get; set; }
    }
}