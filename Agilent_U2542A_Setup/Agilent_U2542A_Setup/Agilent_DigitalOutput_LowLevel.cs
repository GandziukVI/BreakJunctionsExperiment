using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ivi.Driver.Interop;
using Agilent.AgilentU254x.Interop;

using Agilent.TMFramework.InstrumentIO;
using System.Threading;

namespace Agilent_U2542A_Setup
{
    public class Agilent_DigitalOutput_LowLevel
    {
        #region Agilent_DigitalOutput_LowLevel settings

        private byte[] _bitmask,
                       _byte501, _byte502, _byte503, _byte504,
                       _byte501_pinNumbers, _byte502_pinNumbers, _byte503_pinNumbers, _byte504_pinNumbers;

        private List<byte[]> _bytes;

        private AgilentU254x _Driver;

        #endregion

        #region Singleton pattren implementation

        private static Agilent_DigitalOutput_LowLevel _Instance;
        public static Agilent_DigitalOutput_LowLevel Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new Agilent_DigitalOutput_LowLevel();

                return _Instance;
            }
        }

        #endregion

        #region Constructor

        private Agilent_DigitalOutput_LowLevel()
        {
            _Driver = AgilentU254xDriver.Instance.Driver;

            _bitmask = new byte[8] { 1, 2, 4, 8, 16, 32, 64, 128 };

            _byte501 = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            _byte502 = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            _byte503 = new byte[4] { 0, 0, 0, 0 };
            _byte504 = new byte[4] { 0, 0, 0, 0 };

            _byte501_pinNumbers = new byte[8] { 68, 34, 67, 33, 66, 32, 65, 31 };
            _byte502_pinNumbers = new byte[8] { 59, 25, 58, 24, 57, 23, 56, 22 };
            _byte503_pinNumbers = new byte[4] { 64, 30, 63, 29 };
            _byte504_pinNumbers = new byte[4] { 61, 27, 60, 26 };

            _bytes = new List<byte[]> { _byte501_pinNumbers, _byte502_pinNumbers, _byte503_pinNumbers, _byte504_pinNumbers };
        }

        #endregion

        #region Agilent_DigitalOutput_LowLevel functionality

        private void _ParseNumberToBitMask(ref byte[] byteX, byte number)
        {
            for (byte i = 0; i < byteX.Length; i++)
            {
                if ((this._bitmask[i] & number) == this._bitmask[i])
                    byteX[i] = 1;
                else
                    byteX[i] = 0;
            }
        }

        private byte _ByteToNumber(byte[] byteX)
        {
            byte result = 0;
            int i = 0;
            foreach (byte bit in byteX)
            {
                result = (byte)(result | (bit * _bitmask[i]));
                i++;
            }
            return result;
        }

        private string _FindChannelByByteNumber(int byteNumber)
        {
            var result = string.Empty;

            switch (byteNumber)
            {
                case 501:
                    {
                        result = _Driver.Digital.Channels.get_Name(1);
                    }break;
                case 502:
                    {
                        result = _Driver.Digital.Channels.get_Name(2);
                    } break;
                case 503:
                    {
                        result = _Driver.Digital.Channels.get_Name(3);
                    } break;
                case 504:
                    {
                        result = _Driver.Digital.Channels.get_Name(4);
                    } break;
                default:
                    break;
            }

            return result;
        }

        private void _WriteByteToDAQ(byte[] byteX, int byteNumber)
        {
            byte WhatToWrite = _ByteToNumber(byteX);
            try
            {
                _Driver.Digital.WriteByte(_FindChannelByByteNumber(byteNumber), WhatToWrite);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        public void AllToZero()
        {
            try 
            {
                _Driver.System.DirectIO.WriteString("SOUR:DIG:DATA 256, (@501:504)");
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private void _SetToOneOrZeroByPinNumber(byte pinNumber, byte value)
        {
            int IndexOfPinNumber = Array.IndexOf(_byte501_pinNumbers, pinNumber);

            if (IndexOfPinNumber != -1)
            {
                _byte501[IndexOfPinNumber] = value;
                this._WriteByteToDAQ(_byte501, 501);
                return;
            }

            IndexOfPinNumber = Array.IndexOf(_byte502_pinNumbers, pinNumber);

            if (IndexOfPinNumber != -1)
            {
                _byte502[IndexOfPinNumber] = value;
                this._WriteByteToDAQ(_byte502, 502);
                return;
            }

            IndexOfPinNumber = Array.IndexOf(_byte503_pinNumbers, pinNumber);

            if (Array.IndexOf(_byte503_pinNumbers, pinNumber) != -1)
            {
                _byte503[IndexOfPinNumber] = value;
                this._WriteByteToDAQ(_byte503, 503);
                return;
            }

            IndexOfPinNumber = Array.IndexOf(_byte504_pinNumbers, pinNumber);

            if (Array.IndexOf(_byte504_pinNumbers, pinNumber) != -1)
            {
                _byte504[IndexOfPinNumber] = value;
                this._WriteByteToDAQ(_byte504, 504);
                return;
            }
        }

        private void _SetToOneByPinNumber(byte pinNumber)
        {
            this._SetToOneOrZeroByPinNumber(pinNumber, 1);
        }

        private void _SetToZeroByPinNumber(byte pinNumber)
        {
            this._SetToOneOrZeroByPinNumber(pinNumber, 0);
        }

        private void MakePulseByPinNumber(byte pinNumber)
        {
            this._SetToOneByPinNumber(pinNumber);
            Thread.Sleep(2);
            this._SetToZeroByPinNumber(pinNumber);
        }

        public void SetToOne(int ByteNumber, byte bitNumber)
        {
            _SetToOneByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
        }

        public void SetToZero(int ByteNumber, byte bitNumber)
        {
            _SetToZeroByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
        }

        public void BitPulse(int ByteNumber, byte bitNumber)
        {
            _SetToOneByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
            Thread.Sleep(100);
            _SetToZeroByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
        }

        public void BitRelayPulse(int ByteNumber, byte bitNumber)
        {
            SetToOne(ByteNumber, bitNumber);
            Thread.Sleep(100);
            SetToZero(ByteNumber, bitNumber);
        }

        private byte readByte(int byteNumber/* 501, 502, 503, 504 */)
        {
            try
            {
                int result = 0;
                _Driver.Digital.ReadByte(_FindChannelByByteNumber(byteNumber), ref result);
                
                return Convert.ToByte(result);
            }
            catch (Exception e) { MessageBox.Show(e.Message); return 0; }
        }

        public int GetValue(int ByteNumber, byte bitNumber)
        {
            bool TheValue = GetValueByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
            if (TheValue == true) return 1;
            return 0;
        }

        public bool GetValueByPinNumber(byte pinNumber)
        {
            int IndexOfPinNumber = Array.IndexOf(_byte501_pinNumbers, pinNumber);

            if (IndexOfPinNumber != -1)
            {
                this._ParseNumberToBitMask(ref _byte501, this.readByte(501));
                return Convert.ToBoolean(_byte501[Array.IndexOf(_byte501_pinNumbers, pinNumber)]);
            }
            IndexOfPinNumber = Array.IndexOf(_byte502_pinNumbers, pinNumber);
            if (IndexOfPinNumber != -1)
            {
                this._ParseNumberToBitMask(ref _byte502, this.readByte(502));
                return Convert.ToBoolean(_byte502[Array.IndexOf(_byte502_pinNumbers, pinNumber)]);
            }
            IndexOfPinNumber = Array.IndexOf(_byte503_pinNumbers, pinNumber);
            if (Array.IndexOf(_byte503_pinNumbers, pinNumber) != -1)
            {
                this._ParseNumberToBitMask(ref _byte503, this.readByte(503));
                return Convert.ToBoolean(_byte503[Array.IndexOf(_byte503_pinNumbers, pinNumber)]);
            }
            IndexOfPinNumber = Array.IndexOf(_byte504_pinNumbers, pinNumber);
            if (Array.IndexOf(_byte504_pinNumbers, pinNumber) != -1)
            {
                this._ParseNumberToBitMask(ref _byte504, this.readByte(504));
                return Convert.ToBoolean(_byte504[Array.IndexOf(_byte504_pinNumbers, pinNumber)]);
            }

            return true;
        }

        #endregion
    }
}
