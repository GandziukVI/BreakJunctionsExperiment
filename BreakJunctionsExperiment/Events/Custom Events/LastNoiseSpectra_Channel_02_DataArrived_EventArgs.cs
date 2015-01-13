using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakJunctions.Events
{
    class LastNoiseSpectra_Channel_02_DataArrived_EventArgs : EventArgs
    {
        private List<Point> _SpectraData;
        public List<Point> SpectraData 
        {
            get { return _SpectraData; }
            set { _SpectraData = value; }
        }

        public LastNoiseSpectra_Channel_02_DataArrived_EventArgs(List<Point> __SpectraData)
            : base()
        {
            _SpectraData = __SpectraData;
        }
    }
}
