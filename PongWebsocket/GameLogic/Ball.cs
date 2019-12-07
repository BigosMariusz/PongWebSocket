using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongWebsocket.GameLogic
{
    public class Ball
    {
        readonly double _axisX;
        readonly double _axisY;
        readonly double _speed;
        readonly double _degree;
        public double AxisX
        {
            get { return _axisX; }
        }
        public double AxisY
        {
            get { return _axisY; }
        }
        public double Speed
        {
            get { return _speed; }
        }
        public double Degree
        {
            get { return _degree; }
        }

        public Ball(double axisX, double axisY, double speed, double degree)
        {
            _axisX = axisX;
            _axisY = axisY;
            _speed = speed;
            _degree = degree;
        }

        Ball Translate(double axisX, double axisY)
        {
            return new Ball(
                _axisX + axisX,
                _axisY + axisY,
                _speed,
                _degree
            );
        }

        public Ball Move(double timeMillis, Pad firstPad, Pad secondPad)
        {
            var degreeSin = Math.Sin(_degree);
            var degreeCos = Math.Cos(_degree);
            var sx = degreeSin * _speed * (timeMillis / 1000);
            var sy = degreeCos * _speed * (timeMillis / 1000);

            return Translate(sx, sy)
                .TryBouncePad(firstPad)
                .TryBouncePad(secondPad)
                .TryBounceWalls()
                .TryMoveToArea(firstPad)
                .TryMoveToArea(secondPad)
                .TryChangeScore();
        }

        Ball TryChangeScore()
        {
            if (_axisY < 0)
            {
                var scoreNow = Game.Score.Split(':');
                Game.Score = $"{(Int32.Parse(scoreNow[0]) + 1).ToString()}:{scoreNow[1]}";

                return new Ball(0.5, 0.5, _speed + 0.1, (32 / 30) * Math.PI);
            }
            else if (_axisY > 1)
            {
                var scoreNow = Game.Score.Split(':');
                Game.Score = $"{scoreNow[0]}:{(Int32.Parse(scoreNow[1]) + 1).ToString()}";

                return new Ball(0.5, 0.5, _speed + 0.1, (32 / 30) * Math.PI);
            }

            return this;
        }
        Ball TryBouncePad(Pad pad)
        {
            if (_axisX >= pad.AxisX - pad.Width / 2 &&
                _axisX <= pad.AxisX + pad.Width / 2 &&
                _axisY >= pad.AxisY - pad.Height / 2 &&
                _axisY <= pad.AxisY + pad.Height / 2)
            {
                var padBounceSection = (_axisX - (pad.AxisX - pad.Width / 2)) / pad.Width;
                var degreeAdd = 0.0;
                var maxDegree = Math.PI / 16;
                if (padBounceSection < 0.5)
                {
                    degreeAdd = (0.5 - padBounceSection) / 0.5 * maxDegree;
                }
                else
                {
                    degreeAdd = -(padBounceSection - 0.5) / 0.5 * maxDegree;
                }
                if (_axisY < 0.5)
                {
                    degreeAdd *= -1;
                }

                return new Ball(_axisX, _axisY, _speed, Math.PI - _degree + degreeAdd);
            }

            return this;
        }

        Ball TryBounceWalls()
        {
            var newDegree = _degree;
            if (_axisX < 0 || _axisX > 1.0)
            {
                newDegree = 2 * Math.PI - _degree;
            }
            else
            {
                return this;
            }

            return new Ball(_axisX, _axisY, _speed, newDegree);
        }

        Ball TryMoveToArea(Pad pad)
        {
            if (_axisX <= 0)
            {
                return new Ball(0, _axisY, _speed, _degree);
            }
            else if (_axisX >= 1.0)
            {
                return new Ball(1.0, _axisY, _speed, _degree);
            }
            else if (_axisY <= 0)
            {
                return new Ball(_axisX, 0, _speed, _degree);
            }
            else if (_axisY >= 1.0)
            {
                return new Ball(_axisX, 1.0, _speed, _degree);
            }
            else if (_axisX >= pad.AxisX - pad.Width / 2 &&
              _axisX <= pad.AxisX + pad.Width / 2 &&
              _axisY >= pad.AxisY - pad.Height / 2 &&
              _axisY <= pad.AxisY + pad.Height / 2)
            {
                if (_axisY < 0.5)
                {
                    return new Ball(_axisX, pad.AxisY + pad.Height / 2, _speed, _degree);
                }
                else
                {
                    return new Ball(_axisX, pad.AxisY - pad.Height / 2, _speed, _degree);
                }
            }

            return this;
        }
    }

}
