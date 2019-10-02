using System;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;

namespace JsonRpcNet.Docs
{
    public static class DocGenerator
    {
        public static JsonRpcDoc JsonRpcDoc { get; set; }
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