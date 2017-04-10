using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    /// <summary>
    /// One IV point measured from 1-st channel event
    /// </summary>
    public class IV_PointReceivedChannel_02_EventArgs : EventArgs
    {
        private double _X;
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }
        private double _Y;
        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        public IV_PointReceivedChannel_02_EventArgs(double X_Val, double Y_Val)
            : base()
        {
            _X = X_Val;
            _Y = Y_Val;
        }
    }
}
