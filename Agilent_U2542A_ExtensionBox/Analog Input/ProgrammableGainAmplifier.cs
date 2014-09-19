using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;

namespace Agilent_U2542A_ExtensionBox
{
    public class ProgrammableGainAmplifier : IDisposable
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
                    case 1: { PGA_Bit0.value = 0; PGA_Bit1.value = 0; break; }
                    case 10: { PGA_Bit0.value = 1; PGA_Bit1.value = 0; break; }
                    case 100: { PGA_Bit0.value = 0; PGA_Bit1.value = 1; break; }
                }
            }
        }

        #endregion

        #region Constructor / Destructor

        public ProgrammableGainAmplifier()
        {
            PGA_Bit0 = new DAQ_Bit(503, 0, ImportantConstants.DeviceID);
            PGA_Bit1 = new DAQ_Bit(503, 1, ImportantConstants.DeviceID);
        }

        ~ProgrammableGainAmplifier()
        {
            this.Dispose();
        }

        #endregion

        #region Correctly disposing the instance

        public void Dispose()
        {
            PGA_Bit0.Dispose();
            PGA_Bit1.Dispose();
        }

        #endregion
    }
}
