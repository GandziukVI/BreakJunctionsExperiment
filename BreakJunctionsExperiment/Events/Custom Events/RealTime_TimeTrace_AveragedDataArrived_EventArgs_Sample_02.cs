using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Events
{
    class RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02 : EventArgs
    {
        private double _Averaged_RealTime_TimeTrace_Data;
        public double Averaged_RealTime_TimeTrace_Data
        {
            get { return _Averaged_RealTime_TimeTrace_Data; }
            set { _Averaged_RealTime_TimeTrace_Data = value; }
        }
        public RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02(double AveragedData)
            : base()
        {
            this._Averaged_RealTime_TimeTrace_Data = AveragedData;
        }
    }
}
