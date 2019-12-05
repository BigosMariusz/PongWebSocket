using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongWebsocket.GameLogic.Models
{
    public class GameState
    {
        public Dictionary<int, double> PlayersXPositions { get; set; }
        public double BallX { get; set; }
        public double BallY { get; set; }
        public double Speed { get; set; }
        public double Degree { get; set; }
        public string Score { get; set; }
    }
}
