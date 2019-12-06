using PongWebsocket.GameLogic;
using PongWebsocket.GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace PongWebsocket.GameLogic
{
    public static class Game
    {
        private static KeyValuePair<int, double> _firstPlayer = new KeyValuePair<int, double>();
        private static KeyValuePair<int, double> _secondPlayer = new KeyValuePair<int, double>();

        public static Ball Ball { get; set; }
        public static Pad FirstPad { get; set; }
        public static Pad SecondPad { get; set; }
        public static string Score { get; set; }
        public static Dictionary<int, WebSocket> Clients { get; set; } = new Dictionary<int, WebSocket>();

        public static void InitialSavePosition(double padX, int playerId)
        {
            _firstPlayer = new KeyValuePair<int, double>(playerId, padX);
        }

        public static void SavePosition(double padX, int playerId)
        {
            if (_firstPlayer.Key == playerId)
            {
                _firstPlayer = new KeyValuePair<int, double>(playerId, padX);
            }
            else
            {
                _secondPlayer = new KeyValuePair<int, double>(playerId, padX);
            }
        }

        public static void InitializeData()
        {
            Ball = new Ball(0.5, 0.5, 0.1, (32 / 30) * Math.PI);
            FirstPad = new Pad(_firstPlayer.Value, 0.9, 0.4, 0.03);
            SecondPad = new Pad(_secondPlayer.Value, 0.1, 0.4, 0.03);
            Score = "0:0";
        }

        public static GameState GetActaulState()
        {
            double time = 1000;

            FirstPad = FirstPad.Translate(_firstPlayer.Value);
            SecondPad = SecondPad.Translate(_secondPlayer.Value);
            Ball = Ball.Move(time, FirstPad, SecondPad);

            var players = new Dictionary<int, double>()
            {
                { _firstPlayer.Key, _firstPlayer.Value },
                { _secondPlayer.Key, _secondPlayer.Value }
            };

            return new GameState()
            {
                BallX = Ball.AxisX,
                BallY = Ball.AxisY,
                Degree = Ball.Degree,
                Speed = Ball.Speed,
                PlayersXPositions = players,
                Score = Score
            };
        }
    }
}
