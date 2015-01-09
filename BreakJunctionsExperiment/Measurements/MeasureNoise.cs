using Agilent_U2542A_With_ExtensionBox.Classes;
using Aids.Graphics;
using FourierTransformProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZedGraph;

namespace BreakJunctions.Measurements
{
    public static class Extension
    {
        public static List<Aids.Graphics.PointD> AsListPointD(this ZedGraph.PointPairList obj)
        {
            var result = new List<Aids.Graphics.PointD>();
            for (int i = 0; i < obj.Count; i++)
            {
                result.Add(new Aids.Graphics.PointD(obj[i].X, obj[i].Y));
            }

            return result;
        }

        public static PointPairList AsPPL(this List<Aids.Graphics.PointD> obj)
        {
            var result = new PointPairList();
            for (int i = 0; i < obj.Count; i++)
            {
                result.Add(obj[i].X, obj[i].Y);
            }

            return result;
        }
    }

    class MeasureNoise : IDisposable
    {
        #region MeasureNoise settings

        public bool MeasurementInProgress { get; set; }

        private int _NumberOfSpectra;
        private int _DisplayUpdateNumber;

        private BackgroundWorker _MeasurementWorker;

        private VoltageMeasurement _VoltageMeasurement;
        private IRealTime_TimeTrace_Factory _TimeTracesAcquisitionfactory;
        private RealTime_TimeTrace_Controller _TimeTracesAcquisition;
        private AI_Channels _Channels;
        private DataStringConverter _DataConverter;
        private AdvancedFourierTransform _FFT;
        private Thread _FFT_Processing;
        private PointPairList FinalFFT;
        private int AveragedSpectraCounter;
        private Queue<PointPairList> QueueToGetProcessed;
        //public int Averaging;
        //public int SpectraPerShow;

        #endregion

        #region Constructor / Destructor

        public MeasureNoise(int __NumberOfSpectra, int __DisplayUpdateNumber, ref BackgroundWorker __MeasurementWorker)
        {
            _NumberOfSpectra = __NumberOfSpectra;
            _DisplayUpdateNumber = __DisplayUpdateNumber;

            _MeasurementWorker = __MeasurementWorker;

            _VoltageMeasurement = new VoltageMeasurement();

            _TimeTracesAcquisitionfactory = new RT_Agilent_U2542A_TimeTrace_Controller_Factory();
            _TimeTracesAcquisition = _TimeTracesAcquisitionfactory.GetRealTime_TimeTraceController();

            _FFT = new AdvancedFourierTransform(DigitalAnalyzerNamespace.DigitalAnalyzerSpectralRange.Discret499712Freq1_1600Step1Freq1647_249856Step61);
            _Channels = AI_Channels.Instance;
            _DataConverter = new DataStringConverter();
            QueueToGetProcessed = new Queue<PointPairList>();
            FinalFFT = new PointPairList();
            this._FFT_Processing = new Thread(new ThreadStart(MakeFFTOfQueue));
            this._FFT_Processing.Priority = ThreadPriority.Highest;
        }

        ~MeasureNoise()
        {
            this.Dispose();
        }

        #endregion

        #region MeasureNoise functionality

        public void MakeNoiseMeasurement()
        {
            Agilent_DigitalOutput_LowLevel.Instance.AllToZero();

            _VoltageMeasurement.PerformVoltagePresiseMeasurement();

            _Channels.Read_AI_Channel_Status();
            var ACQ_Rate = _Channels.ACQ_Rate;
            _Channels.SetChannelsToAC();
            _Channels.ACQ_Rate = 499712;
            _Channels.SingleShotPointsPerBlock = 499712;
            var DataPack = new PointPairList();
            QueueToGetProcessed.Clear();
            AveragedSpectraCounter = 0;
            FillFrequenciesInFinalFFT(_Channels.SingleShotPointsPerBlock);
            for (int i = 0; (i < _NumberOfSpectra) && (MeasurementInProgress); i++)
            {
                QueueToGetProcessed.Enqueue(_TimeTracesAcquisition.MakeSingleShot(1).AsPPL());
                if (!_FFT_Processing.IsAlive) this.StartFFTThread();
            }

            if (MeasurementInProgress)
                _VoltageMeasurement.PerformVoltagePresiseMeasurement();

            _Channels.ACQ_Rate = ACQ_Rate;
        }

        private void StartFFTThread()
        {
            this._FFT_Processing = new Thread(new ThreadStart(MakeFFTOfQueue));
            this._FFT_Processing.Priority = ThreadPriority.Highest;
            this._FFT_Processing.Start();
        }
        private void FillFrequenciesInFinalFFT(int NumberOFPoints)
        {
            FinalFFT.Clear();
            FinalFFT = _FFT.GetFrequencyList();
            //double ACQ_rate = _Channels.ACQ_Rate;
            //double NumberOfPoints_d = NumberOFPoints;
            //for (int i = 0; i < NumberOFPoints / 2; i++)
            //{
            //    FinalFFT.Add((i*(ACQ_rate/NumberOfPoints_d)),0);
            //}

        }
        private void MakeFFTOfQueue()
        {
            while ((QueueToGetProcessed.Count != 0) && MeasurementInProgress && (AveragedSpectraCounter <= _NumberOfSpectra))
            {
                PointPairList result = _FFT.AdvancedFFT(QueueToGetProcessed.Dequeue());//.makeFFT(QueueToGetProcessed.Dequeue());
                AddPointPairListToFinal(result);
                AveragedSpectraCounter++;
                //AllCustomEvents.Instance.OnNoiseMeasurementStatusChanged(this, new StatusEventArgs("Spectra Acquired " + AveragedSpectraCounter + "/" + Averaging, 1, Averaging, AveragedSpectraCounter));
                //if (AveragedSpectraCounter % SpectraPerShow == 0)
                //    AllCustomEvents.Instance.OnNoiseSpectraArrived(this, new NoiseEventArgs(DividePointPairList(FinalFFT, AveragedSpectraCounter)));
            }
            if (AveragedSpectraCounter >= _NumberOfSpectra)
            {
                PointPairList RawData = DividePointPairList(FinalFFT, AveragedSpectraCounter);
                PointPairList FinalData = DividePointPairList(RawData, ImportantConstants.K_Ampl_first_Channel * ImportantConstants.K_Ampl_first_Channel);
                //AllCustomEvents.Instance.OnLastNoiseSpectraArrived(this, new FinalNoiseEventArgs(RawData, FinalData, "last spectra"));
            }

        }
        private void AddPointPairListToFinal(PointPairList data)
        {
            for (int i = 0; i < FinalFFT.Count; i++)
            {
                FinalFFT[i].Y += data[i].Y;
            }

        }
        private PointPairList DividePointPairList(PointPairList data, double divider)
        {
            PointPairList result = new PointPairList();
            foreach (PointPair a in data)
            {
                result.Add(a.X, a.Y / divider);
            }
            return result;
        }

        #endregion

        #region Disposing the instance

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
