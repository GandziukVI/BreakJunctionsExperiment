using System;
using System.Collections.Generic;
using System.Text;

using Keithley.Ke26XXA.Interop;

namespace KeithleyInstruments
{
    public class Keithley2602A
    {
        private Ke26XXA _driver;
        public Ke26XXA Driver
        {
            get { return _driver; }
        }

        public string ResourceName { get; set; }

        public Keithley2602A(string VisaID)
        {
            _driver = new Ke26XXA();

            ResourceName = VisaID;
        }

        private Keithley2602AChannel _channelA;
        public Keithley2602AChannel ChannelA
        {
            get
            {
                if (_channelA == null)
                    _channelA = new Keithley2602AChannel(this, Keithley2602AChannels.ChannelA);

                return _channelA;
            }
        }

        private Keithley2602AChannel _channelB;
        public Keithley2602AChannel ChannelB
        {
            get
            {
                if (_channelB == null)
                    _channelB = new Keithley2602AChannel(this, Keithley2602AChannels.ChannelB);

                return _channelB;
            }
        }
    }
}
