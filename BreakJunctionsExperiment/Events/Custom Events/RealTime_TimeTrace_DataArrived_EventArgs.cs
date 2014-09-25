using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aids.Graphics;

namespace BreakJunctions.Events
{
    public class RealTime_TimeTrace_DataArrived_EventArgs : EventArgs
    {
        private List<PointD>[] _Data;
        public List<PointD>[] Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public RealTime_TimeTrace_DataArrived_EventArgs(List<PointD>[] data)
            : base()
        {
            _Data = data;
        }
    }
}
