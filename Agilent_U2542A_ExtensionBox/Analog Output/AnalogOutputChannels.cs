using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;

namespace Agilent_U2542A_ExtensionBox
{
    class AnalogOutputChannels
    {
        #region AnalogOutputChannels settings

        public AnalogOutputChannel[] Channels;
        public DAQ_Bit AO_Channels_LatchEnable;

        #endregion

        #region Constructor

        private AnalogOutputChannels()
        {
            Channels = new AnalogOutputChannel[] { new AnalogOutputChannel(201), new AnalogOutputChannel(202) };
            AO_Channels_LatchEnable = new DAQ_Bit(504, 3);
        }

        #endregion

        #region AnalogOutputChannels functionality

        public void ReloadChannelsToLatch()
        {
            Channels[0].OutputNumber = Channels[0].OutputNumber;
            Channels[1].OutputNumber = Channels[1].OutputNumber;
            AO_Channels_LatchEnable.Pulse();
        }

        public void SetDC_Output_To_Certain_Channel(int OutputNumber, double DCVoltage)
        {

            if ((OutputNumber <= 8) && (OutputNumber >= 1))
            {
                Channels[1].OutputNumber = OutputNumber;
                Channels[0].OutputNumber = Channels[0].OutputNumber;
                AO_Channels_LatchEnable.Pulse();
                Channels[1].Enabled = true;
                Channels[1].DCVoltage = DCVoltage;
                Channels[1].SineOut = false;
            }
            else
            {
                if ((OutputNumber <= 16) && (OutputNumber >= 9))
                {
                    Channels[0].OutputNumber = OutputNumber;
                    Channels[1].OutputNumber = Channels[1].OutputNumber;
                    AO_Channels_LatchEnable.Pulse();
                    Channels[0].Enabled = true;
                    Channels[0].DCVoltage = DCVoltage;
                    Channels[0].SineOut = false;
                }

            }
        }

        public void SetAC_Output_To_Certain_Channel(int OutputNumber, double AC_Amplitude, int Freq, double offset = 0)
        {

            if ((OutputNumber <= 8) && (OutputNumber >= 1))
            {
                Channels[1].DCOffset = offset;
                Channels[1].AC_Frequency = Freq;
                Channels[1].OutputNumber = OutputNumber;
                ReloadChannelsToLatch();
                Channels[1].AC_Amplitude = AC_Amplitude;
                Channels[1].SineOut = false;
                Channels[1].SineOut = true;

            }
            else
            {
                if ((OutputNumber <= 16) && (OutputNumber >= 9))
                {
                    Channels[0].DCOffset = offset;
                    Channels[0].AC_Frequency = Freq;
                    Channels[0].OutputNumber = OutputNumber;
                    ReloadChannelsToLatch();
                    Channels[0].AC_Amplitude = AC_Amplitude;
                    Channels[0].SineOut = false;
                    Channels[0].SineOut = true;

                }

            }
        }

        public void SilenceChannel(int OutputNumber)
        {
            if ((OutputNumber <= 8) && (OutputNumber >= 1))
            {
                Channels[1].SineOut = false;
                Channels[1].Enabled = false;
            }
            else
            {
                if ((OutputNumber <= 16) && (OutputNumber >= 9))
                {
                    Channels[0].SineOut = false;
                    Channels[0].Enabled = false;
                }
            }
        }

        #endregion
    }
}
