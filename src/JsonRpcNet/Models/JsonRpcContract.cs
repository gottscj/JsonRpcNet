using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace JsonRpcNet.Models
{
	public abstract class JsonRpcContract
	{
		private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
		{
			Converters = new List<JsonConverter> { new StringEnumConverter(new CamelCaseNamingStrategy()) },
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			
		};
		public static JsonSerializerSettings SerializerSettings
		{
			get => _serializerSettings;
			set => _serializerSettings = value ?? throw new ArgumentException($"Cannot set {nameof(SerializerSettings)} to null");
		}

		[JsonProperty("jsonrpc", Required = Required.Always)]
		public string JsonRpc => "2.0";
		
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public object Id { get; set; }

		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, _serializerSettings);
		}
	}
}