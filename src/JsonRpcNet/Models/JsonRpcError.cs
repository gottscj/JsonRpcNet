using Newtonsoft.Json;

namespace JsonRpcNet.Models
{
	public sealed class JsonRpcError
	{
		[JsonProperty("code", Required = Required.Always)]
		public int Code { get; set; }

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; set; }
	}
}