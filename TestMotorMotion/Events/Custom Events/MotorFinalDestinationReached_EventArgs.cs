using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class MotorFinalDestinationReached_EventArgs : EventArgs
    {
        private double _FinalDestination;
        public double FinalDestination
        {
            get { return _FinalDestination; }
            set { _FinalDestination = value; }
        }

        public MotorFinalDestinationReached_EventArgs(double finalDestination)
            : base()
        {
            _FinalDestination = finalDestination;
        }

    }
}
