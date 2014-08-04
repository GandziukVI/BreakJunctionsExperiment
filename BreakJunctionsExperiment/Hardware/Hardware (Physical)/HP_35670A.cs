using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NationalInstruments;
using NationalInstruments.NI4882;

namespace BreakJunctions.Hardware
{
    class HP_35670A
    {
        private byte _PrimaryAddress = 27;
        public byte PrimaryAddress
        {
            get { return _PrimaryAddress; }
            set { _PrimaryAddress = value; }
        }

        private byte _SecondaryAddress = 0;
        public byte SecondaryAddress
        {
            get { return _SecondaryAddress; }
            set { _SecondaryAddress = value; }
        }

        private byte _BoardNumber = 0;
        public byte BoardNumber
        {
            get { return _BoardNumber; }
            set { _BoardNumber = value; }
        }

        public HP_35670A()
        { }

        public void InitDevice()
        {

        }
    }
}
