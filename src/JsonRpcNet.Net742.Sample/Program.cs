using System;
using System.Threading;
using JsonRpcNet.WebSocketSharp.Extensions;
using WebSocketSharp.Server;

namespace JsonRpcNet.Net742.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer(5000);
            
           server.AddJsonRpcService(()  => new ChatJsonRpcWebSocketService());
           server.UseJsonRpcApi("help");
           
           server.Start();
           
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            server.Stop();
            
        }
    }
}