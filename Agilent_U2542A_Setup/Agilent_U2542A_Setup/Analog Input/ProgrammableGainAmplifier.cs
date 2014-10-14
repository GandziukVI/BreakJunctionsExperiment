using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A_Setup
{
    public class ProgrammableGainAmplifier
    {
        #region ProgrammableGainAmplifier settings

        private DAQ_Bit PGA_Bit0, PGA_Bit1;

        private int _Gain;
        public int Gain
        {
            get { return _Gain; }
            set
            {
                if (!ImportantConstants.ProgrammableAmplifierGains.Contains(value)) throw new Exception("Wrong Amplification passed to PGA" + value);
                _Gain = value;
                switch (value)
                {
                    case 1: { PGA_Bit0.TheValue = 0; PGA_Bit1.TheValue = 0; break; }
                    case 10: { PGA_Bit0.TheValue = 1; PGA_Bit1.TheValue = 0; break; }
                    case 100: { PGA_Bit0.TheValue = 0; PGA_Bit1.TheValue = 1; break; }
                }
            }
        }

        #endregion

        #region Constructor

        public ProgrammableGainAmplifier()
        {
            PGA_Bit0 = new DAQ_Bit(503, 0);
            PGA_Bit1 = new DAQ_Bit(503, 1);
        }

        #endregion
    }
}
