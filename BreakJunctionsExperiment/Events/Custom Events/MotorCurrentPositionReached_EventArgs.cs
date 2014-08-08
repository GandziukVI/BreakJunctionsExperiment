using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class MotorCurrentPositionReached_EventArgs : EventArgs
    {
        private double _CurrentPosition;
        public double CurrentPosition
        {
            get { return _CurrentPosition; }
            set { CurrentPosition = value; }
        }

        public MotorCurrentPositionReached_EventArgs(double currentPosition)
            : base()
        {
            _CurrentPosition = currentPosition;
        }
    }
}
