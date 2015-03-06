using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using BreakJunctions.Events;
using BreakJunctions.Plotting;

using Devices.SMU;
using System.Threading;

namespace BreakJunctions.Measurements
{
    class MeasureIV
    {
        private double _StartValue;
        public double StartValue
        {
            get { return _StartValue; }
            set { _StartValue = value; }
        }
        private double _EndValue;
        public double EndValue
        {
            get { return _EndValue; }
            set { _EndValue = value; }
        }
        private double _Step;
        public double Step
        {
            get { return _Step; }
            set { _Step = value; }
        }

        private int _NumberOfAverages;
        public int NumberOfAverages
        {
            get { return _NumberOfAverages; }
            set { _NumberOfAverages = value; }
        }

        private double _TimeDelay;
        public double TimeDelay
        {
            get { return _TimeDelay; }
            set { _TimeDelay = value; }
        }

        private SourceMode _sourceMode;
        public SourceMode sourceMode
        {
            get { return _sourceMode; }
            set { _sourceMode = value; }
        }

        private I_SMU _Device;
        public I_SMU Device
        {
            get { return _Device; }
            set { _Device = value; }
        }

        ChannelsToInvestigate _Channel;

        private bool _IsWaitNeeded = true;

        private AutoResetEvent _Thread_01_Step;
        private AutoResetEvent _Thread_02_Step;

        public MeasureIV(double startVal, double endVal, double step, int numberOfAverages, double timeDelay, SourceMode deviceSourceMode, I_SMU device, ChannelsToInvestigate Channel, ref AutoResetEvent __Thread_01_Step, ref AutoResetEvent __Thread_02_Step) 
        {
            _StartValue = startVal;
            _EndValue = endVal;
            _Step = step;
            _NumberOfAverages = numberOfAverages;
            _TimeDelay = timeDelay;
            _sourceMode = deviceSourceMode;
            _Device = device;
            _Channel = Channel;

            _Thread_01_Step = __Thread_01_Step;
            _Thread_02_Step = __Thread_02_Step;

            AllEventsHandler.Instance.IV_MeasurementsStateChanged += On_IV_MeasurementStateChanged;
        }

        public void StartMeasurement(object sender, DoWorkEventArgs e)
        {
            AllEventsHandler.Instance.OnIV_MeasurementsStateChanged(sender, new IV_MeasurementStateChanged_EventArgs(true));

            var worker = sender as BackgroundWorker;

            switch (_sourceMode)
            {
                case SourceMode.Voltage:
                    {
                        _Device.SwitchON();
                        for (double V = _StartValue; V <= _EndValue; V += _Step)
                        {
                            if (worker.CancellationPending == true)
                            {
                                AllEventsHandler.Instance.OnIV_MeasurementsStateChanged(sender, new IV_MeasurementStateChanged_EventArgs(false));
                                e.Cancel = true;
                                break;
                            }
                            else
                            {
                                var X = 0.0;
                                switch (_Channel)
                                {
                                    case ChannelsToInvestigate.Channel_01:
                                        {
                                            if(_IsWaitNeeded)
                                                _Thread_02_Step.WaitOne();
                                            
                                            X = V;
                                            _Device.SetSourceVoltage(V);
                                            var Y = _Device.MeasureCurrent(_NumberOfAverages, _TimeDelay);

                                            AllEventsHandler.Instance.OnIV_PointReceivedChannel_01(this, new IV_PointReceivedChannel_01_EventArgs(X, Y));
                                            
                                            if(_IsWaitNeeded)
                                                _Thread_01_Step.Set();
                                        } break;
                                    case ChannelsToInvestigate.Channel_02:
                                        {
                                            if(_IsWaitNeeded)
                                                _Thread_01_Step.WaitOne();

                                            X = V;
                                            _Device.SetSourceVoltage(V);
                                            var Y = _Device.MeasureCurrent(_NumberOfAverages, _TimeDelay);

                                            AllEventsHandler.Instance.OnIV_PointReceivedChannel_02(this, new IV_PointReceivedChannel_02_EventArgs(X, Y));

                                            if(_IsWaitNeeded)
                                                _Thread_02_Step.Set();
                                        } break;
                                    default:
                                        break;
                                }

                                worker.ReportProgress((int)(100 - (Math.Abs(_StartValue) + Math.Abs(_EndValue) - Math.Abs(_StartValue - X)) / (Math.Abs(_StartValue) + Math.Abs(_EndValue)) * 100));
                            }
                        }
                        _Device.SetSourceVoltage(0.0);
                        _Device.SwitchOFF();
                        worker.ReportProgress(0);
                        AllEventsHandler.Instance.OnIV_MeasurementsStateChanged(sender, new IV_MeasurementStateChanged_EventArgs(false));
                    } break;
                case SourceMode.Current:
                    {
                        _Device.SwitchON();
                        for (double I = _StartValue; I <= _EndValue; I += _Step)
                        {
                            if (worker.CancellationPending == true)
                            {
                                AllEventsHandler.Instance.OnIV_MeasurementsStateChanged(sender, new IV_MeasurementStateChanged_EventArgs(false));
                                e.Cancel = true;
                                break;
                            }
                            else
                            {
                                _Device.SetSourceCurrent(I);
                                var X = _Device.MeasureVoltage(_NumberOfAverages, _TimeDelay);
                                var Y = I;

                                if (!(double.IsNaN(X) || double.IsNaN(Y)))
                                {
                                    switch (_Channel)
                                    {
                                        case ChannelsToInvestigate.Channel_01:
                                            {
                                                AllEventsHandler.Instance.OnIV_PointReceivedChannel_01(this, new IV_PointReceivedChannel_01_EventArgs(X, Y));
                                            } break;
                                        case ChannelsToInvestigate.Channel_02:
                                            {
                                                AllEventsHandler.Instance.OnIV_PointReceivedChannel_02(this, new IV_PointReceivedChannel_02_EventArgs(X, Y));
                                            } break;
                                        default:
                                            break;
                                    }

                                    worker.ReportProgress((int)(100 - (Math.Abs(_StartValue) + Math.Abs(_EndValue) - Math.Abs(_StartValue - X)) / (Math.Abs(_StartValue) + Math.Abs(_EndValue)) * 100));
                                }
                            }
                        }
                        _Device.SetSourceCurrent(0.0);
                        _Device.SwitchOFF();
                        worker.ReportProgress(0);
                        AllEventsHandler.Instance.OnIV_MeasurementsStateChanged(sender, new IV_MeasurementStateChanged_EventArgs(false));
                    } break;
                default:
                    break;
            }
        }

        private void On_IV_MeasurementStateChanged(object sender, IV_MeasurementStateChanged_EventArgs e)
        {
            _IsWaitNeeded = e.IV_MeasurementState;
        }
    }
}
