using System;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;

namespace JsonRpcNet.Docs
{
    public static class DocGenerator
    {
        public static JsonRpcDoc GenerateJsonRpcDoc(JsonRpcInfoDoc info)
        {
            var jsonRpcServices = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && typeof(JsonRpcWebSocketService).IsAssignableFrom(t)))
                .ToList();
            
            var jsonRpcDoc = new JsonRpcDoc
            {
                Info = info
            };
            foreach (var jsonRpcService in jsonRpcServices)
            {
                jsonRpcDoc.Services.Add(GenerateJsonRpcServiceDoc(jsonRpcService, jsonRpcDoc));
            }

            return jsonRpcDoc;
        }
        
        public static JsonRpcServiceDoc GenerateJsonRpcServiceDoc(Type type, JsonRpcDoc jsonRpcDoc)
        {
            var serviceAttribute =
                (JsonRpcServiceAttribute) type.GetCustomAttribute(typeof(JsonRpcServiceAttribute));

            var serviceDoc = new JsonRpcServiceDoc
            {
                Name = serviceAttribute.Name,
                Path = serviceAttribute?.Path ?? type.Name.ToLower(),
                Description = serviceAttribute?.Description ?? string.Empty
            };

            var methodMetaData = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.GetCustomAttribute(typeof(JsonRpcMethodAttribute)) != null)
                .Select(m => new
                {
                    Attribute = (JsonRpcMethodAttribute) m.GetCustomAttribute(typeof(JsonRpcMethodAttribute)),
                    MethodInfo = m
                })
                .ToList();
            var generator = new JSchemaGenerator
            {
                SchemaLocationHandling = SchemaLocationHandling.Definitions,
                GenerationProviders = { new StringEnumGenerationProvider()},
                DefaultRequired = Required.Always,
                SchemaPropertyOrderHandling = SchemaPropertyOrderHandling.Alphabetical
            };
            
            foreach (var m in methodMetaData)
            {
                var parameters = m.MethodInfo.GetParameters();
                
                foreach (var parameterInfo in parameters.Where(p => Type.GetTypeCode(p.ParameterType) == TypeCode.Object))
                {
                    var schema = generator.Generate(parameterInfo.ParameterType);
                    jsonRpcDoc.Definitions[parameterInfo.Name] = schema;
                }
                serviceDoc.Methods.Add(new JsonRpcMethodDoc(m.MethodInfo, parameters)
                {
                    Name = m.Attribute.Name,
                    Description = m.Attribute.Description
                });
            }

            return serviceDoc;
        }
    }
}