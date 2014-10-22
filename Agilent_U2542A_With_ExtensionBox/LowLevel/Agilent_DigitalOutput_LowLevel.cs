using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Agilent_U2542A_With_ExtensionBox.Classes
{
    public class Agilent_DigitalOutput_LowLevel
    {
        private byte[] _bitmask,
                       _byte501,_byte502,_byte503,_byte504,
                       _byte501_pinNumbers,_byte502_pinNumbers,_byte503_pinNumbers,_byte504_pinNumbers;
        
        private List<byte[]> _bytes;

        private static Agilent_DigitalOutput_LowLevel _instance;
        public static Agilent_DigitalOutput_LowLevel Instance
        {
            get
            {
                if (_instance == null) _instance = new Agilent_DigitalOutput_LowLevel();
                return _instance;
            }
        }

        private Agilent_DigitalOutput_LowLevel()
        {
            if (!Agilent_USB_DAQ.Instance.isAlive())
                Agilent_USB_DAQ.Instance.Open();
            if (!Agilent_USB_DAQ.Instance.isAlive()) 
                throw new Exception("Device Not Connected");

            _bitmask = new byte[8] { 1, 2, 4, 8, 16, 32, 64, 128 };
            _byte501 = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            _byte502 = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            _byte503 = new byte[4] { 0, 0, 0, 0 };
            _byte504 = new byte[4] { 0, 0, 0, 0 };
            _byte501_pinNumbers = new byte[8] { 68,34,67,33,66,32,65,31};
            _byte502_pinNumbers = new byte[8] { 59,25,58,24,57,23,56,22};
            _byte503_pinNumbers = new byte[4] { 64,30,63,29 };
            _byte504_pinNumbers = new byte[4] { 61,27,60,26 };
            _bytes = new List<byte[]> { _byte501_pinNumbers, _byte502_pinNumbers, _byte503_pinNumbers, _byte504_pinNumbers };
            
            Agilent_USB_DAQ.Instance.WriteString("CONF:DIG:DIR OUTP,(@501:504)");
        }
        
        private void parseNumberToBitMask(ref byte[] byteX,byte number)
        {
            for (byte i = 0; i < byteX.Length; i++)
            {
                if ((this._bitmask[i] & number) == this._bitmask[i])
                    byteX[i] = 1;
                else
                    byteX[i] = 0;
            }
        }

        private byte byteToNumber(byte[] byteX)
        {
            byte result = 0;
            int i = 0;
            foreach (byte bit in byteX)
            {
                result = (byte)(result | (bit*_bitmask[i]));
                i++;
            }
            return result;
        }

        private void writeByteToDAQ(byte[] byteX,int byteNumber)
        {
            byte WhatToWrite = byteToNumber(byteX);
            try
            {
                Agilent_USB_DAQ.Instance.WriteString(String.Format("SOUR:DIG:DATA {0},(@{1})", WhatToWrite, byteNumber));
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        public void AllToZero()
        {
            try { Agilent_USB_DAQ.Instance.WriteString("SOUR:DIG:DATA 256, (@501:504)"); }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private void SetToOneOrZeroByPinNumber(byte pinNumber, byte value)
        {
            int IndexOfPinNumber = Array.IndexOf(_byte501_pinNumbers, pinNumber);
            if (IndexOfPinNumber != -1)
            {
                _byte501[IndexOfPinNumber] = value;
                this.writeByteToDAQ(_byte501, 501);
                return;
            }
            IndexOfPinNumber = Array.IndexOf(_byte502_pinNumbers, pinNumber);
            if (IndexOfPinNumber != -1)
            {
                _byte502[IndexOfPinNumber] = value;
                this.writeByteToDAQ(_byte502, 502);
                return;
            }
            IndexOfPinNumber = Array.IndexOf(_byte503_pinNumbers, pinNumber);
            if (Array.IndexOf(_byte503_pinNumbers, pinNumber) != -1)
            {
                _byte503[IndexOfPinNumber] = value;
                this.writeByteToDAQ(_byte503, 503);
                return;
            }
            IndexOfPinNumber = Array.IndexOf(_byte504_pinNumbers, pinNumber);
            if (Array.IndexOf(_byte504_pinNumbers, pinNumber) != -1)
            {
                _byte504[IndexOfPinNumber] = value;
                this.writeByteToDAQ(_byte504, 504);
                return;
            }
        }
        private void setToOneByPinNumber(byte pinNumber)
        {
            this.SetToOneOrZeroByPinNumber(pinNumber, 1);
        }
        private void setToZeroByPinNumber(byte pinNumber)
        {
            this.SetToOneOrZeroByPinNumber(pinNumber, 0);
        }
        private void MakePulseByPinNumber(byte pinNumber)
        {
            this.setToOneByPinNumber(pinNumber);
            Thread.Sleep(2);
            this.setToZeroByPinNumber(pinNumber);
        }
        public void setToOne(int ByteNumber, byte bitNumber)
        {
            setToOneByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
        }
        public void setToZero(int ByteNumber, byte bitNumber)
        {
            setToZeroByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
        }
        public void BitPulse(int ByteNumber, byte bitNumber)
        {
            setToOneByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
            Thread.Sleep(100);
            setToZeroByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
        }

        public void BitRelayPulse(int ByteNumber, byte bitNumber)
        {
            setToOne(ByteNumber,bitNumber);
            Thread.Sleep(100);
            setToZero(ByteNumber,bitNumber);
        }

        private byte readByte(int byteNumber/*501,502,503,504*/)
        {
            try
            {
                return Convert.ToByte(Agilent_USB_DAQ.Instance.QueryString(String.Format("SOURce:DIGital:DATA? (@{0})", byteNumber)));
            }
            catch (Exception e) { MessageBox.Show(e.Message); return 0; }
        }

        public int getValue(int ByteNumber, byte bitNumber)
        {
            bool value = getValueByPinNumber(_bytes[ByteNumber - 501][bitNumber]);
            if (value) return 1;
            return 0;
        }
        public bool getValueByPinNumber(byte pinNumber)
        {
            int IndexOfPinNumber = Array.IndexOf(_byte501_pinNumbers, pinNumber);
            if (IndexOfPinNumber != -1)
            {
                this.parseNumberToBitMask(ref _byte501, this.readByte(501));
                return Convert.ToBoolean(_byte501[Array.IndexOf(_byte501_pinNumbers, pinNumber)]);
            }
            IndexOfPinNumber = Array.IndexOf(_byte502_pinNumbers, pinNumber);
            if (IndexOfPinNumber != -1)
            {
                this.parseNumberToBitMask(ref _byte502, this.readByte(502));
                return Convert.ToBoolean(_byte502[Array.IndexOf(_byte502_pinNumbers, pinNumber)]);
            }
            IndexOfPinNumber = Array.IndexOf(_byte503_pinNumbers, pinNumber);
            if (Array.IndexOf(_byte503_pinNumbers, pinNumber) != -1)
            {
                this.parseNumberToBitMask(ref _byte503, this.readByte(503));
                return Convert.ToBoolean(_byte503[Array.IndexOf(_byte503_pinNumbers, pinNumber)]);
            }
            IndexOfPinNumber = Array.IndexOf(_byte504_pinNumbers, pinNumber);
            if (Array.IndexOf(_byte504_pinNumbers, pinNumber) != -1)
            {
                this.parseNumberToBitMask(ref _byte504, this.readByte(504));
                return Convert.ToBoolean(_byte504[Array.IndexOf(_byte504_pinNumbers, pinNumber)]);
            }
            
            return true;
        }
    }
}
