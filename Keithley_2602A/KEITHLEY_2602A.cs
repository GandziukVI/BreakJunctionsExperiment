using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Devices;
using Devices.SMU;
using System.Globalization;

/*     Realizes KEITHLY 2602A functionality     */

namespace SMU.KEITHLEY_2602A
{
    public class KEITHLEY_2602A
    {
        private IExperimentalDevice _TheDevice;

        public KEITHLEY_2602A(byte _PrimaryAddress, byte _SecondaryAddress, byte _BoardNumber)
        {
            var The_GPIB_Device = new GPIB_Device(_PrimaryAddress, _SecondaryAddress, _BoardNumber);
            _TheDevice = The_GPIB_Device;
        }

        private KEITHLEY_2602A() { }

        private static KEITHLEY_2602A _Instance;
        public static KEITHLEY_2602A Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new KEITHLEY_2602A();

                return _Instance;
            }
        }

        public void SetDevice(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;
        }

        /*     Realizing advanced device functionality     */

        #region Functionality

        public void EnableBeeper()
        {
            _TheDevice.SendCommandRequest("beeper.enable = 1 ");
        }

        private double _FastestSpeed = 0.001;
        private double _LowestSpeed = 25.0;
        public void SetSpeed(double Speed, Channels SelectedChannel)
        {
            var Command = "smu{0}.measure.nplc = {1} ";

            if (Speed < _FastestSpeed)
                Speed = _FastestSpeed;
            else if (Speed > _LowestSpeed)
                Speed = _LowestSpeed;

            switch (SelectedChannel)
            {
                case Channels.ChannelA:
                    {
                        _TheDevice.SendCommandRequest(String.Format(Command, "a", Speed.ToString(NumberFormatInfo.InvariantInfo)));
                    } break;
                case Channels.ChannelB:
                    {
                        _TheDevice.SendCommandRequest(String.Format(Command, "b", Speed.ToString(NumberFormatInfo.InvariantInfo)));
                    } break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Switching appropriate channel ON or OFF
        /// </summary>
        /// <param name="Channel">Channel to be swithed</param>
        /// <param name="Status">Status of the channel ON / OFF</param>
        public void SwitchChannelState(Channels Channel, Channel_Status Status)
        {
            var Command = 
                "beeper.beep(0.15, 2400) " + 
                "smu{0}.source.output = smu{0}.OUTPUT_STATUS ";

            switch (Status)
            {
                case Channel_Status.Channel_ON:
                    {
                        Command = Command.Replace("OUTPUT_STATUS", "OUTPUT_ON");
                    } break;
                case Channel_Status.Channel_OFF:
                    {
                        Command = Command.Replace("OUTPUT_STATUS", "OUTPUT_OFF");
                    } break;
            }

            switch (Channel)
            {
                case Channels.ChannelA:
                    {
                        var ExequtionRequest = String.Format(Command, "a");
                        _TheDevice.SendCommandRequest(ExequtionRequest);
                    } break;
                case Channels.ChannelB:
                    {
                        var ExequtionRequest = String.Format(Command, "b");
                        _TheDevice.SendCommandRequest(ExequtionRequest);
                    } break;
            }
        }

        /// <summary>
        /// Sets source mode of appropriate channel
        /// </summary>
        /// <param name="Channel">Channel</param>
        /// <param name="SourceMode">Source mode (voltage / current)</param>
        public void SetChannelSourceMode(Channels Channel, SourceMode SourceMode)
        {
            var Command = "smu{0}.source.autorange{2} = smu{0}.AUTORANGE_ON ";

            Command = Command.Insert(0, "smu{0}.source.func = smu{0}.{1} ");
            Command += "smu{0}.source.level{2} = 0 ";

            switch (SourceMode)
            {
                case SourceMode.Voltage:
                    {
                        switch (Channel)
                        {
                            case Channels.ChannelA:
                                {
                                    Command = String.Format(Command, "a", "OUTPUT_DCVOLTS", "v");
                                    _TheDevice.SendCommandRequest(Command);
                                } break;
                            case Channels.ChannelB:
                                {
                                    Command = String.Format(Command, "b", "OUTPUT_DCVOLTS", "v").ToString();
                                    _TheDevice.SendCommandRequest(Command);
                                } break;
                        }
                    } break;
                case SourceMode.Current:
                    {
                        switch (Channel)
                        {
                            case Channels.ChannelA:
                                {
                                    Command = String.Format(Command, "a", "OUTPUT_DCAMPS", "i");
                                    _TheDevice.SendCommandRequest(Command);
                                } break;
                            case Channels.ChannelB:
                                {
                                    Command = String.Format(Command, "b", "OUTPUT_DCAMPS", "i");
                                    _TheDevice.SendCommandRequest(Command);
                                } break;
                            default:
                                break;
                        }
                    } break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <param name="Script"></param>
        /// <param name="QueryResult"></param>
        private void ExecuteQuery(string Script, ref string QueryResult)
        {
            QueryResult = _TheDevice.RequestQuery(Script);
        }
           
        /// <summary>
        /// Measures the value in appropriate channel
        /// </summary>
        /// <param name="Channel">Channel</param>
        /// <param name="MeasureMode">Measure mode (voltage / current)</param>
        /// <param name="NumberOfAverages">Number of averages per one measure</param>
        /// <param name="TimeDelay">Time delay between two measurenments</param>
        /// <returns></returns>
        public string MeasureIV_ValueInChannel(Channels Channel, MeasureMode MeasureMode, int NumberOfAverages, double TimeDelay)
        {
            var _TimeDelay = TimeDelay.ToString().Replace(',', '.');
                
            var MeasuredValue = "";

            var IV_Script = 
                "loadscript MeasureValueInChannel\n" + 
                "smu{0}.measure.autorange{1} = smu{0}.AUTORANGE_ON\n" +
                "display.screen = display.{4}\n" +
                "display.smu{0}.measure.func = display.{5}\n" +
                "trigger.clear()\n" + 
                "result = 0.0\n" +
                "for parameterMeasure = 1, {2} do\n" +
                "trigger.wait({3})\n" + 
                "result = result + smu{0}.measure.{1}()\n" + 
                "end\n" + 
                "result = result / ({2} - 1)\n" + 
                "print (result)\n" + 
                "endscript\n" +
                "MeasureValueInChannel()\n";

            switch (MeasureMode)
            {
                case MeasureMode.Voltage:
                    {
                        switch (Channel)
                        {
                            case Channels.ChannelA:
                                {
                                    IV_Script = String.Format(IV_Script, "a", "v", NumberOfAverages, _TimeDelay, "SMUA_SMUB", "MEASURE_DCVOLTS");
                                    ExecuteQuery(IV_Script, ref MeasuredValue);
                                } break;
                            case Channels.ChannelB:
                                {
                                    IV_Script = String.Format(IV_Script, "b", "v", NumberOfAverages, _TimeDelay, "SMUA_SMUB", "MEASURE_DCVOLTS");
                                    ExecuteQuery(IV_Script, ref MeasuredValue);
                                } break;
                            default:
                                break;
                        }
                    } break;
                case MeasureMode.Current:
                    {
                        switch (Channel)
                        {
                            case Channels.ChannelA:
                                {
                                    IV_Script = String.Format(IV_Script, "a", "i", NumberOfAverages, _TimeDelay, "SMUA_SMUB", "MEASURE_DCAMPS");
                                    ExecuteQuery(IV_Script, ref MeasuredValue);
                                } break;
                            case Channels.ChannelB:
                                {
                                    IV_Script = String.Format(IV_Script, "b", "i", NumberOfAverages, _TimeDelay, "SMUA_SMUB", "MEASURE_DCAMPS");
                                    ExecuteQuery(IV_Script, ref MeasuredValue);
                                } break;
                        }
                    } break;
                case MeasureMode.Resistance:
                    {
                        throw new NotImplementedException();
                    }
                case MeasureMode.Power:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    break;
            }

            return MeasuredValue;
        }

        /// <summary>
        /// Measures the resistance or power in appropriate channel
        /// </summary>
        /// <param name="Channel">Channel</param>
        /// <param name="SourceMode">Source mode (voltage / current)</param>
        /// <param name="MeasureMode">Measure mode (resistance / power)</param>
        /// <param name="NumberOfAverages">Number of averages per one measure</param>
        /// <param name="TimeDelay">Time delay between two measurenments</param>
        /// <returns></returns>
        public string MeasureResistanceOrPowerValueInChannel(Channels Channel, SourceMode SourceMode, MeasureMode MeasureMode, double valueThroughTheStructure, int NumberOfAverages, double TimeDelay)
        {
            var _TimeDelay = TimeDelay.ToString(NumberFormatInfo.InvariantInfo);
            var _valueThroughTheStructure = valueThroughTheStructure.ToString(NumberFormatInfo.InvariantInfo);
            var _limiti = (1.0).ToString(NumberFormatInfo.InvariantInfo);
            var _limitv = (40.0).ToString(NumberFormatInfo.InvariantInfo);

            var MeasuredValue = "";

            var R_Script =
                "loadscript MeasureResistanceInChannel\n" +
                "smu{0}.source.func = smu{0}.{1}\n" + 
                "smu{0}.source.autorange{2} = smu{0}.AUTORANGE_ON\n" + 
                "smu{0}.source.level{2} = {3}\n" + 
                "smu{0}.source.limit{4} = {5}\n" + 
                "smu{0}.measure.autorange{4} = smu{0}.AUTORANGE_ON\n" +
                "display.screen = display.{6}\n" +
                "display.smu{0}.measure.func = display.{7}\n" +
                "trigger.clear()\n" +
                "result = 0.0\n" +
                "for parameterMeasure = 1, {8} do\n" +
                "trigger.wait({9})\n" +
                "result = result + smu{0}.measure.r()\n" +
                "end\n" +
                "result = result / {8}\n" +
                "print (result)\n" +
                "endscript\n" +
                "MeasureResistanceInChannel()\n";

            var P_Script =
                "loadscript MeasurePowerInChannel\n" +
                "smu{0}.measure.autorange{1} = smu{0}.AUTORANGE_ON\n" +
                "display.screen = display.{2}\n" +
                "display.smu{0}.measure.func = display.{3}\n" +
                "trigger.clear()\n" +
                "result = 0.0\n" +
                "for parameterMeasure = 1, {4} do\n" +
                "trigger.wait({5})\n" +
                "result = result + smu{0}.measure.p()\n" +
                "end\n" +
                "result = result / ({4} - 1)\n" +
                "print (result)\n" +
                "endscript\n" +
                "MeasurePowerInChannel()\n";

            switch (MeasureMode)
            {
                case MeasureMode.Voltage:
                    {
                        throw new NotImplementedException();
                    }
                case MeasureMode.Current:
                    {
                        throw new NotImplementedException();
                    }
                case MeasureMode.Resistance:
                    {
                        switch (Channel)
                        {
                            case Channels.ChannelA:
                                {
                                    switch (SourceMode)
                                    {
                                        case SourceMode.Voltage:
                                            {
                                                R_Script = String.Format(R_Script, "a", "OUTPUT_DCVOLTS", "v", _valueThroughTheStructure, "i", _limiti, "SMUA_SMUB", "MEASURE_OHMS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(R_Script, ref MeasuredValue);
                                            } break;
                                        case SourceMode.Current:
                                            {
                                                R_Script = String.Format(R_Script, "a", "OUTPUT_DCAMPS", "i", _valueThroughTheStructure, "v", _limitv, "SMUA_SMUB", "MEASURE_OHMS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(R_Script, ref MeasuredValue);
                                            } break;
                                    }
                                } break;
                            case Channels.ChannelB:
                                {
                                    switch (SourceMode)
                                    {
                                        case SourceMode.Voltage:
                                            {
                                                R_Script = String.Format(R_Script, "b", "OUTPUT_DCVOLTS", "v", _valueThroughTheStructure, "i", _limiti, "SMUA_SMUB", "MEASURE_OHMS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(R_Script, ref MeasuredValue);
                                            } break;
                                        case SourceMode.Current:
                                            {
                                                R_Script = String.Format(R_Script, "b", "OUTPUT_DCAMPS", "i", _valueThroughTheStructure, "v", _limitv, "SMUA_SMUB", "MEASURE_OHMS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(R_Script, ref MeasuredValue);
                                            } break;
                                    }
                                } break;
                        }
                    } break;
                case MeasureMode.Power:
                    {
                        switch (Channel)
                        {
                            case Channels.ChannelA:
                                {
                                    switch (SourceMode)
                                    {
                                        case SourceMode.Voltage:
                                            {
                                                P_Script = String.Format(P_Script, "a", "i", "SMUA_SMUB", "MEASURE_WATTS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(P_Script, ref MeasuredValue);
                                            } break;
                                        case SourceMode.Current:
                                            {
                                                P_Script = String.Format(P_Script, "a", "v", "SMUA_SMUB", "MEASURE_WATTS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(P_Script, ref MeasuredValue);
                                            } break;
                                    }
                                } break;
                            case Channels.ChannelB:
                                {
                                    switch (SourceMode)
                                    {
                                        case SourceMode.Voltage:
                                            {
                                                P_Script = String.Format(P_Script, "b", "i", "SMUA_SMUB", "MEASURE_WATTS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(P_Script, ref MeasuredValue);
                                            } break;
                                        case SourceMode.Current:
                                            {
                                                P_Script = String.Format(P_Script, "b", "v", "SMUA_SMUB", "MEASURE_WATTS", NumberOfAverages, _TimeDelay);
                                                ExecuteQuery(P_Script, ref MeasuredValue);
                                            } break;
                                    }
                                } break;
                        }
                    } break;
            }

            return MeasuredValue;
        }

        /// <summary>
        /// Sets value to the appropriate channel
        /// </summary>
        /// <param name="Value">Value to be set into device</param>
        /// <param name="SourceMode">Defines voltage or current value should be written to the device</param>
        /// <param name="Channel">Defines channel, on which the value is setted</param>
        public void SetValueToChannel(double Value, SourceMode SourceMode, Channels Channel)
        {
            SetChannelSourceMode(Channel, SourceMode);

            //Changing value to appropriate format

            var _Value = Value.ToString().Replace(',', '.');

            var script = "smu{0}.source.level{1} = {2} ";

            switch (Channel)
            {
                case Channels.ChannelA:
                    {
                        switch (SourceMode)
                        {
                            case SourceMode.Voltage:
                                {
                                    script = String.Format(script, "a", "v", _Value);
                                    _TheDevice.SendCommandRequest(script);
                                } break;
                            case SourceMode.Current:
                                {
                                    script = String.Format(script, "a", "i", _Value);
                                    _TheDevice.SendCommandRequest(script);
                                } break;
                        }
                    } break;
                case Channels.ChannelB:
                    {
                        switch (SourceMode)
                        {
                            case SourceMode.Voltage:
                                {
                                    script = String.Format(script, "b", "v", _Value);
                                    _TheDevice.SendCommandRequest(script);
                                } break;
                            case SourceMode.Current:
                                {
                                    script = String.Format(script, "b", "i", _Value);
                                    _TheDevice.SendCommandRequest(script);
                                } break;
                        }
                    } break;
            }
        }

        /// <summary>
        /// Sets sense SENSE_LOCAL (2-wire) or SENSE_REMOTE (4-wire)
        /// </summary>
        /// <param name="Channel">Channel</param>
        /// <param name="Sense">Sense</param>
        public void SetSence(Channels Channel, Sense Sense)
        {
            var SetSenseScript =
                "smu{0}.sense = smu{0}.{1}";

            switch (Channel)
            {
                case Channels.ChannelA:
                    {
                        switch (Sense)
                        {
                            case Sense.SENSE_LOCAL:
                                {
                                    SetSenseScript = String.Format(SetSenseScript, "a", "SENSE_LOCAL");
                                    _TheDevice.SendCommandRequest(SetSenseScript);
                                } break;
                            case Sense.SENSE_REMOTE:
                                {
                                    SetSenseScript = String.Format(SetSenseScript, "a", "SENSE_REMOTE");
                                    _TheDevice.SendCommandRequest(SetSenseScript);
                                } break;
                        }
                    } break;
                case Channels.ChannelB:
                    {
                        switch (Sense)
                        {
                            case Sense.SENSE_LOCAL:
                                {
                                    SetSenseScript = String.Format(SetSenseScript, "b", "SENSE_LOCAL");
                                    _TheDevice.SendCommandRequest(SetSenseScript);
                                } break;
                            case Sense.SENSE_REMOTE:
                                {
                                    SetSenseScript = String.Format(SetSenseScript, "b", "SENSE_REMOTE");
                                    _TheDevice.SendCommandRequest(SetSenseScript);
                                } break;
                        }
                    } break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets appropriate limits to defined channel
        /// </summary>
        /// <param name="LimitValue">Limit value</param>
        /// <param name="Mode">Source mode</param>
        /// <param name="Channel">Channel</param>
        public void SetSourceLimit(double LimitValue, LimitMode Mode, Channels Channel)
        {
            var SetLimitScript =
                "smu{0}.source.limit{1} = {2} ";

            var _LimitValue = LimitValue.ToString().Replace(',', '.');

            switch (Channel)
            {
                case Channels.ChannelA:
                    {
                        switch (Mode)
                        {
                            case LimitMode.Voltage:
                                {
                                    SetLimitScript = String.Format(SetLimitScript, "a", "v", _LimitValue);
                                    _TheDevice.SendCommandRequest(SetLimitScript);
                                } break;
                            case LimitMode.Current:
                                {
                                    SetLimitScript = String.Format(SetLimitScript, "a", "i", _LimitValue);
                                    _TheDevice.SendCommandRequest(SetLimitScript);
                                } break;
                        }
                    } break;
                case Channels.ChannelB:
                    {
                        switch (Mode)
                        {
                            case LimitMode.Voltage:
                                {
                                    SetLimitScript = String.Format(SetLimitScript, "b", "v", _LimitValue);
                                    _TheDevice.SendCommandRequest(SetLimitScript);
                                } break;
                            case LimitMode.Current:
                                {
                                    SetLimitScript = String.Format(SetLimitScript, "b", "i", _LimitValue);
                                    _TheDevice.SendCommandRequest(SetLimitScript);
                                } break;
                            default:
                                break;
                        }
                    } break;
                default:
                    break;
            }
        }

        #endregion
    }
}