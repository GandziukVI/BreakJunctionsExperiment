using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Measurements
{
    public interface IRealTime_TimeTrace_Factory
    {
        RealTime_TimeTrace_Controller GetRealTime_TimeTraceController();
    }
}
