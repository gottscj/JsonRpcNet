using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace JsonRpcNet.Docs
{
    public class JsonRpcNotification
    {
        public JsonRpcNotification(EventInfo eventInfo)
        {
            if (!eventInfo.EventHandlerType.Name.Equals("EventHandler`1"))
            {
                throw new InvalidOperationException("Event has to be of type 'EventHandler`1'");
            }

            var eventHandlerType = eventInfo.EventHandlerType.GetGenericArguments().Single();
            Parameter = new JsonRpcTypeInfo(eventHandlerType);
        }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonProperty("params")]
        public JsonRpcTypeInfo Parameter { get; }
    }
}