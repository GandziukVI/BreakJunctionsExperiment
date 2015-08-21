using Agilent_ExtensionBox;
using Agilent_ExtensionBox.IO;
using Agilent_U2542A_With_ExtensionBox.Classes;
using BreakJunctions.Events;
using FourierTransformProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BreakJunctions.Measurements
{
    class MeasureNoise : IDisposable
    {
        #region MeasureNoise settings

        public bool MeasurementInProgress { get; set; }

        private int _NumberOfSpectra;
        private int _DisplayUpdateNumber;

        private BackgroundWorker _MeasurementWorker;

        //private VoltageMeasurement _VoltageMeasurement;
        //private IRealTime_TimeTrace_Factory _TimeTracesAcquisitionfactory;
        //private RealTime_TimeTrace_Controller _TimeTracesAcquisition;
        //private AI_Channels _Channels;
        //private DataStringConverter _DataConverter;

        private BoxController _boxController;

        private AdvancedFourierTransform _FFT_Channel_01;
        private AdvancedFourierTransform _FFT_Channel_02;

        private Thread _FFT_Processing_Channel_01;
        private Thread _FFT_Processing_Channel_02;

        private List<Point> FinalFFT_Channel_01;
        private List<Point> FinalFFT_Channel_02;
        
        private int AveragedSpectraCounter_Channel_01;
        private int AveragedSpectraCounter_Channel_02;

        private Queue<List<Point>> QueueToGetProcessed_Channel_01;
        private Queue<List<Point>> QueueToGetProcessed_Channel_02;

        private bool _FirstChannel_NewSpectraArrived = false;
        private bool _SecondChannel_NewSpectraArrived = false;

        private bool _FirstChannel_LastSpectraArrived = false;
        private bool _SecondChannel_LastSpectraArrived = false;

        private double _AmplificationCoefficient_CH_01 = 10000.0;
        public double AmplificationCoefficient_CH_01
        {
            get { return _AmplificationCoefficient_CH_01; }
            set { _AmplificationCoefficient_CH_01 = value; }
        }

        private double _AmplificationCoefficient_CH_02 = 10000.0;
        public double AmplificationCoefficient_CH_02
        {
            get { return _AmplificationCoefficient_CH_02; }
            set { _AmplificationCoefficient_CH_02 = value; }
        }

        #endregion

        #region Constructor / Destructor

        public MeasureNoise(int __NumberOfSpectra, int __DisplayUpdateNumber, ref BackgroundWorker __MeasurementWorker, double __AmplificationCoefficient_CH_01 = 10000.0, double __AmplificationCoefficient_CH_02 = 10000.0)
        {
            _NumberOfSpectra = __NumberOfSpectra;
            _DisplayUpdateNumber = __DisplayUpdateNumber;

            _AmplificationCoefficient_CH_01 = __AmplificationCoefficient_CH_01;
            _AmplificationCoefficient_CH_02 = __AmplificationCoefficient_CH_02;

            _MeasurementWorker = __MeasurementWorker;

            //_VoltageMeasurement = new VoltageMeasurement();

            //_TimeTracesAcquisitionfactory = new RT_Agilent_U2542A_TimeTrace_Controller_Factory();
            //_TimeTracesAcquisition = _TimeTracesAcquisitionfactory.GetRealTime_TimeTraceController();

            _boxController = new BoxController();
            _boxController.Init("USB0::0x0957::0x1718::TW54334510::0::INSTR");

            _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn2].DataReady += MeasureNoise_CH_02_DataReady;            
            _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn4].DataReady += MeasureNoise_CH_04_DataReady;

            _FFT_Channel_01 = new AdvancedFourierTransform(DigitalAnalyzerNamespace.DigitalAnalyzerSpectralRange.Discret499712Freq1_1600Step1Freq1647_249856Step61);
            _FFT_Channel_02 = new AdvancedFourierTransform(DigitalAnalyzerNamespace.DigitalAnalyzerSpectralRange.Discret499712Freq1_1600Step1Freq1647_249856Step61);

            //_Channels = AI_Channels.Instance;
            //_DataConverter = new DataStringConverter();

            QueueToGetProcessed_Channel_01 = new Queue<List<Point>>();
            QueueToGetProcessed_Channel_02 = new Queue<List<Point>>();

            FinalFFT_Channel_01 = new List<Point>();
            FinalFFT_Channel_02 = new List<Point>();

            this._FFT_Processing_Channel_01 = new Thread(new ThreadStart(MakeFFTOfQueue_Channel_01));
            this._FFT_Processing_Channel_01.Priority = ThreadPriority.Highest;

            AllEventsHandler.Instance.NoiseMeasurement_StateChanged += On_NoiseMeasurement_StateChanged;
        }

        void MeasureNoise_CH_02_DataReady(object sender, EventArgs e)
        {
            var res = new LinkedList<Point>();
            var readSuccess = _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn2].ChannelData.TryDequeue(out res);

            var toProcess = res.ToList<Point>();

            if(readSuccess)
                QueueToGetProcessed_Channel_01.Enqueue(toProcess);
        }

        void MeasureNoise_CH_04_DataReady(object sender, EventArgs e)
        {
            var res = new LinkedList<Point>();
            var readSuccess = _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn4].ChannelData.TryDequeue(out res);

            var toProcess = res.ToList<Point>();

            if (readSuccess)
                QueueToGetProcessed_Channel_01.Enqueue(toProcess);
        }

        ~MeasureNoise()
        {
            this.Dispose();
        }

        #endregion

        #region MeasureNoise functionality

        public void MakeNoiseMeasurement()
        {
            _FirstChannel_LastSpectraArrived = false;
            _SecondChannel_LastSpectraArrived = false;

            //Agilent_DigitalOutput_LowLevel.Instance.AllToZero();

            //_VoltageMeasurement.PerformVoltagePresiseMeasurement();

            //_Channels.Read_AI_Channel_Status();
            //var ACQ_Rate = _Channels.ACQ_Rate;
            //_Channels.SetChannelsToAC();
            //_Channels.ACQ_Rate = 499712;
            //_Channels.SingleShotPointsPerBlock = 499712;
            
            QueueToGetProcessed_Channel_01.Clear();
            QueueToGetProcessed_Channel_02.Clear();

            AveragedSpectraCounter_Channel_01 = 0;
            AveragedSpectraCounter_Channel_02 = 0;

            //FillFrequenciesInFinalFFT(_Channels.SingleShotPointsPerBlock);
            FillFrequenciesInFinalFFT(499712);

            for (int i = 0; (i < _NumberOfSpectra) && (MeasurementInProgress) && !_MeasurementWorker.CancellationPending; i++)
            {
                //QueueToGetProcessed_Channel_01.Enqueue(_TimeTracesAcquisition.MakeSingleShot(2));
                //QueueToGetProcessed_Channel_02.Enqueue(_TimeTracesAcquisition.MakeSingleShot(4));
                
                _boxController.AcquireSingleShot(499712);

                if (!(_FFT_Processing_Channel_01.IsAlive && _FFT_Processing_Channel_02.IsAlive))
                    this.StartFFTThread();
            }

            while(!((_FirstChannel_LastSpectraArrived == true) && (_SecondChannel_LastSpectraArrived == true) || _MeasurementWorker.CancellationPending));

            //if (MeasurementInProgress)
            //    _VoltageMeasurement.PerformVoltagePresiseMeasurement();

            //_Channels.ACQ_Rate = ACQ_Rate;
        }

        private void StartFFTThread()
        {
            this._FFT_Processing_Channel_01 = new Thread(new ThreadStart(MakeFFTOfQueue_Channel_01));
            this._FFT_Processing_Channel_02 = new Thread(new ThreadStart(MakeFFTOfQueue_Channel_02));

            this._FFT_Processing_Channel_01.Priority = ThreadPriority.Highest;
            this._FFT_Processing_Channel_02.Priority = ThreadPriority.Highest;

            this._FFT_Processing_Channel_01.Start();
            this._FFT_Processing_Channel_02.Start();
        }
        private void FillFrequenciesInFinalFFT(int NumberOFPoints)
        {
            FinalFFT_Channel_01.Clear();
            FinalFFT_Channel_02.Clear();

            FinalFFT_Channel_01 = _FFT_Channel_01.GetFrequencyList();
            FinalFFT_Channel_02 = _FFT_Channel_02.GetFrequencyList();
        }

        private void MakeFFTOfQueue_Channel_01()
        {
            while ((QueueToGetProcessed_Channel_01.Count != 0) && MeasurementInProgress && (AveragedSpectraCounter_Channel_01 <= _NumberOfSpectra))
            {
                var result = _FFT_Channel_01.AdvancedFFT(QueueToGetProcessed_Channel_01.Dequeue());
                AddPointListToFinal_Channel_01(result);

                AveragedSpectraCounter_Channel_01++;
                _FirstChannel_NewSpectraArrived = true;
                if (_FirstChannel_NewSpectraArrived && _SecondChannel_NewSpectraArrived)
                {
                    _FirstChannel_NewSpectraArrived = false;
                    _SecondChannel_NewSpectraArrived = false;

                    try
                    {
                        _MeasurementWorker.ReportProgress((int)((double)AveragedSpectraCounter_Channel_01 / _NumberOfSpectra * 100.0));
                    }
                    catch { }
                }
                
                if ((AveragedSpectraCounter_Channel_01 % _DisplayUpdateNumber == 0) && (AveragedSpectraCounter_Channel_01 < _NumberOfSpectra))
                    AllEventsHandler.Instance.On_NoiseSpectra_DataArrived_Channel_01(this, new NoiseSpectra_DataArrived_Channel_01_EventArgs(DividePointList(FinalFFT_Channel_01, AveragedSpectraCounter_Channel_01)));
            }
            if (AveragedSpectraCounter_Channel_01 >= _NumberOfSpectra)
            {
                List<Point> RawData = DividePointList(FinalFFT_Channel_01, AveragedSpectraCounter_Channel_01);
                List<Point> FinalData = DividePointList(RawData, _AmplificationCoefficient_CH_01 * _AmplificationCoefficient_CH_01);
                
                AllEventsHandler.Instance.On_LastNoiseSpectra_Channel_01_DataArrived(this, new LastNoiseSpectra_Channel_01_DataArrived_EventArgs(FinalData));
                _FirstChannel_LastSpectraArrived = true;
            }
        }

        private void MakeFFTOfQueue_Channel_02()
        {
            while ((QueueToGetProcessed_Channel_02.Count != 0) && MeasurementInProgress && (AveragedSpectraCounter_Channel_01 <= _NumberOfSpectra))
            {
                var result = _FFT_Channel_02.AdvancedFFT(QueueToGetProcessed_Channel_02.Dequeue());
                AddPointListToFinal_Channel_02(result);

                AveragedSpectraCounter_Channel_02++;
                _SecondChannel_NewSpectraArrived = true;
                if (_FirstChannel_NewSpectraArrived && _SecondChannel_NewSpectraArrived)
                {
                    _FirstChannel_NewSpectraArrived = false;
                    _SecondChannel_NewSpectraArrived = false;

                    try
                    {
                        _MeasurementWorker.ReportProgress((int)((double)AveragedSpectraCounter_Channel_01 / _NumberOfSpectra * 100.0));
                    }
                    catch { }
                }

                if ((AveragedSpectraCounter_Channel_02 % _DisplayUpdateNumber == 0) && (AveragedSpectraCounter_Channel_02 < _NumberOfSpectra))
                    AllEventsHandler.Instance.On_NoiseSpectra_DataArrived_Channel_02(this, new NoiseSpectra_DataArrived_Channel_02_EventArgs(DividePointList(FinalFFT_Channel_02, AveragedSpectraCounter_Channel_02)));
            }
            if (AveragedSpectraCounter_Channel_02 >= _NumberOfSpectra)
            {
                List<Point> RawData = DividePointList(FinalFFT_Channel_02, AveragedSpectraCounter_Channel_02);
                List<Point> FinalData = DividePointList(RawData, _AmplificationCoefficient_CH_02 * _AmplificationCoefficient_CH_02);
                
                AllEventsHandler.Instance.On_LastNoiseSpectra_Channel_02_DataArrived(this, new LastNoiseSpectra_Channel_02_DataArrived_EventArgs(FinalData));
                _SecondChannel_LastSpectraArrived = true;
            }

        }

        private void AddPointListToFinal_Channel_01(List<Point> data)
        {
            for (int i = 0; i < FinalFFT_Channel_01.Count; i++)
            {
                var temp = new Point();

                temp.X = FinalFFT_Channel_01[i].X;
                temp.Y = FinalFFT_Channel_01[i].Y + data[i].Y;

                FinalFFT_Channel_01[i] = temp;
            }
        }

        private void AddPointListToFinal_Channel_02(List<Point> data)
        {
            for (int i = 0; i < FinalFFT_Channel_02.Count; i++)
            {
                var temp = new Point();

                temp.X = FinalFFT_Channel_02[i].X;
                temp.Y = FinalFFT_Channel_02[i].Y + data[i].Y;

                FinalFFT_Channel_02[i] = temp;
            }
        }

        private List<Point> DividePointList(List<Point> data, double divider)
        {
            List<Point> result = new List<Point>();
            foreach (Point a in data)
            {
                result.Add(new Point(a.X, a.Y / divider));
            }
            return result;
        }

        private void On_NoiseMeasurement_StateChanged(object sender, NoiseMeasurement_StateChanged_EventArgs e)
        {
            //_TimeTracesAcquisition.MeasurementInProcess = e.MeasurementIsInProgress;
            _boxController.AcquisitionInProgress = e.MeasurementIsInProgress;
            this.MeasurementInProgress = e.MeasurementIsInProgress;
        }

        #endregion

        #region Disposing the instance

        public void Dispose()
        {
            _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn2].DataReady -= MeasureNoise_CH_02_DataReady;
            _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn4].DataReady -= MeasureNoise_CH_04_DataReady;

            _boxController.Close();

            FinalFFT_Channel_01.Clear();
            FinalFFT_Channel_02.Clear();

            QueueToGetProcessed_Channel_01.Clear();
            QueueToGetProcessed_Channel_02.Clear();

            AllEventsHandler.Instance.NoiseMeasurement_StateChanged -= On_NoiseMeasurement_StateChanged;
        }

        #endregion
    }
}
