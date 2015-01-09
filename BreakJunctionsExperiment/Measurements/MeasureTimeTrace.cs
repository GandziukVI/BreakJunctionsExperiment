using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BreakJunctions.Events;
using System.ComponentModel;
using System.Windows.Threading;
using BreakJunctions.Plotting;

using Devices.SMU;
using SMU.KEITHLEY_2602A;

using BreakJunctions.Motion;

namespace BreakJunctions.Measurements
{
    class MeasureTimeTrace : IDisposable
    {
        #region MeasureTimeTrace settings

        private double _QuantumConductance = 0.00007748091734625;

        private MotionController _Motor;
        /// <summary>
        /// Gets or sets motion controller to be responsible for
        /// the motion
        /// </summary>
        public MotionController Motor
        {
            get { return _Motor; }
            set { _Motor = value; }
        }

        private double _StartPosition;
        /// <summary>
        /// Gets or sets the start potition of the motor (meters)
        /// </summary>
        public double StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }

        private double _CurrentPosition = 0.0;
        /// <summary>
        /// Gets the current position of the motor (meters)
        /// </summary>
        public double CurrentPosition
        {
            get { return _CurrentPosition; }
        }

        private double _FinalDestination;
        /// <summary>
        /// Gets or sets the final position of the motor, to be
        /// reached in measurement process (meters)
        /// </summary>
        public double FinalDestination
        {
            get { return _FinalDestination; }
            set { _FinalDestination = value; }
        }

        private I_SMU _MeasureDevice;
        /// <summary>
        /// Source mesaure unit, responsible for the measurements of
        /// voltage, current, resistance, etc.
        /// </summary>
        public I_SMU MeasureDevice
        {
            get { return _MeasureDevice; }
            set { _MeasureDevice = value; }
        }

        private double _ValueThroughTheStructure;
        /// <summary>
        /// Gets or sets the value of voltage or current througt the structrue
        /// according to the power supply mode
        /// </summary>
        public double ValueThroughTheStructure
        {
            get { return _ValueThroughTheStructure; }
            set { _ValueThroughTheStructure = value; }
        }

        private int _NumberOfAverages = 2;
        /// <summary>
        /// Gets or sets the number of scans at the same value of
        /// current or voltage through the structure
        /// </summary>
        public int NumberOfAverages
        {
            get { return _NumberOfAverages; }
            set { _NumberOfAverages = value; }
        }
        private double _TimeDelay = 0.005;
        /// <summary>
        /// Gets or sets the value of time delay between scans, if
        /// NumberOfAverages > 1
        /// </summary>
        public double TimeDelay
        {
            get { return _TimeDelay; }
            set { _TimeDelay = value; }
        }

        private SourceMode _SourceMode;
        /// <summary>
        /// The source mode of SMU
        /// </summary>
        public SourceMode SourceMode
        {
            get { return _SourceMode; }
            set { _SourceMode = value; }
        }

        private MeasureMode _MeasureMode;
        /// <summary>
        /// The measure mode of SMU
        /// </summary>
        public MeasureMode MeasureMode
        {
            get { return _MeasureMode; }
            set { _MeasureMode = value; }
        }

        private MotionKind _CurrentMotionKind;
        /// <summary>
        /// The motion kind of motion controller: single / repetitive
        /// </summary>
        public MotionKind CurrentMotionKind
        {
            get { return _CurrentMotionKind; }
        }

        private int _NumberRepetities = 1;
        /// <summary>
        /// The number of repetities for the cycle measurement
        /// </summary>
        public int NumberRepetities
        {
            get { return _NumberRepetities; }
        }

        private BackgroundWorker _worker;

        private ChannelsToInvestigate _Channel;
        private MeasureTimeTraceChannelController _ChannelController;

        private bool _CancelMeasures = false;

        #endregion

        #region Constructor / Destructor

        public MeasureTimeTrace(MotionController __Motor, double __StartPosition, double __FinalDestination, I_SMU __MeasureDevice, SourceMode __SourceMode, MeasureMode __MeasureMode, double __ValueThroughTheStructure, ChannelsToInvestigate __Channel, MeasureTimeTraceChannelController __ChannelController, ref BackgroundWorker __MeasurementWorker)
        {
            _Motor = __Motor;
            _StartPosition = __StartPosition;
            _FinalDestination = __FinalDestination;
            _MeasureDevice = __MeasureDevice;
            _SourceMode = __SourceMode;
            _MeasureMode = __MeasureMode;
            _ValueThroughTheStructure = __ValueThroughTheStructure;
            _Channel = __Channel;
            _ChannelController = __ChannelController;

            _worker = __MeasurementWorker;

            AllEventsHandler.Instance.TimeTraceMeasurementsStateChanged += OnTimeTraceMeasurementsStateChanged;
            AllEventsHandler.Instance.Motion += OnMotionPositionMeasured;
        }

        ~MeasureTimeTrace()
        {
            this.Dispose();
        }

        #endregion

        #region MeasureTimeTrace functionality

        public void StartMeasurement(object sender, DoWorkEventArgs e, MotionKind __MotionKind, int __NumberRepetities = 1)
        {
            _CurrentMotionKind = __MotionKind;
            _NumberRepetities = __NumberRepetities;

            switch (_SourceMode)
            {
                case SourceMode.Voltage:
                    {
                        _MeasureDevice.SetSourceVoltage(_ValueThroughTheStructure);
                    } break;
                case SourceMode.Current:
                    {
                        _MeasureDevice.SetSourceCurrent(_ValueThroughTheStructure);
                    } break;
                default:
                    break;
            }

            AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(this, new TimeTraceMeasurementStateChanged_EventArgs(true));

            _MeasureDevice.SwitchON();

            switch (__MotionKind)
            {
                case MotionKind.Single:
                    {
                        _Motor.StartMotion(_StartPosition, _FinalDestination, MotionKind.Single);
                    } break;
                case MotionKind.Repetitive:
                    {
                        _Motor.StartMotion(_StartPosition, _FinalDestination, MotionKind.Repetitive, __NumberRepetities);
                    } break;
                default:
                    break;
            }

            while (true)
            {
                if (_worker.CancellationPending == true)
                {
                    _Motor.StopMotion();
                    e.Cancel = true;
                    break;
                }
                if (_CancelMeasures == true)
                {
                    _Motor.StopMotion();
                    e.Cancel = true;
                    break;
                }
            }

            _MeasureDevice.SwitchOFF();
        }

        private void StopMeasurement()
        {
            _Motor.StopMotion();
            _MeasureDevice.SwitchOFF();
        }

        private void OnMotionPositionMeasured(object sender, Motion_EventArgs e)
        {
            switch (_MeasureMode)
            {
                case MeasureMode.Voltage:
                    {
                        var measuredVoltage = _MeasureDevice.MeasureVoltage(_NumberOfAverages, _TimeDelay);
                        if (!(double.IsNaN(e.Position) || double.IsNaN(measuredVoltage)))
                        {
                            switch (_Channel)
                            {
                                case ChannelsToInvestigate.Channel_01:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(this, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredVoltage));
                                        _CurrentPosition = e.Position;
                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                    } break;
                                case ChannelsToInvestigate.Channel_02:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(this, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredVoltage));
                                        _CurrentPosition = e.Position;
                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                    } break;
                                default:
                                    break;
                            }
                        }
                    } break;
                case MeasureMode.Current:
                    {
                        var measuredCurrent = _MeasureDevice.MeasureCurrent(_NumberOfAverages, _TimeDelay);
                        if (!(double.IsNaN(e.Position) || double.IsNaN(measuredCurrent)))
                        {
                            switch (_Channel)
                            {
                                case ChannelsToInvestigate.Channel_01:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(null, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredCurrent));
                                        _CurrentPosition = e.Position;
                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                    } break;
                                case ChannelsToInvestigate.Channel_02:
                                    {
                                        AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(null, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredCurrent));
                                        _CurrentPosition = e.Position;
                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                    } break;
                                default:
                                    break;
                            }
                        }
                    } break;
                case MeasureMode.Resistance:
                    {
                        switch (_SourceMode)
                        {
                            case SourceMode.Voltage:
                                {
                                    var measuredResistance = _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Devices.SMU.SourceMode.Voltage);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredResistance)))
                                    {
                                        switch (_Channel)
                                        {
                                            case ChannelsToInvestigate.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(this, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredResistance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            case ChannelsToInvestigate.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(this, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredResistance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            case SourceMode.Current:
                                {
                                    var measuredResistance = _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Devices.SMU.SourceMode.Current);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredResistance)))
                                    {
                                        switch (_Channel)
                                        {
                                            case ChannelsToInvestigate.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(this, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredResistance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            case ChannelsToInvestigate.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(this, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredResistance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
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
                case MeasureMode.Conductance:
                    {
                        switch (_SourceMode)
                        {
                            case SourceMode.Voltage:
                                {
                                    var measuredConductance = 0.0;
                                    try
                                    {
                                        measuredConductance = (1.0 / _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Devices.SMU.SourceMode.Voltage)) / _QuantumConductance;
                                    }
                                    catch { }

                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredConductance)))
                                    {
                                        switch (_Channel)
                                        {
                                            case ChannelsToInvestigate.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(this, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredConductance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            case ChannelsToInvestigate.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(this, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredConductance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            case SourceMode.Current:
                                {
                                    var measuredConductance = (1.0 / _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Devices.SMU.SourceMode.Current)) / _QuantumConductance;
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredConductance)))
                                    {
                                        switch (_Channel)
                                        {
                                            case ChannelsToInvestigate.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(this, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredConductance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            case ChannelsToInvestigate.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(this, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredConductance));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            default:
                                break;
                        }
                    }break;
                case MeasureMode.Power:
                    {
                        switch (_SourceMode)
                        {
                            case SourceMode.Voltage:
                                {
                                    var measuredPower = _MeasureDevice.MeasurePower(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Devices.SMU.SourceMode.Voltage);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredPower)))
                                    {
                                        switch (_Channel)
                                        {
                                            case ChannelsToInvestigate.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(this, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredPower));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            case ChannelsToInvestigate.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(this, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredPower));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            default:
                                                break;
                                        }
                                    }
                                } break;
                            case SourceMode.Current:
                                {
                                    var measuredPower = _MeasureDevice.MeasurePower(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Devices.SMU.SourceMode.Current);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredPower)))
                                    {
                                        switch (_Channel)
                                        {
                                            case ChannelsToInvestigate.Channel_01:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_01(this, new TimeTracePointReceivedChannel_01_EventArgs(e.Position, measuredPower));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
                                                } break;
                                            case ChannelsToInvestigate.Channel_02:
                                                {
                                                    AllEventsHandler.Instance.OnTimeTracePointReceivedChannel_02(this, new TimeTracePointReceivedChannel_02_EventArgs(e.Position, measuredPower));
                                                    _CurrentPosition = e.Position;
                                                    try
                                                    {
                                                        _worker.ReportProgress(Convert.ToInt32((_CurrentPosition - _StartPosition) / (_FinalDestination - _StartPosition) * 100.0));
                                                    }
                                                    catch { }
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

        #endregion

        #region Disposing the instance

        public void Dispose()
        {
            AllEventsHandler.Instance.Motion -= OnMotionPositionMeasured;
            AllEventsHandler.Instance.TimeTraceMeasurementsStateChanged -= OnTimeTraceMeasurementsStateChanged;

            this._Motor = null;
            this._FinalDestination = 0.0;
            this._MeasureDevice = null;
            this._ValueThroughTheStructure = 0.0;
        }

        #endregion
    }
}
