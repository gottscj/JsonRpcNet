using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonRpcNet.Docs.Components
{
    public class JsonRpcHeader
    {
        public InfoDoc Info { get; set; }
        public ContactDoc Contact { get; set; }
        public IList<JsonRpcServiceDoc> Services { get; set; }
    }

    public class ContactDoc
    {
        public string Email { get; set; }
    }

    public class InfoDoc
    {
        public string Description { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
    }

    public class JsonRpcServiceDoc
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public IList<JsonRpcMethodDoc> Methods { get; set; }
    }

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

    public class JsonRpcParameterDoc
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}