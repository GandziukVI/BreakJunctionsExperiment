using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakJunctions.Events
{
    class LastNoiseSpectra_Channel_01_DataArrived_EventArgs : EventArgs
    {
        public List<Point> SpectraData { get; set; }

        public LastNoiseSpectra_Channel_01_DataArrived_EventArgs(List<Point> __SpectraData)
            : base()
        {
            SpectraData = __SpectraData;
        }
    }
}
