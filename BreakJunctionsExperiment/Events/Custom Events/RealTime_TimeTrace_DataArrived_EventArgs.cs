using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BreakJunctions.Events
{
    public class RealTime_TimeTrace_DataArrived_EventArgs : EventArgs
    {
        private LinkedList<Point>[] _Data;
        public LinkedList<Point>[] Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public RealTime_TimeTrace_DataArrived_EventArgs(ref LinkedList<Point>[] data)
            : base()
        {
            _Data = data;
        }
    }
}
