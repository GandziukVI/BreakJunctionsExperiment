using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Measurements
{
    class RealTime_Agilent_U2542A_TimeTrace_Controller_Factory : IRealTime_TimeTrace_Factory
    {
        private RealTime_Agilent_U2542A_TimeTrace_Controller _RealTime_TimeTrace_Controller;

        public RealTime_TimeTrace_Controller GetRealTime_TimeTraceController()
        {
            return _RealTime_TimeTrace_Controller;
        }
    }
}
