using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace JsonRpcNet.Docs
{
    public class JsonRpcDoc
    {
        public JsonRpcInfoDoc Info { get; set; }
        public IList<JsonRpcServiceDoc> Services { get; } = new List<JsonRpcServiceDoc>();
        public IDictionary<string, JSchema> Definitions { get; set; } = new Dictionary<string, JSchema>();
    }
}