using FourierTransformProvider;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;


namespace DigitalAnalyzerNamespace
{


    public delegate void NewSpectraProcessed(object sender, List<Point> SpectraData);
    public class DigitalAnalyzer
    {
        public event NewSpectraProcessed SpectraProcessedEvent;
        public event Action DataArrivedEvent;
        private DigitizingInfo m_digitizingInfo;
        private AdvancedFTRangeHandler[] m_Ranges;
        private BackgroundWorker m_DataQueueHandler;
        private ConcurrentQueue<FourierDataSet> m_DataQueue;
        private AdvancedFTInitialization m_RangeCreator;

        public DigitalAnalyzer(DigitalAnalyzerSpectralRange Range)
        {
            m_RangeCreator = new AdvancedFTInitialization(Range);
            m_Ranges = m_RangeCreator.FourierTransformRanges;
            m_digitizingInfo = m_RangeCreator.DigitizingInfo;
            //m_Ranges = DigitalAnalyzerRangesCreator.CreateRanges(Range);
            DataArrivedEvent += DigitalAnalyzer_DataArrivedEvent;
            m_DataQueue = new ConcurrentQueue<FourierDataSet>();

            SpectraProcessedEvent += DigitalAnalyzer_SpectraProcessedEvent;

            m_DataQueueHandler = new BackgroundWorker();
            m_DataQueueHandler.WorkerSupportsCancellation = true;
            m_DataQueueHandler.WorkerReportsProgress = true;
            m_DataQueueHandler.Disposed += DataQueueHandler_Disposed;
            m_DataQueueHandler.DoWork += DataQueueHandler_DoWork;
            m_DataQueueHandler.ProgressChanged += DataQueueHandler_ProgressChanged;
            m_DataQueueHandler.RunWorkerCompleted += DataQueueHandler_RunWorkerCompleted;

            m_PSDToWrite = new List<Point>();
        }

        private List<Point> m_PSDToWrite;
        private int spectraCount = 0;

        void DigitalAnalyzer_SpectraProcessedEvent(object sender, List<Point> SpectraData)
        {
            if (m_PSDToWrite.Count == 0)
                m_PSDToWrite = SpectraData;
            else
                for (int i = 0; i < m_PSDToWrite.Count; i++)
                {
                    m_PSDToWrite.ToArray()[i].Y += SpectraData[i].Y;
                }
            spectraCount++;
            //using (StreamWriter sw = new StreamWriter("F:\\psd.txt"))
            //{
            //    foreach (var point in SpectraData)
            //    {
            //        sw.WriteLine(String.Format("{0}\t{1}",point.X,point.Y));
            //    }
            //    sw.Close();
            //}
        }
        private void OnDataArrived()
        {
            if (null != DataArrivedEvent)
                DataArrivedEvent();
        }

        private void OnSpectraProcessed(object sender, List<Point> SpectraData)
        {
            if (SpectraProcessedEvent != null)
                SpectraProcessedEvent(sender, SpectraData);
        }
        void DigitalAnalyzer_DataArrivedEvent()
        {
            if (m_DataQueueHandler.IsBusy != true)
                m_DataQueueHandler.RunWorkerAsync();
            //throw new NotImplementedException();
        }

        void DataQueueHandler_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 0; i < m_PSDToWrite.Count; i++)
            {
                m_PSDToWrite.ToArray()[i].Y /= spectraCount;
            }
            using (StreamWriter sw = new StreamWriter("F:\\psd.txt"))
            {
                foreach (var point in m_PSDToWrite)
                {
                    sw.WriteLine(String.Format("{0}\t{1}", point.X, point.Y));
                }
                sw.Close();
            }
            //throw new NotImplementedException();
        }

        void DataQueueHandler_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void DataQueueHandler_DoWork(object sender, DoWorkEventArgs e)
        {

            FourierDataSet data;
            var bw = sender as BackgroundWorker;
            var percentage = 0;
            var perStep = 100 / m_Ranges.Length;
            while (m_DataQueue.TryDequeue(out data))
            {
                //
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                //
                Parallel.For(0, m_Ranges.Length, i =>
                {
                    m_Ranges[i].DataChanged(this, data);
                    m_Ranges[i].ProcessRangeData();
                    percentage += perStep;
                    bw.ReportProgress(percentage);
                });
                OnSpectraProcessed(this, m_Ranges.GetWholePSDfromRanges());
                //m_Ranges.ClearPSD();
                //
                sw.Stop();
                //
            }


        }

        void DataQueueHandler_Disposed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void ProcessSampledDataAsync(double[] NewData)
        {
            m_DataQueue.Enqueue(NewData.AsFourierDataSet());
            OnDataArrived();
        }
        public void ProcessSampledDataAsync(List<Point> NewData)
        {
            m_DataQueue.Enqueue(NewData.AsFourierDataSet());
            OnDataArrived();
        }

        public List<Point> ProcessSampledDataSyncronously(List<Point> NewData)
        {
            //throw new NotImplementedException();
            Parallel.For(0, m_Ranges.Length, i =>
            {
                m_Ranges[i].DataChanged(this, NewData.AsFourierDataSet());
                m_Ranges[i].ProcessRangeData();
            });
            OnSpectraProcessed(this, m_Ranges.GetWholePSDfromRanges());
            return m_Ranges.GetWholePSDfromRanges();
        }






    }




}
