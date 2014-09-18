using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;
using Agilent_U2542A_ExtensionBox;

namespace BreakJunctions.Measurements
{
    class VoltageMeasurement
    {
        #region VoltageMeasurement settings

        private Agilent_U2542A_AnalogInput _AI;
        private AnalogInputChannels _Channels = AnalogInputChannels.Instance;

        private bool _MeasurementInProcess;
        private bool MeasurementInProcess
        {
            get { return _MeasurementInProcess; }
            set { _MeasurementInProcess = value; }
        }

        #endregion

        #region Constructor

        public VoltageMeasurement()
        {
            _AI = new Agilent_U2542A_AnalogInput(_Channels.DeviceID);
        }

        #endregion

        #region VoltageMeasurement functionality

        public void PerformVoltagePresiseMeasurement()
        {

            int AveragingFactor = 10;
            List<double[]> results = new List<double[]> { new double[AveragingFactor], new double[AveragingFactor], new double[AveragingFactor], new double[AveragingFactor] };
            double[] result = new double[] { };
            
            int prevAverage = _Channels.DC_Average;
            
            _Channels.DC_Average = 1000;

            for (int i = 0; i < AveragingFactor; i++)
            {
                if (_MeasurementInProcess) 
                    return;

                result = _Channels.VoltageMeasurement101_104();
                
                for (int j = 0; j < results.Count; j++)
                    results[j][i] = result[j];
            }
            
            result = new double[] { 0, 0, 0, 0 };
            
            foreach (double[] ChannelVoltages in results)
            {
                int count = 0;
                double min = ChannelVoltages.Min();
                double max = ChannelVoltages.Max();
                for (int i = 0; i < ChannelVoltages.Length; i++)
                {
                    if ((ChannelVoltages[i] > min) && (ChannelVoltages[i] < max))
                    {
                        count++;
                        result[results.IndexOf(ChannelVoltages)] += ChannelVoltages[i];
                    }
                }
                if (count != 0)
                    result[results.IndexOf(ChannelVoltages)] /= count;

            }
            
            _Channels.DC_Average = prevAverage;
        }

        private void PerformVoltageNormalMeasurement()
        {
            if (_MeasurementInProcess == false)
                return;
            
            double[] result = _Channels.VoltageMeasurement101_104();
            if (_MeasurementInProcess == false)
                return;
        }

        #endregion
    }
}
