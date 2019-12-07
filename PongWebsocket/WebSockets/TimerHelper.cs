using Newtonsoft.Json;
using PongWebsocket.GameLogic;
using PongWebsocket.WebSockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebSocketSharp.Server;

namespace PongWebsocket.WebSockets
{
    public class TimerHelper : WebSocketBehavior
    {
        private static Timer _timer = null;

        public static void StartTimer(double interval)
        {
            _timer = new Timer();
            _timer.Interval = interval;
            _timer.AutoReset = true;
            _timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            _timer.Start();
        }

        public static void StopTimer()
        {
            _timer.Stop();
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var gameState = Game.GetActaulState();

            foreach (var client in Game.Clients)
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
    }
}
