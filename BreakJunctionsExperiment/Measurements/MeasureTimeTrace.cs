using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hardware;
using Hardware.KEITHLEY_2602A;

using BreakJunctions.Events;
using System.ComponentModel;
using System.Windows.Threading;

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

        private int _NumberOfAverages = IV_And_TimeTraceViewModel.Instance.TimeTraceMeasurementNumberOfAverages;
        private double _TimeDelay = IV_And_TimeTraceViewModel.Instance.TimeTraceMeasurementTimeDelay;
        
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

        private bool _CancelMeasures = false;

        public MeasureTimeTrace(IMotion motor, double startPosition, double destination, I_SMU measureDevice, KEITHLEY_2601A_SourceMode sourceMode, KEITHLEY_2601A_MeasureMode measureMode, double valueThroughTheStructure)
        {
            _Motor = motor;
            _StartPosition = startPosition;
            _Destination = destination;
            _MeasureDevice = measureDevice;
            _SourceMode = sourceMode;
            _MeasureMode = measureMode;
            _ValueThroughTheStructure = valueThroughTheStructure;

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
            switch (_MeasureMode)
            {
                case KEITHLEY_2601A_MeasureMode.Voltage:
                    {
                        var measuredVoltage = _MeasureDevice.MeasureVoltage(_NumberOfAverages, _TimeDelay);
                        if (!(double.IsNaN(e.Position) || double.IsNaN(measuredVoltage)))
                            AllEventsHandler.Instance.OnTimeTracePointReceived(null, new TimeTracePointReceived_EventArgs(e.Position, measuredVoltage));
                    } break;
                case KEITHLEY_2601A_MeasureMode.Current:
                    {
                        var measuredCurrent = _MeasureDevice.MeasureCurrent(_NumberOfAverages, _TimeDelay);
                        if (!(double.IsNaN(e.Position) || double.IsNaN(measuredCurrent)))
                            AllEventsHandler.Instance.OnTimeTracePointReceived(null, new TimeTracePointReceived_EventArgs(e.Position, measuredCurrent));
                    } break;
                case KEITHLEY_2601A_MeasureMode.Resistance:
                    {
                        switch (_SourceMode)
                        {
                            case KEITHLEY_2601A_SourceMode.Voltage:
                                {
                                    var measuredResistance = _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Hardware.SourceMode.Voltage);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredResistance)))
                                        AllEventsHandler.Instance.OnTimeTracePointReceived(null, new TimeTracePointReceived_EventArgs(e.Position, measuredResistance));
                                } break;
                            case KEITHLEY_2601A_SourceMode.Current:
                                {
                                    var measuredResistance = _MeasureDevice.MeasureResistance(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Hardware.SourceMode.Current);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredResistance)))
                                        AllEventsHandler.Instance.OnTimeTracePointReceived(null, new TimeTracePointReceived_EventArgs(e.Position, measuredResistance));
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
                                        AllEventsHandler.Instance.OnTimeTracePointReceived(null, new TimeTracePointReceived_EventArgs(e.Position, measuredPower));
                                } break;
                            case KEITHLEY_2601A_SourceMode.Current:
                                {
                                    var measuredPower = _MeasureDevice.MeasurePower(_ValueThroughTheStructure, _NumberOfAverages, _TimeDelay, Hardware.SourceMode.Current);
                                    if (!(double.IsNaN(e.Position) || double.IsNaN(measuredPower)))
                                        AllEventsHandler.Instance.OnTimeTracePointReceived(null, new TimeTracePointReceived_EventArgs(e.Position, measuredPower));
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
