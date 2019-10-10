using System;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;

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
                jsonRpcDoc.Services.Add(GenerateJsonRpcServiceDoc(jsonRpcService));
            }

            return jsonRpcDoc;
        }
        public static JsonRpcServiceDoc GenerateJsonRpcServiceDoc<T>()
        {
            return GenerateJsonRpcServiceDoc(typeof(T));
        }
        public static JsonRpcServiceDoc GenerateJsonRpcServiceDoc(Type type)
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
                    MethonInfo = m
                })
                .ToList();

            serviceDoc.Methods = methodMetaData.Select(m => new JsonRpcMethodDoc(m.MethonInfo)
            {
                Description = m.Attribute.Description
            }).ToList();

            return serviceDoc;
        }
    }
}