namespace JsonRpcNet
{
    public enum JsonRpcWebSocketState: ushort
    {
        /// <summary>
        /// Equivalent to numeric value 0. Indicates that the connection hasn't yet been established.
        /// </summary>
        Connecting,
        /// <summary>
        /// Equivalent to numeric value 1. Indicates that the connection has been established,
        /// and the communication is possible.
        /// </summary>
        Open,
        /// <summary>
        /// Equivalent to numeric value 2. Indicates that the connection is going through
        /// the closing handshake, or the <c>WebSocket.Close</c> method has been invoked.
        /// </summary>
        Closing,
        /// <summary>
        /// Equivalent to numeric value 3. Indicates that the connection has been closed or
        /// couldn't be established.
        /// </summary>
        Closed,
    }
}