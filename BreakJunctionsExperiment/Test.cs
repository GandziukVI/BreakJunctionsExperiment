using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BreakJunctions
{
    public static class Test
    {
        public static Keithley2602A.Keithley2602A Device = new Keithley2602A.Keithley2602A("GPIB0::26::INSTR");
    }
}
