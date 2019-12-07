using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using PongWebsocket.WebSockets.Models;
using Newtonsoft.Json;
using PongWebsocket.GameLogic;

namespace PongWebsocket.WebSockets
{
    public class GameHandler : WebSocketBehavior
    {
        private double _communicationInterval = 100;

        protected override void OnMessage(MessageEventArgs e)
        {
            var dataReceived = JsonConvert.DeserializeObject<DataReceived>(e.Data);

            if (Game.Clients.Count == 0)
            {
                Game.Clients.Add(dataReceived.Id, Context.WebSocket);
                Game.InitialSavePosition(dataReceived.PadX, dataReceived.Id);
            }
            else if (Game.Clients.ContainsKey(dataReceived.Id))
            {
                Game.SavePosition(dataReceived.PadX, dataReceived.Id);
            }
            else
            {
                Game.Clients.Add(dataReceived.Id, Context.WebSocket);
                Game.SavePosition(dataReceived.PadX, dataReceived.Id);

                Game.InitializeData();
                StartSendingGameData();
            }
        }

        private void StartSendingGameData()
        {
            TimerHelper.StartTimer(_communicationInterval);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            TimerHelper.StopTimer();
            var key = Game.Clients.Where(x => x.Value == Context.WebSocket).FirstOrDefault().Key;
            Game.Clients.Remove(key);
            Console.WriteLine("Close");
        }

    }
}
