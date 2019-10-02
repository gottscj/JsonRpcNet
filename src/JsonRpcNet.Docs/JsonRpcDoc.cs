using System.Collections.Generic;

namespace JsonRpcNet.Docs
{
    public class JsonRpcDoc
    {
        public ContactDoc Contact { get; set; }
        public JsonRpcInfoDoc GeneralInfo { get; set; }
        public IList<JsonRpcServiceDoc> Services { get; set; }
    }
}