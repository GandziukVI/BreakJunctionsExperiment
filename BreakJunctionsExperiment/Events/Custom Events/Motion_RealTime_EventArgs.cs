using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Events
{
    class Motion_RealTime_EventArgs : EventArgs
    {
        private double _Time;
        public double Time
        {
            get { return _Time; }
        }

        private double _Position;
        public double Position
        {
            get { return _Position; }
        }

        public Motion_RealTime_EventArgs (double MotionTime, double MotionPosition)
            : base()
        {
            _Time = MotionTime;
            _Position = MotionPosition;
        }
    }
}
