using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMU.Keithley_2400
{
    public class Keithley_2400:GPIB_Device
    {
        public Keithley_2400(byte PrimaryAddress,byte SecondaryAddress, byte BoardNumber):base(PrimaryAddress,SecondaryAddress,BoardNumber)
        {
            
        }

    }
}
