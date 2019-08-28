using Newtonsoft.Json;

namespace JsonRpcNet.Models
{
	public sealed class JsonRpcErrorResponse : JsonRpcContract
	{
		[JsonProperty("error", Required = Required.Always)]
		public JsonRpcError Error { get; set; }
	}
}