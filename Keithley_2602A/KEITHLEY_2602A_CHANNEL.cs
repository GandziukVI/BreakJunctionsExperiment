﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Devices.SMU;
using Devices;

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

        public KEITHLEY_2602A_CHANNEL(ref IExperimentalDevice __TheDevice, Channels _Channel)
        {
            _Style = NumberStyles.Float;
            _Culture = CultureInfo.InvariantCulture;

            _SelectedChannel = _Channel;

            KEITHLEY_2602A.Instance.SetDevice(ref __TheDevice);
            InitDevice();
        }

        public bool InitDevice()
        {
            try
            {
                KEITHLEY_2602A.Instance.EnableBeeper();
                return true;
            }
            catch { return false; }
        }

        public void SwitchON()
        {
            KEITHLEY_2602A.Instance.SwitchChannelState(_SelectedChannel, Channel_Status.Channel_ON);
        }

        public void SwitchOFF()
        {
            KEITHLEY_2602A.Instance.SwitchChannelState(_SelectedChannel, Channel_Status.Channel_OFF);
        }

        public bool SetVoltageLimit(double Value)
        {
            try
            {
                KEITHLEY_2602A.Instance.SetSourceLimit(Value, LimitMode.Voltage, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetCurrentLimit(double Value)
        {
            try
            {
                KEITHLEY_2602A.Instance.SetSourceLimit(Value, LimitMode.Current, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceVoltage(double Value)
        {
            try
            {
                KEITHLEY_2602A.Instance.SetValueToChannel(Value, SourceMode.Voltage, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceCurrent(double Value)
        {
            try
            {
                KEITHLEY_2602A.Instance.SetValueToChannel(Value, SourceMode.Current, _SelectedChannel);
                return true;
            }
            catch { return false; }
        }

        public double MeasureVoltage(int NumberOfAverages, double TimeDelay)
        {
            double MeasuredVoltage;

            var MeasuredVoltageString = KEITHLEY_2602A.Instance.MeasureIV_ValueInChannel(_SelectedChannel, MeasureMode.Voltage, NumberOfAverages, TimeDelay).Trim("\r\n".ToCharArray());
            var isSucceed = double.TryParse(MeasuredVoltageString, _Style, _Culture, out MeasuredVoltage);

            if (isSucceed)
                return MeasuredVoltage;
            else return double.NaN;
        }

        public double MeasureCurrent(int NumberOfAverages, double TimeDelay)
        {
            double MeasuredCurrent;

            var MeasuredCurrentString = KEITHLEY_2602A.Instance.MeasureIV_ValueInChannel(_SelectedChannel, MeasureMode.Current, NumberOfAverages, TimeDelay).Trim("\r\n".ToCharArray());
            var isSucceed = double.TryParse(MeasuredCurrentString, _Style, _Culture, out MeasuredCurrent);

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
            var measuredRessitanceString = KEITHLEY_2602A.Instance.MeasureResistanceOrPowerValueInChannel(_SelectedChannel, _sourceMode, MeasureMode.Resistance, valueThroughTheStructure, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(measuredRessitanceString, _Style, _Culture, out measuredResistance);

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
            var measuredPowerString = KEITHLEY_2602A.Instance.MeasureResistanceOrPowerValueInChannel(_SelectedChannel, _sourceMode, MeasureMode.Power, valueThroughTheStructure, NumberOfAverages, TimeDelay).TrimEnd('\n');
            var isSucceed = double.TryParse(measuredPowerString, _Style, _Culture, out measuredPower);

            if (isSucceed)
                return measuredPower;
            else return double.NaN;
        }
    }
}
