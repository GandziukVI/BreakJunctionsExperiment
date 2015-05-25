using Devices.SMU;
using Keithley.Ke26XXA.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keithley2602A
{
    public class Keithley2602AChannel : I_SMU
    {
        private Keithley2602AChannels _selectedChannel;
        private Keithley2602A _device;

        public Keithley2602AChannel(Keithley2602A Device, Keithley2602AChannels SelectedChannel)
        {
            _ChannelAccuracyParams = new AccuracyParams();

            _device = Device;
            _selectedChannel = SelectedChannel;

            InitDevice();
        }

        public bool InitDevice()
        {
            if (!_device.Driver.Initialized)
            {
                var options = "QueryInstrStatus=True";
                _device.Driver.Initialize(_device.ResourceName, true, true, options);
                _device.Driver.Utility.Reset();
                _device.Driver.Display.Screen = Ke26XXADisplayScreenEnum.Ke26XXADisplayScreenSmuASmuB;

                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            _device.Driver.Source.set_AutoDisableOutput("A", false);
                        } break;
                    case Keithley2602AChannels.ChannelB:
                        {
                            _device.Driver.Source.set_AutoDisableOutput("B", false);
                        } break;
                }
            }

            return _device.Driver.Initialized;
        }

        public double MeasureCurrent(int NumberOfAverages, double TimeDelay)
        {
            try
            {
                //_device.Driver.Utility.Reset();
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            _device.Driver.Display.set_MeasureFunction("A", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionDCAmps);

                            _device.Driver.Measurement.Current.set_AutoRangeEnabled("A", true);
                            _device.Driver.Measurement.set_Count("A", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("A", TimeDelay);

                            var bufferName = "CurrentBuf";
                            _device.Driver.Measurement.Buffer.Create("A", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Current.MeasureMultiple("A", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    case Keithley2602AChannels.ChannelB:
                        {
                            _device.Driver.Display.set_MeasureFunction("B", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionDCAmps);

                            _device.Driver.Measurement.Current.set_AutoRangeEnabled("B", true);
                            _device.Driver.Measurement.set_Count("B", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("B", TimeDelay);

                            var bufferName = "CurrentBuf";
                            _device.Driver.Measurement.Buffer.Create("B", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Current.MeasureMultiple("B", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    default:
                        return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }

        public double MeasureVoltage(int NumberOfAverages, double TimeDelay)
        {
            try
            {
                //_device.Driver.Utility.Reset();
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            _device.Driver.Display.set_MeasureFunction("A", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionDCVolts);

                            _device.Driver.Measurement.Voltage.set_AutoRangeEnabled("A", true);
                            _device.Driver.Measurement.set_Count("A", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("A", TimeDelay);

                            var bufferName = "VoltageBuf";
                            _device.Driver.Measurement.Buffer.Create("A", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Voltage.MeasureMultiple("A", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    case Keithley2602AChannels.ChannelB:
                        {
                            _device.Driver.Display.set_MeasureFunction("B", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionDCVolts);

                            _device.Driver.Measurement.Voltage.set_AutoRangeEnabled("B", true);
                            _device.Driver.Measurement.set_Count("B", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("B", TimeDelay);

                            var bufferName = "VoltageBuf";
                            _device.Driver.Measurement.Buffer.Create("B", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Voltage.MeasureMultiple("B", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    default:
                        return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }

        public double MeasurePower(double valueThroughTheStrusture, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            try
            {
                //_device.Driver.Utility.Reset();
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            _device.Driver.Display.set_MeasureFunction("A", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionWatts);

                            _device.Driver.Measurement.set_Count("A", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("A", TimeDelay);

                            var bufferName = "PowerBuf";
                            _device.Driver.Measurement.Buffer.Create("A", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Power.MeasureMultiple("A", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    case Keithley2602AChannels.ChannelB:
                        {
                            _device.Driver.Display.set_MeasureFunction("B", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionWatts);

                            _device.Driver.Measurement.set_Count("B", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("B", TimeDelay);

                            var bufferName = "PowerBuf";
                            _device.Driver.Measurement.Buffer.Create("B", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Power.MeasureMultiple("B", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    default:
                        return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }

        public double MeasureResistance(double valueThroughTheStrusture, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            try
            {
                //_device.Driver.Utility.Reset();
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            _device.Driver.Display.set_MeasureFunction("A", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionOhms);

                            _device.Driver.Measurement.set_SenseMode("A", Ke26XXASenseModeEnum.Ke26XXASenseModeLocal);
                            _device.Driver.Measurement.set_Count("A", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("A", TimeDelay);

                            var bufferName = "ResistanceBuf";
                            _device.Driver.Measurement.Buffer.Create("A", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Resistance.MeasureMultiple("A", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    case Keithley2602AChannels.ChannelB:
                        {
                            _device.Driver.Display.set_MeasureFunction("B", Ke26XXAMeasurementFunctionEnum.Ke26XXAMeasurementFunctionOhms);

                            _device.Driver.Measurement.set_SenseMode("B", Ke26XXASenseModeEnum.Ke26XXASenseModeLocal);
                            _device.Driver.Measurement.set_Count("B", NumberOfAverages);
                            _device.Driver.Measurement.set_Delay("B", TimeDelay);

                            var bufferName = "ResistanceBuf";
                            _device.Driver.Measurement.Buffer.Create("B", bufferName, NumberOfAverages);
                            _device.Driver.Measurement.Resistance.MeasureMultiple("B", bufferName);

                            var result = _device.Driver.Measurement.Buffer.MeasureData.GetAllReadings(bufferName).Average();
                            CheckValueAccuracy(result);

                            return result;
                        }
                    default:
                        return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }

        public bool SetSourceVoltage(double Value)
        {
            try
            {
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            if(_device.Driver.Source.get_Function("A") != Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCVolts)
                                _device.Driver.Source.set_Function("A", Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCVolts);
                            _device.Driver.Source.Voltage.set_Level("A", Value);
                        } break;
                    case Keithley2602AChannels.ChannelB:
                        {
                            if (_device.Driver.Source.get_Function("B") != Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCVolts)
                                _device.Driver.Source.set_Function("B", Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCVolts);
                            _device.Driver.Source.Voltage.set_Level("B", Value);
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
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            if(_device.Driver.Source.get_Function("A") != Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCAmps)
                                _device.Driver.Source.set_Function("A", Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCAmps);
                            _device.Driver.Source.Current.set_Level("A", Value);
                        } break;
                    case Keithley2602AChannels.ChannelB:
                        {
                            if (_device.Driver.Source.get_Function("B") != Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCAmps)
                                _device.Driver.Source.set_Function("B", Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCAmps);
                            _device.Driver.Source.Current.set_Level("B", Value);
                        } break;
                }

                return true;
            }
            catch { return false; }
        }

        public bool SetVoltageLimit(double Value)
        {
            try
            {
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            _device.Driver.Source.Voltage.set_Limit("A", Value);
                        } break;
                    case Keithley2602AChannels.ChannelB:
                        {
                            _device.Driver.Source.Voltage.set_Limit("B", Value);
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
                switch (_selectedChannel)
                {
                    case Keithley2602AChannels.ChannelA:
                        {
                            _device.Driver.Source.Current.set_Limit("A", Value);
                        } break;
                    case Keithley2602AChannels.ChannelB:
                        {
                            _device.Driver.Source.Current.set_Limit("B", Value);
                        } break;
                }

                return true;
            }
            catch { return false; }
        }

        public void SwitchOFF()
        {
            switch (_selectedChannel)
            {
                case Keithley2602AChannels.ChannelA:
                    {
                        _device.Driver.Source.set_OutputEnabled("A", false);
                    } break;
                case Keithley2602AChannels.ChannelB:
                    {
                        _device.Driver.Source.set_OutputEnabled("B", false);
                    } break;
            }
        }

        public void SwitchON()
        {
            switch (_selectedChannel)
            {
                case Keithley2602AChannels.ChannelA:
                    {
                        _device.Driver.Source.set_OutputEnabled("A", true);
                    } break;
                case Keithley2602AChannels.ChannelB:
                    {
                        _device.Driver.Source.set_OutputEnabled("B", true);
                    } break;
            }
        }

        public void SetSpeed(double Value)
        {
            if (Value < 0.01)
                Value = 0.01;
            else if (Value > 25)
                Value = 25.0;

            switch (_selectedChannel)
            {
                case Keithley2602AChannels.ChannelA:
                    {
                        _device.Driver.Measurement.set_NPLC("A", Value);
                    } break;
                case Keithley2602AChannels.ChannelB:
                    {
                        _device.Driver.Measurement.set_NPLC("B", Value);
                    } break;
            }
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

        private Keithley2602A_RangeAccuracySet _CurrentRow;
        private void CheckValueAccuracy(double value)
        {
            if (ChannelAccuracyParams.RangeAccuracySet != null)
            {
                try
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
                                SetSpeed(row.Accuracy);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }
        }
    }
}
