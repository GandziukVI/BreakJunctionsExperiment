using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using BreakJunctions.Events;

using Aids.Graphics;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace BreakJunctions.Plotting
{
    class Experimental_RealTime_TimeTrace_DataSource_Sample : IPointDataSource
    {
        private List<PointD> _ExperimentalData;
        public List<PointD> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<PointD> _ExperimentalDataSource;

        private Samples _SampleNumber;

        public Experimental_RealTime_TimeTrace_DataSource_Sample(List<PointD> Data, Samples SampleNumber)
        {
            _ExperimentalData = Data;
            _ExperimentalDataSource = new EnumerableDataSource<PointD>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            _SampleNumber = SampleNumber;

            _Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(DependencyObject context)
        {
            return new EnumerablePointEnumerator<PointD>(_ExperimentalDataSource);
        }

        public virtual void AttachPointReceiveEvent() 
        {
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived; 
        }

        public virtual void DetachPointReceiveEvent()
        {
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
        }

        public void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            _ExperimentalData.Clear();
            var DataLendgth = (new int[4] { e.Data[0].Count, e.Data[1].Count, e.Data[2].Count, e.Data[3].Count }).Min();

            var number = 0;

            switch (_SampleNumber)
            {
                case Samples.Sample_01:
                    {
                        number = 0;
                    } break;
                case Samples.Sample_02:
                    {
                        number = 2;
                    } break;
                default:
                    break;
            }

            for (int i = 0; i < DataLendgth; i++)
            {
                if(e.Data[number][i].Y != 0.0)
                    _ExperimentalData.Add(new PointD(e.Data[number][i].X, e.Data[number + 1][i].Y / e.Data[number][i].Y));
            }

            _ExperimentalDataSource.RaiseDataChanged();
            
            _Dispatcher.BeginInvoke(new Action(delegate()
            {
                try
                {
                    DataChanged(sender, new EventArgs());
                }
                catch { }
            }));
        }
    }
}
