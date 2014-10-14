using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ivi.Driver.Interop;
using Agilent.AgilentU254x.Interop;
using System.Threading;

namespace Agilent_U2542A_Setup
{
    public class DAQ_Bit
    {
        #region DAQ_Bit settings

        private int _byteNumber, _bitNumber;

        private Agilent_DigitalOutput_LowLevel _DIO;

        #endregion

        #region Constructor

        public DAQ_Bit(int byteNumber, byte bitNumber)
        {
            if ((byteNumber < 505) && (byteNumber > 500)) _byteNumber = byteNumber;
            else throw new Exception("Wrong byte number" + byteNumber);

            if ((bitNumber < 8)) _bitNumber = bitNumber;
            else throw new Exception("Wrong byte number" + bitNumber);

            _DIO = Agilent_DigitalOutput_LowLevel.Instance;
        }

        #endregion

        #region DAQ_Bit functioality

        private bool _boolean_value;
        private int _TheValue;
        public int TheValue
        {
            get
            {
                _TheValue = _DIO.GetValue(_byteNumber, (byte)_bitNumber);
                return _TheValue;
            }
            set
            {
                if (value == 1) _DIO.SetToOne(_byteNumber, (byte)_bitNumber);
                if (value == 0) _DIO.SetToZero(_byteNumber, (byte)_bitNumber);
                
                _TheValue = value;
            }
        }

        public void Pulse()
        {
            _DIO.BitPulse(_byteNumber, (byte)_bitNumber);
        }

        public void longPulse()
        {
            _DIO.BitRelayPulse(_byteNumber, (byte)_bitNumber);
        }

        #endregion
    }
}
