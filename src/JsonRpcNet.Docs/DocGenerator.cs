using System;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;

namespace JsonRpcNet.Docs
{
    public static class DocGenerator
    {
        public static JsonRpcDoc GenerateJsonRpcDoc(JsonRpcInfo info)
        {
            var jsonRpcServices = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && typeof(JsonRpcWebSocketService).IsAssignableFrom(t)))
                .ToList();

            var jsonRpcDoc = new JsonRpcDoc(info);
            
            foreach (var jsonRpcService in jsonRpcServices)
            {
                jsonRpcDoc.Services.Add(GenerateJsonRpcServiceDoc(jsonRpcService, jsonRpcDoc));
            }

            return jsonRpcDoc;
        }
        
        public static JsonRpcService GenerateJsonRpcServiceDoc(Type type, JsonRpcDoc jsonRpcDoc)
        {
            var serviceAttribute =
                (JsonRpcServiceAttribute) type.GetCustomAttribute(typeof(JsonRpcServiceAttribute));

            var serviceDoc = new JsonRpcService
            {
                Name = serviceAttribute?.Name ?? type.Name,
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
            
            foreach (var m in methodMetaData)
            {
                var parameters = m.MethodInfo.GetParameters();
                var method = new JsonRpcMethod(m.MethodInfo, parameters)
                {
                    Name = m.Attribute.Name,
                    Description = m.Attribute.Description
                };
                serviceDoc.Methods.Add(method);
            }

            var notificationMetaData = type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute(typeof(JsonRpcNotificationAttribute)) != null)
                .Select(m => new
                {
                    Attribute = (JsonRpcNotificationAttribute) m.GetCustomAttribute(typeof(JsonRpcNotificationAttribute)),
                    EventInfo = m
                })
                .ToList();
            
            foreach (var m in notificationMetaData)
            {
                var notification = new JsonRpcNotification(m.EventInfo)
                {
                    Name = m.Attribute.Name,
                    Description = m.Attribute.Description
                };
                serviceDoc.Notifications.Add(notification);
            }
            
            return serviceDoc;
        }
    }
}