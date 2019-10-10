using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonRpcNet.Docs
{
    public class JsonRpcMethodDoc
    {
        public JsonRpcMethodDoc(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
            Name = methodInfo.Name;
            Returns = methodInfo.ReturnType.Name;
            
            Parameters = methodInfo.GetParameters()
                .Select(p => new JsonRpcParameterDoc {Name = p.Name, Type = p.ParameterType.Name}).ToList();
        }
        public string Name { get; }
        public string Description { get; set; } = string.Empty;
        public string Returns { get; }
        public IList<JsonRpcParameterDoc> Parameters { get; }
    }
}