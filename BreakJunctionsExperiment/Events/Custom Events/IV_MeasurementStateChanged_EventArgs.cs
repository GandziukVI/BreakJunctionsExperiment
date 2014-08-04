using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class IV_MeasurementStateChanged_EventArgs : EventArgs
    {
        /// <summary>
        /// Is true, if the measurement started
        /// and false, if the measurement stopped
        /// </summary>
        private bool _IV_MeasurementState = false;
        public bool IV_MeasurementState
        {
            get { return _IV_MeasurementState; }
            set { _IV_MeasurementState = value; }
        }

        public IV_MeasurementStateChanged_EventArgs(bool _IV_measurementState) 
            : base() 
        {
            _IV_MeasurementState = _IV_measurementState;
        }
    }
}
