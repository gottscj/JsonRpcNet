using System.Collections.Generic;

namespace JsonRpcNet.Docs
{
    public class JsonRpcServiceDoc
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public IList<JsonRpcMethodDoc> Methods { get; set; }
    }
}