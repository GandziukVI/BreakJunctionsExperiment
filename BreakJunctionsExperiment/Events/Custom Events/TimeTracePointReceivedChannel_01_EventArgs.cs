using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    public class TimeTracePointReceivedChannel_01_EventArgs : EventArgs
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

        public TimeTracePointReceivedChannel_01_EventArgs(double X_Val, double Y_Val)
            : base()
        {
            _X = X_Val;
            _Y = Y_Val;
        }
    }
}
