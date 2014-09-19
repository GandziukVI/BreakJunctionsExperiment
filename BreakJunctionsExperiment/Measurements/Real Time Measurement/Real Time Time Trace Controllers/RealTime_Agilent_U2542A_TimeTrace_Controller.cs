using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aids.Graphics;
using Agilent_U2542A;
using Agilent_U2542A_ExtensionBox;

namespace BreakJunctions.Measurements
{
    class RealTime_Agilent_U2542A_TimeTrace_Controller : RealTime_TimeTrace_Controller
    {
        #region RealTime_Agilent_U2542A_TimeTrace_Controller settings

        private bool _MeasurementInProcess;
        public bool MeasurementInProcess
        {
            get { return _MeasurementInProcess; }
            set { _MeasurementInProcess = value; }
        }

        private Agilent_U2542A_DigitalOutput _DIO;
        private AnalogInputChannels _Channels = AnalogInputChannels.Instance;
        private DataStringConverter _DataConverter;
        private VoltageMeasurement _VoltageMeasurement;

        #endregion

        #region Constructor / Destructor

        public RealTime_Agilent_U2542A_TimeTrace_Controller(string deviceID)
        {
            _DIO = new Agilent_U2542A_DigitalOutput(deviceID);

            _Channels.Read_AI_Channel_Status();

            _DataConverter = new DataStringConverter();
            _VoltageMeasurement = new VoltageMeasurement();
        }

        ~RealTime_Agilent_U2542A_TimeTrace_Controller()
        {
            this.Dispose();
        }

        #endregion

        #region RealTime_TimeTrace_Controller implementation

        public override void ContiniousAcquisition()
        {
            _DIO.AllToZero();
            
            int ACQ_Rate = _Channels.ACQ_Rate;
            
            _Channels.SetChannelsToAC();
            _Channels.StartAnalogAcqusition();

            while (_MeasurementInProcess == true)
            {
                while (!_Channels.CheckAcquisitionStatus()) ;
                string result = _Channels.AcquireStringWithData();
                Int16[] resultInt = _DataConverter.ParseDataStringToInt(result);
                List<PointD>[] ChannelData = _DataConverter.ParseIntArrayIntoChannelData(resultInt, ACQ_Rate);
            }

            _Channels.StopAnalogAcqusition();
        }

        public override void ContiniousAcquisitionWithPresiseVoltageMeasurement()
        {
            _DIO.AllToZero();

            _VoltageMeasurement.PerformVoltagePresiseMeasurement();
            
            if (!_MeasurementInProcess)
                return;

            _Channels.Read_AI_Channel_Status();

            int ACQ_Rate = _Channels.ACQ_Rate;
            
            _Channels.SetChannelsToAC();
            _Channels.StartAnalogAcqusition();

            while ((_MeasurementInProcess))// && (TimeTraceFileManager.Instance.WritingInProgress))
            {
                while (!_Channels.CheckAcquisitionStatus()) ;

                string result = _Channels.AcquireStringWithData();
                
                Int16[] resultInt = _DataConverter.ParseDataStringToInt(result);
                
                List<PointD>[] ChannelData = _DataConverter.ParseIntArrayIntoChannelData(resultInt, ACQ_Rate);
            }
            _Channels.StopAnalogAcqusition();

            if (!_MeasurementInProcess)
            {
                //AllCustomEvents.Instance.BeforeMeasurementFinished += RestartPreciseVoltageMeasurementInThread;
            }
            else
            {
                _VoltageMeasurement.PerformVoltagePresiseMeasurement();
            }
        }

        public override List<PointD> MakeSingleShot(int NumberOfChannel)
        {
            _Channels.DisableAllChannelsForContiniousDataAcquisition();

            _Channels.ChannelArray[NumberOfChannel - 1].Enabled = true;
            _Channels.ChannelArray[NumberOfChannel - 1].ChannelProperties.isAC = true;

            _Channels.Read_AI_Channel_Status();

            int ACQ_Rate = _Channels.ACQ_Rate;

            _Channels.AcquireSingleShot();

            while ((!_Channels.CheckSingleShotAcquisitionStatus()) && _MeasurementInProcess) ;
            
            if (!_MeasurementInProcess) 
                return null;

            string result = _Channels.AcquireStringWithData();

            Int16[] resultInt = _DataConverter.ParseDataStringToInt(result);
            
            List<PointD>[] ChannelData = _DataConverter.ParseIntArrayIntoChannelData(resultInt, ACQ_Rate);

            return ChannelData[NumberOfChannel - 1];
        }

        public override void startAC_Autorange(int NumberOfChannel)
        {
            _Channels.SingleShotPointsPerBlock = 10000;
            _Channels.ChannelArray[NumberOfChannel - 1].AC_Range = 10;
            
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

        #region Disposing the instance

        public override void Dispose()
        {
            _DIO.Dispose();
            _VoltageMeasurement.Dispose();
        }

        #endregion

        #endregion
    }
}
