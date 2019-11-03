using Newtonsoft.Json.Schema;
using JsonSchema = NJsonSchema.JsonSchema;

namespace JsonRpcNet.Docs
{
    public class JsonRpcParameterDoc
    {
        public string Name { get; set; } = string.Empty;
        public JsonSchema Type { get; set; }
    }
}