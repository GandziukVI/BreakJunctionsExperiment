using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class Motion_EventArgs : EventArgs
    {
        private double _Position;
        public double Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        public Motion_EventArgs(double position)
            : base()
        {
            _Position = position;
        }
    }
}
