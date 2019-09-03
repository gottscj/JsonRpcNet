using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonRpcNet.AspNetCore.Sample
{
    [JsonRpcRoutePrefix("chat")]
    public class ChatJsonRpcWebSocketHandler : JsonRpcWebSocketHandler
    {
        [JsonRpcMethod("sendMessage")]
        public void SendMessage(string message)
        {

        }
    }
}
