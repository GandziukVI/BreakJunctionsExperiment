using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Events
{
    class NoiseMeasurement_StateChanged_EventArgs : EventArgs
    {
        public bool MeasurementIsInProgress { get; set; }

        public NoiseMeasurement_StateChanged_EventArgs(bool __MeasurementIsInProgress)
            : base()
        {
            MeasurementIsInProgress = __MeasurementIsInProgress;
        }
    }
}
