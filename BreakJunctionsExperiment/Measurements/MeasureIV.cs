using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hardware;
using BreakJunctions.Events;
using System.ComponentModel;

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

        public MeasureIV() { }
        
        public MeasureIV(double startVal, double endVal, double step, int numberOfAverages, double timeDelay, SourceMode deviceSourceMode, I_SMU device) 
        {
            _StartValue = startVal;
            _EndValue = endVal;
            _Step = step;
            _NumberOfAverages = numberOfAverages;
            _TimeDelay = timeDelay;
            _sourceMode = deviceSourceMode;
            _Device = device;
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
                                var X = V;
                                _Device.SetSourceVoltage(V);
                                var Y = _Device.MeasureCurrent(_NumberOfAverages, _TimeDelay);

                                if (!(double.IsNaN(X) || double.IsNaN(Y)))
                                {
                                    AllEventsHandler.Instance.OnIV_PointReceived(null, new IV_PointReceived_EventArgs(X, Y));

                                    worker.ReportProgress((int)(Math.Abs(1.0 - (_EndValue - X) / _EndValue) * 100 + 1));
                                }
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
                                   AllEventsHandler.Instance.OnIV_PointReceived(null, new IV_PointReceived_EventArgs(X, Y));
                                   worker.ReportProgress((int)(Math.Abs(1.0 - (_EndValue - X) / _EndValue) * 100 + 1));
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
    }
}
