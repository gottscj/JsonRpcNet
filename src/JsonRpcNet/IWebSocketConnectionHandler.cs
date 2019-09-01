using System.Threading.Tasks;

namespace JsonRpcNet
{
    public interface IWebSocketConnectionHandler
    {
        Task InitializeConnectionAsync(IJsonRpcWebSocket socket);
    }
}