using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Measurements
{
    class RT_Agilent_U2542A_TimeTrace_Controller_Factory : IRealTime_TimeTrace_Factory
    {
        private RT_Agilent_U2542A_TimeTrace_Controller _RealTime_TimeTrace_Controller;

        public RealTime_TimeTrace_Controller GetRealTime_TimeTraceController()
        {
            _RealTime_TimeTrace_Controller = new RT_Agilent_U2542A_TimeTrace_Controller();
            return _RealTime_TimeTrace_Controller;
        }
    }
}
