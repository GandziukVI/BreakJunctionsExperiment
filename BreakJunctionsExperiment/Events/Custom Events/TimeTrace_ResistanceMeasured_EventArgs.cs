using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Events
{
    public class TimeTrace_ResistanceMeasured_EventArgs : EventArgs
    {
        public double ResistanceValue { get; set; }

        public TimeTrace_ResistanceMeasured_EventArgs(double R_Val)
            :base()
        {
            ResistanceValue = R_Val;
        }
    }
}
