using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Devices.SMU;

namespace SMU.KEITHLEY_2602A
{
    public class GPIB_KEITHLEY_2602A_CHANNEL : GPIB_KEITHLEY_2602A, I_SMU
    {
        private NumberStyles style;
        private CultureInfo culture;

        private Channels _SelectedChannel;
        public Channels SelectedChannel
        {
            get { return _SelectedChannel; }
            set { _SelectedChannel = value; }
        }

        public GPIB_KEITHLEY_2602A_CHANNEL(byte _PrimaryAddress, byte _SecondaryAddress, byte _BoardNumber, Channels _Channel)
            : base(_PrimaryAddress, _SecondaryAddress, _BoardNumber)
        {
            style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign;
            culture = CultureInfo.CreateSpecificCulture("en-US");

            _SelectedChannel = _Channel;
        }

        public void SwitchON()
        {
            SwitchChannelState(_SelectedChannel, Channel_Status.Channel_ON);
        }

        public void SwitchOFF()
        {
            SwitchChannelState(_SelectedChannel, Channel_Status.Channel_OFF);
        }

        public bool SetVoltageLimit(double Value)
        {
            try
            {
                SetSourceLimit(Value, LimitMode.Voltage, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetCurrentLimit(double Value)
        {
            try
            {
                SetSourceLimit(Value, LimitMode.Current, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceVoltage(double Value)
        {
            try
            {
                SetValueToChannel(Value, SourceMode.Voltage, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceCurrent(double Value)
        {
            try
            {
                SetValueToChannel(Value, SourceMode.Current, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public double MeasureVoltage(int NumberOfAverages, double TimeDelay)
        {
            double MeasuredVoltage;

            var MeasuredVoltageString = MeasureIV_ValueInChannel(_SelectedChannel, MeasureMode.Voltage, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(MeasuredVoltageString, style, culture, out MeasuredVoltage);

            if (isSucceed)
                return MeasuredVoltage;
            else return double.NaN;
        }

        public double MeasureCurrent(int NumberOfAverages, double TimeDelay)
        {
            double MeasuredCurrent;

            var MeasuredCurrentString = MeasureIV_ValueInChannel(_SelectedChannel, MeasureMode.Current, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(MeasuredCurrentString, style, culture, out MeasuredCurrent);

            if (isSucceed)
                return MeasuredCurrent;
            else return double.NaN;
        }


        public double MeasureResistance(double valueThroughTheStructure, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            double measuredResistance;
            SourceMode _sourceMode = SourceMode.Voltage;

            switch (sourceMode)
            {
                case SourceMode.Voltage:
                    {
                        _sourceMode = SourceMode.Voltage;
                    } break;
                case SourceMode.Current:
                    { 
                        _sourceMode = SourceMode.Current;
                    } break;
                default:
                    break;
            }
            var measuredRessitanceString = MeasureResistanceOrPowerValueInChannel(_SelectedChannel, _sourceMode, MeasureMode.Resistance, valueThroughTheStructure, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(measuredRessitanceString, style, culture, out measuredResistance);

            if (isSucceed)
                return measuredResistance;
            else return double.NaN;
        }

        public double MeasurePower(double valueThroughTheStructure, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            double measuredPower;
            SourceMode _sourceMode = SourceMode.Voltage;

            switch (sourceMode)
            {
                case SourceMode.Voltage:
                    {
                        _sourceMode = SourceMode.Voltage;
                    } break;
                case SourceMode.Current:
                    {
                        _sourceMode = SourceMode.Current;
                    } break;
                default:
                    break;
            }
            var measuredPowerString = MeasureResistanceOrPowerValueInChannel(_SelectedChannel, _sourceMode, MeasureMode.Power, valueThroughTheStructure, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(measuredPowerString, style, culture, out measuredPower);

            if (isSucceed)
                return measuredPower;
            else return double.NaN;
        }
    }
}
