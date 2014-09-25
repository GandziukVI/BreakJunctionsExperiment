using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Events
{
    public class RealTime_TimeTraceMeasurementStateChanged_EventArgs : EventArgs
    {
        private bool _MeasurementInProcess;
        public bool MeasurementInProcess
        {
            get { return _MeasurementInProcess; }
            set { _MeasurementInProcess = value; }
        }

        public RealTime_TimeTraceMeasurementStateChanged_EventArgs(bool _IsMeasurementInProgress)
            : base()
        {
            _MeasurementInProcess = _IsMeasurementInProgress;
        }
    }
}
