using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakJunctions.Events
{
    class NoiseSpectra_DataArrived_Channel_01_EventArgs : EventArgs
    {
        private List<Point> _SpectraData;
        public List<Point> SpectraData 
        {
            get { return _SpectraData; }
            set { _SpectraData = value; }
        }

        public NoiseSpectra_DataArrived_Channel_01_EventArgs(List<Point> __SpectraData)
            : base()
        {
            _SpectraData = __SpectraData;
        }
    }
}
