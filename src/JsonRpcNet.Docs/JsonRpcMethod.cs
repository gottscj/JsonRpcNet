using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Annotations;

namespace JsonRpcNet.Docs
{
    public class JsonRpcMethod
    {
        public JsonRpcMethod(MethodInfo methodInfo, IEnumerable<ParameterInfo> parameters)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
            Name = methodInfo.Name;

            Response = new JsonRpcTypeInfo(methodInfo.ReturnType);
            Parameters = parameters
                .Select(p => new JsonRpcTypeInfo(p.Name, p.ParameterType)).ToList();
        }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("response")]
        public JsonRpcTypeInfo Response { get; }
        
        [JsonProperty("params")]
        public IList<JsonRpcTypeInfo> Parameters { get; }
    }
}