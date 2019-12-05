using PongWebsocket.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace LastTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new WebSocketServer(4649);
            server.AddWebSocketService<GameHandler>("/Game");

            server.Start();
            if (server.IsListening)
            {
                Console.WriteLine($"Listening on port {server.Port}, and providing WebSocket endpoint /Game");
                Console.WriteLine($"Address ws://localhost:{server.Port}/Game");
            }

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();

            server.Stop();
        }
    }
}
