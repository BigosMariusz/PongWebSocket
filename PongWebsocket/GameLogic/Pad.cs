using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongWebsocket.GameLogic
{
    public class Pad
    {
        readonly double _axisX;
        readonly double _axisY;
        readonly double _width;
        readonly double _height;
        public double AxisX 
        {
            get { return _axisX; }
        }
        public double AxisY
        {
            get { return _axisY; }
        }
        public double Width
        {
            get { return _width; }
        }
        public double Height
        {
            get { return _height; }
        }


        public Pad(double axisX, double axisY, double width, double height)
        {
            _axisX = axisX;
            _axisY = axisY;
            _width = width;
            _height = height;
        }

        public Pad Translate(double axisX)
        {
            return new Pad(_axisX + axisX, _axisY, _width, _height);
        }
    }

}
