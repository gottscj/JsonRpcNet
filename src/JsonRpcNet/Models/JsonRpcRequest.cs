using Newtonsoft.Json;

namespace JsonRpcNet.Models
{
	public sealed class JsonRpcRequest : JsonRpcContract
	{
		[JsonProperty("method", Required = Required.Always)]
		public string Method { get; set; }

		[JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
		public object Params { get; set; }
	}
}