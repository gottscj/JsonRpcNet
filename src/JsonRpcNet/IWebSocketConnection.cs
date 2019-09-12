using System.Threading;
using System.Threading.Tasks;

namespace JsonRpcNet
{
    public interface IWebSocketConnection
    {
        Task HandleMessagesAsync(IWebSocket socket);
    }
}