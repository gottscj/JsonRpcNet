using System.Collections.Generic;
using System.Linq;

namespace JsonRpcNet
{
    public abstract class WebSocketConnectionManager
    {
        private readonly Dictionary<string, IJsonRpcWebSocket> _sockets = new Dictionary<string, IJsonRpcWebSocket>();
        private readonly object _syncRoot = new object();
        
        public IJsonRpcWebSocket GetSocketById(string id)
        {
            lock (_syncRoot)
            {
                if (_sockets.TryGetValue(id, out var websocket))
                {
                    return websocket;
                }
            }

            return null;
        }

        public Dictionary<string, IJsonRpcWebSocket> GetAll()
        {
            lock (_syncRoot)
            {
                return _sockets.ToDictionary(s => s.Key, s => s.Value);
            }
        }

        public string GetId(IJsonRpcWebSocket socket)
        {
            lock (_syncRoot)
            {
                return _sockets.FirstOrDefault(p => p.Value == socket).Key;    
            }
        }
        
        public void AddSocket(IJsonRpcWebSocket socket)
        {
            lock (_syncRoot)
            {
                _sockets[socket.Id] = socket;
            }
        }

        public void RemoveSocket(string id)
        {
            lock (_syncRoot)
            {
                _sockets.Remove(id);
            }
        }
    }
}