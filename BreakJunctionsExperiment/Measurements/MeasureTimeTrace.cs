using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hardware;
using Hardware.KEITHLEY_2602A;

using BreakJunctions.Events;
using System.ComponentModel;
using System.Windows.Threading;
using BreakJunctions.Plotting;

namespace BreakJunctions.Measurements
{
    class MeasureTimeTrace : IDisposable
    {
        private IMotion _Motor;
        public IMotion Motor
        {
            get { return _Motor; }
            set { _Motor = value; }
        }

        private double _StartPosition;
        public double StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }

        private double _Destination;
        public double Destination
        {
            get { return _Destination; }
            set { _Destination = value; }
        }

        private I_SMU _MeasureDevice;
        public I_SMU MeasureDevice
        {
            get { return _MeasureDevice; }
            set { _MeasureDevice = value; }
        }
        
        private double _ValueThroughTheStructure;
        public double ValueThroughTheStructure
        {
            get { return _ValueThroughTheStructure; }
            set { _ValueThroughTheStructure = value; }
        }

        private int _NumberOfAverages = 2;
        public int NumberOfAverages
        {
            get { return _NumberOfAverages; }
            set { _NumberOfAverages = value; }
        }
        private double _TimeDelay = 0.005;
        public double TimeDelay
        {
            get { return _TimeDelay; }
            set { _TimeDelay = value; }
        }
        
        private KEITHLEY_2601A_SourceMode _SourceMode;
        public KEITHLEY_2601A_SourceMode SourceMode
        {
            get { return _SourceMode; }
            set { _SourceMode = value; }
        }

        private KEITHLEY_2601A_MeasureMode _MeasureMode;
        public KEITHLEY_2601A_MeasureMode MeasureMode
        {
            get { return _MeasureMode; }
            set { _MeasureMode = value; }
        }

        private Channels _Channel;
        private MeasureTimeTraceChannelController _ChannelController;

        private bool _CancelMeasures = false;

        public MeasureTimeTrace(IMotion motor, double startPosition, double destination, I_SMU measureDevice, KEITHLEY_2601A_SourceMode sourceMode, KEITHLEY_2601A_MeasureMode measureMode, double valueThroughTheStructure, Channels Channel, MeasureTimeTraceChannelController ChannelController)
        {
            _Motor = motor;
            _StartPosition = startPosition;
            _Destination = destination;
            _MeasureDevice = measureDevice;
            _SourceMode = sourceMode;
            _MeasureMode = measureMode;
            _ValueThroughTheStructure = valueThroughTheStructure;
            _Channel = Channel;
            _ChannelController = ChannelController;

            AllEventsHandler.Instance.TimeTraceMeasurementsStateChanged += OnTimeTraceMeasurementsStateChanged;
            AllEventsHandler.Instance.Motion += OnMotionPositionMeasured;
       }

        public void StartMeasurement(object sender, DoWorkEventArgs e, MotionKind motionKind, int numberRepetities = 1)
        {
            var worker = sender as BackgroundWorker;

            switch (_SourceMode)
            {
                case KEITHLEY_2601A_SourceMode.Voltage:
                    {
                        _MeasureDevice.SetSourceVoltage(_ValueThroughTheStructure);
                    } break;
                case KEITHLEY_2601A_SourceMode.Current:
                    {
                        _MeasureDevice.SetSourceCurrent(_ValueThroughTheStructure);
                    } break;
                default:
                    break;
            }

            AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(this, new TimeTraceMeasurementStateChanged_EventArgs(true));

            _MeasureDevice.SwitchON();

            switch (motionKind)
            {
                case MotionKind.Single:
                    {
                        _Motor.StartMotion(_StartPosition, _Destination, MotionKind.Single);
                    } break;
                case MotionKind.Repetitive:
                    {
                        _Motor.StartMotion(_StartPosition, _Destination, MotionKind.Repetitive, numberRepetities);
                    } break;
                default:
                    break;
            }

            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    _Motor.StopMotion();                    
                    e.Cancel = true;
                    break;
                }
                if (_CancelMeasures == true)
                {
                    _Motor.StopMotion();
                    break;
                }
            }

            _MeasureDevice.SwitchOFF();
        }

        private void OnMotionPositionMeasured(object sender, Motion_EventArgs e)
        {
            var worker = sender as BackgroundWorker;

            switch (_MeasureMode)
            {
                case KEITHLEY_2601A_MeasureMode.Voltage:
                    {
                        var measuredVoltage = _MeasureDevice.MeasureVoltage(_NumberOfAverages, _TimeDelay);
                        if (!(double.IsNaN(e.Position) || double.IsNaN(measuredVoltage)))
                        {
                            switch (_Channel)
                            {
                                case Channels.Channel_01:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(null, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredVoltage));
                                        worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                    } break;
                                case Channels.Channel_02:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(null, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredVoltage));
                                        worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                    } break;
                                default:
                                    break;
                            }
                        }
                    } break;
                case KEITHLEY_2601A_MeasureMode.Current:
                    {
                        var measuredCurrent = _MeasureDevice.MeasureCurrent(_NumberOfAverages, _TimeDelay);
                        if (!(double.IsNaN(e.Position) || double.IsNaN(measuredCurrent)))
                        {
                            switch (_Channel)
                            {
                                case Channels.Channel_01:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(null, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredCurrent));
                                        worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                    } break;
                                case Channels.Channel_02:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(null, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredCurrent));
                                        worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                    } break;
                                default:
                                    break;
                            }
                        }
                    } break;
                case KEITHLEY_2601A_MeasureMode.Resistance:
                    {
                        switch (_SourceMode)
                        {
                            case KEITHLEY_2601A_SourceMode.Voltage:
                                {
                                    var measuredResistance = _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Hardware.SourceMode.Voltage);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredResistance)))
                                    {
                                        switch (_Channel)
                                        {
                                            case Channels.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(null, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredResistance));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            case Channels.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(null, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredResistance));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            case KEITHLEY_2601A_SourceMode.Current:
                                {
                                    var measuredResistance = _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Hardware.SourceMode.Current);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredResistance)))
                                    {
                                        switch (_Channel)
                                        {
                                            case Channels.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(null, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredResistance));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            case Channels.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(null, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredResistance));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            default:
                                break;
                        }
                    } break;
                case KEITHLEY_2601A_MeasureMode.Power:
                    {
                        switch (_SourceMode)
                        {
                            case KEITHLEY_2601A_SourceMode.Voltage:
                                {
                                    var measuredPower = _MeasureDevice.MeasurePower(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Hardware.SourceMode.Voltage);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredPower)))
                                    {
                                        switch (_Channel)
                                        {
                                            case Channels.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(null, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredPower));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            case Channels.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(null, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredPower));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            case KEITHLEY_2601A_SourceMode.Current:
                                {
                                    var measuredPower = _MeasureDevice.MeasurePower(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Hardware.SourceMode.Current);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredPower)))
                                    {
                                        switch (_Channel)
                                        {
                                            case Channels.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(null, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredPower));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            case Channels.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(null, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredPower));
                                                    worker.ReportProgress(Convert.ToInt32(e.Position / _Destination * 100));
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            default:
                                break;
                        }
                    } break;
                default:
                    break;
            }
        }

        private void OnTimeTraceMeasurementsStateChanged(object sender, TimeTraceMeasurementStateChanged_EventArgs e)
        {
            _CancelMeasures = !e.TimeTrace_MeasurementState;
        }

        public void Dispose()
        {
            AllEventsHandler.Instance.Motion -= OnMotionPositionMeasured;
            AllEventsHandler.Instance.TimeTraceMeasurementsStateChanged -= OnTimeTraceMeasurementsStateChanged;

            this._Motor = null;
            this._Destination = 0.0;
            this._MeasureDevice = null;
            this._ValueThroughTheStructure = 0.0;
        }
    }
}
