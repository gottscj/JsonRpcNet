using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonRpcNet.Docs
{
    public class JsonRpcInfo
    {
        [JsonProperty("description")]
        public string Description { get; set; }= string.Empty;
        [JsonProperty("version")]
        public string Version { get; set; }= string.Empty;
        [JsonProperty("title")]
        public string Title { get; set; }= string.Empty;

        [JsonProperty("contact")]
        public JsonRpcContact Contact { get; set; } = new JsonRpcContact();

        [JsonProperty("jsonRpcApiEndpoint")]
        public string JsonRpcApiEndpoint { get; set; } = "/jsonrpc";
    }
}