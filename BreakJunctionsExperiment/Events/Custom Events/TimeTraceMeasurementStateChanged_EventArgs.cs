using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class TimeTraceMeasurementStateChanged_EventArgs : EventArgs
    {
         /// <summary>
        /// Is true, if the measurement started
        /// and false, if the measurement stopped
        /// </summary>
        private bool _TimeTrace_MeasurementState = false;
        public bool TimeTrace_MeasurementState
        {
            get { return _TimeTrace_MeasurementState; }
            set { _TimeTrace_MeasurementState = value; }
        }

        public TimeTraceMeasurementStateChanged_EventArgs(bool _TimeTrace_measurementState) 
            : base() 
        {
            _TimeTrace_MeasurementState = _TimeTrace_measurementState;
        }
    }
}
