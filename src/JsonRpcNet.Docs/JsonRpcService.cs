using System.Collections.Generic;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Annotations;

namespace JsonRpcNet.Docs
{
    public class JsonRpcService
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;
        
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("methods")]
        public IList<JsonRpcMethod> Methods { get; set; } = new List<JsonRpcMethod>();
        
        [JsonProperty("notifications")]
        public IList<JsonRpcNotification> Notifications { get; set; } = new List<JsonRpcNotification>();
    }
}