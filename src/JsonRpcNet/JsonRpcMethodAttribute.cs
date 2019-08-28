namespace JsonRpcNet
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

}
