using System;

namespace JsonRpcNet.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class JsonRpcMethodAttribute : System.Attribute
	{
		public string Name { get; }

		public JsonRpcMethodAttribute(string name)
		{
			Name = name;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class JsonRpcServiceAttribute : System.Attribute
	{
		public string Path { get; set; }

		public JsonRpcServiceAttribute(string path)
		{
			if (!path.StartsWith("/"))
			{
				path = "/" + path;
			}

			Path = path;
		}
	}

}
