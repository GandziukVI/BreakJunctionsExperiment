using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Ivi.Driver.Interop;
using Keithley.Ke26XXA.Interop;

using Devices.SMU;

namespace SMU.KEITHLEY_2602A
{
    public class GPIB_KEITHLEY_2602A_CHANNEL : I_SMU
    {
        private NumberStyles style;
        private CultureInfo culture;

        private Channels _SelectedChannel;
        public Channels SelectedChannel
        {
            get { return _SelectedChannel; }
            set { _SelectedChannel = value; }
        }

        private IKe26XXA _Driver;

        public GPIB_KEITHLEY_2602A_CHANNEL(byte _PrimaryAddress, byte _SecondaryAddress, byte _BoardNumber, Channels _Channel)
        {
            _Driver = new Ke26XXA();
            //Initialization
            if (_Driver.Initialized)
                _Driver.Close();
            var options = "QueryInstrStatus=True";
            _Driver.Initialize(String.Format("GPIB::{0}::INSTR", _PrimaryAddress), true, true, options);

            style = NumberStyles.Float;
            culture = CultureInfo.CreateSpecificCulture("en-US");

            _SelectedChannel = _Channel;
        }

        public bool InitDevice()
        {

            try
            {
                _Driver.System.DirectIO.WriteString("beeper.enable = 1 ");
                return true;
            }
            catch { return false; }
        }

        private double _FastestSpeed = 0.001;
        private double _LowestSpeed = 25.0;
        public void SetSpeed(double Speed, Channels SelectedChannel)
        {
            var Command = "smu{0}.measure.nplc = {1} ";
            var _Speed = Speed.ToString().Replace(',', '.');

            if (Speed < _FastestSpeed) Speed = _FastestSpeed;
            else if (Speed > _LowestSpeed) Speed = _LowestSpeed;

            switch (SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _Driver.System.DirectIO.WriteString(String.Format(Command, "a", _Speed));
                    } break;
                case Channels.ChannelB:
                    {
                        _Driver.System.DirectIO.WriteString(String.Format(Command, "b", _Speed));
                    } break;
                default:
                    break;
            }
        }

        public void SwitchON()
        {
            _Driver.Utility.Reset();
            switch (_SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _Driver.Source.set_OutputEnabled("A", true);
                    } break;
                case Channels.ChannelB:
                    {
                        _Driver.Source.set_OutputEnabled("B", true);
                    } break;
            }
        }

        public void SwitchOFF()
        {
            _Driver.Utility.Reset();
            switch (_SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _Driver.Source.set_OutputEnabled("A", false);
                    } break;
                case Channels.ChannelB:
                    {
                        _Driver.Source.set_OutputEnabled("B", false);
                    } break;
            }
        }

        public bool SetVoltageLimit(double Value)
        {
            try
            {
                switch (_SelectedChannel)
                {
                    case Channels.ChannelA:
                        {
                            _Driver.Source.Voltage.set_Limit("A", Value);
                        } break;
                    case Channels.ChannelB:
                        {
                            _Driver.Source.Voltage.set_Limit("B", Value);
                        } break;
                }
                return true;
            }
            catch { return false; }
        }

        public bool SetCurrentLimit(double Value)
        {
            try
            {
                switch (_SelectedChannel)
                {
                    case Channels.ChannelA:
                        {
                            _Driver.Source.Current.set_Limit("A", Value);
                        } break;
                    case Channels.ChannelB:
                        {
                            _Driver.Source.Current.set_Limit("B", Value);
                        } break;
                }
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceVoltage(double Value)
        {
            try
            {
                switch (_SelectedChannel)
                {
                    case Channels.ChannelA:
                        {
                            _Driver.Source.Voltage.set_Level("A", Value);
                        } break;
                    case Channels.ChannelB:
                        {
                            _Driver.Source.Voltage.set_Level("B", Value);
                        } break;
                }
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceCurrent(double Value)
        {
            try
            {
                switch (_SelectedChannel)
                {
                    case Channels.ChannelA:
                        {
                            _Driver.Source.Current.set_Level("A", Value);
                        } break;
                    case Channels.ChannelB:
                        {
                            _Driver.Source.Current.set_Level("B", Value);
                        } break;
                }
                return true;
            }
            catch { return false; }
        }

        public double MeasureVoltage(int NumberOfAverages, double TimeDelay)
        {
            var bufName = "MyBuf";
            switch (_SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _Driver.Measurement.Voltage.set_AutoRangeEnabled("A", true);
                        _Driver.Measurement.set_Count("A", NumberOfAverages);
                        _Driver.Measurement.set_Delay("A", TimeDelay);
                        _Driver.Measurement.Buffer.Create("A", bufName, NumberOfAverages);
                        _Driver.Measurement.Voltage.MeasureMultiple("A", bufName);
                    } break;
                case Channels.ChannelB:
                    {
                        _Driver.Measurement.Voltage.set_AutoRangeEnabled("B", true);
                        _Driver.Measurement.set_Count("B", NumberOfAverages);
                        _Driver.Measurement.set_Delay("B", TimeDelay);
                        _Driver.Measurement.Buffer.Create("B", bufName, NumberOfAverages);
                        _Driver.Measurement.Voltage.MeasureMultiple("B", bufName);
                    } break;
            }

            return _Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufName).Average();
        }

        public double MeasureCurrent(int NumberOfAverages, double TimeDelay)
        {
            var bufName = "MyBuf";
            switch (_SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _Driver.Measurement.Current.set_AutoRangeEnabled("A", true);
                        _Driver.Measurement.set_Count("A", NumberOfAverages);
                        _Driver.Measurement.set_Delay("A", TimeDelay);
                        _Driver.Measurement.Buffer.Create("A", bufName, NumberOfAverages);
                        _Driver.Measurement.Current.MeasureMultiple("A", bufName);
                    } break;
                case Channels.ChannelB:
                    {
                        _Driver.Measurement.Current.set_AutoRangeEnabled("B", true);
                        _Driver.Measurement.set_Count("B", NumberOfAverages);
                        _Driver.Measurement.set_Delay("B", TimeDelay);
                        _Driver.Measurement.Buffer.Create("B", bufName, NumberOfAverages);
                        _Driver.Measurement.Current.MeasureMultiple("B", bufName);
                    } break;
            }

            return _Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufName).Average();
        }


        public double MeasureResistance(double valueThroughTheStructure, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            var bufName = "MyBuf";
            switch (_SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _Driver.Measurement.set_Count("A", NumberOfAverages);
                        _Driver.Measurement.set_Delay("A", TimeDelay);
                        _Driver.Measurement.Buffer.Create("A", bufName, NumberOfAverages);
                        _Driver.Measurement.Resistance.MeasureMultiple("A", bufName);
                    } break;
                case Channels.ChannelB:
                    {
                        _Driver.Measurement.set_Count("B", NumberOfAverages);
                        _Driver.Measurement.set_Delay("B", TimeDelay);
                        _Driver.Measurement.Buffer.Create("B", bufName, NumberOfAverages);
                        _Driver.Measurement.Resistance.MeasureMultiple("B", bufName);
                    } break;
            }

            return _Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufName).Average();
        }

        public double MeasurePower(double valueThroughTheStructure, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            var bufName = "MyBuf";
            switch (_SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _Driver.Measurement.set_Count("A", NumberOfAverages);
                        _Driver.Measurement.set_Delay("A", TimeDelay);
                        _Driver.Measurement.Buffer.Create("A", bufName, NumberOfAverages);
                        _Driver.Measurement.Power.MeasureMultiple("A", bufName);
                    } break;
                case Channels.ChannelB:
                    {
                        _Driver.Measurement.set_Count("B", NumberOfAverages);
                        _Driver.Measurement.set_Delay("B", TimeDelay);
                        _Driver.Measurement.Buffer.Create("B", bufName, NumberOfAverages);
                        _Driver.Measurement.Power.MeasureMultiple("B", bufName);
                    } break;
            }

            return _Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufName).Average();
        }
    }
}
