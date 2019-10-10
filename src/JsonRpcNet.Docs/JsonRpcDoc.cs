using System.Collections.Generic;

namespace JsonRpcNet.Docs
{
    public class JsonRpcDoc
    {
        public JsonRpcInfoDoc Info { get; set; }
        public IList<JsonRpcServiceDoc> Services { get; } = new List<JsonRpcServiceDoc>();
    }
}