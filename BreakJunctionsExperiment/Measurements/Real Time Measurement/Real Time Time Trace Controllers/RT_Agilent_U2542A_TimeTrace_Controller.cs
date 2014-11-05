using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Agilent_U2542A_With_ExtensionBox.Classes;
using Agilent_U2542A_With_ExtensionBox.Interfaces;

using Aids.Graphics;
using BreakJunctions.Events;

namespace BreakJunctions.Measurements
{
    class RT_Agilent_U2542A_TimeTrace_Controller : RealTime_TimeTrace_Controller
    {
        #region RealTime_Agilent_U2542A_TimeTrace_Controller settings

        private AI_Channels _Channels;
        private DataStringConverter _DataConverter;
        private VoltageMeasurement _VoltageMeasurement;

        #endregion

        #region Constructor

        public RT_Agilent_U2542A_TimeTrace_Controller()
        {
            _Channels = AI_Channels.Instance;
            _Channels.Read_AI_Channel_Status();
            _DataConverter = new DataStringConverter();
            _VoltageMeasurement = new VoltageMeasurement();

            AllEventsHandler.Instance.RealTime_TimeTraceMeasurementStateChanged += OnRealTime_TimeTraceMeasurementStateChanged;
        }

        private int ACQ_Rate;

        ConcurrentQueue<string> _StringDataQueue;
        Thread _DataTransformingAndSendingThread;

        #endregion

        private void _TransformAndEmitData()
        {
            while(MeasurementInProcess)
            {
                string _Data;

                var _DequeueSuccess = _StringDataQueue.TryDequeue(out _Data);

                if(_DequeueSuccess == true)
                {
                    var resultInt = _DataConverter.ParseDataStringToInt(_Data);
                    var ChannelData = _DataConverter.ParseIntArrayIntoChannelData(resultInt, ACQ_Rate);
                    AllEventsHandler.Instance.OnRealTime_TimeTraceDataArrived(this, new RealTime_TimeTrace_DataArrived_EventArgs(ref ChannelData));
                }
            }
        }

        public override void ContiniousAcquisition()
        {
            _StringDataQueue = new ConcurrentQueue<string>();

            Agilent_DigitalOutput_LowLevel.Instance.AllToZero();
            _Channels.Read_AI_Channel_Status();
            ACQ_Rate = _Channels.ACQ_Rate;
            _Channels.SetChannelsToDC();
            _Channels.StartAnalogAcqusition();

            _DataTransformingAndSendingThread = new Thread(_TransformAndEmitData);
            _DataTransformingAndSendingThread.Priority = ThreadPriority.Normal;
            _DataTransformingAndSendingThread.Start();

            while(MeasurementInProcess)
            {
                while (!_Channels.CheckAcquisitionStatus()) ;
                string result = _Channels.AcquireStringWithData();
                _StringDataQueue.Enqueue(result);
            }

            _Channels.StopAnalogAcqusition();
        }

        public override void ContiniousAcquisitionWithPresiseVoltageMeasurement()
        {
            Agilent_DigitalOutput_LowLevel.Instance.AllToZero();

            _VoltageMeasurement.PerformVoltagePresiseMeasurement();
            if (!MeasurementInProcess)
                return;

            _Channels.Read_AI_Channel_Status();
            int ACQ_Rate = _Channels.ACQ_Rate;
            _Channels.SetChannelsToDC();
            _Channels.StartAnalogAcqusition();

            while (MeasurementInProcess)
            {
                while (!_Channels.CheckAcquisitionStatus()) ;
                string result = AI_Channels.Instance.AcquireStringWithData();
                _StringDataQueue.Enqueue(result);
            }

            _Channels.StopAnalogAcqusition();
        }

        public override List<PointD> MakeSingleShot(int NumberOfChannel)
        {
            _Channels.DisableAllChannelsForContiniousDataAcquisition();
            _Channels.ChannelArray[NumberOfChannel - 1].Enabled = true;
            _Channels.ChannelArray[NumberOfChannel - 1].ChannelProperties.isAC = true;
            _Channels.Read_AI_Channel_Status();
            int ACQ_Rate = _Channels.ACQ_Rate;
            _Channels.AcquireSingleShot();
            while ((!_Channels.CheckSingleShotAcquisitionStatus()) && MeasurementInProcess) ;
            if (!MeasurementInProcess) return null;
            string result = AI_Channels.Instance.AcquireStringWithData();
            Int16[] resultInt = _DataConverter.ParseDataStringToInt(result);
            List<PointD>[] ChannelData = _DataConverter.ParseIntArrayIntoChannelData(resultInt, ACQ_Rate);

            return ChannelData[NumberOfChannel - 1];
        }

        public override void startAC_Autorange(int NumberOfChannel)
        {
            AI_Channels.Instance.SingleShotPointsPerBlock = 10000;
            AI_Channels.Instance.ChannelArray[NumberOfChannel - 1].AC_Range = 10;
            List<PointD> data = MakeSingleShot(NumberOfChannel);

            double[] AcquidredYData = data.Select(p => p.Y).ToArray();
            double Max = AcquidredYData.Max();
            double Min = AcquidredYData.Min();

            if (Min < 0) _Channels.ChannelArray[NumberOfChannel - 1].isBiPolarAC = true;
            else _Channels.ChannelArray[NumberOfChannel - 1].isBiPolarAC = false;

            Min = Math.Abs(Min);
            Max = Math.Max(Min, Max);

            foreach (double rng in ImportantConstants.Ranges)
            {
                if (Max < rng)
                {
                    _Channels.ChannelArray[NumberOfChannel - 1].AC_Range = rng;
                    break;
                }
            }
        }

        public override void Dispose()
        {
            AllEventsHandler.Instance.RealTime_TimeTraceMeasurementStateChanged -= OnRealTime_TimeTraceMeasurementStateChanged;
        }

        public void OnRealTime_TimeTraceMeasurementStateChanged(object sender, RealTime_TimeTraceMeasurementStateChanged_EventArgs e)
        {
            this.MeasurementInProcess = e.MeasurementInProcess;
        }
    }
}
