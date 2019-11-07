using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Annotations;
using NJsonSchema.Generation;
using JsonSchema = NJsonSchema.JsonSchema;
using JsonSchemaGenerator = NJsonSchema.Generation.JsonSchemaGenerator;
using JsonSchemaResolver = NJsonSchema.Generation.JsonSchemaResolver;

namespace JsonRpcNet.Docs
{
    public class JsonRpcDoc
    {
        [JsonProperty("info")]
        public JsonRpcInfo Info { get; }
        [JsonProperty("services")]
        public IList<JsonRpcService> Services { get; } = new List<JsonRpcService>();

        public JsonRpcDoc(JsonRpcInfo info)
        {
            Info = info;
        }
    }
}