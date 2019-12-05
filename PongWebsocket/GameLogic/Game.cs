using PongWebsocket.GameLogic;
using PongWebsocket.GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongWebsocket.GameLogic
{
    public class Game
    {
        private KeyValuePair<int, double> _firstPlayer = new KeyValuePair<int, double>();
        private KeyValuePair<int, double> _secondPlayer = new KeyValuePair<int, double>();

        public Ball Ball { get; set; }
        public Pad FirstPad { get; set; }
        public Pad SecondPad { get; set; }
        public static string Score { get; set; }

        public void SavePosition(double padX, int playerId)
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

        public void InitializeData()
        {
            Ball = new Ball(0.5, 0.5, 0.1, (32 / 30) * Math.PI);
            FirstPad = new Pad(_firstPlayer.Value, 0.9, 0.4, 0.03);
            SecondPad = new Pad(_secondPlayer.Value, 0.1, 0.4, 0.03);
            Score = "0:0";
        }

        public GameState GetActaulState()
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
