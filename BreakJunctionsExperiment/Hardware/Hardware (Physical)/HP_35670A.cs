using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NationalInstruments;
using NationalInstruments.NI4882;
using System.Windows;

namespace Hardware
{
    class GPIB_HP_35670A : GPIB_Device
    {
        public GPIB_HP_35670A(byte primaryAddress, byte secondaryAddress, byte boardNumber)
            : base(primaryAddress, secondaryAddress, boardNumber) { }
    }
}
