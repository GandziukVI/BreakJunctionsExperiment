using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Devices.SMU;
using Devices;
using Keithley_2602A.DeviceConfiguration;

namespace SMU.KEITHLEY_2602A
{
    public class KEITHLEY_2602A_CHANNEL : I_SMU
    {
        private NumberStyles _Style;
        private CultureInfo _Culture;

        private Channels _SelectedChannel;
        public Channels SelectedChannel
        {
            get { return _SelectedChannel; }
            set { _SelectedChannel = value; }
        }

        private KEITHLEY_2602A _TheDevice;

        public KEITHLEY_2602A_CHANNEL(KEITHLEY_2602A __TheDevice, Channels _Channel)
        {
            _Style = NumberStyles.Float;
            _Culture = CultureInfo.InvariantCulture;

            _SelectedChannel = _Channel;

            _TheDevice = __TheDevice;

            _ChannelAccuracyParams = new AccuracyParams();

            InitDevice();
        }

        public bool InitDevice()
        {
            try
            {
                _TheDevice.EnableBeeper();
                return true;
            }
            catch { return false; }
        }

        public void SetSpeed(double Speed, Channels SelectedChannel)
        {
            _TheDevice.SetSpeed(Speed, SelectedChannel);
        }

        public void SwitchON()
        {
            _TheDevice.SwitchChannelState(_SelectedChannel, Channel_Status.Channel_ON);
        }

        public void SwitchOFF()
        {
            _TheDevice.SwitchChannelState(_SelectedChannel, Channel_Status.Channel_OFF);
        }

        public bool SetVoltageLimit(double Value)
        {
            try
            {
                _TheDevice.SetSourceLimit(Value, LimitMode.Voltage, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetCurrentLimit(double Value)
        {
            try
            {
                _TheDevice.SetSourceLimit(Value, LimitMode.Current, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceVoltage(double Value)
        {
            try
            {
                _TheDevice.SetValueToChannel(Value, SourceMode.Voltage, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceCurrent(double Value)
        {
            try
            {
                _TheDevice.SetValueToChannel(Value, SourceMode.Current, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        private AccuracyParams _ChannelAccuracyParams;
        public AccuracyParams ChannelAccuracyParams 
        {
            get
            {
                return _ChannelAccuracyParams;
            }
            set
            {
                _ChannelAccuracyParams = value;
            }
        }

        private RangeAccuracySet _CurrentRow;
        private void CheckValueAccuracy(double value)
        {
            if (ChannelAccuracyParams.RangeAccuracySet != null)
            {
                foreach (var row in ChannelAccuracyParams.RangeAccuracySet)
                {
                    var min = (new double[] { row.MinRangeLimit, row.MaxRangeLimit }).Min();
                    var max = (new double[] { row.MinRangeLimit, row.MaxRangeLimit }).Max();

                    if ((value >= min) && (value <= max))
                    {
                        if (row != _CurrentRow)
                        {
                            _CurrentRow = row;
                            SetSpeed(row.Accuracy, _SelectedChannel);
                            break;
                        }
                    }
                }
            }
        }

        public double MeasureVoltage(int NumberOfAverages, double TimeDelay)
        {
            double MeasuredVoltage;

            var MeasuredVoltageString = _TheDevice.MeasureIV_ValueInChannel(_SelectedChannel, MeasureMode.Voltage, NumberOfAverages, TimeDelay).Trim("\r\n".ToCharArray());
            var isSucceed = double.TryParse(MeasuredVoltageString, _Style, _Culture, out MeasuredVoltage);

            if (isSucceed)
            {
                CheckValueAccuracy(MeasuredVoltage);
                return MeasuredVoltage;
            }
            else return double.NaN;
        }

        public double MeasureCurrent(int NumberOfAverages, double TimeDelay)
        {
            double MeasuredCurrent;

            var MeasuredCurrentString = _TheDevice.MeasureIV_ValueInChannel(_SelectedChannel, MeasureMode.Current, NumberOfAverages, TimeDelay).Trim("\r\n".ToCharArray());
            var isSucceed = double.TryParse(MeasuredCurrentString, _Style, _Culture, out MeasuredCurrent);

            if (isSucceed)
            {
                CheckValueAccuracy(MeasuredCurrent);
                return MeasuredCurrent;
            }
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
            var measuredRessitanceString = _TheDevice.MeasureResistanceOrPowerValueInChannel(_SelectedChannel, _sourceMode, MeasureMode.Resistance, valueThroughTheStructure, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(measuredRessitanceString, _Style, _Culture, out measuredResistance);

            if (isSucceed)
            {
                CheckValueAccuracy(measuredResistance);
                return measuredResistance;
            }
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
            var measuredPowerString = _TheDevice.MeasureResistanceOrPowerValueInChannel(_SelectedChannel, _sourceMode, MeasureMode.Power, valueThroughTheStructure, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(measuredPowerString, _Style, _Culture, out measuredPower);

            if (isSucceed)
            {
                CheckValueAccuracy(measuredPower);
                return measuredPower;
            }
            else return double.NaN;
        }
    }
}
