using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class TimeTraceBothChannelsPointsReceived_EventArgs : EventArgs
    {
        public double CH_01_Conductance { get; private set; }
        public double CH_02_Conductance { get; private set; }

        public TimeTraceBothChannelsPointsReceived_EventArgs(double ch_01_Conductance, double ch_02_Conductance)
            : base() 
        {
            CH_01_Conductance = ch_01_Conductance;
            CH_02_Conductance = ch_02_Conductance;
        }
    }
}
