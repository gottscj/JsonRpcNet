using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;

namespace JsonRpcNet.Docs.Components
{
    public static class DocGenerator
    {
        public static JsonRpcServiceDoc GenerateJsonRpcServiceDoc<T>()
        {
            var serviceAttribute =
                (JsonRpcServiceAttribute) typeof(T).GetCustomAttribute(typeof(JsonRpcServiceAttribute));

            var serviceDoc = new JsonRpcServiceDoc
            {
                Name = serviceAttribute.Name,
                Path = serviceAttribute?.Path ?? typeof(T).Name,
                Description = serviceAttribute?.Description ?? string.Empty
            };

            var methodMetaData = typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Public)
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