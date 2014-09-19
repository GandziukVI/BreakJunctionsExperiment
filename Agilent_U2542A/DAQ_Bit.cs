﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A
{
    public class DAQ_Bit : IDisposable
    {
        #region DAQ_Bit settings

        private int _byteNumber, _bitNumber, _value;
        private Agilent_U2542A_DigitalOutput _DIO;

        #endregion

        #region Constructor

        public DAQ_Bit(int byteNumber, byte bitNumber, string deviceID)
        {
            if ((byteNumber < 505) && (byteNumber > 500)) _byteNumber = byteNumber;
            else throw new Exception("Wrong byte number" + byteNumber);

            if ((bitNumber < 8)) _bitNumber = bitNumber;
            else throw new Exception("Wrong byte number" + bitNumber);

            _DIO = new Agilent_U2542A_DigitalOutput(deviceID);
            _DIO.InitDevice();
        }

        public int value
        {
            get
            {
                value = _DIO.getValue(_byteNumber, (byte)_bitNumber);
                return value;
            }
            set
            {
                if (value == 1) _DIO.setToOne(_byteNumber, (byte)_bitNumber);
                if (value == 0) _DIO.setToZero(_byteNumber, (byte)_bitNumber);
                
                _value = value;
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

        #region Correctly disposing the instance

        public void Dispose()
        {
            _DIO.Dispose();
        }

        #endregion
    }
}