using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devices;

namespace HP_35670A
{
    class GPIB_HP_35670A : GPIB_Device
    {
        public GPIB_HP_35670A(byte primaryAddress, byte secondaryAddress, byte boardNumber)
            : base(primaryAddress, secondaryAddress, boardNumber)
        { }
    }
}
