using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class TimeTraceBothChannelsPointsReceived_EventArgs : EventArgs
    {
        public double CH_01_Val { get; set; }
        public double CH_02_Val { get; set; }

        public TimeTraceBothChannelsPointsReceived_EventArgs(double CH_01_Reading, double CH_02_Reading)
            : base() 
        {
            CH_01_Val = CH_01_Reading;
            CH_02_Val = CH_02_Reading;
        }
    }
}
