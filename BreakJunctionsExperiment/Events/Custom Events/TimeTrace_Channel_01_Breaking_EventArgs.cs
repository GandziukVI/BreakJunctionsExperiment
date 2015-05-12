using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Events
{
    public class TimeTrace_Channel_01_Breaking_EventArgs : EventArgs
    {
        public double Distance { get; set; }

        public TimeTrace_Channel_01_Breaking_EventArgs(double __Distance)
            : base()
        {
            Distance = __Distance;
        }
    }
}
