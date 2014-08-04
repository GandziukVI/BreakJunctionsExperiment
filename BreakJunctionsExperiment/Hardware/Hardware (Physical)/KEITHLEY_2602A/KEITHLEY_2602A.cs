﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;

namespace Hardware
{
    /*     Realizes KEITHLY 2602A functionality     */

    namespace KEITHLEY_2602A
    {
        //KEITHLEY modes and parameters

        public enum KEITHLEY_2602A_Channels { ChannelA, ChannelB }
        public enum KEITHLEY_2602A_Channel_Status { Channel_ON, Channel_OFF }
        public enum KEITHLEY_2602A_Sense { SENSE_LOCAL, SENSE_REMOTE }
        public enum KEITHLEY_2601A_SourceMode { Voltage, Current}
        public enum KEITHLEY_2601A_MeasureMode { Voltage, Current, Resistance, Power }
        public enum KEITHLEY_2601A_LimitMode { Voltage, Current }

        public class GPIB_KEITHLEY_2602A : IExperimentalDevice
        {

            public GPIB_KEITHLEY_2602A(byte _PrimaryAddress, byte _SecondaryAddress, byte _BoardNumber)
            {
                this._primaryAddress = _PrimaryAddress;
                this._secondaryAddress = _SecondaryAddress;
                this._boardNumber = _BoardNumber;
                
                InitDevice();
            }

            //Addresses of device

            private byte _primaryAddress;
            public byte primaryAddress
            {
                get { return _primaryAddress; }
                set { _primaryAddress = value; }
            }
            private byte _secondaryAddress;
            public byte secondaryAddress
            {
                get { return _secondaryAddress; }
                set { _secondaryAddress = value; }
            }

            private byte _boardNumber;
            public byte boardNumber
            {
                get { return _boardNumber; }
                set { _boardNumber = value; }
            }

            //For controlling the device

            private NationalInstruments.NI4882.Address GPIB_KEITHLEY_2602A_Adderss;
            private NationalInstruments.NI4882.Device GPIB_KEITHLEY_2602A_Device;

            //Overriding basical device functionality

            /// <summary>
            /// Initializes the device
            /// </summary>
            /// <returns>Returns true, if initialization succeed</returns>
            public bool InitDevice()
            {
                try
                {
                    GPIB_KEITHLEY_2602A_Adderss = new NationalInstruments.NI4882.Address(_primaryAddress, _secondaryAddress);
                    GPIB_KEITHLEY_2602A_Device = new NationalInstruments.NI4882.Device(_boardNumber, GPIB_KEITHLEY_2602A_Adderss);

                    GPIB_KEITHLEY_2602A_Device.Clear();

                    GPIB_KEITHLEY_2602A_Device.Write("beeper.enable = 1 ");

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// Sends command request to the device
            /// </summary>
            /// <param name="RequestString">Command, to be sent to the device</param>
            /// <returns></returns>
            public bool SendCommandRequest(string RequestString)
            {
                try
                {
                    GPIB_KEITHLEY_2602A_Device.Write(RequestString);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            /// <summary>
            /// Receives the answer of the denvice
            /// </summary>
            /// <returns>Returns the ansver, if succeed else returns empty string</returns>
            public string ReceiveDeviceAnswer()
            {
                GPIB_KEITHLEY_2602A_Device.IOTimeout = NationalInstruments.NI4882.TimeoutValue.None;
                var GPIB_KEITHLEY_2602A_Responce = string.Empty;
                
                try
                {
                    GPIB_KEITHLEY_2602A_Responce = GPIB_KEITHLEY_2602A_Device.ReadString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    GPIB_KEITHLEY_2602A_Responce = string.Empty;
                }

                return GPIB_KEITHLEY_2602A_Responce;
            }

            /*     Realizing advanced device functionality     */

            //For constructing command requests

            private StringBuilder CommandBuilder;

            /// <summary>
            /// Switching appropriate channel ON or OFF
            /// </summary>
            /// <param name="Channel">Channel to be swithed</param>
            /// <param name="Status">Status of the channel ON / OFF</param>
            public void SwitchChannelState(KEITHLEY_2602A_Channels Channel, KEITHLEY_2602A_Channel_Status Status)
            {
                var Command = 
                    "beeper.beep(0.15, 2400) " + 
                    "smu{0}.source.output = smu{0}.OUTPUT_STATUS ";

                switch (Status)
                {
                    case KEITHLEY_2602A_Channel_Status.Channel_ON:
                        {
                            Command = Command.Replace("OUTPUT_STATUS", "OUTPUT_ON");
                        }
                        break;
                    case KEITHLEY_2602A_Channel_Status.Channel_OFF:
                        {
                            Command = Command.Replace("OUTPUT_STATUS", "OUTPUT_OFF");
                        }
                        break;
                    default:
                        break;
                }

                switch (Channel)
                {
                    case KEITHLEY_2602A_Channels.ChannelA:
                        {
                            CommandBuilder = new StringBuilder();
                            CommandBuilder.AppendFormat(Command, "a").ToString();

                            var ExequtionRequest = CommandBuilder.ToString();

                            SendCommandRequest(ExequtionRequest);
                        } break;
                    case KEITHLEY_2602A_Channels.ChannelB:
                        {
                            CommandBuilder = new StringBuilder();
                            CommandBuilder.AppendFormat(Command, "b").ToString();

                            var ExequtionRequest = CommandBuilder.ToString();

                            SendCommandRequest(ExequtionRequest);
                        } break;
                    default:
                        {
                            //Some default actions...
                        } break;
                }
            }

            /// <summary>
            /// Sets source mode of appropriate channel
            /// </summary>
            /// <param name="Channel">Channel</param>
            /// <param name="SourceMode">Source mode (voltage / current)</param>
            public void SetChannelSourceMode(KEITHLEY_2602A_Channels Channel, KEITHLEY_2601A_SourceMode SourceMode)
            {
                var Command = "smu{0}.source.autorange{2} = smu{0}.AUTORANGE_ON ";

                CommandBuilder = new StringBuilder();

                Command = Command.Insert(0, "smu{0}.source.func = smu{0}.{1} ");
                Command += "smu{0}.source.level{2} = 0 ";

                switch (SourceMode)
                {
                    case KEITHLEY_2601A_SourceMode.Voltage:
                        {
                            switch (Channel)
                            {
                                case KEITHLEY_2602A_Channels.ChannelA:
                                    {
                                        Command = CommandBuilder.AppendFormat(Command, "a", "OUTPUT_DCVOLTS", "v").ToString();
                                        SendCommandRequest(Command);
                                    } break;
                                case KEITHLEY_2602A_Channels.ChannelB:
                                    {
                                        Command = CommandBuilder.AppendFormat(Command, "b", "OUTPUT_DCVOLTS", "v").ToString();
                                        SendCommandRequest(Command);
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    case KEITHLEY_2601A_SourceMode.Current:
                        {
                            switch (Channel)
                            {
                                case KEITHLEY_2602A_Channels.ChannelA:
                                    {
                                        Command = CommandBuilder.AppendFormat(Command, "a", "OUTPUT_DCAMPS", "i").ToString();
                                        SendCommandRequest(Command);
                                    } break;
                                case KEITHLEY_2602A_Channels.ChannelB:
                                    {
                                        Command = CommandBuilder.AppendFormat(Command, "b", "OUTPUT_DCAMPS", "i").ToString();
                                        SendCommandRequest(Command);
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
                SendCommandRequest(Script);
                QueryResult = ReceiveDeviceAnswer();
            }
           
            /// <summary>
            /// Measures the value in appropriate channel
            /// </summary>
            /// <param name="Channel">Channel</param>
            /// <param name="MeasureMode">Measure mode (voltage / current)</param>
            /// <param name="NumberOfAverages">Number of averages per one measure</param>
            /// <param name="TimeDelay">Time delay between two measurenments</param>
            /// <returns></returns>
            public string MeasureIV_ValueInChannel(KEITHLEY_2602A_Channels Channel, KEITHLEY_2601A_MeasureMode MeasureMode, int NumberOfAverages, double TimeDelay)
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

                CommandBuilder = new StringBuilder();

                switch (MeasureMode)
                {
                    case KEITHLEY_2601A_MeasureMode.Voltage:
                        {
                            switch (Channel)
                            {
                                case KEITHLEY_2602A_Channels.ChannelA:
                                    {
                                        IV_Script = CommandBuilder.AppendFormat(IV_Script, "a", "v", NumberOfAverages, _TimeDelay, "SMUA", "MEASURE_DCVOLTS").ToString();
                                        ExecuteQuery(IV_Script, ref MeasuredValue);
                                    } break;
                                case KEITHLEY_2602A_Channels.ChannelB:
                                    {
                                        IV_Script = CommandBuilder.AppendFormat(IV_Script, "b", "v", NumberOfAverages, _TimeDelay, "SMUB", "MEASURE_DCVOLTS").ToString();
                                        ExecuteQuery(IV_Script, ref MeasuredValue);
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    case KEITHLEY_2601A_MeasureMode.Current:
                        {
                            switch (Channel)
                            {
                                case KEITHLEY_2602A_Channels.ChannelA:
                                    {
                                        IV_Script = CommandBuilder.AppendFormat(IV_Script, "a", "i", NumberOfAverages, _TimeDelay, "SMUA", "MEASURE_DCAMPS").ToString();
                                        ExecuteQuery(IV_Script, ref MeasuredValue);
                                    } break;
                                case KEITHLEY_2602A_Channels.ChannelB:
                                    {
                                        IV_Script = CommandBuilder.AppendFormat(IV_Script, "b", "i", NumberOfAverages, _TimeDelay, "SMUB", "MEASURE_DCAMPS").ToString();
                                        ExecuteQuery(IV_Script, ref MeasuredValue);
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    case KEITHLEY_2601A_MeasureMode.Resistance:
                        {
                            throw new NotImplementedException();
                        }
                    case KEITHLEY_2601A_MeasureMode.Power:
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
            public string MeasureResistanceOrPowerValueInChannel(KEITHLEY_2602A_Channels Channel, KEITHLEY_2601A_SourceMode SourceMode, KEITHLEY_2601A_MeasureMode MeasureMode, double valueThroughTheStructure, int NumberOfAverages, double TimeDelay)
            {
                var _TimeDelay = TimeDelay.ToString().Replace(',', '.');
                var _valueThroughTheStructure = valueThroughTheStructure.ToString().Replace(',', '.');
                var _limiti = (1.0).ToString().Replace(',', '.');
                var _limitv = (40.0).ToString().Replace(',', '.');

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

                CommandBuilder = new StringBuilder();

                switch (MeasureMode)
                {
                    case KEITHLEY_2601A_MeasureMode.Voltage:
                        {
                            throw new NotImplementedException();
                        }
                    case KEITHLEY_2601A_MeasureMode.Current:
                        {
                            throw new NotImplementedException();
                        }
                    case KEITHLEY_2601A_MeasureMode.Resistance:
                        {
                            switch (Channel)
                            {
                                case KEITHLEY_2602A_Channels.ChannelA:
                                    {
                                        switch (SourceMode)
                                        {
                                            case KEITHLEY_2601A_SourceMode.Voltage:
                                                {
                                                    R_Script = CommandBuilder.AppendFormat(R_Script, "a", "OUTPUT_DCVOLTS", "v", _valueThroughTheStructure, "i", _limiti, "SMUA", "MEASURE_OHMS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(R_Script, ref MeasuredValue);
                                                } break;
                                            case KEITHLEY_2601A_SourceMode.Current:
                                                {
                                                    R_Script = CommandBuilder.AppendFormat(R_Script, "a", "OUTPUT_DCAMPS", "i", _valueThroughTheStructure, "v", _limitv, "SMUA", "MEASURE_OHMS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(R_Script, ref MeasuredValue);
                                                } break;
                                            default:
                                                break;
                                        }
                                    } break;
                                case KEITHLEY_2602A_Channels.ChannelB:
                                    {
                                        switch (SourceMode)
                                        {
                                            case KEITHLEY_2601A_SourceMode.Voltage:
                                                {
                                                    R_Script = CommandBuilder.AppendFormat(R_Script, "b", "OUTPUT_DCVOLTS", "v", _valueThroughTheStructure, "i", _limiti, "SMUB", "MEASURE_OHMS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(R_Script, ref MeasuredValue);
                                                } break;
                                            case KEITHLEY_2601A_SourceMode.Current:
                                                {
                                                    R_Script = CommandBuilder.AppendFormat(R_Script, "b", "OUTPUT_DCAMPS", "i", _valueThroughTheStructure, "v", _limitv, "SMUB", "MEASURE_OHMS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(R_Script, ref MeasuredValue);
                                                } break;
                                            default:
                                                break;
                                        }
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    case KEITHLEY_2601A_MeasureMode.Power:
                        {
                            switch (Channel)
                            {
                                case KEITHLEY_2602A_Channels.ChannelA:
                                    {
                                        switch (SourceMode)
                                        {
                                            case KEITHLEY_2601A_SourceMode.Voltage:
                                                {
                                                    P_Script = CommandBuilder.AppendFormat(P_Script, "a", "i", "SMUA", "MEASURE_WATTS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(P_Script, ref MeasuredValue);
                                                } break;
                                            case KEITHLEY_2601A_SourceMode.Current:
                                                {
                                                    P_Script = CommandBuilder.AppendFormat(P_Script, "a", "v", "SMUA", "MEASURE_WATTS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(P_Script, ref MeasuredValue);
                                                } break;
                                            default:
                                                break;
                                        }
                                    } break;
                                case KEITHLEY_2602A_Channels.ChannelB:
                                    {
                                        switch (SourceMode)
                                        {
                                            case KEITHLEY_2601A_SourceMode.Voltage:
                                                {
                                                    P_Script = CommandBuilder.AppendFormat(P_Script, "b", "i", "SMUA", "MEASURE_WATTS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(P_Script, ref MeasuredValue);
                                                } break;
                                            case KEITHLEY_2601A_SourceMode.Current:
                                                {
                                                    P_Script = CommandBuilder.AppendFormat(P_Script, "b", "v", "SMUA", "MEASURE_WATTS", NumberOfAverages, _TimeDelay).ToString();
                                                    ExecuteQuery(P_Script, ref MeasuredValue);
                                                } break;
                                            default:
                                                break;
                                        }
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    default:
                        break;
                }

                return MeasuredValue;
            }

            /// <summary>
            /// Sets value to the appropriate channel
            /// </summary>
            /// <param name="Value">Value to be set into device</param>
            /// <param name="SourceMode">Defines voltage or current value should be written to the device</param>
            /// <param name="Channel">Defines channel, on which the value is setted</param>
            public void SetValueToChannel(double Value, KEITHLEY_2601A_SourceMode SourceMode, KEITHLEY_2602A_Channels Channel)
            {
                SetChannelSourceMode(Channel, SourceMode);

                //Changing value to appropriate format

                var _Value = Value.ToString().Replace(',', '.');

                var script = "smu{0}.source.level{1} = {2} ";

                switch (Channel)
                {
                    case KEITHLEY_2602A_Channels.ChannelA:
                        {
                            CommandBuilder = new StringBuilder();

                            switch (SourceMode)
                            {
                                case KEITHLEY_2601A_SourceMode.Voltage:
                                    {
                                        script = CommandBuilder.AppendFormat(script, "a", "v", _Value).ToString();
                                        SendCommandRequest(script);
                                    } break;
                                case KEITHLEY_2601A_SourceMode.Current:
                                    {
                                        script = CommandBuilder.AppendFormat(script, "a", "i", _Value).ToString();
                                        SendCommandRequest(script);
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    case KEITHLEY_2602A_Channels.ChannelB:
                        {
                            CommandBuilder = new StringBuilder();

                            switch (SourceMode)
                            {
                                case KEITHLEY_2601A_SourceMode.Voltage:
                                    {
                                        script = CommandBuilder.AppendFormat(script, "b", "v", _Value).ToString();
                                        SendCommandRequest(script);
                                    } break;
                                case KEITHLEY_2601A_SourceMode.Current:
                                    {
                                        script = CommandBuilder.AppendFormat(script, "b", "i", _Value).ToString();
                                        SendCommandRequest(script);
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
            /// Sets sense SENSE_LOCAL (2-wire) or SENSE_REMOTE (4-wire)
            /// </summary>
            /// <param name="Channel">Channel</param>
            /// <param name="Sense">Sense</param>
            public void SetSence(KEITHLEY_2602A_Channels Channel, KEITHLEY_2602A_Sense Sense)
            {
                var SetSenseScript =
                    "smu{0}.sense = smu{0}.{1}";

                CommandBuilder = new StringBuilder();

                switch (Channel)
                {
                    case KEITHLEY_2602A_Channels.ChannelA:
                        {
                            switch (Sense)
                            {
                                case KEITHLEY_2602A_Sense.SENSE_LOCAL:
                                    {
                                        SetSenseScript = CommandBuilder.AppendFormat(SetSenseScript, "a", "SENSE_LOCAL").ToString();
                                        SendCommandRequest(SetSenseScript);
                                    } break;
                                case KEITHLEY_2602A_Sense.SENSE_REMOTE:
                                    {
                                        SetSenseScript = CommandBuilder.AppendFormat(SetSenseScript, "a", "SENSE_REMOTE").ToString();
                                        SendCommandRequest(SetSenseScript);
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    case KEITHLEY_2602A_Channels.ChannelB:
                        {
                            switch (Sense)
                            {
                                case KEITHLEY_2602A_Sense.SENSE_LOCAL:
                                    {
                                        SetSenseScript = CommandBuilder.AppendFormat(SetSenseScript, "b", "SENSE_LOCAL").ToString();
                                        SendCommandRequest(SetSenseScript);
                                    } break;
                                case KEITHLEY_2602A_Sense.SENSE_REMOTE:
                                    {
                                        SetSenseScript = CommandBuilder.AppendFormat(SetSenseScript, "b", "SENSE_REMOTE").ToString();
                                        SendCommandRequest(SetSenseScript);
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
            /// Sets appropriate limits to defined channel
            /// </summary>
            /// <param name="LimitValue">Limit value</param>
            /// <param name="Mode">Source mode</param>
            /// <param name="Channel">Channel</param>
            public void SetSourceLimit(double LimitValue, KEITHLEY_2601A_LimitMode Mode, KEITHLEY_2602A_Channels Channel)
            {
                var SetLimitScript =
                    "smu{0}.source.limit{1} = {2} ";

                var _LimitValue = LimitValue.ToString().Replace(',', '.');

                CommandBuilder = new StringBuilder();

                switch (Channel)
                {
                    case KEITHLEY_2602A_Channels.ChannelA:
                        {
                            switch (Mode)
                            {
                                case KEITHLEY_2601A_LimitMode.Voltage:
                                    {
                                        SetLimitScript = CommandBuilder.AppendFormat(SetLimitScript, "a", "v", _LimitValue).ToString();
                                        SendCommandRequest(SetLimitScript);
                                    } break;
                                case KEITHLEY_2601A_LimitMode.Current:
                                    {
                                        SetLimitScript = CommandBuilder.AppendFormat(SetLimitScript, "a", "i", _LimitValue).ToString();
                                        SendCommandRequest(SetLimitScript);
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    case KEITHLEY_2602A_Channels.ChannelB:
                        {
                            switch (Mode)
                            {
                                case KEITHLEY_2601A_LimitMode.Voltage:
                                    {
                                        SetLimitScript = CommandBuilder.AppendFormat(SetLimitScript, "b", "v", _LimitValue).ToString();
                                        SendCommandRequest(SetLimitScript);
                                    } break;
                                case KEITHLEY_2601A_LimitMode.Current:
                                    {
                                        SetLimitScript = CommandBuilder.AppendFormat(SetLimitScript, "b", "i", _LimitValue).ToString();
                                        SendCommandRequest(SetLimitScript);
                                    } break;
                                default:
                                    break;
                            }
                        } break;
                    default:
                        break;
                }
            }
        }
    }
}