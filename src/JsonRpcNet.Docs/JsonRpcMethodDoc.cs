using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonRpcNet.Docs
{
    public class JsonRpcMethodDoc
    {
        public JsonRpcMethodDoc(MethodInfo methodInfo)
        {
            Name = methodInfo.Name;
            Returns = methodInfo.ReturnType.Name;
            
            Parameters = methodInfo.GetParameters()
                .Select(p => new JsonRpcParameterDoc {Name = p.Name, Type = p.ParameterType.Name}).ToList();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Returns { get; set; }
        public IList<JsonRpcParameterDoc> Parameters { get; set; }
    }
}