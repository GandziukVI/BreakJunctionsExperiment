using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Measurements
{
    class RT_Controller_Factory : IRealTime_TimeTrace_Factory
    {
        private RT_Controller _Controller;

        public RealTime_TimeTrace_Controller GetRealTime_TimeTraceController()
        {
            _Controller = new RT_Controller();
            return _Controller;
        }
    }
}
