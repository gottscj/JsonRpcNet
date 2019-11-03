using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Namotion.Reflection;
using NJsonSchema;
using NJsonSchema.Annotations;
using NJsonSchema.Generation;

namespace JsonRpcNet.Docs
{
    public class JsonRpcMethodDoc
    {
        public JsonRpcMethodDoc(MethodInfo methodInfo, IEnumerable<ParameterInfo> parameters,
            JsonSchemaGenerator generator)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
            Name = methodInfo.Name;
            var tst = generator.Generate(methodInfo.DeclaringType);
            Returns = generator.Generate(methodInfo.ReturnType);
            Parameters = parameters
                .Select(p => new JsonRpcParameterDoc
                    {Name = p.Name, Type = generator.Generate(p.ParameterType)}).ToList();
        }

        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public JsonSchema Returns { get; }
        public IList<JsonRpcParameterDoc> Parameters { get; }
    }
}