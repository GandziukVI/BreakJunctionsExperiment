using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakJunctions.Events
{
    class NoiseSpectra_DataArrived_Channel_02_EventArgs : EventArgs
    {
        public List<Point> SpectraData { get; set; }

        public NoiseSpectra_DataArrived_Channel_02_EventArgs(List<Point> __SpectraData)
            : base()
        {
            SpectraData = __SpectraData;
        }
    }
}
