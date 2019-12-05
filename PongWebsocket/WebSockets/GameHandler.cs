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
        private double _communicationInterval = 33;
        private Dictionary<int, WebSocket> _clients = new Dictionary<int, WebSocket>();
        private Game _game = new Game();

        protected override void OnMessage(MessageEventArgs e)
        {
            var dataReceived = JsonConvert.DeserializeObject<DataReceived>(e.Data);

            if (_clients.Count == 0)
            {
                _clients.Add(dataReceived.Id, Context.WebSocket);
                _game.SavePosition(dataReceived.PadX, dataReceived.Id);
            }
            else if (_clients.ContainsKey(dataReceived.Id))
            {
                _game.SavePosition(dataReceived.PadX, dataReceived.Id);
            }
            else
            {
                _clients.Add(dataReceived.Id, Context.WebSocket);
                _game.SavePosition(dataReceived.PadX, dataReceived.Id);

                _game.InitializeData();
                StartSendingGameData();
            }
        }

        private void StartSendingGameData()
        {
            Timer t = new Timer();
            t.Interval = _communicationInterval;
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(TimerElapsed);
            t.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var gameState = _game.GetActaulState();

            foreach (var client in _clients)
            {
                var success = gameState.PlayersXPositions.TryGetValue(client.Key, out double opponent);
                if (!success)
                    return;

                var dataToSend = new DataToSend()
                {
                    Opponent = opponent,
                    BallX = gameState.BallX,
                    BallY = gameState.BallY,
                    Speed = gameState.Speed,
                    Degree = gameState.Degree,
                    Score = gameState.Score
                };

                var json = JsonConvert.SerializeObject(dataToSend);
                client.Value.Send(json);
            }
        }
        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Close");
        }

    }
}
