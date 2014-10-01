using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;

namespace Agilent_U2542A_ExtensionBox
{
    class ProgrammableFilter
    {
        #region ProgrammableFilter settings

        private byte[] _bitmask = new byte[] { 1, 2, 4, 8 };

        private DAQ_Bit FilterGain_Bit0, FilterGain_Bit1, FilterGain_Bit2, FilterGain_Bit3;
        private DAQ_Bit Frequency_Bit0, Frequency_Bit1, Frequency_Bit2, Frequency_Bit3;
        private DAQ_Bit HOLD_CS;
        private DAQ_Bit[] FrequencyBits, GainBits;

        private int _Frequency;
        public int Frequency
        {
            get { return _Frequency; }
            set
            {
                if (!ImportantConstants.CutOffFrequencies.Contains(value)) throw new Exception("Wrong Frequency passed to filter" + value);
                _Frequency = value;
                value = Array.IndexOf(ImportantConstants.CutOffFrequencies, value);
                for (int i = 0; i < _bitmask.Length; i++)
                {
                    if (((value & _bitmask[i]) == _bitmask[i]))
                        FrequencyBits[i].value = 1;
                    else
                        FrequencyBits[i].value = 0;
                }

            }
        }
        
        private int _Gain;
        public int Gain
        {
            get { return _Gain; }
            set
            {
                if ((value > 16) || (value < 1)) throw new Exception("Wring gain passed to filter" + value);
                _Gain = value;
                value--;
                for (int i = 0; i < _bitmask.Length; i++)
                {
                    if (((value & _bitmask[i]) == _bitmask[i]))
                        GainBits[i].value = 1;
                    else
                        GainBits[i].value = 0;
                }
            }            
        }

        #endregion

        #region Constructor / Destructor

        public ProgrammableFilter()
        {
            FilterGain_Bit0 = new DAQ_Bit(501, 4);
            FilterGain_Bit1 = new DAQ_Bit(501, 5);
            FilterGain_Bit2 = new DAQ_Bit(501, 6);
            FilterGain_Bit3 = new DAQ_Bit(501, 7);

            Frequency_Bit0 = new DAQ_Bit(501, 0);
            Frequency_Bit1 = new DAQ_Bit(501, 1);
            Frequency_Bit2 = new DAQ_Bit(501, 2);
            Frequency_Bit3 = new DAQ_Bit(501, 3);

            FrequencyBits = new DAQ_Bit[] { Frequency_Bit0, Frequency_Bit1, Frequency_Bit2, Frequency_Bit3 };
            GainBits = new DAQ_Bit[] { FilterGain_Bit0, FilterGain_Bit1, FilterGain_Bit2, FilterGain_Bit3 };
            
            HOLD_CS = new DAQ_Bit(503, 2);
            HOLD_CS.value = 0;
        }

        #endregion
    }
}
