using System.Net;
using System.Threading.Tasks;

namespace JsonRpcNet
{
    public delegate Task MessageReceived(MessageType messageType, string message);
    public delegate Task ConnectionClosed(CloseStatusCode code, string reason);
    
    public interface IJsonRpcWebSocket
    {
        event MessageReceived OnMessageReceived;
        event ConnectionClosed OnConnectionClosed;
        
        string Id { get; }
        
        IPEndPoint UserEndPoint { get; }
        
        JsonRpcWebSocketState WebSocketState { get; }

        Task SendAsync(string message);

        Task CloseAsync(int code, string reason);
    }
}