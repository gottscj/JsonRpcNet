using System;
using JsonRpcNet.Docs;
using JsonRpcNet.WebSocketSharp.Extensions;
using WebSocketSharp.Server;

namespace JsonRpcNet.Net742.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5000;
            var server = new HttpServer(port);

            server.AddJsonRpcService(() => new ChatJsonRpcWebSocketService());
            server.UseJsonRpcApi(new JsonRpcInfo
            {
                Description = "Api for JsonRpc chat",
                Title = "Chat API",
                Version = "v1",
                Contact = new JsonRpcContact
                {
                    Name = "The Dude",
                    Email = "the@dude.com",
                    Url = "http://www.thedude.com"
                }
            });

            server.Start();

            Console.WriteLine($"Now listening on: http://localhost:{port}");
            Console.ReadLine();
            server.Stop();
        }
    }
}