using System.Collections.Generic;

namespace JsonRpcNet.Docs
{
    public class JsonRpcServiceDoc
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<JsonRpcMethodDoc> Methods { get; set; } = new List<JsonRpcMethodDoc>();
    }
}