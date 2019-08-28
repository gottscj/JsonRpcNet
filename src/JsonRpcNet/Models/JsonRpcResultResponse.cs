using Newtonsoft.Json;

namespace JsonRpcNet.Models
{
	public sealed class JsonRpcResultResponse : JsonRpcContract
	{
		public JsonRpcResultResponse(object result)
		{
			Result = result;
		}

		// TODO: When/if the server can send requests, then we need to consider the deserialization
		//       of the result, since now types are lost (e.g. int -> long) due to the conversion to object.
		[JsonProperty("result", Required = Required.AllowNull)]
		public object Result { get; set; }
	}
}