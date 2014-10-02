using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;

namespace Agilent_U2542A_ExtensionBox
{
    public class AnalogInputChannel_Latch
    {
        #region AnalogInputChannel_Latch settings

        private DAQ_Bit Channel_Selector_A0, Channel_Selector_A1;
        private DAQ_Bit Relay_Pulse, Relay_SetReset;
        private DAQ_Bit Latch_Enabled;
        private int _ChannelNumber;
        private ProgrammableGainAmplifier _PGA;
        private ProgrammableFilter _Filter;
        private bool _isAC;
        public bool HomemadeAmplifier;

        public int PGA_Gain
        {
            get { return _PGA.Gain; }
            set { _PGA.Gain = value; }
        }

        public int Filter_Gain
        {
            get { return _Filter.Gain; }
            set { _Filter.Gain = value; }       
        }

        public int Filter_Frequency
        {
            get { return _Filter.Frequency; }
            set { _Filter.Frequency = value; }
        }

        public bool isAC
        {
            get { return _isAC; }
            set
            {
                if (!value) 
                    this.SetDC_Mode();
                else
                    this.SetAC_Mode();
            }
        }

        #endregion

        #region Constructor

        public AnalogInputChannel_Latch(int ChannelNumber)
        {
            if ((ChannelNumber < 1) && (ChannelNumber > 4))
                throw new Exception("incorrect channel number" + ChannelNumber);

            _ChannelNumber = ChannelNumber;

            Channel_Selector_A0 = new DAQ_Bit(502, 0);
            Channel_Selector_A1 = new DAQ_Bit(502, 1);

            Relay_Pulse = new DAQ_Bit(504, 0);
            Relay_SetReset = new DAQ_Bit(504, 1);

            Latch_Enabled = new DAQ_Bit(504, 2);

            _PGA = new ProgrammableGainAmplifier();
            _Filter = new ProgrammableFilter();
        }

        #endregion

        #region AnalogInputChannel_Latch functionality

        public void SaveValuesToLatch()
        {
            SelectChannel();
            Latch_Enabled.Pulse();
        }

        public void SaveValuesToLatch(int Frequency, int FilterGain, int PGA_Gain, bool homemadeAmplifier)
        {
            this._PGA.Gain = PGA_Gain;
            this._Filter.Gain = FilterGain;
            this._Filter.Frequency = Frequency;
            this.HomemadeAmplifier = homemadeAmplifier;
            SelectChannel();
            Latch_Enabled.Pulse();
        }

        private void SelectChannel()
        {
            switch (_ChannelNumber)
            {
                case 1: Channel_Selector_A0.value = 0; Channel_Selector_A1.value = 0; break;
                case 2: Channel_Selector_A0.value = 1; Channel_Selector_A1.value = 0; break;
                case 3: Channel_Selector_A0.value = 0; Channel_Selector_A1.value = 1; break;
                case 4: Channel_Selector_A0.value = 1; Channel_Selector_A1.value = 1; break;
            }
        }

        public void SetDC_Mode()
        {
            SelectChannel();
            Relay_SetReset.value = 1;
            Relay_Pulse.longPulse();
            _isAC = false;
        }

        public void SetAC_Mode()
        {
            SelectChannel();
            Relay_SetReset.value = 0;
            Relay_Pulse.longPulse();
            _isAC = true;
        }

        #endregion
    }
}
